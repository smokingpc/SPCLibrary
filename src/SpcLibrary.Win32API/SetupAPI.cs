﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace SpcLibrary.Win32API
{
    //String Marshaling 參照： https://docs.microsoft.com/en-us/dotnet/framework/interop/default-marshaling-for-strings

    public static class GUID_DEVINTERFACE
    {
        //DEFINE_GUID(GUID_DEVINTERFACE_DISK,                   0x53f56307L, 0xb6bf, 0x11d0, 0x94, 0xf2, 0x00, 0xa0, 0xc9, 0x1e, 0xfb, 0x8b);
        //DEFINE_GUID(GUID_DEVINTERFACE_PARTITION,              0x53f5630aL, 0xb6bf, 0x11d0, 0x94, 0xf2, 0x00, 0xa0, 0xc9, 0x1e, 0xfb, 0x8b);
        //DEFINE_GUID(GUID_DEVINTERFACE_VOLUME,                 0x53f5630dL, 0xb6bf, 0x11d0, 0x94, 0xf2, 0x00, 0xa0, 0xc9, 0x1e, 0xfb, 0x8b);
        //DEFINE_GUID(GUID_DEVINTERFACE_STORAGEPORT,            0x2accfe60L, 0xc130, 0x11d2, 0xb0, 0x82, 0x00, 0xa0, 0xc9, 0x1e, 0xfb, 0x8b);
        //DEFINE_GUID(GUID_DEVINTERFACE_VMLUN,                  0x6f416619L, 0x9f29, 0x42a5, 0xb2, 0x0b, 0x37, 0xe2, 0x19, 0xca, 0x02, 0xb0);
        public static readonly Guid DISK = new Guid("{0x53f56307, 0xb6bf, 0x11d0, {0x94, 0xf2, 0x00, 0xa0, 0xc9, 0x1e, 0xfb, 0x8b}}");
        public static readonly Guid PARTITION = new Guid("{0x53f5630a, 0xb6bf, 0x11d0, {0x94, 0xf2, 0x00, 0xa0, 0xc9, 0x1e, 0xfb, 0x8b}}");
        public static readonly Guid VOLUME = new Guid("{0x53f5630d, 0xb6bf, 0x11d0, {0x94, 0xf2, 0x00, 0xa0, 0xc9, 0x1e, 0xfb, 0x8b}}");
        public static readonly Guid STORAGEPORT = new Guid("{0x2accfe60, 0xc130, 0x11d2, {0xb0, 0x82, 0x00, 0xa0, 0xc9, 0x1e, 0xfb, 0x8b}}");
        public static readonly Guid VMLUN = new Guid("{0x6f416619, 0x9f29, 0x42a5, {0xb2, 0x0b, 0x37, 0xe2, 0x19, 0xca, 0x02, 0xb0}}");
    }

    [Flags]
    public enum DIGCF : UInt32
    {
        DEFAULT = 0x00000001,  // only valid with DIGCF_DEVICEINTERFACE
        PRESENT = 0x00000002,
        ALLCLASSES = 0x00000004,
        PROFILE = 0x00000008,
        DEVICEINTERFACE = 0x00000010,
    }
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public class SP_DEVICE_INTERFACE_DATA
    {
        public int cbSize = Marshal.SizeOf<SP_DEVICE_INTERFACE_DATA>();
        public Guid InterfaceClassGuid = Guid.Empty;
        public int Flags = 0;
        public UIntPtr Reserved = UIntPtr.Zero;

        public SP_DEVICE_INTERFACE_DATA() { }
        public SP_DEVICE_INTERFACE_DATA(Guid guid, int flags)
        {
            this.InterfaceClassGuid = guid;
            this.Flags = flags;
        }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public class SP_DEVINFO_DATA
    {
        public int cbSize = Marshal.SizeOf<SP_DEVICE_INTERFACE_DATA>();
        public Guid ClassGuid = Guid.Empty;
        public int DevInst = 0;
        public IntPtr Reserved = IntPtr.Zero;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public class SP_DEVICE_INTERFACE_DETAIL_DATA
    {
        public int cbSize = Marshal.SizeOf<SP_DEVICE_INTERFACE_DATA>();
        [MarshalAs(UnmanagedType.LPWStr, SizeConst = 256)]
        public string DevicePath = "";
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public class DEVPROPKEY
    {
        public Guid FmtID = Guid.Empty;
        public uint Pid = 0;
    }

    public class HDEVINFO : IDisposable
    {
        private static readonly IntPtr InvalidValue = new IntPtr(-1);
        public bool IsInvalid
        {
            get { return ((Handle == IntPtr.Zero) || (Handle == InvalidValue)); }
        }
        public bool IsValid
        {
            get { return !IsInvalid; }
        }
        private string AllocTrace = "";        //用來紀錄 "哪段code Allocate這個Handle的？"
        private bool IsDisposed = false;
        private IntPtr Handle = IntPtr.Zero;

        public static implicit operator IntPtr(HDEVINFO value)
        {
            return value.Handle;
        }

        public static implicit operator HDEVINFO(IntPtr value)
        {
            return new HDEVINFO(value);
        }

        public HDEVINFO() { AllocTrace = Environment.StackTrace; }
        public HDEVINFO(IntPtr handle) : this()
        {
            this.Handle = handle;
        }
        ~HDEVINFO() { Dispose(false); }
        protected virtual void Dispose(bool disposing)
        {
            if (IsDisposed == false)
            {
                if (this.IsValid)
                {
                    //Kernel32.CloseHandle(Handle);
                    SetupAPI.SetupDiDestroyDeviceInfoList(Handle);
                    Handle = IntPtr.Zero;
                }
                IsDisposed = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }

    public static class SetupAPI
    {
        public const int DBT_DEVICEARRIVAL = 0x8000;
        public const int DBT_DEVICEREMOVECOMPLETE = 0x8004;
        //public const int DBT_DEVTYP_DEVICEINTERFACE = 5;
        //public const int DBT_DEVTYP_HANDLE = 6;
        public const int DEVICE_NOTIFY_ALL_INTERFACE_CLASSES = 4;
        public const int DEVICE_NOTIFY_SERVICE_HANDLE = 1;
        public const int DEVICE_NOTIFY_WINDOW_HANDLE = 0;
        public const int WM_DEVICECHANGE = 0x219;

        //public const uint DIGCF_DEFAULT = 0x01;
        //public const uint DIGCF_PRESENT = 0x02;
        //public const uint DIGCF_ALLCLASSES = 0x04;
        //public const uint DIGCF_PROFILE = 0x08;
        //public const uint DIGCF_DEVICEINTERFACE = 0x10;

        public const int MAX_DEV_LEN = 1000;
        public const int SPDRP_ADDRESS = 0x1c;
        public const int SPDRP_BUSNUMBER = 0x15;
        public const int SPDRP_BUSTYPEGUID = 0x13;
        public const int SPDRP_CAPABILITIES = 0xf;
        public const int SPDRP_CHARACTERISTICS = 0x1b;
        public const int SPDRP_CLASS = 7;
        public const int SPDRP_CLASSGUID = 8;
        public const int SPDRP_COMPATIBLEIDS = 2;
        public const int SPDRP_CONFIGFLAGS = 0xa;
        public const int SPDRP_DEVICE_POWER_DATA = 0x1e;
        public const int SPDRP_DEVICEDESC = 0;
        public const int SPDRP_DEVTYPE = 0x19;
        public const int SPDRP_DRIVER = 9;
        public const int SPDRP_ENUMERATOR_NAME = 0x16;
        public const int SPDRP_EXCLUSIVE = 0x1a;
        public const int SPDRP_FRIENDLYNAME = 0xc;
        public const int SPDRP_HARDWAREID = 1;
        public const int SPDRP_LEGACYBUSTYPE = 0x14;
        public const int SPDRP_LOCATION_INFORMATION = 0xd;
        public const int SPDRP_LOWERFILTERS = 0x12;
        public const int SPDRP_MFG = 0xb;
        public const int SPDRP_PHYSICAL_DEVICE_OBJECT_NAME = 0xe;
        public const int SPDRP_REMOVAL_POLICY = 0x1f;
        public const int SPDRP_REMOVAL_POLICY_HW_DEFAULT = 0x20;
        public const int SPDRP_REMOVAL_POLICY_OVERRIDE = 0x21;
        public const int SPDRP_SECURITY = 0x17;
        public const int SPDRP_SECURITY_SDS = 0x18;
        public const int SPDRP_SERVICE = 4;
        public const int SPDRP_UI_NUMBER = 0x10;
        public const int SPDRP_UI_NUMBER_DESC_FORMAT = 0x1d;

        public const int SPDRP_UPPERFILTERS = 0x11;

        public static DEVPROPKEY DEVPKEY_Device_BusReportedDeviceDesc =
            new DEVPROPKEY { FmtID = new Guid(0x540b947e, 0x8b40, 0x45bc, 0xa8, 0xa2, 0x6a, 0x0b, 0x89, 0x4c, 0xbd, 0xa2), Pid = 4 };

        #region ======== encapsulated API functions ========
        //IntPtr SetupDiGetClassDevs(ref System.Guid classGuid,
        //[MarshalAs(UnmanagedType.LPWStr)] string enumerator,
        //IntPtr hwndParent, uint flags);
        public static HDEVINFO SetupDiGetClassDevs(Guid classGuid, string enumerator, IntPtr parent, DIGCF flags)
        {
            //GUID guid = new GUID(classGuid);
            HDEVINFO handle = SetupDiGetClassDevs(ref classGuid, enumerator, parent,(UInt32) flags);
            return handle;
        }
        public static HDEVINFO SetupDiGetClassDevs(Guid class_guid, DIGCF flags)
        {
            //GUID guid = new GUID(class_guid);
            HDEVINFO handle = SetupDiGetClassDevs(ref class_guid, IntPtr.Zero, IntPtr.Zero, (UInt32)flags);
            return handle;
        }

        public static bool SetupDiEnumDeviceInterfaces(HDEVINFO handle, ref SP_DEVINFO_DATA infodata, 
                                        Guid class_guid, UInt32 member_id, ref SP_DEVICE_INTERFACE_DATA dev_ifdata)
        {
            GUID guid = new GUID(class_guid);
            return (1== SetupDiEnumDeviceInterfaces(handle, ref infodata, ref class_guid, member_id, ref dev_ifdata));
        }
        public static bool SetupDiEnumDeviceInterfaces(HDEVINFO handle, Guid class_guid, UInt32 member_id,
                                        ref SP_DEVICE_INTERFACE_DATA dev_ifdata)
        {
            bool ok = (1==SetupDiEnumDeviceInterfaces(handle, IntPtr.Zero, ref class_guid, member_id, ref dev_ifdata));
            return ok;
        }

        #endregion

        [DllImport(Win32DLL.SetupApi, CharSet = CharSet.Unicode, SetLastError = true, EntryPoint = "SetupDiGetDeviceRegistryPropertyW")]
        public static extern bool SetupDiGetDeviceRegistryProperty(IntPtr devinfo_set_handle, ref SP_DEVINFO_DATA devinfo_data, uint property_key, out uint reg_data_type, StringBuilder buffer, uint buffer_size, out uint required_size);

        [DllImport(Win32DLL.SetupApi, CharSet = CharSet.Unicode, SetLastError = true, EntryPoint = "SetupDiGetDevicePropertyW")]
        public static extern bool SetupDiGetDeviceProperty(IntPtr deviceInfo, ref SP_DEVINFO_DATA deviceInfoData, DEVPROPKEY propkey, out uint propertyDataType, StringBuilder propertyBuffer, uint propertyBufferSize, out uint requiredSize, uint flags);

        [DllImport(Win32DLL.SetupApi, CharSet = CharSet.Unicode, SetLastError = true)]
        static public extern bool SetupDiEnumDeviceInfo(IntPtr deviceInfoSet, uint memberIndex, ref SP_DEVINFO_DATA deviceInfoData);

        [DllImport(Win32DLL.SetupApi, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern IntPtr SetupDiCreateDeviceInfoList(ref GUID classGuid, IntPtr hwndParent);

        [DllImport(Win32DLL.SetupApi, CharSet = CharSet.Unicode, SetLastError = true)]
        static public extern bool SetupDiDestroyDeviceInfoList(IntPtr deviceInfoSet);

        [DllImport(Win32DLL.SetupApi, CharSet = CharSet.Unicode, SetLastError = true)]
        static public extern int SetupDiEnumDeviceInterfaces(IntPtr deviceInfoSet, ref SP_DEVINFO_DATA deviceInfoData, ref Guid interfaceClassGuid, uint memberIndex, ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData);

        [DllImport(Win32DLL.SetupApi, CharSet = CharSet.Unicode, SetLastError = true)]
        static public extern int SetupDiEnumDeviceInterfaces(IntPtr deviceInfoSet, IntPtr deviceInfoData, ref Guid interfaceClassGuid, uint memberIndex, ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData);

        [DllImport(Win32DLL.SetupApi, CharSet = CharSet.Unicode, SetLastError = true, EntryPoint = "SetupDiEnumDeviceInterfaces")]
        static public extern int SetupDiEnumDeviceInterfaces2(IntPtr deviceInfoSet, IntPtr devinfo_data, IntPtr guid, uint memberIndex, ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData);

        [DllImport(Win32DLL.SetupApi, CharSet = CharSet.Unicode, EntryPoint = "SetupDiGetClassDevsW")]
        static public extern IntPtr SetupDiGetClassDevs(ref Guid classGuid, [MarshalAs(UnmanagedType.LPWStr)] string enumerator, IntPtr hwndParent, uint flags);
        [DllImport(Win32DLL.SetupApi, CharSet = CharSet.Unicode, EntryPoint = "SetupDiGetClassDevsW")]
        static public extern IntPtr SetupDiGetClassDevs(ref Guid classGuid, IntPtr enumerator, IntPtr hwndParent, uint flags);

        [DllImport(Win32DLL.SetupApi, CharSet = CharSet.Unicode)]
        static public extern bool SetupDiGetDeviceInterfaceDetailBuffer(IntPtr deviceInfoSet, ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData, IntPtr deviceInterfaceDetailData, int deviceInterfaceDetailDataSize, out int requiredSize, IntPtr deviceInfoData);

        [DllImport(Win32DLL.SetupApi, CharSet = CharSet.Unicode)]
        static public extern bool SetupDiGetDeviceInterfaceDetail(IntPtr deviceInfoSet, ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData, ref SP_DEVICE_INTERFACE_DETAIL_DATA deviceInterfaceDetailData, int deviceInterfaceDetailDataSize, out UInt32 requiredSize, IntPtr deviceInfoData);
    }
}

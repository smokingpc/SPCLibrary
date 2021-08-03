using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace SpcLibrary.Win32API
{
    //String Marshaling 參照： https://docs.microsoft.com/en-us/dotnet/framework/interop/default-marshaling-for-strings

    [StructLayout(LayoutKind.Sequential)]
    public class SP_DEVICE_INTERFACE_DATA
    {
        public int cbSize;
        public System.Guid InterfaceClassGuid;
        public int Flags;
        public IntPtr Reserved;
    }

    [StructLayout(LayoutKind.Sequential)]
    public class SP_DEVINFO_DATA
    {
        public int cbSize;
        public Guid ClassGuid;
        public int DevInst;
        public IntPtr Reserved;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public class SP_DEVICE_INTERFACE_DETAIL_DATA
    {
        public int Size;
        [MarshalAs(UnmanagedType.LPWStr, SizeConst = 1024)]
        public string DevicePath;
    }

    [StructLayout(LayoutKind.Sequential)]
    public class DEVPROPKEY
    {
        public Guid fmtid;
        public ulong pid;
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

        public const uint DIGCF_DEFAULT = 0x01;
        public const uint DIGCF_PRESENT = 0x02;
        public const uint DIGCF_ALLCLASSES = 0x04;
        public const uint DIGCF_PROFILE = 0x08;
        public const uint DIGCF_DEVICEINTERFACE = 0x10;

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
            new DEVPROPKEY { fmtid = new Guid(0x540b947e, 0x8b40, 0x45bc, 0xa8, 0xa2, 0x6a, 0x0b, 0x89, 0x4c, 0xbd, 0xa2), pid = 4 };

        [DllImport(Win32DLL.SetupApi, CharSet = CharSet.Unicode, SetLastError = true, EntryPoint = "SetupDiGetDeviceRegistryPropertyW")]
        public static extern bool SetupDiGetDeviceRegistryProperty(IntPtr devinfo_set_handle, SP_DEVINFO_DATA devinfo_data, uint property_key, out uint reg_data_type, StringBuilder buffer, uint buffer_size, out uint required_size);

        [DllImport(Win32DLL.SetupApi, CharSet = CharSet.Unicode, SetLastError = true, EntryPoint = "SetupDiGetDevicePropertyW")]
        public static extern bool SetupDiGetDeviceProperty(IntPtr deviceInfo, SP_DEVINFO_DATA deviceInfoData, DEVPROPKEY propkey, out uint propertyDataType, StringBuilder propertyBuffer, uint propertyBufferSize, out uint requiredSize, uint flags);

        [DllImport(Win32DLL.SetupApi, SetLastError = true)]
        static public extern bool SetupDiEnumDeviceInfo(IntPtr deviceInfoSet, int memberIndex, SP_DEVINFO_DATA deviceInfoData);

        [DllImport(Win32DLL.SetupApi, SetLastError = true)]
        public static extern int SetupDiCreateDeviceInfoList(ref Guid classGuid, int hwndParent);

        [DllImport(Win32DLL.SetupApi, SetLastError = true)]
        static public extern int SetupDiDestroyDeviceInfoList(IntPtr deviceInfoSet);

        [DllImport(Win32DLL.SetupApi, SetLastError = true)]
        static public extern bool SetupDiEnumDeviceInterfaces(IntPtr deviceInfoSet, SP_DEVINFO_DATA deviceInfoData, ref Guid interfaceClassGuid, int memberIndex, SP_DEVICE_INTERFACE_DATA deviceInterfaceData);

        [DllImport(Win32DLL.SetupApi, SetLastError = true, EntryPoint = "SetupDiEnumDeviceInterfaces")]
        static public extern bool SetupDiEnumDeviceInterfaces2(IntPtr deviceInfoSet, IntPtr devinfo_data, ref Guid interfaceClassGuid, int memberIndex, SP_DEVICE_INTERFACE_DATA deviceInterfaceData);

        [DllImport(Win32DLL.SetupApi, CharSet = CharSet.Unicode, EntryPoint = "SetupDiGetClassDevsW")]
        static public extern IntPtr SetupDiGetClassDevs(ref System.Guid classGuid, [MarshalAs(UnmanagedType.LPWStr)] string enumerator, int hwndParent, uint flags);

        //[DllImport(Win32DLL.SetupApi, CharSet = CharSet.Unicode, EntryPoint = "SetupDiGetClassDevs")]
        //static public extern IntPtr SetupDiGetClassDevs2(ref System.Guid guid, IntPtr enumerator, int hwndParent, uint flags);

        [DllImport(Win32DLL.SetupApi, CharSet = CharSet.Unicode)]
        static public extern bool SetupDiGetDeviceInterfaceDetailBuffer(IntPtr deviceInfoSet, SP_DEVICE_INTERFACE_DATA deviceInterfaceData, IntPtr deviceInterfaceDetailData, int deviceInterfaceDetailDataSize, out int requiredSize, IntPtr deviceInfoData);

        [DllImport(Win32DLL.SetupApi, CharSet = CharSet.Unicode)]
        static public extern bool SetupDiGetDeviceInterfaceDetail(IntPtr deviceInfoSet, SP_DEVICE_INTERFACE_DATA deviceInterfaceData, SP_DEVICE_INTERFACE_DETAIL_DATA deviceInterfaceDetailData, int deviceInterfaceDetailDataSize, out int requiredSize, IntPtr deviceInfoData);
    }
}

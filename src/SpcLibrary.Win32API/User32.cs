using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace SpcLibrary.Win32API
{
    public static class User32
    {

        public enum DBCH_DEVICETYPE : uint
        {
            OEM = 0,                    //DBT_DEVTYP_OEM
            VOLUME = 0x00000002,        //DBT_DEVTYP_VOLUME
            PORT = 0x00000003,          //DBT_DEVTYP_PORT
            DEVICEINTERFACE = 0x00000005,//DBT_DEVTYP_DEVICEINTERFACE
            HANDLE = 0x00000006,        //DBT_DEVTYP_HANDLE
        }

        [StructLayout(LayoutKind.Sequential)]
        public class DEV_BROADCAST_HDR
        {
            public int dbch_size = 12;
            public int dbch_devicetype = 0;
            public int dbch_reserved = 0;
        }

        [StructLayout(LayoutKind.Sequential)]
        public class DEV_BROADCAST_DEVICEINTERFACE : DEV_BROADCAST_HDR
        {
            public Guid dbcc_classguid;
            public short dbcc_name;
            DEV_BROADCAST_DEVICEINTERFACE() { dbch_size = 12 + 16 + sizeof(short); }
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public class DEV_BROADCAST_DEVICEINTERFACE_1 : DEV_BROADCAST_HDR
        {
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 16)]
            public byte[] dbcc_classguid;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 255)]
            public char[] dbcc_name;
            DEV_BROADCAST_DEVICEINTERFACE_1() { dbch_size = 12 + 16 + 255; }
        }

        [StructLayout(LayoutKind.Sequential)]
        public class DEV_BROADCAST_HANDLE : DEV_BROADCAST_HDR
        {
            public int dbch_handle;
            public int dbch_hdevnotify;

            DEV_BROADCAST_HANDLE() { dbch_size = 20; }
        }

        /// <summary>
        /// 註冊 Device arrive notification 的接收端，hRecipient可以是window也可以是service的 handle
        /// </summary>
        /// <param name="hRecipient">接收端的Handle</param>
        /// <param name="filterData">註冊資料結構，參考DEV_BROADCAST_* 結構</param>
        /// <param name="flags">DBCH_DEVICETYPE flag</param>
        /// <returns></returns>
        [DllImport(Win32DLL.User32, CharSet = CharSet.Unicode, EntryPoint = "RegisterDeviceNotificationW")]
        static public extern IntPtr RegisterDeviceNotification(IntPtr hRecipient, IntPtr filterData, DBCH_DEVICETYPE flags);
        [DllImport(Win32DLL.User32, CharSet = CharSet.Unicode, EntryPoint = "RegisterDeviceNotificationW")]
        static public extern IntPtr RegisterDeviceNotification(IntPtr hRecipient, DEV_BROADCAST_DEVICEINTERFACE filterData, DBCH_DEVICETYPE flags);
        [DllImport(Win32DLL.User32, CharSet = CharSet.Unicode, EntryPoint = "RegisterDeviceNotificationW")]
        static public extern IntPtr RegisterDeviceNotification(IntPtr hRecipient, DEV_BROADCAST_HANDLE filterData, DBCH_DEVICETYPE flags);

        [DllImport(Win32DLL.User32, CharSet = CharSet.Auto)]
        static public extern bool UnregisterDeviceNotification(IntPtr handle);
    }
}

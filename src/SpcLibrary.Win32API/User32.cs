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
            public DEV_BROADCAST_HDR() { dbch_size = sizeof(int) * 3; }
        }

        //"filter" argument of RegisterDeviceNotification()
        [StructLayout(LayoutKind.Sequential)]
        public class DEV_BROADCAST_DEVICEINTERFACE : DEV_BROADCAST_HDR
        {
            public Guid dbcc_classguid;
            public ushort dbcc_name;
            //Guid size in C++ is 16 bytes
            public DEV_BROADCAST_DEVICEINTERFACE() :base()
            { dbch_size = dbch_size + 16 + sizeof(ushort); }
        }

        //lParam of WinProcCallback() , callback when WM_DEVCHANGE
        [StructLayout(LayoutKind.Sequential)]
        public class DEV_BROADCAST_DEVICEINTERFACE_CB : DEV_BROADCAST_HDR
        {
            public Guid dbcc_classguid;
            [MarshalAs(UnmanagedType.LPWStr)] 
            public StringBuilder dbcc_name;
        }

        //"filter" argument of RegisterDeviceNotification()
        [StructLayout(LayoutKind.Sequential)]
        public class DEV_BROADCAST_HANDLE : DEV_BROADCAST_HDR
        {
            public IntPtr dbch_handle;              //file system handle
            public IntPtr dbch_hdevnotify;          //handle returned by  RegisterDeviceNotification()

            Guid dbch_eventguid;
            int dbch_nameoffset;
            byte dbch_data;

            public DEV_BROADCAST_HANDLE() :base()
            { dbch_size = dbch_size + IntPtr.Size + IntPtr.Size + 16 + sizeof(int) + sizeof(byte); }
        }

        //lParam of WinProcCallback() , callback when WM_DEVCHANGE
        [StructLayout(LayoutKind.Sequential)]
        public class DEV_BROADCAST_HANDLE_CB : DEV_BROADCAST_HDR
        {
            public IntPtr dbch_handle;              //file system handle
            public IntPtr dbch_hdevnotify;          //handle returned by  RegisterDeviceNotification()

            Guid dbch_eventguid;
            int dbch_nameoffset;
            byte[] dbch_data;
        }


        /// <summary>
        /// 註冊 Device arrive notification 的接收端，hRecipient可以是window也可以是service的 handle。
        /// 如果hRecipient 是window，會從 WM_DEVCHANGE這訊息callback回來，lParam == filter data structure，
        /// C#要使用這套API要記得去攔截 WndProc() 處理函式，event從這裡回來
        /// </summary>
        /// <param name="hRecipient">接收端Window的Handle , C#裡要用window.safehandle</param>
        /// <param name="filterData">註冊資料結構，參考DEV_BROADCAST_* 結構</param>
        /// <param name="flags">DBCH_DEVICETYPE flag</param>
        /// <returns></returns>
        [DllImport(Win32DLL.User32, CharSet = CharSet.Unicode, EntryPoint = "RegisterDeviceNotificationW")]
        static public extern IntPtr RegisterDeviceNotification(IntPtr hRecipient, DEV_BROADCAST_DEVICEINTERFACE filterData, DBCH_DEVICETYPE flags);
        [DllImport(Win32DLL.User32, CharSet = CharSet.Unicode, EntryPoint = "RegisterDeviceNotificationW")]
        static public extern IntPtr RegisterDeviceNotification(IntPtr hRecipient, DEV_BROADCAST_HANDLE filterData, DBCH_DEVICETYPE flags);

        [DllImport(Win32DLL.User32)]
        static public extern bool UnregisterDeviceNotification(IntPtr handle);
    }
}

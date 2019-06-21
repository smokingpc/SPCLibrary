using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Win32API
{
    public static class User32
    {
        [DllImport(Win32DLL.User32)]
        static public extern IntPtr RegisterDeviceNotification(IntPtr hRecipient, IntPtr notificationFilter, Int32 flags);

        [DllImport(Win32DLL.User32)]
        static public extern bool UnregisterDeviceNotification(IntPtr handle);
    }
}

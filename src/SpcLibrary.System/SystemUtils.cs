using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpcLibrary.Win32API;

namespace SpcLibrary.Common
{
    public static class SysUtils
    {
        public static bool IsNetCore()
        {
            if (RuntimeInformation.FrameworkDescription.Contains(".Net Core"))
                return true;
            return false;
        }
        public static bool IsNetFramework()
        {
            if (RuntimeInformation.FrameworkDescription.Contains(".Net Framework"))
                return true;
            return false;
        }
        public static bool IsXamarine()
        {
            if (Type.GetType("Xamarin.Forms.Device") != null)
                return true;
            return false;
        }

        public static bool SetSystemDateTimeNow()
        { return SetSystemDateTime(DateTime.Now); }
        public static bool SetSystemDateTime(DateTime new_time)
        {
            OperatingSystem os = Environment.OSVersion;
            if (os.Platform != PlatformID.Win32NT)
                throw new NotSupportedException("This function only can run on WindowsNT system.");

            var datetime = new SYSTEMTIME(new_time);
            return Kernel32.SetSystemTime(datetime);
        }

    }
}

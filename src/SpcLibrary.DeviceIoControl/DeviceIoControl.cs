using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpcLibrary.DeviceIoControl
{
    public enum IO_METHOD
    {
        BUFFERED = 0,
        IN_DIRECT = 1,
        OUT_DIRECT = 2,
        NEITHER = 3,
    }

    public enum IO_ACCESS
    {
        ANY_ACCESS = 0,
        SPECIAL_ACCESS = ANY_ACCESS,
        READ_ACCESS = 1,   // file & pipe
        WRITE_ACCESS = 2,   // file & pipe
    }


    public static class DevIoCtl
    {
        //#define CTL_CODE( DeviceType, Function, Method, Access ) (((DeviceType) << 16) | ((Access) << 14) | ((Function) << 2) | (Method) )
        public static UInt32 IOCTL_CODE(UInt32 device_type, int function, int method, int access)
        {
            return IOCTL_CODE((UInt32)device_type, (UInt32)function, (UInt32)method, (UInt32)access);
        }

        public static UInt32 IOCTL_CODE(UInt32 device_type, UInt32 function, UInt32 method, UInt32 access)
        {
            UInt32 result = (device_type << 16) | (access << 14) | (function << 2) | access;

            return result;
        }



    }
}

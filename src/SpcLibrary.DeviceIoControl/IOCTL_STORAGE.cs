using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using SpcCommon.Common.Extension;
using SpcLibrary.Win32API;

namespace SpcLibrary.DeviceIoControl
{
    [StructLayout(LayoutKind.Sequential)]
    public class STORAGE_DEVICE_NUMBER
    {
        public DEVICE_TYPE DeviceType = DEVICE_TYPE.DISK;
        public UInt32 DeviceNumber = 0;
        public UInt32 PartitionNumber = 0;
    }

    public static class IOCTL_STORAGE 
    {
        public const UInt32 IOCTL_STORAGE_BASE = (UInt32) DEVICE_TYPE.MASS_STORAGE;
        public static readonly UInt32 GET_DEVICE_NUMBER = DevIoCtl.IOCTL_CODE(IOCTL_STORAGE_BASE, 0x0420, (int)IO_METHOD.BUFFERED, (int)IO_ACCESS.ANY_ACCESS);

        public static bool GetDiskDeviceNumber(CAutoHandle device, out STORAGE_DEVICE_NUMBER result) 
        {
            bool ok = false;
            UInt32 ret_size = 0;
            result = null;
            byte[] buffer = new byte[Marshal.SizeOf<STORAGE_DEVICE_NUMBER>()];
            if (null == buffer)
                return false;

            ok = Kernel32.DeviceIoControl(device, GET_DEVICE_NUMBER, 
                                            null, 0, 
                                            buffer, (uint)buffer.Length, 
                                            ref ret_size, null);
            if (ok)
            {
                result = buffer.FromBytes<STORAGE_DEVICE_NUMBER>();
            }
            return ok;
        }
    }
}

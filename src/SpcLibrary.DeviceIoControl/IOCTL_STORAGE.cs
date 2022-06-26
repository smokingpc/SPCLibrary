using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using SpcCommon.Common.Extension;
using SpcLibrary.Win32API;

//這邊所有的structure size都包含 alignement padding...

namespace SpcLibrary.DeviceIoControl
{
    public enum PARTITION_STYLE : UInt32
    {
        MBR = 0,
        GPT = 1,
        RAW = 2,
    }

    [StructLayout(LayoutKind.Sequential)]
    public class STORAGE_DEVICE_NUMBER
    {
        public const int SizeInBytes = 12;
        public DEVICE_TYPE DeviceType = DEVICE_TYPE.DISK;
        public UInt32 DeviceNumber = 0;
        public UInt32 PartitionNumber = 0;
    }
    [StructLayout(LayoutKind.Explicit)]
    public class STORAGE_READ_CAPACITY
    {
        public const int SizeInBytes = 32;
        [FieldOffset(0)]
        public UInt32 Version = 0;
        [FieldOffset(4)]
        public UInt32 Size = 0;
        [FieldOffset(8)]
        public UInt32 BlockLength = 0;  //size (in bytes) of one LBA block
        [FieldOffset(12)]
        public UInt32 Padding1;
        [FieldOffset(16)]
        public Int64 NumberOfBlocks = 0; //how many LBA blocks this disk has?
        [FieldOffset(24)]
        public Int64 DiskLength = 0;   //Total size (in bytes) of this disk.

        public override string ToString()
        {
            string ret = "";
            ret = $"BlockLength={BlockLength}\r\n";
            ret += $"NumberOfBlocks={NumberOfBlocks}\r\n";
            ret += $"DiskLength={DiskLength}\r\n";
            return ret;
        }
    }

    [StructLayout(LayoutKind.Explicit)]
    public class PARTITION_INFORMATION_MBR
    {
        public const int SizeInBytes = 24;
        [FieldOffset(0)]
        public byte PartitionType = 0;
        [FieldOffset(1)]
        public byte BootIndicator = 0;
        [FieldOffset(2)]
        public byte RecognizedPartition = 0;
        [FieldOffset(3)]
        public byte Padding1;
        [FieldOffset(4)]
        public UInt32 HiddenSectors = 0;
        [FieldOffset(8)]
        public Guid PartitionId = Guid.Empty;
    }
    [StructLayout(LayoutKind.Sequential)]
    public class PARTITION_INFORMATION_GPT
    {
        public const int SizeInBytes = 112;
        public Guid PartitionType = Guid.Empty;
        public Guid PartitionId = Guid.Empty;
        public UInt64 Attributes = 0;
        //how to map fixed C++ string?
        public char[] NameRaw;
        public string Name { get { return new string(NameRaw); } }
    }

    [StructLayout(LayoutKind.Sequential)]
    public class DRIVE_LAYOUT_INFORMATION_MBR 
    {
        public const int SizeInBytes = 8;
        public UInt32 Signature = 0;
        public UInt32 CheckSum = 0;
    }
    [StructLayout(LayoutKind.Sequential)]
    public class DRIVE_LAYOUT_INFORMATION_GPT
    {
        public const int SizeInBytes = 40;
        //offset 0
        public Guid DiskId = Guid.Empty;
        //offset 16
        public Int64 StartingUsableOffset = 0;
        //offset 24
        public Int64 UsableLength = 0;
        //offset 32
        public UInt32 MaxPartitionCount = 0;
        //4 bytes alignment padding
    }
    [StructLayout(LayoutKind.Explicit)]
    public class PARTITION_INFORMATION_EX
    {
        [FieldOffset(0)]
        public PARTITION_STYLE PartitionStyle;
        [FieldOffset(4)]
        public Int64 StartingOffset = 0;
        [FieldOffset(12)]
        public Int64 PartitionLength = 0;
        [FieldOffset(20)]
        public UInt32 PartitionNumber = 0;
        [FieldOffset(24)]
        public byte RewritePartitionRaw = 0;
        [FieldOffset(25)]
        public byte IsServicePartitionRaw = 0;
        [FieldOffset(26)]
        public DRIVE_LAYOUT_INFORMATION_MBR MBR;
        [FieldOffset(26)]
        public DRIVE_LAYOUT_INFORMATION_GPT GPT;

        public bool RewritePartition { get { return Convert.ToBoolean(RewritePartitionRaw); } }
        public bool IsServicePartition { get { return Convert.ToBoolean(IsServicePartitionRaw); } }
    }

    [StructLayout(LayoutKind.Explicit)]
    public class DRIVE_LAYOUT_INFORMATION_EX_HEADER
    {
        public const int SizeInBytes = 8 + DRIVE_LAYOUT_INFORMATION_GPT.SizeInBytes;
        [FieldOffset(0)]
        public PARTITION_STYLE PartitionStyle;
        [FieldOffset(4)]
        public UInt32 PartitionCount;
        [FieldOffset(8)]
        public DRIVE_LAYOUT_INFORMATION_MBR MBR;
        [FieldOffset(8)]
        public DRIVE_LAYOUT_INFORMATION_GPT GPT;
        
        //C++裡這邊後面接著不定長度 PARTITION_INFORMATION_EX[] 陣列，這在C#實在難辦。
        //只好拆成header，後面不定長度的資料分開parse....
        //PARTITION_INFORMATION_EX Partition[];   //how to map variable length array from C++?
    }

//    [StructLayout(LayoutKind.Explicit)]
    public class DRIVE_LAYOUT_INFORMATION_EX
    {
//        [FieldOffset(0)]
        DRIVE_LAYOUT_INFORMATION_EX_HEADER Header = new DRIVE_LAYOUT_INFORMATION_EX_HEADER();
        List<PARTITION_INFORMATION_EX> Partitions = new List<PARTITION_INFORMATION_EX>();
        //[FieldOffset(DRIVE_LAYOUT_INFORMATION_EX_HEADER.SizeInBytes)]


        public DRIVE_LAYOUT_INFORMATION_EX() { }
        public DRIVE_LAYOUT_INFORMATION_EX(IntPtr ptr)
            :this()
        {
        }
        public DRIVE_LAYOUT_INFORMATION_EX(byte[] buffer)
            : this()
        {
        }
    }

    public static class IOCTL_STORAGE 
    {
        public const UInt32 IOCTL_STORAGE_BASE = (UInt32) DEVICE_TYPE.MASS_STORAGE;
        public static readonly UInt32 GET_DEVICE_NUMBER = DevIoCtl.IOCTL_CODE(IOCTL_STORAGE_BASE, 0x0420, (int)IO_METHOD.BUFFERED, (int)IO_ACCESS.ANY_ACCESS);
        public static readonly UInt32 READ_CAPACITY = DevIoCtl.IOCTL_CODE(IOCTL_STORAGE_BASE, 0x0450, (int)IO_METHOD.BUFFERED, (int)IO_ACCESS.READ_ACCESS);

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
        public static bool GetDiskDeviceNumber(string devpath, out STORAGE_DEVICE_NUMBER result)
        {
            //devpath is DevicePathName of Disk
            //e.g. : "\\?\scsi#disk&ven_intel&prod_ssdsc2bw240a4#4&6dd29aa&0&050000#{53f56307-b6bf-11d0-94f2-00a0c91efb8b}"
            CAutoHandle device = Kernel32.CreateFile(devpath, ACCESS_TYPE.GENERIC_READ,
                    FILE_SHARE_MODE.SHARE_READ | FILE_SHARE_MODE.SHARE_WRITE,
                    FILE_DISPOSITION.OPEN_EXISTING, FILE_ATTR_AND_FLAG.NORMAL);

            return GetDiskDeviceNumber(device, out result);
        }
        public static bool GetDiskCapacity(CAutoHandle device, out STORAGE_READ_CAPACITY result)
        {
            bool ok = false;
            UInt32 ret_size = 0;
            result = null;
            byte[] buffer = new byte[Marshal.SizeOf<STORAGE_READ_CAPACITY>()];
            if (null == buffer)
                return false;

            ok = Kernel32.DeviceIoControl(device, READ_CAPACITY,
                                            null, 0,
                                            buffer, (uint)buffer.Length,
                                            ref ret_size, null);
            if (ok)
            {
                result = buffer.FromBytes<STORAGE_READ_CAPACITY>();
            }
            return ok;
        }
        public static bool GetDiskCapacity(string diskname, out STORAGE_READ_CAPACITY result)
        {
            //diskname is PhysicalDisk DeviceName
            //e.g. : "\\.\PhysicalDrive2"
            CAutoHandle device = Kernel32.CreateFile(diskname, ACCESS_TYPE.GENERIC_READ,
                    FILE_SHARE_MODE.SHARE_READ | FILE_SHARE_MODE.SHARE_WRITE,
                    FILE_DISPOSITION.OPEN_EXISTING, FILE_ATTR_AND_FLAG.NORMAL);

            return GetDiskCapacity(device, out result);
        }

        public static bool GetDiskPartitions(CAutoHandle device, out DRIVE_LAYOUT_INFORMATION_EX result)
        {
            //IOCTL_DISK_GET_DRIVE_LAYOUT_EX 
            result = new DRIVE_LAYOUT_INFORMATION_EX();
            if(device.IsInvalid)
                return false;

            return true;
        }
        public static bool GetDiskPartitions(string diskname, out DRIVE_LAYOUT_INFORMATION_EX result) 
        {
            //diskname is PhysicalDisk DeviceName
            //e.g. : "\\.\PhysicalDrive2"
            CAutoHandle device = Kernel32.CreateFile(diskname, ACCESS_TYPE.GENERIC_READ,
                    FILE_SHARE_MODE.SHARE_READ | FILE_SHARE_MODE.SHARE_WRITE,
                    FILE_DISPOSITION.OPEN_EXISTING, FILE_ATTR_AND_FLAG.NORMAL);

            return GetDiskPartitions(device, out result);
        }
    }
}

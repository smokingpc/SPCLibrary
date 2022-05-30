using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpcLibrary.Common.UI;
using SpcLibrary.Win32API;
using SpcLibrary.DeviceIoControl;

namespace InitAndFormatPhysicalDisk
{
    public partial class frmMain
    {
        //This function is NOT entire disk byte-to-byte cleanup.
        //It is like "clean" command in diskpart.
        //It is just clean up disk init data (MBR/GPT disk init data) 
        public void CleanupDisk(CPhyDisk disk)
        {
            CAutoHandle handle = Kernel32.CreateFile(
                        disk.DiskDevName, ACCESS_TYPE.GENERIC_ALL, FILE_SHARE_MODE.SHARE_READ, 
                        FILE_DISPOSITION.OPEN_EXISTING, FILE_ATTR_AND_FLAG.NORMAL);

            if (handle.IsValid)
            {
                UInt32 write_size = 1024*1024;
                UInt32 written_size = 0;
                byte[] buffer = new byte[write_size];

                //clean up first 3MB of specified physical disk.
                bool ok = Kernel32.WriteFile(handle, buffer, write_size, ref written_size, null);
                ok = Kernel32.WriteFile(handle, buffer, write_size, ref written_size, null);
                ok = Kernel32.WriteFile(handle, buffer, write_size, ref written_size, null);
                handle.Dispose();
            }
        }
        public CPhyDisk[] EnumPhysicalDisks() 
        {
            List<CPhyDisk> ret = new List<CPhyDisk>();

            HDEVINFO handle = SetupAPI.SetupDiGetClassDevs(
                    GUID_DEVINTERFACE.DISK, DIGCF.PRESENT | DIGCF.DEVICEINTERFACE);

            if (handle.IsInvalid)
                throw new Exception( "SetupDiGetClassDevs() failed!");

            SP_DEVICE_INTERFACE_DATA ifdata = new SP_DEVICE_INTERFACE_DATA();
            uint devid = 0;
            while (true == SetupAPI.SetupDiEnumDeviceInterfaces(handle, GUID_DEVINTERFACE.DISK, devid, ifdata))
            {
                SP_DEVICE_INTERFACE_DETAIL_DATA ifdetail = null;
                bool ok = SetupAPI.SetupDiGetDeviceInterfaceDetail(handle, ifdata, out ifdetail);
                if (ok)
                {
                    CAutoHandle device = Kernel32.CreateFile(ifdetail.DevPath, ACCESS_TYPE.GENERIC_READ,
                                FILE_SHARE_MODE.SHARE_READ | FILE_SHARE_MODE.SHARE_WRITE,
                                FILE_DISPOSITION.OPEN_EXISTING, FILE_ATTR_AND_FLAG.NORMAL);
                    if (device.IsValid)
                    {
                        STORAGE_DEVICE_NUMBER devnum = null;
                        if (true == IOCTL_STORAGE.GetDiskDeviceNumber(device, out devnum))
                        {
                            CPhyDisk found = new CPhyDisk();
                            found.DevPath = ifdetail.DevPath;
                            found.DiskDevName = $"\\\\.\\PhysicalDrive{devnum.DeviceNumber}";
                            ret.Add(found);
                        }
                    }
                }
                devid++;
            }

            handle.Dispose();

            return ret.ToArray();
        }

        public void ShowDiskCapacity(CPhyDisk disk)
        {
            CAutoHandle handle = Kernel32.CreateFile(
                        disk.DiskDevName, ACCESS_TYPE.GENERIC_READ, 
                        FILE_SHARE_MODE.SHARE_READ | FILE_SHARE_MODE.SHARE_WRITE,
                        FILE_DISPOSITION.OPEN_EXISTING, FILE_ATTR_AND_FLAG.NORMAL);
            if (handle.IsValid)
            {
                STORAGE_READ_CAPACITY result = null;
                bool ok = IOCTL_STORAGE.GetDiskCapacity(handle, out result);
                if (ok)
                {
                    textBox1.SetText($"BlockSize={result.BlockLength}, NumberOfBlock={result.NumberOfBlocks}, TotalSize={result.DiskLength/1048576} MB\r\n");
                }
                else
                {
                    textBox1.SetText($"[{disk.DiskDevName}]Read Disk Capacity Failed....\r\n");
                }
            }
        }
    }

    public class CPhyDisk
    {
        //Parent Controller Device Path, e.g. "\\?\scsi#disk&ven_intel&prod_ssdsc2bw240a4#4&6dd29aa&0&050000#{53f56307-b6bf-11d0-94f2-00a0c91efb8b}"
        public string DevPath { get; set; }
        //DevName of PhysicalDisk, e.g.  "\\.\PhysicalDrive2"
        public string DiskDevName { get; set; }
    }
}

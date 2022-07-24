using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpcLibrary.Common.UI;
using SpcLibrary.Win32API;
using SpcLibrary.DeviceIoControl;

namespace EnumPartitionOnDisk
{
    public partial class frmMain
    {
        public CPhyDisk[] EnumPhysicalDisks()
        {
            List<CPhyDisk> ret = new List<CPhyDisk>();

            HDEVINFO handle = SetupAPI.SetupDiGetClassDevs(
                    GUID_DEVINTERFACE.DISK, DIGCF.PRESENT | DIGCF.DEVICEINTERFACE);

            if (handle.IsInvalid)
                throw new Exception("SetupDiGetClassDevs() failed!");

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
                            found.PhyDiskName = $"\\\\.\\PhysicalDrive{devnum.DeviceNumber}";
                            ret.Add(found);
                        }
                    }
                }
                devid++;
            }

            handle.Dispose();

            return ret.ToArray();
        }

        private void PrintHeader(DRIVE_LAYOUT_INFORMATION_GPT gpt)
        { 
            string msg = $"Disk[{gpt.DiskId.ToString()}], Max [{gpt.MaxPartitionCount}] Paritions, UsableOffset[{gpt.StartingUsableOffset}], UsableLength[{gpt.UsableLength}]\r\n";
            textBox1.SetText(msg);
        }
        private void PrintHeader(DRIVE_LAYOUT_INFORMATION_MBR mbr)
        {
            string msg = $"Signature [{mbr.Signature.ToString("X8")}], Checksum [{mbr.CheckSum.ToString("X8")}]\r\n";
            textBox1.SetText(msg);
        }

        private void PrintPartitions(List<PARTITION_INFORMATION_EX> list)
        {
            foreach (var item in list)
            {
                string msg = "";
                msg = $"  [Partition {item.PartitionStyle}]\r\n";
                msg += $"  Type={item.PartitionStyle}, Service Partition={item.IsServicePartition}\r\n";
                msg += $"  Start={item.StartingOffset} & Length={item.PartitionLength}\r\n";

                switch (item.PartitionStyle)
                {
                    case PARTITION_STYLE.MBR:
                        msg += $"  Type[{item.MBR.PartitionType}], BootIndicator[{item.MBR.BootIndicator}], RecognizedPartition[{item.MBR.RecognizedPartition}], HiddenSectors[{item.MBR.HiddenSectors}]\r\n";
                        msg += $"  PartitionId[{item.MBR.PartitionId.ToString()}]\r\n";
                        break;
                    case PARTITION_STYLE.GPT:
                        msg += $"  PartitionType[{item.GPT.PartitionType}], Attributes[{item.GPT.Attributes}]\r\n";
                        //msg += $" Name[{item.GPT.Name}]\r\n";
                        break;
                }

                msg += "\r\n";
                textBox1.SetText(msg);
            }
        }

        //diskname => physical disk name. e.g.: "\\.\PhysicalDrive2"
        public void PrintPartitionInfo(CPhyDisk disk)
        {
            textBox1.SetText($"[{disk.PhyDiskName}]\r\n");
            DRIVE_LAYOUT_INFORMATION_EX result = null;
            bool ok = IOCTL_STORAGE.GetDiskPartitions(disk.PhyDiskName, out result);
            string msg = "";

            if (ok)
                msg = $"Disk contains {result.Header.Count} [{result.Header.Style}] Partitions\r\n";
            else
                msg = $"Failed to get partitions for {disk.PhyDiskName}, skip it...\r\n";
            textBox1.SetText(msg);
            msg = "";

            if (ok)
            {
                if (result.Header.Style == PARTITION_STYLE.GPT)
                {
                    PrintHeader(result.Header.GPT);
                }
                else if (result.Header.Style == PARTITION_STYLE.MBR)
                {
                    PrintHeader(result.Header.MBR);
                }
                else if (result.Header.Style == PARTITION_STYLE.RAW)
                {
                    msg = $"Raw Partition, no additional information.\r\n";
                    textBox1.SetText(msg);
                }

                msg = "";
                PrintPartitions(result.Partitions);
            }
        }
    }

    public class CPhyDisk
    {
        //Parent Controller Device Path, e.g. "\\?\scsi#disk&ven_intel&prod_ssdsc2bw240a4#4&6dd29aa&0&050000#{53f56307-b6bf-11d0-94f2-00a0c91efb8b}"
        public string DevPath { get; set; }
        //DevName of PhysicalDisk, e.g.  "\\.\PhysicalDrive2"
        public string PhyDiskName { get; set; }
    }
}

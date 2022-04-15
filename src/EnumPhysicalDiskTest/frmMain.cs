using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SpcCommon.Common;
using SpcCommon.Common.Extension;
using SpcLibrary.Win32API;

namespace EnumPhysicalDiskTest
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        public void ClearAll()
        {
            textBox1.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ClearAll();
            CAutoHandle handle = SetupAPI.SetupDiGetClassDevs(ref GUID_DEVINTERFACE.DISK,
                                            null,
                                            IntPtr.Zero,
                                            SetupAPI.DIGCF_PRESENT | SetupAPI.DIGCF_DEVICEINTERFACE);

            if (handle.IsInvalid)
            {
                textBox1.Text = "SetupDiGetClassDevs() failed!";
                return;
            }

            SP_DEVICE_INTERFACE_DATA ifdata = new SP_DEVICE_INTERFACE_DATA();
            //var size  = sizeof(SP_DEVICE_INTERFACE_DATA);
            uint devid = 0;

            while (true == SetupAPI.SetupDiEnumDeviceInterfaces(handle, IntPtr.Zero, ref GUID_DEVINTERFACE.DISK, devid, ref ifdata))
            {
                SP_DEVICE_INTERFACE_DETAIL_DATA ifdetail = new SP_DEVICE_INTERFACE_DETAIL_DATA();
                UInt32 ret_size = 0;
                bool ok = SetupAPI.SetupDiGetDeviceInterfaceDetail(handle, ref ifdata, ref ifdetail, ifdetail.cbSize, out ret_size, IntPtr.Zero);
                if (ok) 
                {
                    CAutoHandle file = Kernel32.CreateFile(ifdetail.DevicePath, ACCESS_TYPE.GENERIC_READ,
                                FILE_SHARE_MODE.SHARE_READ | FILE_SHARE_MODE.SHARE_WRITE,
                                FILE_DISPOSITION.OPEN_EXISTING, FILE_ATTR_AND_FLAG.NORMAL);
                    if (file.IsValid)
                    {
                        STORAGE_DEVICE_NUMBER devnum = new STORAGE_DEVICE_NUMBER();
                        Kernel32.DeviceIoControl(file, )
                    }
                }
                devid++;
            //    if (TRUE == ok)
            //    {
            //        HANDLE device = CreateFile(ifdetail->DevicePath, GENERIC_READ, FILE_SHARE_READ | FILE_SHARE_WRITE,
            //                            NULL, OPEN_EXISTING, 0, NULL);
            //        if (INVALID_HANDLE_VALUE != device)
            //        {
            //            STORAGE_DEVICE_NUMBER disk_number = { 0 };
            //            return_size = 0;
            //            ok = DeviceIoControl(device,
            //                IOCTL_STORAGE_GET_DEVICE_NUMBER,
            //                NULL,
            //                0,
            //                &disk_number,
            //                sizeof(STORAGE_DEVICE_NUMBER),
            //                &return_size,
            //                NULL);

                //            if (TRUE == ok)
                //                wprintf(PRINT_PHYSICAL_DISK_FORMAT, ifdetail->DevicePath, disk_number.DeviceNumber);
                //            CloseHandle(device);
                //        }
                //        else
                //            printf("CreateFile() failed, LastError=%d\n", GetLastError());
                //    }
                //    else
                //        printf("SetupDiGetDeviceInterfaceDetail() failed, LastError=%d\n", GetLastError());
                //    delete[] buffer;
                //}

            }
        }
    }
}

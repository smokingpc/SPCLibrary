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
using System.Runtime.InteropServices;
using SpcLibrary.Win32API;
using SpcLibrary.DeviceIoControl;
using SpcLibrary.Common.UI;

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
            HDEVINFO handle = SetupAPI.SetupDiGetClassDevs(
                                GUID_DEVINTERFACE.DISK, DIGCF.PRESENT | DIGCF.DEVICEINTERFACE);

            if (handle.IsInvalid)
            {
                textBox1.Text = "SetupDiGetClassDevs() failed!";
                return;
            }

            SP_DEVICE_INTERFACE_DATA ifdata = new SP_DEVICE_INTERFACE_DATA();
            uint devid = 1;
            UInt32 last_error = 0;
            while (true == SetupAPI.SetupDiEnumDeviceInterfaces(handle, GUID_DEVINTERFACE.DISK, devid, ref ifdata))
            {
                SP_DEVICE_INTERFACE_DETAIL_DATA ifdetail = new SP_DEVICE_INTERFACE_DETAIL_DATA();
                UInt32 ret_size = 0;
                bool ok = SetupAPI.SetupDiGetDeviceInterfaceDetail(handle, ref ifdata, ref ifdetail, ifdetail.cbSize, out ret_size, IntPtr.Zero);
                if (ok)
                {
                    CAutoHandle device = Kernel32.CreateFile(ifdetail.DevicePath, ACCESS_TYPE.GENERIC_READ,
                                FILE_SHARE_MODE.SHARE_READ | FILE_SHARE_MODE.SHARE_WRITE,
                                FILE_DISPOSITION.OPEN_EXISTING, FILE_ATTR_AND_FLAG.NORMAL);
                    if (device.IsValid)
                    {
                        STORAGE_DEVICE_NUMBER devnum = null;
                        if (true == IOCTL_STORAGE.GetDiskDeviceNumber(device, out devnum))
                        {
                            //"%s => \\\\?\\PhysicalDrive%d\n"
                            string msg = $"Found disk \\\\.\\PhysicalDrive{devnum.DeviceNumber} from device [{ifdetail.DevicePath}]";
                            textBox1.SetText(msg);
                        }
                    }
                }
                else 
                {
                    last_error = Kernel32.GetLastError();
                    textBox1.SetText($"LastError={last_error}\r\n");
                }
                devid++;
            }
            last_error = Kernel32.GetLastError();
        }
    }
}

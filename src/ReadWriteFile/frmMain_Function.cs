using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SpcLibrary.Common;
using SpcLibrary.Win32API;

namespace ReadWriteFile
{
    public partial class frmMain 
    {
        private byte[] DoReadFile(string filename, int read_len) 
        {
            using (CAutoHandle file = Kernel32.CreateFile(filename,
                                            ACCESS_TYPE.GENERIC_READ,
                                            FILE_SHARE_MODE.SHARE_READ | FILE_SHARE_MODE.SHARE_READ,
                                            FILE_DISPOSITION.OPEN_EXISTING,
                                            FILE_ATTR_AND_FLAG.NORMAL))

            {
                if (file.IsInvalid)
                {
                    MessageBox.Show($"OpenFile failed. LastError={Marshal.GetLastWin32Error()}");
                    return null;
                }

                byte[] buffer = new byte[read_len];
                UInt32 ret_size = 0;
                bool ok = Kernel32.ReadFile(file, buffer, (UInt32)buffer.Length, ref ret_size, null);

                if (!ok)
                {
                    MessageBox.Show($"ReadFile failed. LastError={Marshal.GetLastWin32Error()}");
                    return null;
                }

                return buffer;
            }
        }
        private void DoWriteFile(string filename, byte[] buffer) 
        {
            using (CAutoHandle file = Kernel32.CreateFile(filename,
                                            ACCESS_TYPE.GENERIC_WRITE,
                                            FILE_SHARE_MODE.SHARE_READ | FILE_SHARE_MODE.SHARE_READ,
                                            FILE_DISPOSITION.CREATE_NEW,
                                            FILE_ATTR_AND_FLAG.NORMAL))
            {
                UInt32 written_size = 0;

                if (file.IsInvalid)
                {
                    MessageBox.Show($"OpenFile failed. LastError={Marshal.GetLastWin32Error()}");
                    return;
                }
                bool ok = Kernel32.WriteFile(file, buffer, (UInt32)buffer.Length, ref written_size, null);

                if(!ok)
                    MessageBox.Show($"WriteFile failed. LastError={Marshal.GetLastWin32Error()}");
            }
        }
    }

    public static class BufferExt 
    {
        public static string ToHexString(this byte[] data)
        {
            string ret = "";

            foreach (var item in data)
            {
                ret += string.Format("{0:X2} ", item);
            }
            return ret;
        }
    }
}

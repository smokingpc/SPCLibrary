using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;


namespace Win32API
{
    public static class NTDLL
    {
        //timer resolution 單位是 tick (100ns)
        [DllImport("ntdll.dll", SetLastError = true)]
        public static extern int NtSetTimerResolution(int DesiredResolution, bool SetResolution, ref int CurrentResolution);

    }
}

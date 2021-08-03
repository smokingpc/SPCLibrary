using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace SpcLibrary.Win32API
{
    //TimeProc() callback definition
    public delegate void MMTimerCallback(UInt32 timer_id, UInt32 msg, UIntPtr user_ctx, UIntPtr reserve1, UIntPtr reserve2);

    [Flags]
    public enum MMTIMER_EVENT_TYPE : UInt32
    {
        ONESHOT = 0x0000,
        PERIODIC = 0x0001,

        CB_FUNCTION = 0x0000,     //function callback. (default)
        CB_EVENT_SET = 0x0010,    //timercallback field should be a event. this event will be set when timer signaled.
        CB_EVENT_PULSE = 0x0020,  //timercallback field should be a event. this event will be set then reset when timer signaled.

        KILL_SYNC = 0x0100       //guarantee no timer signaled after TimeKillEvent() called. prevent race condition.
    }

    public class WinMM
    {
        [DllImport("winmm.dll", SetLastError = true, EntryPoint = "timeSetEvent")]
        public static extern UInt32 TimeSetEvent(UInt32 msDelay, UInt32 msResolution, MMTimerCallback callback, UIntPtr userCtx, MMTIMER_EVENT_TYPE eventType = MMTIMER_EVENT_TYPE.PERIODIC);

        [DllImport("winmm.dll", SetLastError = true, EntryPoint = "timeKillEvent")]
        public static extern void TimeKillEvent(UInt32 uTimerId);
    }
}

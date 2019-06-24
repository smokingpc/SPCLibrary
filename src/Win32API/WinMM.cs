using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Win32API
{
    //TimeProc() callback definition
    public delegate void MMTimerCallback(UInt32 timer_id, UInt32 msg, UIntPtr user_ctx, UIntPtr reserve1, UIntPtr reserve2);

    public enum MMTIMER_EVENT_TYPE
    {
        ONESHOT = 0x0000,
        PERIODIC = 0x0001,

        CALLBACK_FUNCTION = 0x0000,     //function callback. (default)
        CALLBACK_EVENT_SET = 0x0010,    //timercallback field should be a event. this event will be set when timer signaled.
        CALLBACK_EVENT_PULSE = 0x0020,  //timercallback field should be a event. this event will be set then reset when timer signaled.

        KILL_SYNCHRONOUS = 0x0100       //guarantee no timer signaled after TimeKillEvent() called. prevent race condition.
    }

    public class WinMM
    {
        [DllImport("winmm.dll", SetLastError = true, EntryPoint = "timeSetEvent")]
        public static extern UInt32 TimeSetEvent(UInt32 msDelay, UInt32 msResolution, MMTimerCallback callback, UIntPtr userCtx, UInt32 eventType = 0x0001);

        [DllImport("winmm.dll", SetLastError = true, EntryPoint = "timeKillEvent")]
        public static extern void TimeKillEvent(UInt32 uTimerId);
    }
}

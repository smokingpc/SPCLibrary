using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Win32API;

namespace System
{
    //High Resolution Timer.
    //Minimun resolution is 0.5ms

    public class CHighResTimer
    {
        public DateTime LastTickTime { get; private set; }
        public event DelegateOnTimerTick OnTimer;

        private UInt32 TimerID = 0;
        private UInt32 Period = 0;
        private UInt32 Resolution = CONST.MaxResolution;      //resolution = 1ms
        private UInt32 EventType = (UInt32)(MMTIMER_EVENT_TYPE.CALLBACK_FUNCTION | MMTIMER_EVENT_TYPE.KILL_SYNCHRONOUS);

        public CHighResTimer()
        { }
        public CHighResTimer(UInt32 interval) : this()
        { Period = interval; }

        //(UInt32 timer_id, UInt32 msg, UIntPtr user_ctx, UIntPtr reserve1, UIntPtr reserve2);

        public void Start()
        {
            if (Period <= 0)
                throw new InvalidOperationException($"Invalid period value [{Period}]");

            Stop();
            TimerID = WinMM.TimeSetEvent(Period, Resolution, new MMTimerCallback(TimerCallback), UIntPtr.Zero, EventType);
        }
        public void Start(UInt32 interval)
        {
            Period = interval;
            Start();
        }

        public void Stop()
        {
            if (TimerID > 0)
            {
                WinMM.TimeKillEvent(TimerID);
                TimerID = 0;
            }
        }

        public void TimerCallback(UInt32 timer_id, UInt32 msg, UIntPtr user_ctx, UIntPtr reserve1, UIntPtr reserve2)
        {
            if (timer_id != this.TimerID)
                throw new InvalidOperationException($"incoming timer id[{timer_id}] != my TimerID[{TimerID}] !!");

            TimerEventArgs arg = new TimerEventArgs();
            LastTickTime = arg.SignalTime;

            OnTimer?.Invoke(this, arg);
        }
    }
}

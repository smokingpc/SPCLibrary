using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

namespace System
{
    public class CWaitTimer
    {
        public DateTime LastSignalTime { get; private set; }
        public event DelegateOnTimerTick OnTimer;

        private int Period = 0;
        private Thread ThreadTimer = null;
        private ManualResetEventSlim EventStopWait = new ManualResetEventSlim(false);
        //private bool StopThread = false;
        private bool IsRunning
        {
            get
            {
                if (ThreadTimer?.IsAlive == true)
                    return true;
                return false;
            }
        }

        public CWaitTimer() { }
        public CWaitTimer(int interval):this() { Period = interval; }
        public void Start()
        {
            if(Period <=0)
                throw new InvalidOperationException($"Invalid period value [{Period}]");

            Stop();
            EventStopWait.Reset();
            ThreadTimer = new Thread(FunctionTimer);
            ThreadTimer.Start();
        }
        public void Start(int interval)
        {
            Period = interval;
            Start();
        }

        public void Stop()
        {
            if (ThreadTimer != null)
            {
                EventStopWait.Set();
                //ThreadTimer.Join(1000);
                ThreadTimer = null;
            }
        }

        public void FunctionTimer()
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            while (EventStopWait.Wait(Period))
            {
                watch.Stop();
                if (watch.ElapsedMilliseconds >= Period)
                {
                    watch.Reset();
                    var arg = new TimerEventArgs();
                    this.LastSignalTime = arg.SignalTime;
                    OnTimer?.Invoke(this, arg);
                }
                watch.Start();
            }

            //multimedia
        }
    }
}

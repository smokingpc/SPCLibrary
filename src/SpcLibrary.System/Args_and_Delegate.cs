using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpcLibrary.System
{
    public delegate void DelegateOnTimerTick(object sender, TimerEventArgs e);
    public class TimerEventArgs
    {
        public DateTime SignalTime;
        public TimerEventArgs() { SignalTime = DateTime.Now; }
    }

}

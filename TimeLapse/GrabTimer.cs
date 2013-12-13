using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace TimeLapse
{
    public class GrabTimer
    {
        public double interval { get; set; }

        private DateTime startTime = DateTime.Today;
        public DateTime StartTime { get { return startTime; } set { startTime = value; } }

        private DateTime stopTime = DateTime.Today.AddMilliseconds(-1);
        public DateTime StopTime { get { return stopTime; } set { stopTime = value; } }

        private Timer baseTimer;

        public event EventHandler<GrabTimerEventArgs> GrabTriggered;

        public GrabTimer(int interval)
        {
            this.interval = interval;

            baseTimer = new Timer();
            baseTimer.AutoReset = false;
            baseTimer.Elapsed += new System.Timers.ElapsedEventHandler(baseTimer_Elapsed);
            baseTimer.Interval = GetInterval();
            baseTimer.Start();
        }

        private double GetInterval()
        {
            DateTime now = DateTime.Now;
            return (this.interval - (now.Second * 1000)%this.interval - now.Millisecond);
        }

        private void baseTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            RaisePublicEvent(e.SignalTime);
            baseTimer.Interval = GetInterval();
            baseTimer.Start();
        }

        private void RaisePublicEvent(DateTime signalTime)
        {
            if (signalTime.TimeOfDay >= StartTime.TimeOfDay && signalTime.TimeOfDay <= StopTime.TimeOfDay)
            {
                GrabTimerEventArgs args = new GrabTimerEventArgs(signalTime);
                OnGrabTriggered(args);
            }
        }

        protected virtual void OnGrabTriggered(GrabTimerEventArgs e)
        {
            EventHandler<GrabTimerEventArgs> handler = GrabTriggered;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }

    public class GrabTimerEventArgs : EventArgs
    {
        public DateTime TriggerTime { get; set; }

        public GrabTimerEventArgs(DateTime TriggerTime)
        {
            this.TriggerTime = TriggerTime;
        }
    }
}

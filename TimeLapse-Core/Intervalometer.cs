using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
using System.Diagnostics;

namespace TimeLapse_Core
{
    public class Intervalometer
    {
        public int interval { get; set; }

        private DateTime startTime = DateTime.Today;
        public DateTime StartTime { get { return startTime; } set { startTime = value; } }

        private DateTime stopTime = DateTime.Today.AddMilliseconds(-1);
        public DateTime StopTime { get { return stopTime; } set { stopTime = value; } }

        private Timer baseTimer;

        private ICamera cam;

        public event FrameReadyEventHandler FrameReady;

        public Intervalometer(ICamera cam, int interval)
        {
            this.cam = cam;
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
            Task.Factory.StartNew(GrabFrame);
            baseTimer.Interval = GetInterval();
            baseTimer.Start();
        }

        private void GrabFrame()
        {
            DateTime now = DateTime.Now;
            if (now.TimeOfDay >= StartTime.TimeOfDay && now.TimeOfDay <= StopTime.TimeOfDay)
            {
                DebugExtension.TimeStampedWriteLine("Trying to grab frame");
                Frame currentFrame = cam.CaptureFrame();
                if (currentFrame != null)
                {
                    DebugExtension.TimeStampedWriteLine("Frame grabbed.  Image size: " + currentFrame.ImageBytes.Count().ToString());
                    FrameReadyEventArgs args = new FrameReadyEventArgs(currentFrame);
                    OnFrameGrabbed(args);
                }
            }
        }

        protected virtual void OnFrameGrabbed(FrameReadyEventArgs e)
        {
            FrameReadyEventHandler handler = FrameReady;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
    
    public delegate void FrameReadyEventHandler(object sender, FrameReadyEventArgs args);

    public class FrameReadyEventArgs : EventArgs
    {
        public Frame frame { get; set; }

        public FrameReadyEventArgs(Frame frame)
        {
            this.frame = frame;
        }
    }
}

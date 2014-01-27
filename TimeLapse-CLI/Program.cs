using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TimeLapse_Core;
using System.Diagnostics;

namespace TimeLapse_CLI
{
    class Program
    {
        public static FrameController controller;

        static void Main(string[] args)
        {
            ConsoleTraceListener debug = new ConsoleTraceListener();
            Debug.Listeners.Add(debug);

            Thread.Sleep(30000);

            controller = new FrameController(new RaspberryPiCam(2));
            controller.intervalometer.StopTime = DateTime.Today.AddHours(23).AddMinutes(59);

            while (true) 
            {
                Thread.Sleep(250);
            }
        }
    }
}

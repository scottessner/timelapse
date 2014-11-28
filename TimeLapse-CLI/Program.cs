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

            controller = new FrameController(
                new UbiquitiAirCam("192.168.142.50",TimeLapseCLISettings.Default.CameraID),
                    TimeLapseCLISettings.Default.GrabFrequency,
                     TimeLapseCLISettings.Default.StartTime,
                     TimeLapseCLISettings.Default.StopTime,
                     TimeLapseCLISettings.Default.WebURL,
                     TimeLapseCLISettings.Default.ServerWorkerThreadCount);

            while (true) 
            {
                Thread.Sleep(250);
            }
        }
    }
}

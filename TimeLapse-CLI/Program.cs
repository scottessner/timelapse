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
                new RaspberryPiCam(TimeLapseCLISettings.Default.CameraID,
                    TimeLapseCLISettings.Default.ImageWidth,
                    TimeLapseCLISettings.Default.ImageHeight,
                    TimeLapseCLISettings.Default.ImageRotation,
                    TimeLapseCLISettings.Default.ImageQuality),
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

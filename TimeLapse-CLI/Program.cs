using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

            controller = new FrameController(new RaspberryPiCam(2));

            while (true) { }
        }
    }
}

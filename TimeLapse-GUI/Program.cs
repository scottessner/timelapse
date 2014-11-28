using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;
using TimeLapse_Core;
using NLog;

namespace TimeLapse_GUI
{
    public static class Program
    {
        static Logger logInstance = LogManager.GetLogger("Main");

        public static FrameController controller;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                controller = new FrameController(new UbiquitiAirCam("192.168.142.50", 3),
                     TimeLapseGUISettings.Default.GrabFrequency,
                     TimeLapseGUISettings.Default.StartTime,
                     TimeLapseGUISettings.Default.StopTime,
                     TimeLapseGUISettings.Default.UploadURL,
                     TimeLapseGUISettings.Default.ServerWorkerThreadCount);

                logInstance.Debug("Created Controller");

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
            }
            catch (Exception ex)
            {
                logInstance.ErrorException("General Error Encountered", ex);
            }
        }
    }
}

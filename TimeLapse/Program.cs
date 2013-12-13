using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;

namespace TimeLapse
{
    public static class Program
    {
        //public static WebUpdater updater = new WebUpdater(Properties.Settings.Default.CameraURL);
        //public static PictureUploader uploader = new PictureUploader(Properties.Settings.Default.UploadURL, 1);
        //public static PictureUploader uploader = new PictureUploader("http://127.0.0.1/webcam/pictureUpload.php", 1);
        //public static GrabTimer timer = new GrabTimer(Properties.Settings.Default.GrabFrequency);

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //timer.StartTime = Properties.Settings.Default.StartTime;
            //timer.StopTime = Properties.Settings.Default.StopTime;
            //timer.GrabTriggered += new EventHandler<GrabTimerEventArgs>(timer_GrabTriggered);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        //private static void timer_GrabTriggered(object sender, GrabTimerEventArgs e)
        //{
        //    Task.Factory.StartNew<Frame>(() => Program.updater.PhoneHome()).ContinueWith(ant => uploader.SendImage(ant.Result));
        //}
    }
}

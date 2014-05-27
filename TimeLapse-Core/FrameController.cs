using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Diagnostics;
using System.Timers;
using NLog;

namespace TimeLapse_Core
{
    public class FrameController
    {
        public Intervalometer intervalometer;
        public FrameServerConnection server;
        public ICamera camera;
        public Timer cacheTimer;
        public Logger logInstance = LogManager.GetLogger("FrameController");
        private int workerCount = 2;

        public FrameController(ICamera Camera)
        {
            this.camera = Camera;
            this.intervalometer = new Intervalometer(camera, CoreSettings.Default.GrabFrequency);
            this.intervalometer.StartTime = CoreSettings.Default.StartTime;
            this.intervalometer.StopTime = CoreSettings.Default.StopTime;

            logInstance.Info("Created FrameController Instance");

            intervalometer.FrameReady += intervalometer_FrameReady;

            this.server = new FrameServerConnection(CoreSettings.Default.UploadURL, workerCount);
            server.UploadComplete += server_UploadComplete;

            logInstance.Info("FrameServerConnection created");

            cacheTimer = new Timer(60000);
            cacheTimer.AutoReset = true;
            cacheTimer.Elapsed += cacheTimer_Elapsed;
            cacheTimer.Start();

            logInstance.Info("Image Cache Folder set to {0}", GetSaveFolder());
        }

        void cacheTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            logInstance.Debug("Consumer Status: {0}", String.Join(", ", server.GetThreadStatus()));
            DebugExtension.TimeStampedWriteLine("Consumer Status: " + String.Join(", ",server.GetThreadStatus()));
            if (Directory.GetFiles(GetSaveFolder()).Count() > workerCount && server.GetCount() == 0)
                UploadFiles();
        }

        void server_UploadComplete(object sender, UploadCompleteEventArgs e)
        {
            if (e.success)
            {
                if (File.Exists(GetSaveFolder() + e.frame.FileName))
                {
                    File.Delete(GetSaveFolder() + e.frame.FileName);
                    logInstance.Debug("Deleting {0}", e.frame.FileName);
                }   
            }
        }

        void intervalometer_FrameReady(object sender, FrameReadyEventArgs args)
        {
            args.frame.Save(GetSaveFolder() + args.frame.FileName);
            server.Upload(args.frame);
        }



        public Frame GrabFromCamera() 
        {            
            Frame thisFrame = camera.CaptureFrame();
            return thisFrame;
        }

        public void UploadFiles()
        {
            try
            {
                List<Frame> stack = server.GetStack();

                IEnumerable<String> filenames = stack.Select(f => f.FileName);

                IEnumerable<String> savedFiles = Directory.EnumerateFiles(GetSaveFolder()).Take(100).Except(filenames);

                foreach (string fileName in savedFiles)
                {
                    logInstance.Debug("Adding : {0} to upload queue", fileName);
                    server.Upload(Frame.FromFile(fileName));
                }
            }
            catch (Exception ex)
            {
                logInstance.ErrorException("File error", ex);
            }
            
        }

        public string GetSaveFolder()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
                    + Path.DirectorySeparatorChar
                    + "TimeLapse"
                    + Path.DirectorySeparatorChar;
        }
    }
}

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

namespace TimeLapse_Core
{
    public class FrameController
    {
        public Intervalometer intervalometer;
        public FrameServerConnection server;
        public ICamera camera;
        public Timer cacheTimer;

        public FrameController(ICamera Camera)
        {
            this.camera = Camera;
            this.intervalometer = new Intervalometer(camera, CoreSettings.Default.GrabFrequency);
            this.intervalometer.StartTime = CoreSettings.Default.StartTime;
            this.intervalometer.StopTime = CoreSettings.Default.StopTime;

            intervalometer.FrameReady += intervalometer_FrameReady;

            this.server = new FrameServerConnection(CoreSettings.Default.UploadURL, 2);
            server.UploadComplete += server_UploadComplete;

            cacheTimer = new Timer(60000);
            cacheTimer.AutoReset = true;
            cacheTimer.Elapsed += cacheTimer_Elapsed;
            cacheTimer.Start();

            DebugExtension.TimeStampedWriteLine("Save Folder: " + GetSaveFolder());
        }

        void cacheTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            DebugExtension.TimeStampedWriteLine("Consumer Status: " + String.Join(", ",server.GetThreadStatus()));
            if (Directory.GetFiles(GetSaveFolder()).Count() > 0 && server.GetCount() == 0)
                UploadFiles();
        }

        void server_UploadComplete(object sender, UploadCompleteEventArgs e)
        {
            if (e.success)
            {
                if (File.Exists(GetSaveFolder() + e.frame.FileName))
                    File.Delete(GetSaveFolder() + e.frame.FileName);
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
                    DebugExtension.TimeStampedWriteLine("Adding : " + fileName + " to upload queue");
                    server.Upload(Frame.FromFile(fileName));
                }
            }
            catch (Exception ex)
            {
                DebugExtension.TimeStampedWriteLine("File error: " + ex.Message);
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

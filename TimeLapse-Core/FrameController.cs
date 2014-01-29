using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Diagnostics;

namespace TimeLapse_Core
{
    public class FrameController
    {
        public Intervalometer intervalometer;
        public FrameServerConnection server;
        public ICamera camera;

        public FrameController(ICamera Camera)
        {
            this.camera = Camera;
            this.intervalometer = new Intervalometer(camera, CoreSettings.Default.GrabFrequency);
            this.intervalometer.StartTime = CoreSettings.Default.StartTime;
            this.intervalometer.StopTime = CoreSettings.Default.StopTime;

            intervalometer.FrameReady += intervalometer_FrameReady;

            this.server = new FrameServerConnection(CoreSettings.Default.UploadURL, 1);
            server.UploadComplete += server_UploadComplete;

            DebugExtension.TimeStampedWriteLine("Save Folder: " + GetSaveFolder());
        }

        void server_UploadComplete(object sender, UploadCompleteEventArgs e)
        {
            if (e.success)
            {
                if (File.Exists(GetSaveFolder() + e.frame.FileName))
                    File.Delete(GetSaveFolder() + e.frame.FileName);
                if (Directory.GetFiles(GetSaveFolder()).Count() > 0 && server.GetCount() == 0)
                    UploadFiles();
            }
            else
            {
                e.frame.Save(GetSaveFolder() + e.frame.FileName);
            }
        }

        void intervalometer_FrameReady(object sender, FrameReadyEventArgs args)
        {
            server.Upload(args.frame);
        }



        public Frame GrabFromCamera() 
        {            
            Frame thisFrame = camera.CaptureFrame();
            return thisFrame;
        }

        public void UploadFiles()
        {
            foreach(string fileName in Directory.EnumerateFiles(GetSaveFolder()).Take(100))
            {
                try
                {
                    server.Upload(Frame.FromFile(fileName));
                }
                catch (Exception ex)
                {
                    DebugExtension.TimeStampedWriteLine(ex.Message);
                }
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

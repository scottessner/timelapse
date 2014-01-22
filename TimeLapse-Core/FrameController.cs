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
        }

        void server_UploadComplete(object sender, UploadCompleteEventArgs e)
        {
            if (!e.success)
            {
                string fileName = CoreSettings.Default.SavePath + e.frame.CameraID + e.frame.CaptureTime.ToString("yyyyMMddHHmmss");
                e.frame.Save(fileName);
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



    }
}

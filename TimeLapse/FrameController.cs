using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using TimeLapse.Properties;

namespace TimeLapse
{
    class FrameController
    {
        Intervalometer intervalometer;
        FrameServerConnection server;
        ICamera camera;

        public FrameController(ICamera Camera)
        {
            this.camera = Camera;
            this.intervalometer = new Intervalometer(camera, Settings.Default.GrabFrequency);
            intervalometer.FrameReady += intervalometer_FrameReady;

            this.server = new FrameServerConnection(Settings.Default.UploadURL, 1);
            server.UploadComplete += server_UploadComplete;
        }

        void server_UploadComplete(object sender, UploadCompleteEventArgs e)
        {
            if (!e.success)
            {
                string fileName = Settings.Default.SavePath + e.frame.CameraID + e.frame.CaptureTime.ToString("yyyyMMddHHmmss");
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

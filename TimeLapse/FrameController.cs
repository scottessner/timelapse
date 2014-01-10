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
        PictureUploader uploader;
        ICamera camera;

        public FrameController(ICamera Camera)
        {
            this.camera = Camera;
            this.intervalometer = new Intervalometer(camera, Settings.Default.GrabFrequency);
            this.uploader = new PictureUploader(Settings.Default.UploadURL, 1);
            intervalometer.FrameReady += intervalometer_FrameReady;
        }

        void intervalometer_FrameReady(object sender, FrameReadyEventArgs args)
        {
            throw new NotImplementedException();
        }

        public Frame GrabFromCamera() 
        {
            
            Frame thisFrame = camera.CaptureFrame();
            return thisFrame;
        }



    }
}

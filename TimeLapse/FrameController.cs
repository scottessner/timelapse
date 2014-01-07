using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace TimeLapse
{
    class FrameController
    {
        public string SavePath { get; private set; }
        public int GrabFrequency { get; set; }
        public string UploadURL { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime StopTime { get; set; }

        List<Frame> myFrames = new List<Frame>();
        GrabTimer timer;
        PictureUploader uploader;
        ICamera camera;

        public FrameController(ICamera Camera, string UploadURL)
        {
            this.camera = Camera;
            this.uploader = new PictureUploader("test.everinview.com/frames", 1);
        }

        public Frame GrabFromCamera() 
        {
            Frame thisFrame = camera.CaptureFrame();
            return thisFrame;
        }

        public void UploadToServer(Frame frame) 
        {
            using (Stream myStream = new MemoryStream())
            {
                frame.Save(myStream);
                uploader.Upload(myStream, UploadURL);
            }
            uploader.SendImage(frame);
        }

        public void SaveImageToFile() { }


    }
}

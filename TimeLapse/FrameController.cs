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
        IFrameSource frameSource;

        public FrameController(IFrameSource FrameSource, string UploadURL)
        {
            this.frameSource = FrameSource;
            this.uploader = new PictureUploader("www.everinview.com/pictureUpload.php", 1);
        }

        public Frame GrabFromCamera() 
        {
            Frame thisFrame = frameSource.CaptureFrame();
            return thisFrame;
        }

        public void UploadToServer(Frame frame) 
        {
            uploader.SendImage(frame);
        }

        public void SaveImageToFile() { }


    }
}

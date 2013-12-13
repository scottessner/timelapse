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
        public string CameraURL { get; set; }
        public string UploadURL { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime StopTime { get; set; }

        GrabTimer timer;
        WebUpdater cam;
        PictureUploader uploader;
        IFrameSource frameSource = new UbiquitiAirCamFrameSource();

        public FrameController()
        {
            timer = new GrabTimer(GrabFrequency);
            cam = new WebUpdater(CameraURL);
            uploader = new PictureUploader(UploadURL, 1);
        }
        //private PictureUploader 

        public Frame GrabImageFromCamera() 
        {
            Frame thisFrame = frameSource.CaptureFrame();

            //string fileName = "";
            //Image grabbedImage = null;
            //Frame im = null;
            
            ////File Name Construction
            ////Round current time based on grab frequency
            //DateTime dt = DateTime.Now;
            //dt = dt.AddSeconds(-dt.Second % (GrabFrequency/1000));
            //fileName = dt.ToString("yyyyMMdd-HHmmss") + ".jpg";

            ////Grab Image using HTTP
            //using( var client = new HttpClient())
            //{
            //    var response = client.GetAsync(CameraURL).Result;
            //    if (response.IsSuccessStatusCode)
            //    {
            //        grabbedImage = Image.FromStream(response.Content.ReadAsStreamAsync().Result);
            //    }
            //}

            ////If we got an image back, create an ImageData object
            //if (grabbedImage != null)
            //{
            //    //im = new Frame(grabbedImage, fileName);
            //}

            return thisFrame;
        }

        public void UploadImageToWebsite() { }

        public void SaveImageToFile() { }


    }
}

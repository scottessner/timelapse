using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Net.Http;

namespace TimeLapse
{
    class UbiquitiAirCamFrameSource : IFrameSource
    {
        //Public info for this Type of FrameSource
        public string Manufacturer { get { return "Ubiquiti"; } }
        public string ModelNumber { get { return "AirCam"; } }
        public int WidthNative { get { return 1280; } }
        public int HeightNative { get { return 720; } }

        public string IPAddress { get; set; }
        private string CameraURL
        {
            get { return "http://" + IPAddress + "snapshot.cgi?chan=0"; }
        }
        

        public string UniqueID { get; set; }


        public Frame CaptureFrame()
        {
            Frame newFrame = new Frame();
            newFrame.CaptureTime = DateTime.Now;
            newFrame.FrameSourceID = this.UniqueID;
            newFrame.ImageBytes = CaptureImage();
            return newFrame;
        }

        private byte[] CaptureImage()
        {
            byte[] grabbedImage = null;

            //Grab Image using HTTP
            using (var client = new HttpClient())
            {
                var response = client.GetAsync(CameraURL).Result;
                if (response.IsSuccessStatusCode)
                {
                    grabbedImage = response.Content.ReadAsByteArrayAsync().Result;
                }
            }
            return grabbedImage;
        }
        
    }
}

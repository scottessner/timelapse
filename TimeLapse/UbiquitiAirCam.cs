using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Net.Http;

namespace TimeLapse
{
    class UbiquitiAirCam : ICamera
    {
        public string IPAddress { get; set; }
        private string CameraURL
        {
            get { return "http://" + IPAddress + "/snapshot.cgi?chan=0"; }
        }

        public UbiquitiAirCam(string IPAddress, int UniqueID)
        {
            this.IPAddress = IPAddress;
            this.UniqueID = UniqueID;
        }

        public int UniqueID { get; private set; }


        public Frame CaptureFrame()
        {
            Frame newFrame = new Frame();
            newFrame.CaptureTime = DateTime.Now;
            newFrame.CameraID = this.UniqueID;
            newFrame.ImageBytes = CaptureImage();
            if (newFrame.ImageBytes != null)
                return newFrame;
            else
                return null;
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
                    System.Windows.Forms.MessageBox.Show("Got Image");
                }
            }
            return grabbedImage;
        }
        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Net.Http;
using NLog;

namespace TimeLapse_Core
{
    public class UbiquitiAirCam : ICamera
    {

        public Logger logInstance = LogManager.GetLogger("AirCam");

        public string IPAddress { get; set; }
        private string CameraURL
        {
            get { return "http://" + IPAddress + "/snapshot.cgi?chan=0"; }
        }

        public UbiquitiAirCam(string IPAddress, int UniqueID)
        {
            this.IPAddress = IPAddress;
            this.UniqueID = UniqueID;
            logInstance.Info("AirCam created with URL: {0}", CameraURL);
        }

        public int UniqueID { get; private set; }


        public Frame CaptureFrame()
        {
            logInstance.Debug("Trying to Grab Frame");
            Frame newFrame = new Frame();
            newFrame.CaptureTime = DateTime.Now;
            newFrame.CameraID = this.UniqueID;
            newFrame.ImageBytes = CaptureImage();
            if (newFrame.ImageBytes != null)
            {
                logInstance.Debug("Frame Captured");
                return newFrame;
            }
            else
                return null;
        }

        private byte[] CaptureImage()
        {
            byte[] grabbedImage = null;

            //Grab Image using HTTP
            using (var client = new HttpClient())
            {
                logInstance.Debug("Sending Frame Grab Request");
                var response = client.GetAsync(CameraURL).Result;

                logInstance.Debug("Got HTTP Result");
                if (response.IsSuccessStatusCode)
                {
                    grabbedImage = response.Content.ReadAsByteArrayAsync().Result;
                }
            }
            return grabbedImage;
        }
        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace TimeLapse_Core
{
    class RaspberryPiCam : ICamera
    {

        public int UniqueID { get; private set; }

        public RaspberryPiCam(int UniqueID)
        {
            this.UniqueID = UniqueID;
        }

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

            Process raspistill = new Process();
            raspistill.StartInfo.FileName = "raspistill";
            raspistill.StartInfo.Arguments = "";
            raspistill.Start();
            raspistill.WaitForExit();

            MemoryStream ms = new MemoryStream();
            raspistill.StandardOutput.BaseStream.CopyTo(ms);

            grabbedImage = ms.ToArray();

            return grabbedImage;

        }
    }
}

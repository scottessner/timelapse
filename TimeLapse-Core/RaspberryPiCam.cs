using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace TimeLapse_Core
{
    public class RaspberryPiCam : ICamera
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
            raspistill.StartInfo.Arguments = "-w 1280 -h 720 -o -";
            raspistill.StartInfo.UseShellExecute = false;
            raspistill.StartInfo.RedirectStandardOutput = true;
            raspistill.Start();

            MemoryStream ms = new MemoryStream();
            raspistill.StandardOutput.BaseStream.CopyTo(ms);

            raspistill.WaitForExit();

            if (raspistill.ExitCode == 0)
                grabbedImage = ms.ToArray();

            return grabbedImage;

        }
    }
}

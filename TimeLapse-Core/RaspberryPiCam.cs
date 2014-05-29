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
        public int Width { get; set; }
        public int Height { get; set; }
        public int RotationAngle { get; set; }
        public int Quality { get; set; }

        public RaspberryPiCam(int UniqueID, int Width, int Height, int RotationAngle, int Quality)
        {
            this.UniqueID = UniqueID;
            this.Width = Width;
            this.Height = Height;
            this.RotationAngle = RotationAngle;
            this.Quality = Quality;
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
            raspistill.StartInfo.Arguments = String.Format("-w {0} -h {1} -rot {2} -q {3} -o -", Width, Height, RotationAngle, Quality);
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

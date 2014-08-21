using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;
using System.Drawing;
using System.Drawing.Imaging;

namespace TimeLapse_Core
{
    public class ImageImporter
    {
        private string[] m_FileList;
        public int CameraID;
        public Frame[] Frames 
        {
            get
            {
                return createFrames();
            }

            private set; 
        }

        public ImageImporter()
        {

        }

        public void GetFiles(string path)
        {
            GetFiles(new string[] { path });
        }

        public void GetFiles(string[] paths)
        {
            m_FileList = paths;
        }

        public string[] ShowFiles()
        {
            return m_FileList;
        }

        public Frame[] createFrames()
        {
            List<Frame> frameList = new List<Frame>();

            foreach (string path in m_FileList)
            {
                FileInfo file = new FileInfo(path);

                Frame frame = new Frame();
                frame.CameraID = CameraID;
                frame.CaptureTime = DateTime.ParseExact(file.Name, "yyyyMMdd-HHmmss", DateTimeFormatInfo.CurrentInfo);
                frame.Image = Image.FromFile(path);

                frameList.Add(frame);
            }

            return frameList.ToArray();

        }

    }


}

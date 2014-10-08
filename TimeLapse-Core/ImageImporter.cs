using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;

namespace TimeLapse_Core
{
    public class ImageImporter
    {
        private string[] m_FileList;
        public int CameraID;

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

        public void cacheFrames(string SaveFolder)
        {
            FileInfo file;
            Frame frame;
            string dateCode;

            foreach (string path in m_FileList)
            {
                file = new FileInfo(path);
                dateCode = file.Name.Split('.')[0];

                if (dateCode.Length == 14)
                {
                    dateCode = dateCode.Insert(9,"0");
                }

                frame = new Frame();
                frame.CameraID = CameraID;
                frame.CaptureTime = DateTime.ParseExact(dateCode, "yyyyMMdd-HHmmss", DateTimeFormatInfo.CurrentInfo);
                frame.Image = Image.FromFile(path);

                frame.Save(SaveFolder + frame.FileName);

                //Thread.Sleep(100);
                
            }

        }

    }


}

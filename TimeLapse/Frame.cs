using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Xml.Serialization;

namespace TimeLapse
{
    [Serializable]
    public class Frame
    {
        public Image Image { get; set; }
        public DateTime CaptureTime { get; set; }
        public string FrameSourceID { get; set; }
        
        public Frame()
        {

        }

        public void Save(Stream stream)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Frame));
            serializer.Serialize(stream, this);
        }

        public void Save(string fileName)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Frame));
            using (TextWriter xmlWriter = new StreamWriter(fileName))
            {
                serializer.Serialize(xmlWriter, this);
            }
        }

        public Stream ToStream()
        {
            var stream = new System.IO.MemoryStream();
            Image.Save(stream, Image.RawFormat);
            stream.Position = 0;
            return stream;
        }

        public static Frame FromFile(string fileName)
        {
            StreamReader reader = new StreamReader(fileName);
            XmlSerializer serializer = new XmlSerializer(typeof(Frame));
            Frame newFrame = (Frame)serializer.Deserialize(reader);
            return newFrame;
        }

    }
}

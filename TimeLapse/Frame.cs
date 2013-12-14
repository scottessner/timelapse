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
        public byte[] ImageBytes;

        [XmlIgnoreAttribute]
        public Image Image 
        {
            get 
            {
                if (ImageBytes != null)
                    return Image.FromStream(new MemoryStream(ImageBytes));
                else return null;
            }
            set
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    value.Save(ms, value.RawFormat);
                    ImageBytes = ms.ToArray();
                }
            }
        }
        public DateTime CaptureTime { get; set; }
        public IFrameSource FrameSource { get; set; }
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

        public static Frame FromStream(Stream inStream)
        {
            using (StreamReader reader = new StreamReader(inStream))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Frame));
                return (Frame)serializer.Deserialize(reader);
            }
        }

        public static Frame FromFile(string fileName)
        {
            return FromStream(new FileStream(fileName,FileMode.Open));
        }

    }
}

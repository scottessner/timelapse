using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Xml.Serialization;
using System.Net.Http;
using Newtonsoft.Json;

namespace TimeLapse_Core
{
    [Serializable]
    public class Frame
    {
        [JsonProperty(PropertyName = "imageBytes", Required = Required.Always)]
        public byte[] ImageBytes;

        [JsonIgnore]
        public Image Image 
        {
            get 
            {
                if (ImageBytes != null)
                    using (MemoryStream memStream = new MemoryStream(ImageBytes))
                    {
                        return Image.FromStream(memStream);
                    }
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

        [JsonIgnore]
        public string FileName
        {
            get
            {
                return
                    this.CameraID
                    + "-"
                    + this.CaptureTime.ToString("yyyyMMdd-HHmmss")
                    + ".eiv";
            }
            private set { }
        }

        [JsonProperty(PropertyName = "captureTime", Required = Required.Always)]
        public DateTime CaptureTime { get; set; }

        [JsonProperty(PropertyName = "camera_id", Required = Required.Always)]
        public int CameraID { get; set; }
        
        public Frame()
        {

        }

        public void Save(Stream stream)
        {
            string output = JsonConvert.SerializeObject(this);
            using (StreamWriter writer = new StreamWriter(stream))
            {
                writer.Write(output);
            }
        }

        public string GetJSON()
        {
            return JsonConvert.SerializeObject(this);
        }

        public void Save(string fileName)
        {
            using (FileStream stream = new FileStream(fileName,FileMode.Create,FileAccess.ReadWrite))
            {
                Save(stream);
            }
        }

        public static Frame FromStream(Stream inStream)
        {
            using (StreamReader reader = new StreamReader(inStream))
            {
                return JsonConvert.DeserializeObject<Frame>(reader.ReadToEnd());
            }
        }

        public static Frame FromFile(string fileName)
        {
            return FromStream(new FileStream(fileName,FileMode.Open));
        }

    }
}

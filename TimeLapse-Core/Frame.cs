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
        [JsonProperty(PropertyName = "imageBytes")]
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
        [JsonProperty(PropertyName = "captureTime")]
        public DateTime CaptureTime { get; set; }

        [JsonProperty(PropertyName = "camera_id")]
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
            using (FileStream stream = new FileStream(fileName,FileMode.OpenOrCreate,FileAccess.ReadWrite))
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

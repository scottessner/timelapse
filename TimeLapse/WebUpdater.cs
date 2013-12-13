using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;
using System.Windows.Forms;

namespace TimeLapse
{
    public class WebUpdater
    {
        private string url;

        public WebUpdater(string url)
        {
            this.url = url;
        }

        public Frame PhoneHome()
        {
            string savePath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            savePath += "\\TimeLapse\\";

            DateTime dt = DateTime.Now;
            dt = dt.AddSeconds(-dt.Second % 10);
            savePath += dt.ToString("yyyyMMdd-HHmmss") + ".jpg";

            Frame result = null;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Timeout = 10000;
            request.ReadWriteTimeout = 10000;

            try
            {
                //using (Stream stream = request.GetRequestStream())
                //{
                //    stream.Write(data, 0, data.Length);
                //}

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Image currentImage = Image.FromStream(response.GetResponseStream());

                    currentImage.Save(savePath);

                    //result = new Frame(savePath);
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }
        
        //public byte[] ToByteArray()
        //{
        //    var stream = new System.IO.MemoryStream();
        //    Image.Save(stream, Image.RawFormat);
        //    stream.Position = 0;

        //    byte[] FileByteArrayData = new byte[stream.Length];

        //    stream.Read(FileByteArrayData, 0, System.Convert.ToInt32(stream.Length));

        //    //Close the File Stream
        //    stream.Close();

        //    return FileByteArrayData; //return the byte data
        //}
    }
}

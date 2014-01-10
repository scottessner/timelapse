using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net;
using System.IO;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;
using System.ComponentModel;

namespace TimeLapse
{
    public class PictureUploader 
    {
        
        private Uri url;

        BlockingCollection<Frame> _taskQ = new BlockingCollection<Frame>(new ConcurrentStack<Frame>());
        public int Count { get; set; }

        public PictureUploader(string url, int workerCount)
        {
            this.url = new Uri(url);

            // Create and start a separate Task for each consumer:
            for (int i = 0; i < workerCount; i++)
                Task.Factory.StartNew(Consume);
        }

        public void Dispose() { _taskQ.CompleteAdding(); }

        public void SendImage(Frame image) { _taskQ.Add(image); }

        void Consume()
        {
            // This sequence that we’re enumerating will block when no elements
            // are available and will end when CompleteAdding is called. 
            foreach (Frame image in _taskQ.GetConsumingEnumerable())
                UploadImage(image);     // Perform task.
        }

        public int GetCount()
        {
            return _taskQ.Count();
        }

        private string UploadImage(Frame image)
        {
            try
            {
                //image.Upload(url);
                return "Success!";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        public bool Upload(Stream myFrame, string URL)
        {
            using (var client = new HttpClient())
            using (var content = new StreamContent(myFrame))
            {
                content.Headers.Add("Content-type", "application/json");

                var response = client.PostAsync(URL, content).Result;
                if (response.IsSuccessStatusCode)
                {
                    if (response.Content.ReadAsStringAsync().Result == "success")
                    {
                        return true;
                    }
                }

                return false;
            }
        }
    }
}

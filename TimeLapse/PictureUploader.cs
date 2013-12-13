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

namespace TimeLapse
{
    public class PictureUploader
    {
        
        private Uri url;
        private DirectoryInfo fromDirectory;
        private DirectoryInfo toDirectory;

        BlockingCollection<Frame> _taskQ = new BlockingCollection<Frame>(new ConcurrentStack<Frame>());
        
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

        public int Count()
        {
            return _taskQ.Count();
        }

        private string UploadImage(Frame image)
        {
            try
            {
                HttpContent stackValue = new StringContent(Count().ToString());
                //HttpContent fileContent = new StreamContent(image.ToStream());

                using(var client = new HttpClient())
                using (var formdata = new MultipartFormDataContent())
                {
                    formdata.Add(stackValue, "stack");
                    //formdata.Add(fileContent, "file1", image.fileName);
                    var response = client.PostAsync(url, formdata).Result;
                    if (!response.IsSuccessStatusCode)
                    {
                        return "";
                    }
                    else
                    {
                        return response.Content.ReadAsStringAsync().Result;
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}

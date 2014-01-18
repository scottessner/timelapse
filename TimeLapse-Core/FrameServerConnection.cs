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

namespace TimeLapse_Core
{
    public class FrameServerConnection 
    {
        private Uri url;
        BlockingCollection<WorkItem> _taskQ = new BlockingCollection<WorkItem>(new ConcurrentStack<WorkItem>());

        public event UploadCompleteEventHandler UploadComplete;

        public FrameServerConnection(string url, int workerCount)
        {
            this.url = new Uri(url);

            // Create and start a separate Task for each consumer:
            for (int i = 0; i < workerCount; i++)
                Task.Factory.StartNew(Consume);
        }

        public void Upload(Frame image)
        {
            _taskQ.Add(new WorkItem(image, WorkTask.upload));
        }

        public void Check(Frame image)
        {
            _taskQ.Add(new WorkItem(image, WorkTask.check));
        }

        public void Dispose() { _taskQ.CompleteAdding(); }

        void Consume()
        {
            // This sequence that we’re enumerating will block when no elements
            // are available and will end when CompleteAdding is called. 
            foreach (WorkItem item in _taskQ.GetConsumingEnumerable())
                Process(item);     // Perform task.
        }

        public int GetCount()
        {
            return _taskQ.Count();
        }

        private void Process(WorkItem item)
        {
            switch(item.task)
            {
                case WorkTask.upload:
                    UploadFrame(item.frame);

                    break;
                case WorkTask.check:
                    break;
                default:
                    throw new NotImplementedException();
            }
        }



        private void UploadFrame(Frame image)
        {
            Stream imageStream = new MemoryStream();
            image.Save(imageStream);

            bool success = false;
            int retries = 5;


            while(retries > 0 && success == false)
            {
                using (var client = new HttpClient())
                using (var content = new StreamContent(imageStream))
                {
                    content.Headers.Add("Content-type", "application/json");

                    var response = client.PostAsync(url, content).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        success = true;
                    }
                }
                retries--;
            }

            UploadCompleteEventArgs args = new UploadCompleteEventArgs(image, success);
            OnUploadCompleted(args);
        }

        protected virtual void OnUploadCompleted(UploadCompleteEventArgs e)
        {
            UploadCompleteEventHandler handler = UploadComplete;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }

    class WorkItem
    {
        public Frame frame;
        public WorkTask task;

        public WorkItem(Frame frame, WorkTask task)
        {
            this.frame = frame;
            this.task = task;
        }
    }

    enum WorkTask
    {
        upload,
        check
    }

    public delegate void UploadCompleteEventHandler(object sender, UploadCompleteEventArgs e);

    public class UploadCompleteEventArgs : EventArgs
    {
        public Frame frame { get; set; }
        public bool success { get; set; }

        public UploadCompleteEventArgs(Frame frame, bool success)
        {
            this.frame = frame;
            this.success = success;
        }
    }
}

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
using System.Diagnostics;
using NLog;

namespace TimeLapse_Core
{
    /// <summary>
    /// Class for connecting to Webcam HTTP Server
    /// </summary>
    public class FrameServerConnection : IDisposable
    {
        /// <summary>
        /// NLog Logger Instance
        /// </summary>
        private Logger logInstance = LogManager.GetLogger("FrameServerConnection");
        
        /// <summary>
        /// URL of web server
        /// </summary>
        private string url;

        /// <summary>
        /// Stack of Items to Upload.  Always prioritizes sending the most recent image first.
        /// </summary>
        BlockingCollection<WorkItem> _taskQ = new BlockingCollection<WorkItem>(new ConcurrentStack<WorkItem>());

        /// <summary>
        /// Reference to the Stack Consumer Threads
        /// </summary>
        Task[] consumer;


        public event UploadCompleteEventHandler UploadComplete;

        public FrameServerConnection(string url, int workerCount)
        {
            this.url = url;
            consumer = new Task[workerCount];
            // Create and start a separate Task for each consumer:
            for (int i = 0; i < workerCount; i++)
            {
                consumer[i] = Task.Factory.StartNew(Consume);
                logInstance.Info("Worker Thread {0} Created", i);
            }
        }

        public void Upload(Frame image)
        {
            _taskQ.Add(new WorkItem(image, WorkTask.upload));
            logInstance.Debug(image.FileName + " was added to the upload queue");
        }

        public string[] GetThreadStatus()
        {
            List<string> result = new List<string>();
            foreach (Task task in consumer)
            {
                result.Add(task.Status.ToString());
            }

            return result.ToArray();
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
                ProcessWorkItem(item);     // Perform task.
        }

        public int GetCount()
        {
            return _taskQ.Count();
        }

        public List<Frame> GetStack()
        {
            List<Frame> outList = new List<Frame>();

            foreach (WorkItem item in _taskQ.ToArray())
            {
                outList.Add(item.frame);
            }

            return outList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        private void ProcessWorkItem(WorkItem item)
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


        /// <summary>
        /// Posts a JSON representation of a frame to the "/frames" page of the website
        /// </summary>
        /// <param name="pFrame">Frame to upload</param>
        private void UploadFrame(Frame pFrame)
        {
            logInstance.Debug("Attempting to Upload " + pFrame.FileName);
            Uri fullurl = new Uri(url + "/frames");

            bool success = false;
            int retries = 5;


            while(retries > 0 && success == false)
            {
                retries--;

                try
                {
                    using (var client = new HttpClient())
                    using (var content = new StringContent(pFrame.GetJSON()))
                    {
                        client.Timeout = new TimeSpan(0, 0, 30);
                        content.Headers.Remove("Content-type");
                        content.Headers.Add("Content-type", "application/json");

                        logInstance.Debug("Attempting to Send " + pFrame.FileName);
                        var response = client.PostAsync(fullurl, content).Result;

                        logInstance.Debug(pFrame.FileName + " Upload Response: " + response.StatusCode);
                        if (response.IsSuccessStatusCode)
                        {
                            success = true;

                        }
                    }
                }
                catch (Exception ex)
                {
                    logInstance.ErrorException(pFrame.FileName + " Upload Error",ex);
                }
            }

            UploadCompleteEventArgs args = new UploadCompleteEventArgs(pFrame, success);
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
.
.*/
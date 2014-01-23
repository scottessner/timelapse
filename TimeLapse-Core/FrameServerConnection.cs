﻿using System;
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

namespace TimeLapse_Core
{
    public class FrameServerConnection : IDisposable
    {
        private string url;
        BlockingCollection<WorkItem> _taskQ = new BlockingCollection<WorkItem>(new ConcurrentStack<WorkItem>());

        public event UploadCompleteEventHandler UploadComplete;

        public FrameServerConnection(string url, int workerCount)
        {
            this.url = url;

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
            DebugExtension.TimeStampedWriteLine("Attempting to Upload Frame");
            Uri fullurl = new Uri(url + "/frames");

            bool success = false;
            int retries = 5;


            while(retries > 0 && success == false)
            {
                retries--;

                try
                {
                    using (var client = new HttpClient())
                    using (var content = new StringContent(image.GetJSON()))
                    {
                        client.Timeout = new TimeSpan(0, 0, 30);
                        content.Headers.Remove("Content-type");
                        content.Headers.Add("Content-type", "application/json");

                        DebugExtension.TimeStampedWriteLine("Upload: Attempting to Send");
                        var response = client.PostAsync(fullurl, content).Result;

                        DebugExtension.TimeStampedWriteLine("Upload Response: " + response.StatusCode);
                        if (response.IsSuccessStatusCode)
                        {
                            success = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    DebugExtension.TimeStampedWriteLine("Upload Error: " + ex.Message);
                }
            }

            DebugExtension.TimeStampedWriteLine(success ? "Frame Uploaded Successfully" : "Upload Failed");
            DebugExtension.TimeStampedWriteLine("Frames Waiting in Queue: " + _taskQ.Count.ToString());
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

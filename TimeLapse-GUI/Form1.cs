using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;
using TimeLapse_Core;
using System.IO;
using System.Diagnostics;

namespace TimeLapse_GUI
{
    public partial class Form1 : Form
    {
        FrameController fc = Program.controller;
        Frame myFrame;
        TextBoxTraceListener debug;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "";
            fc.intervalometer.FrameReady += intervalometer_FrameReady;
            fc.server.UploadComplete += server_UploadComplete;
            debug = new TextBoxTraceListener(debugTextBox);
            Debug.Listeners.Add(debug);
        }

        void server_UploadComplete(object sender, UploadCompleteEventArgs e)
        {

            toolStripStatusLabel1.Text = e.frame.CaptureTime.ToString() + " " + (e.success ? "Uploaded Successfully" : "Failed to Upload");
        }

        void intervalometer_FrameReady(object sender, FrameReadyEventArgs args)
        {
            pictureBox1.Image = Image.FromStream(new MemoryStream(args.frame.ImageBytes));
        }


        private void openButton_Click(object sender, EventArgs e)
        {
            openFileDialog1.Multiselect = false;
            openFileDialog1.ShowDialog();
            textBox1.Text = openFileDialog1.FileName;
            myFrame = Frame.FromFile(textBox1.Text);
            dateTimePicker1.Value = myFrame.CaptureTime;
            pictureBox1.Image = myFrame.Image;
            textBox2.Text = myFrame.CameraID.ToString();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            toolStripStatusLabel2.Text = "Queue: " + fc.server.GetCount().ToString();
            toolStripStatusLabel3.Text = "Consumer Status: " + String.Join(", ",fc.server.GetThreadStatus());
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            saveFileDialog1.ShowDialog();
            myFrame.Save(saveFileDialog1.FileName);
            
        }

        private void chooseImageButton_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            if (openFileDialog1.FileName != "")
            {
                pictureBox1.Image = Image.FromFile(openFileDialog1.FileName);
            }
        }

        private void captureButton_Click(object sender, EventArgs e)
        {
            myFrame = fc.camera.CaptureFrame();
            fc.server.Upload(myFrame);
        }

        private void uploadButton_Click(object sender, EventArgs e)
        {
            fc.server.Upload(myFrame);
        }

        private void importButton_Click(object sender, EventArgs e)
        {
            ImageImporter imp = new ImageImporter();
            imp.CameraID = 1;
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.ShowDialog();

            DirectoryInfo dir = new DirectoryInfo(dialog.SelectedPath);

            List<string> fileList = new List<string>();
            foreach(FileInfo file in dir.GetFiles())
            {
                fileList.Add(file.FullName);
            }

            imp.GetFiles(fileList.ToArray());


            //openFileDialog1.Multiselect = true;
            //openFileDialog1.
            //openFileDialog1.ShowDialog();
            //imp.GetFiles(openFileDialog1.FileNames);
            Task.Factory.StartNew( () => imp.cacheFrames(fc.GetSaveFolder()));
            
        }

    }
}

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

namespace TimeLapse
{
    public partial class Form1 : Form
    {
        Frame myFrame;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


        private void openButton_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            textBox1.Text = openFileDialog1.FileName;
            myFrame = Frame.FromFile(textBox1.Text);
            dateTimePicker1.Value = myFrame.CaptureTime;
            pictureBox1.Image = myFrame.Image;
            textBox2.Text = myFrame.CameraID.ToString();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            Frame newFrame = new Frame();
            newFrame.CaptureTime = dateTimePicker1.Value;
            newFrame.CameraID = Int32.Parse(textBox2.Text);
            newFrame.Image = pictureBox1.Image;
            saveFileDialog1.ShowDialog();
            newFrame.Save(saveFileDialog1.FileName);
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
            //UbiquitiAirCam source = new UbiquitiAirCam("192.168.0.20", 5);
            //myFrame = source.CaptureFrame();
            //dateTimePicker1.Value = myFrame.CaptureTime;
            //pictureBox1.Image = myFrame.Image;
            //textBox2.Text = myFrame.CameraID.ToString();
        }

    }
}

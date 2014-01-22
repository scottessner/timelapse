using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace TimeLapse_GUI
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void goButton_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();

            //Frame image = new Frame(openFileDialog1.FileName);
            //Program.uploader.SendImage(image);
        }
    }
}

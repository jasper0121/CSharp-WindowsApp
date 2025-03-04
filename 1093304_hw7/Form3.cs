using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _1093304_hw7
{
    public partial class Form3 : Form
    {
        private float R = 1, G = 1, B = 1, A = 1;
        public Form3()
        {
            InitializeComponent();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            label5.Text = (R = trackBar1.Value / 10f).ToString();
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            label6.Text = (G = trackBar2.Value / 10f).ToString();
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            label7.Text = (B = trackBar3.Value / 10f).ToString();
        }

        private void trackBar4_Scroll(object sender, EventArgs e)
        {
            label8.Text = (A = trackBar4.Value / 10f).ToString();
        }

        public float getR()
        {
            return R;
        }

        public float getG()
        {
            return G;
        }

        public float getB()
        {
            return B;
        }

        public float getA()
        {
            return A;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}

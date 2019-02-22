using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Paint
{
    public partial class Form1 : Form
    {
        bool draw = false;
        int x0, y0, x1, y1;
        Item currentItem; 

        public Form1()
        {
            InitializeComponent();
        }

        public enum Item { Rectangle, Ellipse }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            draw = true;
            x0 = e.X;
            y0 = e.Y; 
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            draw = false;
            x1 = e.X;
            y1 = e.Y; 
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (draw)
            {
                Graphics graphics = pictureBox1.CreateGraphics();
                switch (currentItem)
                {
                    case Item.Rectangle:
                        graphics.FillRectangle(new SolidBrush(Color.FromArgb(50, Color.Gray)), x0, y0, e.X - x0, e.Y - y0);
                        break; 

                }

            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}

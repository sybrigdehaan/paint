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
        int x0, y0, x1, y1 = 0;
        Item currentItem;

        public Form1()
        {
            InitializeComponent();
        }

        // size canvas 1397; 783
        // location 0; 0 
        private void Form1_Load(object sender, EventArgs e){

            var picturebox = new PictureBox
            {
                Name = "pictureBox",
                Size = new Size(1397, 783),
                Location = new Point(0, 0)
            };
            this.Controls.Add(picturebox);
        }

        public enum Item { Rectangle, Ellipse, DeleteGroup, AddGroup }

        private void pictureBoxMain_MouseDown(object sender, MouseEventArgs e)
        {
            draw = true;
            x0 = e.X;
            y0 = e.Y;
        }

        private void pictureBoxMain_MouseMove(object sender, MouseEventArgs e)
        {
            if (draw)
            {
                Graphics graphics = pictureBoxMain.CreateGraphics();
                switch (currentItem)
                {
                    case Item.Rectangle:
                        graphics.FillRectangle(new SolidBrush(Color.FromArgb(100, Color.Gray)), x0, y0, e.X - x0, e.Y - y0);
                        break;
                    case Item.Ellipse:
                        graphics.FillEllipse(new SolidBrush(Color.FromArgb(100, Color.Red)), x0, y0, e.X - x0, e.Y - y0); 
                        break; 
                }
            }
        }

        private void pictureBoxMain_MouseUp(object sender, MouseEventArgs e)
        {
            draw = false;
            x1 = e.X;
            y1 = e.Y;

            switch (currentItem)
            {
                case Item.Rectangle:
                    Figure rectangle = new Figure("Rectangle", x0, y0, e.X - x0, e.Y - y0);
                    break;
                case Item.Ellipse:
                    Figure ellipse = new Figure("Ellipse", x0, y0, e.X - x0, e.Y - y0);
                    break; 
            }
        }

        private void toolStripDeleteGroup_Click(object sender, EventArgs e)
        {
            currentItem = Item.DeleteGroup;
        }

        private void toolStripAddGroup_Click(object sender, EventArgs e)
        {
            currentItem = Item.AddGroup;
        }

        private void toolStripRectangle_Click(object sender, EventArgs e)
        {
            currentItem = Item.Rectangle; 
        }
        
        private void toolStripEllipse_Click(object sender, EventArgs e)
        {
            currentItem = Item.Ellipse;
        }

    }
}

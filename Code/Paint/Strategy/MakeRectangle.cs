using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing; 

namespace Paint.Strategy
{
    class MakeRectangle : IMakeFigures
    {
       // Graphics graphics = pictureBoxMain.CreateGraphics();
        public void MakeFigure()
        {
            //graphics.FillRectangle(new SolidBrush(Color.FromArgb(100, Color.Gray)), x0, y0, e.X - x0, e.Y - y0);
        }
    }
}

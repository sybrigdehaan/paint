using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paint
{
    public class Rectangle
    {
        public int left, top, width, height; 
        public Rectangle(int left, int top, int width, int height)
        {
            this.left = left;
            this.top = top;
            this.width = width;
            this.height = height; 
        }
    }

    class Ellipse 
    {
        int left, top, width, height;
        public Ellipse(int left, int top, int width, int height)
        {
            this.left = left;
            this.top = top;
            this.width = width;
            this.height = height;
        }
    }
}

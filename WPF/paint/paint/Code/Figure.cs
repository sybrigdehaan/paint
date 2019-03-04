using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace paint
{
    public class Figure : IFigures
    {
        private string name;
        private int left, top, width, height;

        public Figure(string name, int left, int top, int width, int height)
        {
            this.name = name;
            this.left = left;
            this.top = top;
            this.width = width;
            this.height = height;
        }

        public void ShowFigureDetails()
        {
            Console.WriteLine("This is a: " + name + " With the measurement: " + "left: " + left + ", Top: " + top + ", Width: " + width + ", Height: " + height);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paint
{
    public class Figure
    {
        private string name;
        private int left, top, width, height;
        private List<Figure> subFigures;
        
        public Figure(string name, int left, int top, int width, int height)
        {
            this.name = name;
            this.left = left;
            this.top = top;
            this.width = width;
            this.height = height;
            this.subFigures = new List<Figure>(); 
        }

        public void Add(Figure group)
        {
            subFigures.Add(group); 
        }

        public void Remove(Figure group)
        {
            subFigures.Remove(group);
        }

        public List<Figure> GetSubGroups()
        {
            return subFigures; 
        }

        public string SetToString()
        {
            return ("This is a: " + name + " With the measurement: " + "left: " + left + ", Top: " + top + ", Width: " + width + ", Height: " + height); 
        }
    }
}

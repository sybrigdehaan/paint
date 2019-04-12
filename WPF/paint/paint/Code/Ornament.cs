using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace paint
{
    // creates ornament
    public abstract class Ornament : _Shape 
    {
        Color color = Color.FromArgb(0, 0, 0, 0);
        protected TextBlock textBlock = new TextBlock();

        public void Add(ref InkCanvas MyInkCanvas, IFigures shape)
        {
            MyInkCanvas.Children.Add(textBlock);
            if (typeof(_Shape) == shape.GetType())
            {
                (shape as _Shape)._Ornament = this;
            }
        }
    }

    public class Left : Ornament
    {
       public Left(string text, double left, double top)
        {
            textBlock.Text = text;
            InkCanvas.SetLeft(textBlock, left);
            InkCanvas.SetTop(textBlock, top);
        }
    }

    public class Right : Ornament
    {
        public Right(string text, double left, double top)
        {
            textBlock.Text = text;
            InkCanvas.SetLeft(textBlock, left);
            InkCanvas.SetTop(textBlock, top);
        }
    }

    public class Top : Ornament
    {
        public Top(string text, double left, double top)
        {
            textBlock.Text = text;
            InkCanvas.SetLeft(textBlock, left);
            InkCanvas.SetTop(textBlock, top);
        }
    }

    public class Bottom : Ornament
    {
        public Bottom(string text, double left, double top)
        {
            textBlock.Text = text;
            InkCanvas.SetLeft(textBlock, left);
            InkCanvas.SetTop(textBlock, top);
        }
    }

}

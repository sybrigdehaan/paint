using System;
using System.Windows; 
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Controls;
using System.Collections.Generic; 

namespace paint
{
    public abstract class _Shape : IFigures
    {
        protected Shape myShape;
        protected List<Ornament> myOrnament;
        protected double left, top, width, height;

        public Ornament _Ornament { set { myOrnament.Add(value); } }

        public void Draw(ref InkCanvas MyInkCanvas)
        {
            MyInkCanvas.Children.Add(myShape);
        }

        public void ChangeFigure(double x1, double x2, double y1, double y2)
        {
            myShape.Fill = MainWindow.mySolidColorBrushRed;
            myShape.Width = Math.Abs(x2 - x1);
            myShape.Height = Math.Abs(y2 - y1);
            if (x1 < x2)
                InkCanvas.SetLeft(myShape, x1);
            else 
                InkCanvas.SetLeft(myShape, x2);
            if (y1 < y2)
                InkCanvas.SetTop(myShape, y1);
            else
                InkCanvas.SetTop(myShape, y2);
        }

        public void UpdateFigure(Shape shape)
        {
            left = InkCanvas.GetLeft(shape);
            top = InkCanvas.GetTop(shape);
            width = shape.Width;
            height = shape.Height; 
        }

        public virtual void ShowFigureDetails(){}

        public void CheckShape(ref List<_Shape> checkIsTrue, Shape shape)
        {
            if(shape == myShape) { checkIsTrue.Add(this); }
        }
    }

    public class _Rectangle : _Shape
    {
        public _Rectangle()
        {
            myShape = new Rectangle();
        }

        public override void ShowFigureDetails()
        {
            Console.WriteLine("This is a: Ellipse, With the measurement: " + "left: " + InkCanvas.GetLeft(myShape) + ", Top: " + InkCanvas.GetTop(myShape) + ", Width: " + myShape.Width + ", Height: " + myShape.Height);
        }
    }

    public class _Ellipse : _Shape
    {
        public _Ellipse()
        {
            myShape = new Ellipse();
        }

        public override void ShowFigureDetails()
        {
            Console.WriteLine("This is a: Ellipse, With the measurement: " + "left: " + InkCanvas.GetLeft(myShape) + ", Top: " + InkCanvas.GetTop(myShape) + ", Width: " + myShape.Width + ", Height: " + myShape.Height);
        }
    }
}

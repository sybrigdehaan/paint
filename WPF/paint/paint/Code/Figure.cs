using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Controls;

namespace paint
{
    public abstract class _Shape
    {
        protected Shape myShape;

        public void Draw(ref InkCanvas MyInkCanvas)
        {
            MyInkCanvas.Children.Add(myShape);
        }

        public void ChangeFigure()
        {
            myShape.Fill = MainWindow.mySolidColorBrushRed;
            myShape.Width = Math.Abs(MainWindow.x2 - MainWindow.x1);
            myShape.Height = Math.Abs(MainWindow.y2 - MainWindow.y1);
            if (MainWindow.x1 < MainWindow.x2)
                InkCanvas.SetLeft(myShape, MainWindow.x1);
            else if (MainWindow.x2 < MainWindow.x1)
                InkCanvas.SetLeft(myShape, MainWindow.x2);
            if (MainWindow.y1 < MainWindow.y2)
                InkCanvas.SetTop(myShape, MainWindow.y1);
            else if (MainWindow.y2 < MainWindow.y1)
                InkCanvas.SetTop(myShape, MainWindow.y2);
        }
    }

    public class MykExtensionRect : IFigures
    {
        public static void Draw(Rectangle rectangle, ref InkCanvas MyInkCanvas)
        {
            MyInkCanvas.Children.Add(rectangle);
        }

        public static void ShowFigureDetails(Shape rectangle)
        {
            Console.WriteLine("This is a: Rectangle, With the measurement: " + "left: " + InkCanvas.GetLeft(rectangle) + ", Top: " + InkCanvas.GetTop(rectangle) + ", Width: " + rectangle.Width + ", Height: " + rectangle.Height);
        }

        public static void ChangeFigure(Rectangle rectangle)
        {
            rectangle.Fill = MainWindow.mySolidColorBrushRed;
            rectangle.Width = Math.Abs(MainWindow.x2 - MainWindow.x1);
            rectangle.Height = Math.Abs(MainWindow.y2 - MainWindow.y1);
            if (MainWindow.x1 < MainWindow.x2)
                InkCanvas.SetLeft(rectangle, MainWindow.x1);
            else if (MainWindow.x2 < MainWindow.x1)
                InkCanvas.SetLeft(rectangle, MainWindow.x2);
            if (MainWindow.y1 < MainWindow.y2)
                InkCanvas.SetTop(rectangle, MainWindow.y1);
            else if (MainWindow.y2 < MainWindow.y1)
                InkCanvas.SetTop(rectangle, MainWindow.y2);
        }
    }

    public class _Ellipse : _Shape, IFigures
    {
        public _Ellipse()
        {
            myShape = new Ellipse();
        }

        public void ShowFigureDetails()
        {
            Console.WriteLine("This is a: Ellipse, With the measurement: " + "left: " + InkCanvas.GetLeft(myShape) + ", Top: " + InkCanvas.GetTop(myShape) + ", Width: " + myShape.Width + ", Height: " + myShape.Height);
        }
    }
}

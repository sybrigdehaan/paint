using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace paint
{
    public class ChangeGroup
    {
        public static void AddTo_Group(_Group myGroup, List<IFigures> selectedFigures)
        {
            Canvas groupCanvas = (myGroup.GetShape() as Canvas);
            groupCanvas.Background = Brushes.Transparent;
            groupCanvas.SizeChanged += new SizeChangedEventHandler(SizeChanged);

            double nearestTop = InkCanvas.GetTop(selectedFigures[0].GetShape()), nearestLeft = InkCanvas.GetLeft(selectedFigures[0].GetShape());
            double farthestRight = 0, farthestBottom = 0;

            foreach (IFigures figures in selectedFigures)
            {
                MyMainGroup.GetInstance().Remove(figures);
                MyInkCanvas.GetInstance().Children.Remove(figures.GetShape());

                InkCanvas ink = MyInkCanvas.GetInstance();
                _Group grop = MyMainGroup.GetInstance();

                myGroup.Add(figures);
                groupCanvas.Children.Add(figures.GetShape());

                InkCanvas ink1 = MyInkCanvas.GetInstance();
                _Group grop1 = MyMainGroup.GetInstance();

                //Checking how the inkcanvas position must be.
                if (InkCanvas.GetLeft(figures.GetShape()) < nearestLeft) { nearestLeft = InkCanvas.GetLeft(figures.GetShape()); }
                if (InkCanvas.GetTop(figures.GetShape()) < nearestTop) { nearestTop = InkCanvas.GetTop(figures.GetShape()); }
            }


            foreach (IFigures figures in selectedFigures)
            {
                FrameworkElement myShape = figures.GetShape();
                //Set the right top and left for the object to set in the inkcanvas
                double myTop = InkCanvas.GetTop(myShape);
                double myLeft = InkCanvas.GetLeft(myShape);
                Canvas.SetTop(myShape, (myTop - nearestTop));
                Canvas.SetLeft(myShape, (myLeft - nearestLeft));

                //Checking which object is the farthest away for the width and height of the inkcanvas.
                if (Canvas.GetLeft(myShape) + myShape.Width > farthestRight) { farthestRight = Canvas.GetLeft(myShape) + myShape.Width; }
                if (Canvas.GetTop(myShape) + myShape.Height > farthestBottom) { farthestBottom = Canvas.GetTop(myShape) + myShape.Height; }
            }

            InkCanvas.SetTop(groupCanvas, nearestTop);
            InkCanvas.SetLeft(groupCanvas, nearestLeft);
            groupCanvas.Width = farthestRight;
            groupCanvas.Height = farthestBottom;

            MyMainGroup.GetInstance().Add(myGroup);
            MyInkCanvas.GetInstance().Children.Add(groupCanvas);
        }

        public static void Un_Group(_Group figure)
        {
            MyMainGroup.GetInstance().Remove(figure);
            MyInkCanvas.GetInstance().Children.Remove(figure.GetShape());

            foreach (IFigures inGroupFigure in figure.SubFigures.ToList())
            {
                (figure.GetShape() as Canvas).Children.Remove(inGroupFigure.GetShape());
                figure.Remove(inGroupFigure);

                FrameworkElement element = inGroupFigure.GetShape();
                double elementTop = Canvas.GetTop(element) + InkCanvas.GetTop(figure.GetShape());
                double elementLeft = Canvas.GetLeft(element) + InkCanvas.GetLeft(figure.GetShape());

                InkCanvas.SetTop(element, elementTop);
                InkCanvas.SetLeft(element, elementLeft);

                MyMainGroup.GetInstance().Add(inGroupFigure);
                MyInkCanvas.GetInstance().Children.Add(element);
            }
        }


        private static void SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //----------------< Canvas_SizeChanged() >----------------
            Canvas canvas = sender as Canvas;
            SizeChangedEventArgs canvas_Changed_Args = e;

            //< check >
            //*if size=0 then initial
            if (canvas_Changed_Args.PreviousSize.Width == 0) return;

            //< init >
            double old_Height = canvas_Changed_Args.PreviousSize.Height;
            double new_Height = canvas_Changed_Args.NewSize.Height;
            double old_Width = canvas_Changed_Args.PreviousSize.Width;
            double new_Width = canvas_Changed_Args.NewSize.Width;

            double scale_Width = new_Width / old_Width;
            double scale_Height = new_Height / old_Height;

            //----< adapt all children >----
            foreach (FrameworkElement element in canvas.Children)
            {
                //< get >
                double old_Left = Canvas.GetLeft(element);
                double old_Top = Canvas.GetTop(element);

                // < set Left-Top>
                Canvas.SetLeft(element, old_Left * scale_Width);
                Canvas.SetTop(element, old_Top * scale_Height);

                //< set Width-Heigth >
                element.Width = element.Width * scale_Width;
                element.Height = element.Height * scale_Height;
            }
        }
    }
}

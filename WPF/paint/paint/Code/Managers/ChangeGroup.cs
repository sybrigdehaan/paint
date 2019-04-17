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
            double nearestTop = InkCanvas.GetTop(selectedFigures[0].GetShape()); 
            double nearestLeft = InkCanvas.GetLeft(selectedFigures[0].GetShape());
            double farthestRight = 0, farthestBottom = 0;

            foreach (IFigures figures in selectedFigures)
            {
                MyMainGroup.GetInstance().Remove(figures);
                myGroup.Add(figures);

                //Checking how the inkcanvas position must be.
                if (InkCanvas.GetLeft(figures.GetShape()) < nearestLeft) { nearestLeft = InkCanvas.GetLeft(figures.GetShape()); }
                if (InkCanvas.GetTop(figures.GetShape()) < nearestTop) { nearestTop = InkCanvas.GetTop(figures.GetShape()); }
            }

            foreach (IFigures figures in selectedFigures)
            {
                FrameworkElement myShape = figures.GetShape();

                //Set the right top and left for the object to set in the inkcanvas
                InkCanvas.SetTop(myShape, (InkCanvas.GetTop(myShape) - nearestTop));
                InkCanvas.SetLeft(myShape, (InkCanvas.GetLeft(myShape) - nearestLeft));

                //Checking which object is the farthest away for the width and height of the inkcanvas.
                if (InkCanvas.GetLeft(myShape) + myShape.Width > farthestRight) { farthestRight = InkCanvas.GetLeft(myShape) + myShape.Width; }
                if (InkCanvas.GetTop(myShape) + myShape.Height > farthestBottom) { farthestBottom = InkCanvas.GetTop(myShape) + myShape.Height; }
            }

            InkCanvas.SetTop(myGroup.GetShape(), nearestTop);
            InkCanvas.SetLeft(myGroup.GetShape(), nearestLeft);
            myGroup.GetShape().Width = farthestRight;
            myGroup.GetShape().Height = farthestBottom;

            MyMainGroup.GetInstance().Add(myGroup);
        }

        public static void Un_Group(_Group figure)
        {
            MyMainGroup.GetInstance().Remove(figure);

            foreach (IFigures inGroupFigure in figure.SubFigures.ToList())
            {
                figure.Remove(inGroupFigure);

                FrameworkElement element = inGroupFigure.GetShape();
                double elementTop = InkCanvas.GetTop(element) + InkCanvas.GetTop(figure.GetShape());
                double elementLeft = InkCanvas.GetLeft(element) + InkCanvas.GetLeft(figure.GetShape());

                InkCanvas.SetTop(element, elementTop);
                InkCanvas.SetLeft(element, elementLeft);

                MyMainGroup.GetInstance().Add(inGroupFigure);
            }
        }


        public static void SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //----------------< Canvas_SizeChanged() >----------------
            InkCanvas canvas = sender as InkCanvas;
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
                double old_Left = InkCanvas.GetLeft(element);
                double old_Top = InkCanvas.GetTop(element);

                // < set Left-Top>
                InkCanvas.SetLeft(element, old_Left * scale_Width);
                InkCanvas.SetTop(element, old_Top * scale_Height);

                //< set Width-Heigth >
                element.Width = element.Width * scale_Width;
                element.Height = element.Height * scale_Height;
            }
        }
    }
}

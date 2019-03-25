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
        public static void AddTo_Group(FrameworkElement[] myArray, ref Group myMainGroup, ref InkCanvas MyInkCanvas)
        {
            Group myGroup = new Group();
            InkCanvas groupInkCanvas = myGroup.groupInkCanvas;

            groupInkCanvas.EditingMode = InkCanvasEditingMode.None;
            groupInkCanvas.Background = Brushes.Transparent;
            groupInkCanvas.SizeChanged += new SizeChangedEventHandler(SizeChanged);

            double nearestTop = InkCanvas.GetTop(myArray[0]), nearestLeft = InkCanvas.GetLeft(myArray[0]);
            double farthestRight = 0, farthestBottom = 0;

            List<IFigures> _ShapesList = new List<IFigures>();

            for (int i = 0; i < myArray.Length; i++)
            {
                myMainGroup.Get_Shape(myArray[i], ref _ShapesList);
                myMainGroup.Remove(_ShapesList[i]);

                MyInkCanvas.Children.Remove(myArray[i]);
                groupInkCanvas.Children.Add(myArray[i]);
                myGroup.Add(_ShapesList[i]);

                //Checking how the inkcanvas position must be. 
                if (InkCanvas.GetLeft(myArray[i]) < nearestLeft) { nearestLeft = InkCanvas.GetLeft(myArray[i]); }
                if (InkCanvas.GetTop(myArray[i]) < nearestTop) { nearestTop = InkCanvas.GetTop(myArray[i]); }
            }

            foreach (FrameworkElement myShape in myArray)
            {
                //Set the right top and left for the object to set in the inkcanvas
                double myTop = InkCanvas.GetTop(myShape);
                double myLeft = InkCanvas.GetLeft(myShape);
                InkCanvas.SetTop(myShape, (myTop - nearestTop));
                InkCanvas.SetLeft(myShape, (myLeft - nearestLeft));

                //Checking which object is the farthest away for the width and height of the inkcanvas. 
                if (InkCanvas.GetLeft(myShape) + myShape.Width > farthestRight) { farthestRight = InkCanvas.GetLeft(myShape) + myShape.Width; }
                if (InkCanvas.GetTop(myShape) + myShape.Height > farthestBottom) { farthestBottom = InkCanvas.GetTop(myShape) + myShape.Height; }
            }

            InkCanvas.SetTop(groupInkCanvas, nearestTop);
            InkCanvas.SetLeft(groupInkCanvas, nearestLeft);
            groupInkCanvas.Width = farthestRight;
            groupInkCanvas.Height = farthestBottom;

            myMainGroup.Add(myGroup);
            MyInkCanvas.Children.Add(groupInkCanvas);
        }

        //Select haalt een frameworkelement op uit de canvas die in myarray staat
        //Deze frameworkelement moet je opzoeken in mymaincanvas, je moet dan door alle custum objecten in de mymaincanvas en check of het inkcanvas object hetzelfde is dat geselecteert is 
        //Als dit zou is return dan het custum object. Kijk in dit custum object naar wat voor sub custum objecten hij heeft en zet deze in een list (dit kunnen custom shapes en of custom groups zijn).
        //Verwijder de custum object uit de myMainCanvas en zet de andere custum objecten er weer in. 

        public static void Un_Group(FrameworkElement[] myArray, ref Group myMainGroup, ref InkCanvas MyInkCanvas)
        {
            if (myArray.Length == 1 && myArray[0].GetType() == typeof(InkCanvas))
            {
                Group selectedFrameworkGroup = new Group(); //The selected framework custum group
                List<IFigures> inGroupList = new List<IFigures>(); //The selected framework custum group subFigures

                myMainGroup.Find(myArray[0], ref selectedFrameworkGroup);
                myMainGroup.Remove(selectedFrameworkGroup);
                MyInkCanvas.Children.Remove(myArray[0]);
                inGroupList = selectedFrameworkGroup.SubFigures;

                foreach (IFigures figure in inGroupList)
                {
                    (myArray[0] as InkCanvas).Children.Remove(figure.GetShape());

                    FrameworkElement element = figure.GetShape();
                    double elementTop = InkCanvas.GetTop(element) + InkCanvas.GetTop(myArray[0]);
                    double elementLeft = InkCanvas.GetLeft(element) + InkCanvas.GetLeft(myArray[0]);

                    InkCanvas.SetTop(element, elementTop);
                    InkCanvas.SetLeft(element, elementLeft);

                    myMainGroup.Add(figure);
                    MyInkCanvas.Children.Add(element);
                }
            } else {
                MessageBox.Show("Je hebt meer dan 1 object of geen groep object geselecteerd!"); 
            }
        }

        private static void SizeChanged(object sender, SizeChangedEventArgs e)
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

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

            double top = InkCanvas.GetTop(myArray[0]);
            double left = InkCanvas.GetLeft(myArray[0]);
            double farthestLeft = 0;
            double farthestTop = 0;

            FrameworkElement shapeTop = myArray[0], shapeLeft = myArray[0], shapeFarthestLeft = myArray[0], shapeFarthestTop = myArray[0];

            groupInkCanvas.SizeChanged += new SizeChangedEventHandler(SizeChanged);
            List<IFigures> _ShapesList = new List<IFigures>();
            List<IFigures> _SubListSelected = new List<IFigures>();

            for (int i = 0; i < myArray.Length; i++)
            {
                myMainGroup.Get_Shape(myArray[i], ref _ShapesList);
                myMainGroup.Find(_ShapesList[i], ref _SubListSelected);
                myMainGroup.Remove(_SubListSelected[i]);

                MyInkCanvas.Children.Remove(myArray[i]);
                groupInkCanvas.Children.Add(myArray[i]);
                myGroup.Add(_ShapesList[i]);

                //Checking how the inkcanvas group position must be. 
                if (InkCanvas.GetLeft(myArray[i]) < left) { left = InkCanvas.GetLeft(myArray[i]); shapeLeft = myArray[i]; }
                if (InkCanvas.GetTop(myArray[i]) < top) { top = InkCanvas.GetTop(myArray[i]); shapeTop = myArray[i]; }
            }

            foreach (FrameworkElement myShape in myArray)
            {
                double myTop = InkCanvas.GetTop(myShape);
                double myLeft = InkCanvas.GetLeft(myShape);
                InkCanvas.SetTop(myShape, (myTop - top));
                InkCanvas.SetLeft(myShape, (myLeft - left));

                //Checking which object is the farthest away. 
                if (InkCanvas.GetLeft(myShape) > farthestLeft) { farthestLeft = InkCanvas.GetLeft(myShape); shapeFarthestLeft = myShape; }
                if (InkCanvas.GetTop(myShape) > farthestTop) { farthestTop = InkCanvas.GetTop(myShape); shapeFarthestTop = myShape; }
            }

            InkCanvas.SetTop(groupInkCanvas, top);
            InkCanvas.SetLeft(groupInkCanvas, left);
            groupInkCanvas.Width = farthestLeft + shapeFarthestLeft.Width;
            groupInkCanvas.Height = farthestTop + shapeFarthestTop.Height;

            myMainGroup.Add(myGroup);
            MyInkCanvas.Children.Add(groupInkCanvas);
        }

        public static void Un_Group(FrameworkElement[] myArray, ref Group myMainGroup, ref InkCanvas MyInkCanvas)
        {
            List<Group> frameworkList = new List<Group>(); //The selected framework custum group
            List<IFigures> inGroupList = new List<IFigures>(); //The selected framework custum group subFigures
            List<FrameworkElement> shapesList = new List<FrameworkElement>(); //The subfigures there FrameworkElements

            for (int i = 0; i < myArray.Length; i++)
            {
                myMainGroup.Find(myArray[i], ref frameworkList);
                myMainGroup.Remove(frameworkList[i]);
                inGroupList = frameworkList[i].SubFigures;
            }

            foreach (IFigures figure in inGroupList)
            {
                myMainGroup.Add(figure);
                MyInkCanvas.Children.Add(figure.GetShape());
            }
        }

        //Select haalt een frameworkelement op uit de canvas die in myarray staat
        //Deze frameworkelement moet je opzoeken in mymaincanvas, je moet dan door alle custum objecten in de mymaincanvas en check of het inkcanvas object hetzelfde is dat geselecteert is 
        //Als dit zou is return dan het custum object. Kijk in dit custum object /// naar wat voor sub custum objecten hij heeft en zet deze in een list (dit kunnen custom shapes en of custom groups zijn).
        //Verwijder de custum object uit de myMainCanvas en zet de andere custum objecten er weer in. 

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

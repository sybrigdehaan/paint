﻿using System;
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
        public static void AddTo_Group(FrameworkElement[] myArray, ref _Group myMainGroup, UndoRedoManager undoRedoManager)
        {
            _Group myGroup = new _Group();
            Canvas groupCanvas = myGroup.groupCanvas;
            
            groupCanvas.Background = Brushes.Transparent;
            groupCanvas.SizeChanged += new SizeChangedEventHandler(SizeChanged);
            
            double nearestTop = InkCanvas.GetTop(myArray[0]), nearestLeft = InkCanvas.GetLeft(myArray[0]);
            double farthestRight = 0, farthestBottom = 0;

            List<IFigures> _ShapesList = new List<IFigures>();
            myMainGroup.Get_Shape(ref _ShapesList);
            for (int i = 0; i < myArray.Length; i++)
            {
                IFigures selectedFigure = null;
                foreach (IFigures figure in _ShapesList)
                {
                    if (figure.GetShape() == myArray[i])
                        selectedFigure = figure;
                }

                myMainGroup.Remove(selectedFigure);
                Singleton.GetInstance().Children.Remove(selectedFigure.GetShape()); 
                groupCanvas.Children.Add(selectedFigure.GetShape()); 
                myGroup.Add(selectedFigure);

                //Checking how the inkcanvas position must be. 
                if (InkCanvas.GetLeft(myArray[i]) < nearestLeft) { nearestLeft = InkCanvas.GetLeft(myArray[i]); }
                if (InkCanvas.GetTop(myArray[i]) < nearestTop) { nearestTop = InkCanvas.GetTop(myArray[i]); }
            }

            foreach (FrameworkElement myShape in myArray)
            {
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

            SimpleRemoteControl remote = new SimpleRemoteControl { SetCommand = new _MakeGroup(myGroup, myMainGroup) };
            remote.buttonWasPressed();
            undoRedoManager.AddToRedo(remote);

            SimpleRemoteControl reverseRemote = new SimpleRemoteControl { SetCommand = new _DestroyGroup(myGroup, myMainGroup) };
            undoRedoManager.AddToUndo(reverseRemote);
        }

        public static void Un_Group(FrameworkElement[] myArray, ref _Group myMainGroup, UndoRedoManager undoRedoManager)
        {
            if (myArray.Length == 1 && myArray[0].GetType() == typeof(Canvas))
            {
                _Group selectedFrameworkGroup = new _Group(); //The selected framework custum group
                List<IFigures> inGroupList = new List<IFigures>(); //The selected framework custum group subFigures

                foreach (IFigures figure in myMainGroup.SubFigures)
                {
                    if (typeof(_Group) == figure.GetType())
                    {
                        if ((figure as _Group).groupCanvas == myArray[0])
                            selectedFrameworkGroup = ((figure as _Group));
                    }
                }

                SimpleRemoteControl remote = new SimpleRemoteControl { SetCommand = new _DestroyGroup(selectedFrameworkGroup, myMainGroup) };
                remote.buttonWasPressed();
                undoRedoManager.AddToUndo(remote);

                SimpleRemoteControl reverseRemote = new SimpleRemoteControl { SetCommand = new _MakeGroup(selectedFrameworkGroup, myMainGroup) };
                undoRedoManager.AddToRedo(reverseRemote);


                foreach (IFigures figure in selectedFrameworkGroup.SubFigures)
                {
                    (myArray[0] as Canvas).Children.Remove(figure.GetShape());

                    FrameworkElement element = figure.GetShape();
                    double elementTop = Canvas.GetTop(element) + InkCanvas.GetTop(myArray[0]);
                    double elementLeft = Canvas.GetLeft(element) + InkCanvas.GetLeft(myArray[0]);

                    InkCanvas.SetTop(element, elementTop);
                    InkCanvas.SetLeft(element, elementLeft);
                }
            }
            else
            {
                MessageBox.Show("Je hebt meer dan 1 object of geen groep object geselecteerd!");
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

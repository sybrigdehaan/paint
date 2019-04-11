using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Windows.Ink;

namespace paint
{
    public partial class MainWindow : Window
    {
        public static double x1, y1, x2, y2;
        public static SolidColorBrush mySolidColorBrushRed = new SolidColorBrush();

        private Items currentItem;
        bool drawn = false;

        InkCanvas MyInkCanvas = Singleton.GetInstance();

        SimpleRemoteControl remote = new SimpleRemoteControl();
        List<SimpleRemoteControl> reverseRemoteControls = new List<SimpleRemoteControl>();
        List<SimpleRemoteControl> remoteControls = new List<SimpleRemoteControl>();
        _Group myMainGroup = new _Group();
        _Ellipse myEllipse;
        _Rectangle myRectangle;

        public MainWindow()
        {
            InitializeComponent();
            myGrid.Children.Add(MyInkCanvas);
            MyInkCanvas.EditingMode = InkCanvasEditingMode.None;
            mySolidColorBrushRed.Color = Color.FromArgb(255, 255, 0, 0);
        }

        public enum Items { None, OpenFile, SaveFile, DeleteGroup, AddGroup, Select, Eraser, Ornament, Rectangle, Ellipse }

        private void Button_MakeFigure_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MyInkCanvas.EditingMode = InkCanvasEditingMode.None;
            switch (((FrameworkElement)sender).Name)
            {
                case "Rectangle":
                    currentItem = Items.Rectangle;
                    break;
                case "Ellipse":
                    currentItem = Items.Ellipse;
                    break;
            }
        }

        private void Button_ChangeFile_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            switch (((FrameworkElement)sender).Name)
            {
                case "OpenFile":
                    break;
                case "Save":
                    break;
                case "Redo":
                    remote = remoteControls[remoteControls.Count - 1];
                    remote.buttonWasPressed();
                    remoteControls.Remove(remote);
                    break;
                case "Undo":
                    remote = reverseRemoteControls[reverseRemoteControls.Count - 1];
                    remote.buttonWasPressed();
                    reverseRemoteControls.Remove(remote);
                    break;
            }
        }

        private void Button_ChangeFigure_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            currentItem = Items.None;
            FrameworkElement[] myArray = new FrameworkElement[MyInkCanvas.GetSelectedElements().Count];
            MyInkCanvas.GetSelectedElements().CopyTo(myArray, 0);

            switch (((FrameworkElement)sender).Name)
            {
                case "Delete_Group":
                    ChangeGroup.Un_Group(myArray, ref myMainGroup);
                    break;
                case "Add_Group":
                    ChangeGroup.AddTo_Group(myArray, ref myMainGroup);
                    break;
                case "Select":
                    MyInkCanvas.EditingMode = InkCanvasEditingMode.Select;
                    break;
                case "Eraser":
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
                        MyInkCanvas.Children.Remove(myArray[i]);
                        myMainGroup.Remove(selectedFigure);
                    }
                    break;
            }
        }

        private void Button_AddToFigure_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            // haalt alle geslecteerde items op en zet het in array Shape
            FrameworkElement[] myArray = new FrameworkElement[MyInkCanvas.GetSelectedElements().Count];
            MyInkCanvas.GetSelectedElements().CopyTo(myArray, 0);

            List<IFigures> _ShapesList = new List<IFigures>();
            myMainGroup.Get_Shape(ref _ShapesList);

            IFigures selectedFigure = null;
            foreach (IFigures figure in _ShapesList)
            {
                if (figure.GetShape() == myArray[0])
                    selectedFigure = figure;
            }

            // haalt naam op van de het aangeklikte element
            switch (((FrameworkElement)sender).Name)
            {
                case "OrnamentRight":
                    double ornamentRightLeft = InkCanvas.GetLeft(myArray[0]);
                    double ornamentRightTop = InkCanvas.GetTop(myArray[0]);
                    double OrnamentRightHeigth = myArray[0].Height / 2;
                    double OrnamentRightWidht = myArray[0].Width;
                    Right ornamentRight = new Right("ornament", (ornamentRightLeft + OrnamentRightWidht), (OrnamentRightHeigth + ornamentRightTop));
                    ornamentRight.Add(ref MyInkCanvas, selectedFigure);
                    break;

                case "OrnamentLeft":
                    double OrnamentLeftLeft = InkCanvas.GetLeft(myArray[0]);
                    double OrnamentLeftTop = InkCanvas.GetTop(myArray[0]);
                    double OrnamentLeftHeigth = myArray[0].Height / 2;
                    Left ornamentLeft = new Left("ornament", OrnamentLeftLeft, (OrnamentLeftHeigth + OrnamentLeftTop));
                    ornamentLeft.Add(ref MyInkCanvas, selectedFigure);
                    break;

                case "OrnamentTop":
                    double OrnamentTopRight = InkCanvas.GetLeft(myArray[0]);
                    double ornamentTopTop = InkCanvas.GetTop(myArray[0]);
                    double OrnamentTopWidth = myArray[0].Width / 2;
                    Top ornamenTtop = new Top("ornament", (OrnamentTopWidth + OrnamentTopRight), ornamentTopTop);
                    ornamenTtop.Add(ref MyInkCanvas, selectedFigure);
                    break;

                case "OrnamentBottom":
                    double OrnamentBottomRight = InkCanvas.GetLeft(myArray[0]);
                    double ornamentBottomTop = InkCanvas.GetTop(myArray[0]);
                    double OrnamentBottomWidth = myArray[0].Width / 2;
                    double OrnamentBottomHeight = myArray[0].Height;
                    Bottom OrnamentBottom = new Bottom("ornament", (ornamentBottomTop + OrnamentBottomWidth), (ornamentBottomTop + OrnamentBottomHeight));
                    OrnamentBottom.Add(ref MyInkCanvas, selectedFigure);
                    break;
            }
        }

        private void InkCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            drawn = true;
            Point p = e.GetPosition(MyInkCanvas);
            x1 = p.X; y1 = p.Y;

            switch (currentItem)
            {
                case Items.Rectangle:
                    myRectangle = new _Rectangle();


                    remote = new SimpleRemoteControl();
                    remote.SetCommand = new _RectagleDestroy(myRectangle);
                    reverseRemoteControls.Add(remote);

                    MyInkCanvas.Children.Add(myRectangle.GetShape());
                    break;
                case Items.Ellipse:
                    myEllipse = new _Ellipse();
                    remote.SetCommand = new _EllipseMake(myEllipse);
                    remote.buttonWasPressed();
                    remoteControls.Add(remote);
                    MyInkCanvas.Children.Add(myEllipse.GetShape());
                    break;
            }
        }

        private void InkCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            drawn = false;
            switch (currentItem)
            {
                case Items.Rectangle:
                    myMainGroup.Add(myRectangle);
                    remote = new SimpleRemoteControl();
                    remote.SetCommand = new _RectangleMake(myRectangle);
                    remoteControls.Add(remote);
                    break;
                case Items.Ellipse:
                    myMainGroup.Add(myEllipse);
                    break;
            }
        }

        private void InkCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            Point p = e.GetPosition(MyInkCanvas);
            x2 = p.X; y2 = p.Y;

            if (drawn)
            {
                switch (currentItem)
                {
                    case Items.Rectangle:
                        myRectangle.ChangeFigure(x1, x2, y1, y2);
                        break;
                    case Items.Ellipse:
                        myEllipse.ChangeFigure(x1, x2, y1, y2);
                        break;
                }
            }
        }
    }

}

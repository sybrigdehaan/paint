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
        
        List<_Shape> checkIsTrue; 
        Group myMainGroup = new Group();
        _Ellipse myEllipse;
        _Rectangle myRectangle;

        public MainWindow()
        {
            InitializeComponent();
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

        private void Button_ChangeFigure_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            currentItem = Items.None;
            FrameworkElement[] myArray = new FrameworkElement[MyInkCanvas.GetSelectedElements().Count];
            MyInkCanvas.GetSelectedElements().CopyTo(myArray, 0);

            switch (((FrameworkElement)sender).Name)
            {
                case "Delete_Group":
                    ChangeGroup.Un_Group(myArray, ref myMainGroup, ref MyInkCanvas);
                    break;
                case "Add_Group":
                    ChangeGroup.AddTo_Group(myArray, ref myMainGroup, ref MyInkCanvas);
                    break;
                case "Select":
                    MyInkCanvas.EditingMode = InkCanvasEditingMode.Select;
                    break;
                case "Eraser":
                    List<IFigures> _ShapesList = new List<IFigures>();
                    for (int i = 0; i < myArray.Length; i++)
                    {
                        MyInkCanvas.Children.Remove(myArray[i]);
                        myMainGroup.Get_Shape(myArray[i], ref _ShapesList);
                        myMainGroup.Remove(_ShapesList[i]);
                    }
                    break;
            }
        }

        private void Button_AddToFigure_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            UIElement[] myArray = new UIElement[MyInkCanvas.GetSelectedElements().Count];
            MyInkCanvas.GetSelectedElements().CopyTo(myArray, 0);

            switch (currentItem)
            {
                case Items.Select:
                    break;

                    //for (int i = 0; i < myArray.Length; i++)
                    //{

                    //}
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var color = Color.FromArgb(0, 0, 0, 0);
            TextBlock textBlock = new TextBlock();
            textBlock.Text = "ornament";
            InkCanvas.SetLeft(textBlock, 100);
            InkCanvas.SetTop(textBlock, 100);
            
        }

        private void Button_AddToFigure_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            // haalt alle geslecteerde items op en zet het in array Shape
            FrameworkElement[] myArray = new FrameworkElement[MyInkCanvas.GetSelectedElements().Count];
            MyInkCanvas.GetSelectedElements().CopyTo(myArray, 0);

            // haalt naam op van de het aangeklikte element
            switch (((FrameworkElement)sender).Name)
            {
                case "OrnamentRight":
                    double ornamentRightLeft = InkCanvas.GetLeft(myArray[0]);
                    double ornamentRightTop = InkCanvas.GetTop(myArray[0]);
                    double OrnamentRightHeigth = myArray[0].Height / 2;
                    double OrnamentRightWidht = myArray[0].Width;
                    Right ornamentRight = new Right("ornament", (ornamentRightLeft + OrnamentRightWidht), (OrnamentRightHeigth + ornamentRightTop));
                    ornamentRight.Add(ref MyInkCanvas);
                    break;

                case "OrnamentLeft":
                    double OrnamentLeftLeft = InkCanvas.GetLeft(myArray[0]);
                    double OrnamentLeftTop = InkCanvas.GetTop(myArray[0]);
                    double OrnamentLeftHeigth = myArray[0].Height / 2;
                    Left ornamentLeft = new Left("ornament", OrnamentLeftLeft, (OrnamentLeftHeigth + OrnamentLeftTop));
                    ornamentLeft.Add(ref MyInkCanvas);
                    break;

                case "OrnamentTop":
                    double OrnamentTopRight = InkCanvas.GetLeft(myArray[0]);
                    double ornamentTopTop = InkCanvas.GetTop(myArray[0]);
                    double OrnamentTopWidth = myArray[0].Width /2;
                    Top ornamenTtop = new Top("ornament", (OrnamentTopWidth + OrnamentTopRight), ornamentTopTop);
                    ornamenTtop.Add(ref MyInkCanvas);
                    break;

                case "OrnamentBottom":
                    double OrnamentBottomRight = InkCanvas.GetLeft(myArray[0]);
                    double ornamentBottomTop = InkCanvas.GetTop(myArray[0]);
                    double OrnamentBottomWidth = myArray[0].Width / 2;
                    double OrnamentBottomHeight = myArray[0].Height;
                    Bottom OrnamentBottom = new Bottom("ornament", (ornamentBottomTop + OrnamentBottomWidth), (ornamentBottomTop + OrnamentBottomHeight));
                    OrnamentBottom.Add(ref MyInkCanvas);
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
                    myRectangle.Draw(ref MyInkCanvas);
                    break;
                case Items.Ellipse:
                    myEllipse = new _Ellipse();
                    myEllipse.Draw(ref MyInkCanvas);
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

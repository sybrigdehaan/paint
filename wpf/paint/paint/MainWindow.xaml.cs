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
                    for (int i = 0; i < myArray.Length; i++)
                        MyInkCanvas.Children.Remove(myArray[i]);
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

                    //for (int i = 0; i < myArray.Length; i++)
                    //{

                    //}

                    //.Add(new TextBlock { Text = "ornament" });

                    var color = Color.FromArgb(0, 0, 0, 0);
                    TextBlock textBlock = new TextBlock();
                    textBlock.Text = "ornament";
                    InkCanvas.SetLeft(textBlock, 100);
                    InkCanvas.SetTop(textBlock, 100);
                    textBlock.Margin = new Thickness(100, 100, 0, 0);
                    MyInkCanvas.Children.Add(textBlock);
                    break;
            }

            Left ornament = new Left();

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

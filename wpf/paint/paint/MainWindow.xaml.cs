using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace paint
{
    public partial class MainWindow : Window
    {
        public static double x1, y1, x2, y2;
        public static SolidColorBrush mySolidColorBrushRed = new SolidColorBrush();

        private Items currentItem;
        bool drawn = false; 
        
        _Ellipse myEllipse;
        _Rectangle myRectangle; 
 

        public MainWindow()
        {
            InitializeComponent();
            mySolidColorBrushRed.Color = Color.FromArgb(255, 255, 0, 0);
        }

        public enum Items { None, OpenFile, SaveFile, DeleteGroup, AddGroup, Select, Eraser, Rectangle, Ellipse }

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
            switch (((FrameworkElement)sender).Name)
            {
                case "Delete_Group":
                    currentItem = Items.DeleteGroup;
                    break;
                case "Add_Group":
                    currentItem = Items.AddGroup;
                    break;
                case "Select":
                    MyInkCanvas.EditingMode = InkCanvasEditingMode.Select;
                    break;
                case "Eraser":
                    UIElement[] myArray = new UIElement[MyInkCanvas.GetSelectedElements().Count];
                    MyInkCanvas.GetSelectedElements().CopyTo(myArray, 0);

                    for (int i = 0; i < myArray.Length; i++)
                    {
                        if(MyInkCanvas.Children.Contains(myArray[i]))
                        {
                            MyInkCanvas.Children.Remove(myArray[i]); 
                        }
                    }
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
                        myRectangle.ChangeFigure(); 
                        break;
                    case Items.Ellipse:
                        myEllipse.ChangeFigure();
                        break;
                }
            }
        }
    }
}

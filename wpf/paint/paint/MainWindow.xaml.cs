using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace paint
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Items currentItem;
        private Figure myFigure;
        bool drawn = false; 
        double x1, y1, x2, y2;
        SolidColorBrush mySolidColorBrushYellow = new SolidColorBrush();
        SolidColorBrush mySolidColorBrushRed = new SolidColorBrush();
        Ellipse myEllipse;
        Rectangle myRectangle; 
 

        public MainWindow()
        {
            InitializeComponent();
            mySolidColorBrushYellow.Color = Color.FromArgb(255, 0, 0, 255);
            mySolidColorBrushRed.Color = Color.FromArgb(255, 255, 0, 0);
        }

        public enum Items { OpenFile, SaveFile, DeleteGroup, AddGroup, Select, Rectangle, Ellipse };
        
        private void changeFigure(Shape figure)
        {
            figure.Width = Math.Abs(x2 - x1);
            figure.Height = Math.Abs(y2 - y1);
            if(x1 < x2)
                 InkCanvas.SetLeft(figure, x1);
            else if(x2 < x1)
                 InkCanvas.SetLeft(figure, x2);
            if(y1 < y2)
                InkCanvas.SetTop(figure, y1);
            else if(y2 < y1)
                InkCanvas.SetTop(figure, y2);
        }

        private void Button_MakeFigure_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
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

        private void InkCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            drawn = true;
            Point p = e.GetPosition(MyInkCanvas);
            x1 = p.X; y1 = p.Y;

            switch (currentItem)
            {
                case Items.Rectangle:
                    myRectangle = new Rectangle();
                    MyInkCanvas.Children.Add(myRectangle);
                    break;
                case Items.Ellipse:
                    myEllipse = new Ellipse();
                    MyInkCanvas.Children.Add(myEllipse);
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
                        changeFigure(myRectangle);
                        myRectangle.Fill = mySolidColorBrushRed;
                        break;
                    case Items.Ellipse:
                        changeFigure(myEllipse);
                        myEllipse.Fill = mySolidColorBrushYellow;
                        break;
                }
            }
        }
    }
}

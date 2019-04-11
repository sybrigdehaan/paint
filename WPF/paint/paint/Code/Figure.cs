using System;
using System.Windows; 
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Controls;
using System.Collections.Generic; 

namespace paint
{
    public abstract class _Shape : IFigures
    {
        protected Shape myShape;
        protected List<Ornament> myOrnament;

        public FrameworkElement GetShape(){ return myShape;  }

        public Ornament _Ornament { set { myOrnament.Add(value); } }

        public virtual void ShowFigureDetails() { }

        public void ChangeFigure(double x1, double x2, double y1, double y2)
        {
            myShape.Fill = MainWindow.mySolidColorBrushRed;
            myShape.Width = Math.Abs(x2 - x1);
            myShape.Height = Math.Abs(y2 - y1);
            if (x1 < x2) InkCanvas.SetLeft(myShape, x1); else InkCanvas.SetLeft(myShape, x2);
            if (y1 < y2) InkCanvas.SetTop(myShape, y1); else InkCanvas.SetTop(myShape, y2);
        }

        public void Get_Shape(ref List<IFigures> _ShapesList)
        {
            _ShapesList.Add(this);
        }
    }

    public class _Rectangle : _Shape
    {
        public _Rectangle()
        {
            myShape = new Rectangle();
        }

        public void Make()
        {
            Singleton.GetInstance().Children.Add(myShape);
        }

        public void Destroy()
        {
            Singleton.GetInstance().Children.Remove(myShape); 
        }

        public override void ShowFigureDetails()
        {
            Console.WriteLine("This is a: Ellipse, With the measurement: " + "left: " + InkCanvas.GetLeft(myShape) + ", Top: " + InkCanvas.GetTop(myShape) + ", Width: " + myShape.Width + ", Height: " + myShape.Height);
        }
    }

    public class _RectangleMake : ICommand
    {
        private _Rectangle _myShape; 
        public _RectangleMake(_Rectangle _myShape)
        {
            this._myShape = _myShape; 
        }

        public void Execute()
        {
            _myShape.Make(); 
        }
    }

    public class _RectagleDestroy : ICommand
    {
        private _Rectangle _myShape;
        public _RectagleDestroy(_Rectangle _myShape)
        {
            this._myShape = _myShape;
        }

        public void Execute()
        {
            _myShape.Destroy();
        }
    }


    public class _Ellipse : _Shape
    {
        public void Make()
        {
            myShape = new Ellipse();
        }

        public void Destroy()
        {
            myShape = null;
        }

        public override void ShowFigureDetails()
        {
            Console.WriteLine("This is a: Ellipse, With the measurement: " + "left: " + InkCanvas.GetLeft(myShape) + ", Top: " + InkCanvas.GetTop(myShape) + ", Width: " + myShape.Width + ", Height: " + myShape.Height);
        }
    }

    public class _EllipseMake : ICommand
    {
        private _Ellipse _myShape;
        public _EllipseMake(_Ellipse _myShape)
        {
            this._myShape = _myShape;
        }

        public void Execute()
        {
            _myShape.Make();
        }
    }

    public class _EllipseDestroy : ICommand
    {
        private _Ellipse _myShape;
        public _EllipseDestroy(_Ellipse _myShape)
        {
            this._myShape = _myShape;
        }

        public void Execute()
        {
            _myShape.Destroy();
        }
    }

    
}

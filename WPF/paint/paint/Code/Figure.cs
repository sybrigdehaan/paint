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

        public void Make(_Group group)
        {
            group.Add(this); 
            Singleton.GetInstance().Children.Add(myShape);
        }

        public void Destroy(_Group group)
        {
            group.Remove(this); 
            Singleton.GetInstance().Children.Remove(myShape);
        }
    }

    public class _Rectangle : _Shape
    {
        public _Rectangle()
        {
            myShape = new Rectangle();
        }

        public override void ShowFigureDetails()
        {
            Console.WriteLine("This is a: Ellipse, With the measurement: " + "left: " + InkCanvas.GetLeft(myShape) + ", Top: " + InkCanvas.GetTop(myShape) + ", Width: " + myShape.Width + ", Height: " + myShape.Height);
        }
    }

    public class _Ellipse : _Shape
    {
        public _Ellipse()
        {
            myShape = new Ellipse();
        }

        public override void ShowFigureDetails()
        {
            Console.WriteLine("This is a: Ellipse, With the measurement: " + "left: " + InkCanvas.GetLeft(myShape) + ", Top: " + InkCanvas.GetTop(myShape) + ", Width: " + myShape.Width + ", Height: " + myShape.Height);
        }
    }

    public class _MakeShape : ICommand
    {
        private IFigures _myShape;
        private _Group group; 
        public _MakeShape(IFigures _myShape, _Group group)
        {
            this._myShape = _myShape;
            this.group = group; 
        }

        public void Execute()
        {
            _myShape.Make(group);
        }
    }

    public class _DestroyShape : ICommand
    {
        private IFigures _myShape;
        private _Group group;
        public _DestroyShape(IFigures _myShape, _Group group)
        {
            this._myShape = _myShape;
            this.group = group;
        }

        public void Execute()
        {
            _myShape.Destroy(group);
        }
    }
}

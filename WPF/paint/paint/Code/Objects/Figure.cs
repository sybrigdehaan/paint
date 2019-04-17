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
        protected int depthInList; 

        public int GetDepthInList() { return depthInList; }
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

        public void SetColor()
        {
            myShape.Fill = MainWindow.mySolidColorBrushRed;
        }

        public void DepthInList(int depthInList)
        {
            this.depthInList = depthInList;
        }

        public void Accept(IWriteToFileVisitor visitor)
        {
            visitor.Visit(this);
        }

        public void Make()
        {
            MainWindow.addRemoveVisitor.AddVisit(this); 
        }
        
        public void Destroy()
        {
            MainWindow.addRemoveVisitor.RemoveVisit(this); 
        }
    }

    public class _Rectangle : _Shape
    {
        public _Rectangle()
        {
            myShape = new Rectangle();
        }
    }

    public class _Ellipse : _Shape
    {
        public _Ellipse()
        {
            myShape = new Ellipse();
        }
    }

    public class _MakeShape : ICommand
    {
        private IFigures _myShape;
        public _MakeShape(IFigures _myShape)
        {
            this._myShape = _myShape;
        }

        public void Execute()
        {
            _myShape.Make();
        }
    }

    public class _DestroyShape : ICommand
    {
        private IFigures _myShape;
        public _DestroyShape(IFigures _myShape)
        {
            this._myShape = _myShape;
        }

        public void Execute()
        {
            _myShape.Destroy();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows;

namespace paint
{
    public class _Group : IFigures
    {
        public Canvas groupInkCanvas = new Canvas();

        public FrameworkElement GetShape () {return groupInkCanvas; }

        public List<IFigures> SubFigures { get; } = new List<IFigures>();

        public void Make(ICustomObjectVisitor customObject)
        {
            customObject.VisitMake(this);
        }

        public void Destroy(ICustomObjectVisitor customObject)
        {
            customObject.VisitMake(this);
        }

        public void Add(IFigures figure)
        {
            SubFigures.Add(figure);
        }

        public void Remove(IFigures figure)
        {
            SubFigures.Remove(figure);
        }

        public void ShowFigureDetails()
        {
            Console.WriteLine("Group");
            foreach (IFigures fig in SubFigures)
            {
                fig.ShowFigureDetails();
            }
        }
        
        public void Get_Shape(ref List<IFigures> _ShapesList)
        {
            _ShapesList.Add(this); 
            foreach (IFigures fig in SubFigures)
            {
                fig.Get_Shape(ref _ShapesList); 
            }
        }


    }
}

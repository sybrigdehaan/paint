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
        public InkCanvas groupInkCanvas = new InkCanvas();
        private List<IFigures> subFigures = new List<IFigures>();

        public FrameworkElement GetShape () {return groupInkCanvas; } 

        public List<IFigures> SubFigures { get { return subFigures; } }

        public void Add(IFigures figure)
        {
            subFigures.Add(figure);
        }

        public void Remove(IFigures figure)
        {
            subFigures.Remove(figure);
        }

        public void ShowFigureDetails()
        {
            Console.WriteLine("Group");
            foreach (IFigures fig in subFigures)
            {
                fig.ShowFigureDetails();
            }
        }
        
        public void Get_Shape(ref List<IFigures> _ShapesList)
        {
            _ShapesList.Add(this); 
            foreach (IFigures fig in subFigures)
            {
                fig.Get_Shape(ref _ShapesList); 
            }
        }
    }
}

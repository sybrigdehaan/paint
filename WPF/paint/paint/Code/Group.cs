using System;
using System.Collections.Generic;
using System.Windows.Shapes;
using System.Windows.Controls;

namespace paint
{
    public class Group : IFigures
    {
        private Grid grid = new Grid(); 
        private List<IFigures> subFigures = new List<IFigures>();

        public void AddToInkCanvas(InkCanvas myInkCanvas)
        {
            myInkCanvas.Children.Add(grid); 
        }

        public void Add(IFigures group)
        {
            subFigures.Add(group);
        }

        public void Remove(IFigures group)
        {
            subFigures.Remove(group);
        }

        public void ShowFigureDetails()
        {
            Console.WriteLine("Group");
            foreach (IFigures fig in subFigures)
            {
                fig.ShowFigureDetails();
            }
        }

        public void CheckShape(ref List<_Shape> checkIsTrue, Shape shape)
        {
            foreach (IFigures fig in subFigures)
            {
                fig.CheckShape(ref checkIsTrue, shape); 
            }
        }
    }
}

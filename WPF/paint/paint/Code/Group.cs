using System;
using System.Collections.Generic;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows;

namespace paint
{
    public class Group : IFigures
    {
        public InkCanvas groupInkCanvas = new InkCanvas();
        private List<IFigures> subFigures = new List<IFigures>();

        public FrameworkElement GetShape () {return groupInkCanvas; } 

        public List<IFigures> SubFigures { get { return subFigures; } }

        public void Add(IFigures group)
        {
            subFigures.Add(group);
        }

        public void Remove(IFigures group)
        {
            subFigures.Remove(group);
        }
        
        //Is used for ungroup
        public void Find(FrameworkElement element, ref Group selectedFrameworkGroup)
        {
            foreach (IFigures fig in subFigures)
            {
                if (typeof(Group) == fig.GetType())
                {
                    if ((fig as Group).groupInkCanvas == element)
                        selectedFrameworkGroup = ((fig as Group));
                }
            }
        }

        public void ShowFigureDetails()
        {
            Console.WriteLine("Group");
            foreach (IFigures fig in subFigures)
            {
                fig.ShowFigureDetails();
            }
        }

        //Is used for group
        public void Get_Shape(FrameworkElement shape, ref List<IFigures> _ShapesList)
        {
            if (groupInkCanvas == shape) _ShapesList.Add(this);
            foreach (IFigures fig in subFigures)
            {
                fig.Get_Shape(shape, ref _ShapesList); 
            }
        }
    }
}

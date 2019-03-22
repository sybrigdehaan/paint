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
        public void Find(FrameworkElement element, ref List<Group> frameworkList)
        {
            foreach (IFigures fig in subFigures)
            {
                if (typeof(Group) == fig.GetType())
                {
                    if ((fig as Group).groupInkCanvas == element)
                        frameworkList.Add((fig as Group));
                }
            }
        }
        
        //Is used for group
        public void Find(IFigures group, ref List<IFigures> _SubListSelected)
        {
            foreach(IFigures fig in subFigures)
            {
                if (fig == group)
                    _SubListSelected.Add(fig); 
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

        //public void GetShape(IFigures figure, ref List<FrameworkElement> shapesList)
        //{
        //    if (figure == this) { shapesList.Add(groupInkCanvas); }
        //    foreach (IFigures fig in subFigures)
        //    {
        //        fig.GetShape(figure, ref shapesList);
        //    }
        //}

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace paint
{
    public class AddRemoveVisitor : IAddRemoveVisitor
    {
        public void AddVisit(IFigures figure)
        {
            MyMainGroup.GetInstance().Add(figure);
            MyInkCanvas.GetInstance().Children.Add(figure.GetShape());
        }

        public void RemoveVisit(IFigures figure)
        {
            if(typeof(_Group) == figure.GetType())
            {
                foreach (IFigures inGroupFigure in (figure as _Group).SubFigures.ToList())
                {
                    (figure.GetShape() as Canvas).Children.Remove(inGroupFigure.GetShape());
                    (figure as _Group).Remove(inGroupFigure);
                }
            }

            MyMainGroup.GetInstance().Remove(figure);
            MyInkCanvas.GetInstance().Children.Remove(figure.GetShape());
        }
    }
}

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
        }

        public void RemoveVisit(IFigures figure)
        {
            if(typeof(_Group) == figure.GetType())
            {
                foreach (IFigures inGroupFigure in (figure as _Group).SubFigures.ToList())
                {
                    (figure as _Group).Remove(inGroupFigure);
                }
            }
            
            MyMainGroup.GetInstance().Remove(figure);
        }
    }
}

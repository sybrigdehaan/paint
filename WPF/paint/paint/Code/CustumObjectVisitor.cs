using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace paint
{
    class CustumObjectVisitor : ICustomObjectVisitor
    {
        public void VisitMake(IFigures figure)
        {
            Singleton.GetInstance().Children.Add(figure.GetShape());
        }

        public void VisitDestroy(IFigures figure)
        {
            Singleton.GetInstance().Children.Remove(figure.GetShape());
        }
    }
}

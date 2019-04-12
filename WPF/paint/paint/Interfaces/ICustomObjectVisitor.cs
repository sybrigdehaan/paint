using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace paint
{
    public interface ICustomObjectVisitor
    {
        void VisitMake(IFigures figure);
        void VisitDestroy(IFigures figure); 
    }
}

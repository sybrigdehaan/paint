using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace paint
{
    public interface IAddRemoveVisitor
    {
        void AddVisit(IFigures figure);
        void RemoveVisit(IFigures figure); 
    }
}

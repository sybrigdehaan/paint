using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace paint
{
    public interface IOrnamentDecorator : IFigures
    {
        void SetPosition(); 
    }
}

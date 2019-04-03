using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes; 

namespace paint
{
    public interface IFigures
    {
        void ShowFigureDetails();
        void Get_Shape(ref List<IFigures> _ShapesList);
        FrameworkElement GetShape();
    }
}

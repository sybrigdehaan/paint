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
        FrameworkElement GetShape();
        int GetDepthInList(); 
        void DepthInList(int depthInList);

        //Commands 
        void Make();
        void Destroy();

        //Visitor
        void Accept(IWriteToFileVisitor visitor);
    }
}

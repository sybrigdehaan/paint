using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace paint
{
    public class ChangeOrnament
    {
        public static void GetPostion(FrameworkElement[] myArray, InkCanvas canvas)
        {         
            if (myArray.Length != 0)
            {
                var mousePosition = Mouse.GetPosition(canvas);
                var PositionLeft = InkCanvas.GetLeft(myArray[0]);
                var PositionTop = InkCanvas.GetTop(myArray[0]);

                if (Mouse.GetPosition(canvas) != mousePosition)
                {
                    PositionLeft = InkCanvas.GetLeft(myArray[0]);
                    PositionTop = InkCanvas.GetTop(myArray[0]);
                    mousePosition = Mouse.GetPosition(canvas);
                }
                //return new KeyValuePair<double, double>(PositionLeft, PositionTop);
            }
           
        }
    }
}

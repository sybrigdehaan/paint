using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;
using System.Windows.Controls;
using System.Windows.Input;

namespace paint
{
    public class MyInkCanvas : InkCanvas
    {
        // Thread-safe oplossing om slechts één instantie aan te maken.
        private static MyInkCanvas myInkCanvas; 

        // Private constructor om te voorkomen dat anderen een instantie kunnen aanmaken.
        private MyInkCanvas() { }

        // Via een static read-only property kan de instantie benaderd worden.
        public static MyInkCanvas GetInstance()
        {
            if (myInkCanvas == null)
                myInkCanvas = new MyInkCanvas();
            return myInkCanvas; 
        }
    }
}

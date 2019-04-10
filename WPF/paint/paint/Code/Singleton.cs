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
    public class Singleton : InkCanvas
    {
        // Thread-safe oplossing om slechts één instantie aan te maken.
        private static Singleton inkCanvas; 

        // Private constructor om te voorkomen dat anderen een instantie kunnen aanmaken.
        private Singleton() { }

        // Via een static read-only property kan de instantie benaderd worden.
        public static Singleton GetInstance()
        {
            if (inkCanvas == null)
                inkCanvas = new Singleton();
            return inkCanvas; 
        }
    }

}

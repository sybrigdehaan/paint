using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace paint
{
    public class MyMainGroup : _Group
    {
        // Thread-safe oplossing om slechts één instantie aan te maken.
        private static MyMainGroup myMainGroup;

        // Private constructor om te voorkomen dat anderen een instantie kunnen aanmaken.
        private MyMainGroup() {
        }

        // Via een static read-only property kan de instantie benaderd worden.
        public static MyMainGroup GetInstance()
        {
            if (myMainGroup == null)
                myMainGroup = new MyMainGroup();
            return myMainGroup;
        }
    }
}

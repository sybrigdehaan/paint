using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace paint
{
    public class MyMainGroup
    {
        // Thread-safe oplossing om slechts één instantie aan te maken.
        private static MyMainGroup _instance = new MyMainGroup();

        // Private constructor om te voorkomen dat anderen een instantie kunnen aanmaken.
        private MyMainGroup() { }

        // Via een static read-only property kan de instantie benaderd worden.
        public static MyMainGroup Instance
        {
            get
            {
                return _instance;
            }
        }
    }

}

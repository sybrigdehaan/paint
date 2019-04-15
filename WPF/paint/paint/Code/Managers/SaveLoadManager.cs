using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Controls;

namespace paint
{
    public class SaveLoadManager
    {
        public readonly string filePath = "../../../../../Drawings/Test001.txt";
        
        public void Save()
        {
            MyMainGroup.GetInstance().DepthInList(0); 
            ICustomObjectVisitor visitor = new CustumObjectVisitor();
            File.WriteAllText(filePath, string.Empty);
            MyMainGroup.GetInstance().Accept(visitor);
        }

        public void Load()
        {

        }
    }
}

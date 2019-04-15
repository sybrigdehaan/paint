using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.IO;

namespace paint
{
    public interface ICustomObjectVisitor
    {
        void SetTextFile(StreamWriter textFile);
        StreamWriter GetTextFile(); 
        void Visit(IFigures figure); 
    }
}

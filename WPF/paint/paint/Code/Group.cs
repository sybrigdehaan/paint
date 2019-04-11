using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows.Ink;
using System.Windows.Controls;


namespace paint
{
    public class Group : InkCanvas, IFigures; 
    {
        private string name;
        private List<IFigures> subFigures = new List<IFigures>();

        public Group(string name)
        {
            this.name = name;
        }

        public void Add(IFigures group)
        {
            subFigures.Add(group);
        }

        public void Remove(IFigures group)
        {
            subFigures.Remove(group);
        }

        public void ShowFigureDetails()
        {
            Console.WriteLine("Group: " + name);
            foreach (IFigures fig in subFigures)
            {
                fig.ShowFigureDetails();
            }
        }
    }
}

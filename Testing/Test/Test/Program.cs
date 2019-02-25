using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Figure figure1 = new Figure("Rectangle", 0, 0, 0, 0);
            Figure figure2 = new Figure("Rectangle", 10, 0, 100, 0);
            Group group1 = new Group("group1");
            group1.Add(figure1);
            group1.Add(figure2);

            Figure fig1 = new Figure("Ellipse", 0, 0, 0, 0);
            Figure fig2 = new Figure("Ellipse", 10, 0, 100, 0);
            Group group2 = new Group("group2");
            group2.Add(fig1);
            group2.Add(fig2);

            Group directorGroup = new Group("directorGroup");
            directorGroup.Add(group1);
            directorGroup.Add(group2);

            directorGroup.showEmployeeDetails();
            Console.ReadKey(); 



        }

        public interface Figures
        {
            void showEmployeeDetails();
        }


        public class Figure : Figures
        {
            private string name;
            private int left, top, width, height;

            public Figure(string name, int left, int top, int width, int height)
            {
                this.name = name;
                this.left = left;
                this.top = top;
                this.width = width;
                this.height = height;
            }

            public void showEmployeeDetails()
            {
                Console.WriteLine("This is a: " + name + " With the measurement: " + "left: " + left + ", Top: " + top + ", Width: " + width + ", Height: " + height);
            }
        }


        public class Group : Figures
        {
            private string name; 
            private List<Figures> subFigures = new List<Figures>();

            public Group (string name)
            {
                this.name = name; 
            }

            public void Add(Figures group)
            {
                subFigures.Add(group);
            }

            public void Remove(Figures group)
            {
                subFigures.Remove(group);
            }

            public void showEmployeeDetails()
            {
                Console.WriteLine("Group: " + name); 
                foreach (Figures fig in subFigures)
                {
                    fig.showEmployeeDetails();
                }
            }
        }
    }
}

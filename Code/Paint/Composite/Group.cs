﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paint
{
    public class Group : IFigures
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

        public void ShowEmployeeDetails()
        {
            Console.WriteLine("Group: " + name);
            foreach (IFigures fig in subFigures)
            {
                fig.ShowEmployeeDetails();
            }
        }
    }
}

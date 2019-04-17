using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Windows.Controls;
using System.Windows;

namespace paint
{
    public class SaveLoadManager
    {
        public readonly string filePath = "../../../../../Drawings/Test001.txt";

        /// <summary>
        /// In deze functie wordt het opslaan van jou canvas geregeld
        /// </summary>
        public void Save()
        {
            MyMainGroup.GetInstance().DepthInList(0);
            IWriteToFileVisitor visitor = new WriteToFileVisitor();
            File.WriteAllText(filePath, string.Empty);
            MyMainGroup.GetInstance().Accept(visitor);
        }

        /// <summary>
        /// In deze functie wordt alles wat in de textfile staat gelezen en wordt gegekenen wat voor object eerst aangemaakt moet worden.
        /// Je werkt dus van achter naar voren. Alleereerst worden de diepste objecten aangemaakt en deze objecten worden in een group gedaan.
        /// Daarna ga je een stap terug, uiteindelijk kom je dan bij je main canvas terecht!
        /// </summary>
        public void Load()
        {
            _Shape _myShape = null; 
            SimpleRemoteControl remote = null;
            FrameworkElement myShape = null; 
            int maxJump = 0; 

            List<IFigures> subFigures = new List<IFigures>(); 
            foreach (IFigures figure in MyMainGroup.GetInstance().SubFigures.ToList())
            {
                MyMainGroup.GetInstance().Remove(figure);
            }
            
            List<string> lines = File.ReadAllLines(filePath).ToList();
            foreach (string line in lines)
            {
                int countJump = line.TakeWhile(c => c == '-').Count();
                if (countJump > maxJump)
                    maxJump = countJump;
            }

            for (int i = maxJump; i > 0 ; i--)
            {
                 _Group group = new _Group();
                
                foreach (string line in lines)
                {
                    int countJump = line.TakeWhile(c => c == '-').Count();
                    if (countJump == i)
                    {
                        string[] entries = line.Split(' ');
                        string nameObject = entries[1];

                        if (nameObject == "Rectangle") {
                             _myShape = new _Rectangle(); }

                        if (nameObject == "Ellipse") {
                             _myShape = new _Ellipse(); }

                        if (nameObject == "Rectangle" || nameObject == "Ellipse")
                        {
                            remote = new SimpleRemoteControl { SetCommand = new _MakeShape(_myShape) };
                            myShape = _myShape.GetShape();
                            subFigures.Add(_myShape);
                            _myShape.SetColor();

                            InkCanvas.SetLeft(myShape, Convert.ToDouble(entries[2]));
                            InkCanvas.SetTop(myShape, Convert.ToDouble(entries[3]));
                            myShape.Width = Convert.ToDouble(entries[4]);
                            myShape.Height = Convert.ToDouble(entries[5]);
                            remote.buttonWasPressed();
                        }
                    }
                }

                if ((i - 1 ) != 0)
                {
                    remote = new SimpleRemoteControl { SetCommand = new _EnGroup(group, subFigures) };
                    remote.buttonWasPressed();
                    subFigures = new List<IFigures>();
                    subFigures.Add(group);
                }
            }
        }
    }
}
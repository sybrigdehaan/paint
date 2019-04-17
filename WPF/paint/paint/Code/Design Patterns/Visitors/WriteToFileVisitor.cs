using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.IO;
using System.Windows; 

namespace paint
{
    public class WriteToFileVisitor : IWriteToFileVisitor
    {
        private StreamWriter textFile; 
        public void SetTextFile(StreamWriter textFile)
        {
            this.textFile = textFile; 
        }

        public StreamWriter GetTextFile()
        {
            return textFile; 
        }

        public void Visit(IFigures figure)
        {
            string jump = new String('-', figure.GetDepthInList()); 

            StreamWriter textFile = File.AppendText(MainWindow.saveLoadManager.filePath);
            SetTextFile(textFile); 
            FrameworkElement element = figure.GetShape();
            if (typeof(_Group) == figure.GetType())
                textFile.WriteLine(jump + " " + "Group" + " " + InkCanvas.GetLeft(element) + " " + InkCanvas.GetTop(element));

            if (typeof(_Rectangle) == figure.GetType())
                textFile.WriteLine(jump + " " + "Rectangle" + " " + InkCanvas.GetLeft(element) + " " + InkCanvas.GetTop(element)
                    + " " + element.Width + " " + element.Height);

            if (typeof(_Ellipse) == figure.GetType())
                textFile.WriteLine(jump + " " + "Ellipse" + " " + InkCanvas.GetLeft(element) + " " + InkCanvas.GetTop(element)
                    + " " + element.Width + " " + element.Height);

            textFile.Close(); 
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;

namespace paint
{
    // creates ornament
    public abstract class Ornament : IOrnamentDecorator
    {
        protected IFigures figure;
        protected TextBlock textBlock = new TextBlock();

        public int GetDepthInList() { return figure.GetDepthInList(); }
        public FrameworkElement GetShape() { return figure.GetShape(); }
        public void DepthInList(int depthInList) { figure.DepthInList(depthInList); }
        public void Make() { figure.Make(); }
        public void Destroy(){ figure.Destroy(); }
        public void Accept(IWriteToFileVisitor visitor){ figure.Accept(visitor); }
        public void RemoveTextBlok() { (MyMainGroup.GetInstance().GetShape() as InkCanvas).Children.Remove(figure.GetShape()); }
        public virtual void SetPosition() { } 
    }

    public class Left : Ornament
    {
        public Left (IFigures figure)
        {
            this.figure = figure;
            SetPosition(); 
        }
        
        public override void SetPosition()
        {
            double left = InkCanvas.GetLeft(figure.GetShape());
            double top = InkCanvas.GetTop(figure.GetShape());
            double heigth = figure.GetShape().Height / 2;

            textBlock.Text = "Left";
            InkCanvas.SetLeft(textBlock, left - (textBlock.Text.Length * 8));
            InkCanvas.SetTop(textBlock, top + heigth);
            (MyMainGroup.GetInstance().GetShape() as InkCanvas).Children.Add(textBlock);
        }
    }

    public class Right : Ornament
    {
        public Right (IFigures figure)
        {
            this.figure = figure;
            SetPosition();
        }

        public override void SetPosition()
        {
            double left = InkCanvas.GetLeft(figure.GetShape());
            double top = InkCanvas.GetTop(figure.GetShape());
            double heigth = figure.GetShape().Height / 2;
            double width = figure.GetShape().Width;

            textBlock.Text = "Right";
            InkCanvas.SetLeft(textBlock, left + width + 10);
            InkCanvas.SetTop(textBlock, top + heigth);
            (MyMainGroup.GetInstance().GetShape() as InkCanvas).Children.Add(textBlock);
        }
    }

    public class Top : Ornament
    {
        public Top(IFigures figure)
        {
            this.figure = figure;
            SetPosition();
        }

        public override void SetPosition()
        {
            double left = InkCanvas.GetLeft(figure.GetShape());
            double top = InkCanvas.GetTop(figure.GetShape());
            double width = figure.GetShape().Width / 2;

            textBlock.Text = "Top";
            InkCanvas.SetLeft(textBlock, left + width - (textBlock.Text.Length / 2));
            InkCanvas.SetTop(textBlock, top - 25);
            (MyMainGroup.GetInstance().GetShape() as InkCanvas).Children.Add(textBlock);
        }
    }

    public class Bottom : Ornament
    {
        public Bottom(IFigures figure)
        {
            this.figure = figure;
            SetPosition();
        }

        public override void SetPosition()
        {
            double left = InkCanvas.GetLeft(figure.GetShape());
            double top = InkCanvas.GetTop(figure.GetShape());
            double heigth = figure.GetShape().Height;
            double width = figure.GetShape().Width / 2;

            textBlock.Text = "Bottom";
            InkCanvas.SetLeft(textBlock, left + width - (textBlock.Text.Length / 2));
            InkCanvas.SetTop(textBlock, top + heigth + 10);
            (MyMainGroup.GetInstance().GetShape() as InkCanvas).Children.Add(textBlock); 
        }
    }
}

using System;
using System.Collections.Generic;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows;

namespace paint
{
    public class _Group : IFigures
    {
        public Canvas groupCanvas = new Canvas();

        public FrameworkElement GetShape () {return groupCanvas; }

        public List<IFigures> SubFigures { get; } = new List<IFigures>();

        public void Add(IFigures figure)
        {
            SubFigures.Add(figure);
        }

        public void Remove(IFigures figure)
        {
            SubFigures.Remove(figure);
        }

        public void ShowFigureDetails()
        {
            Console.WriteLine("Group");
            foreach (IFigures fig in SubFigures)
            {
                fig.ShowFigureDetails();
            }
        }
        
        public void Get_Shape(ref List<IFigures> _ShapesList)
        {
            _ShapesList.Add(this); 
            foreach (IFigures fig in SubFigures)
            {
                fig.Get_Shape(ref _ShapesList); 
            }
        }

        public void Make(_Group group)
        {
            group.Add(this);
            Singleton.GetInstance().Children.Add(groupCanvas);
        }

        public void Destroy(_Group group)
        {
            group.Remove(this);
            Singleton.GetInstance().Children.Remove(groupCanvas);
            foreach (var item in SubFigures)
            {
                groupCanvas.Children.Remove(item.GetShape());
            }

            foreach (var figure in SubFigures)
            {
                group.Add(figure); 
                Singleton.GetInstance().Children.Add(figure.GetShape());
            }
           
        }
    }

    public class _MakeGroup : ICommand
    {
        private _Group _myGroup;
        private IFigures figure;
        public _MakeGroup(_Group _myGroup, IFigures figure)
        {
            this._myGroup = _myGroup;
            this.figure = figure;
        }

        public void Execute()
        {
            _myGroup.Make(figure as _Group);
        }
    }

    public class _DestroyGroup : ICommand
    {
        private _Group _myGroup;
        private IFigures figure;
        public _DestroyGroup(_Group _myGroup, IFigures figure)
        {
            this._myGroup = _myGroup;
            this.figure = figure;
        }

        public void Execute()
        {
            _myGroup.Destroy(figure as _Group);
        }
    }
}

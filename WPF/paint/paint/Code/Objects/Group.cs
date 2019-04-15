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

        public void Make(List<IFigures> selectedFigures)
        {
            ChangeGroup.AddTo_Group(this, selectedFigures);
        }

        public void Destroy()
        {
            ChangeGroup.Un_Group(this);
        }
    }

    public class _MakeGroup : ICommand
    {
        private _Group _myGroup;
        private List<IFigures> selectedFigures;
        public _MakeGroup(_Group _myGroup, List<IFigures> selectedFigures)
        {
            this._myGroup = _myGroup;
            this.selectedFigures = selectedFigures;
        }

        public void Execute()
        {
            _myGroup.Make(selectedFigures);
        }
    }

    public class _DestroyGroup : ICommand
    {
        private _Group _myGroup;
        public _DestroyGroup(_Group _myGroup)
        {
            this._myGroup = _myGroup;
        }

        public void Execute()
        {
            _myGroup.Destroy();
        }
    }
}

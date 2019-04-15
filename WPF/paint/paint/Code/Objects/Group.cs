﻿using System;
using System.Collections.Generic;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows;

namespace paint
{
    public class _Group : IFigures
    {
        private int depthInList; 
        public Canvas groupCanvas = new Canvas();

        public int GetDepthInList() { return depthInList; }
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

        public void DepthInList(int depthInList)
        {
            this.depthInList = depthInList;
            depthInList += 1;
            foreach (IFigures fig in SubFigures)
            {
                fig.DepthInList(depthInList);
            }
           
        }
        
        public void Accept(ICustomObjectVisitor visitor)
        {
            visitor.Visit(this); 
            foreach (IFigures fig in SubFigures)
            {
                fig.Accept(visitor);
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

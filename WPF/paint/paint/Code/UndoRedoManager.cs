using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace paint
{
    public class UndoRedoManager
    {
        private Stack<SimpleRemoteControl> undoRemoteControls;
        private Stack<SimpleRemoteControl> redoRemoteControls;

        public UndoRedoManager()
        {
            Reset();
        }

        public int UndoCount
        {
            get
            {
                return undoRemoteControls.Count;
            }
        }

        public int RedoCount
        {
            get
            {
                return redoRemoteControls.Count;
            }
        }

        public void AddToUndo(SimpleRemoteControl cmd)
        {
            undoRemoteControls.Push(cmd);
        }

        public void AddToRedo(SimpleRemoteControl cmd)
        {
            redoRemoteControls.Push(cmd);
        }

        public void Reset()
        {
            undoRemoteControls = new Stack<SimpleRemoteControl>();
            redoRemoteControls = new Stack<SimpleRemoteControl>();
        }

        public void Undo()
        {
            if (UndoCount > 0)
            {
                SimpleRemoteControl cmd = undoRemoteControls.Pop();
                cmd.buttonWasPressed();
            }
            else
                MessageBox.Show("Er zijn geen undo's meer!");
        }

        public void Redo()
        {
            if (RedoCount > 0)
            {
                SimpleRemoteControl cmd = redoRemoteControls.Pop();
                cmd.buttonWasPressed();
            }
            else
                MessageBox.Show("Er zijn geen redo's meer!");
        }
    }
}

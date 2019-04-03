using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace paint
{
    public class SimpleRemoteControl
    {
        ICommand slot;

        public SimpleRemoteControl(){ }

        public ICommand SetCommand { set { slot = value; } }

        public void buttonWasPressed()
        {
            slot.Execute(); 
        }
    }
}

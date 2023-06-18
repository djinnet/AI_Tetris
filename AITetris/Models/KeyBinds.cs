using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AITetris.Classes
{
    public class KeyBinds
    {
        // Constructor for setting KeyBinds with default value.
        public KeyBinds()
        {
            Insta = Key.Space;
            Drop = Key.S;
            Rotate = Key.W;
            Right = Key.D;
            Left = Key.A;
            Swap = Key.E;
            Pause = Key.Escape;
        }

        public Key Insta { get; set; }
        public Key Drop { get; set; }
        public Key Rotate { get; set; }
        public Key Right { get; set; }
        public Key Left { get; set; }
        public Key Swap { get; set; }
        public Key Pause { get; set; }
    }
}

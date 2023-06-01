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
        public KeyBinds()
        {

        }

        public Key insta { get; set; }
        public Key drop { get; set; }
        public Key rotate { get; set; }
        public Key right { get; set; }
        public Key left { get; set; }
        public Key swap { get; set; }
        public Key pause { get; set; }
    }
}

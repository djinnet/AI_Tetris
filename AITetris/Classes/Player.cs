using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AITetris.Classes
{
    internal class Player : Character
    {
        public Player(string name) : base(name)
        {
            metaCurrency = 0;
        }

        public int metaCurrency;
    }
}

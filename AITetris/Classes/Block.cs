using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AITetris.Classes
{
    internal class Block
    {
        public Block()
        {
            spritePath = "../Assets/Sprits/BlueGreenPrimary.png";
        }
        int coordinateX;
        int coordinateY;
        string spritePath;
    }
}

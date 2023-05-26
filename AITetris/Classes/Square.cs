using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AITetris.Classes
{
    public class Square
    {
        public Square(int coordinateX, int coordinateY)
        {
            this.coordinateX = coordinateX;
            this.coordinateY = coordinateY;
            spritePath = "/Assets/Sprits/BlueGreenPrimary.png";
        }
        public int coordinateX;
        public int coordinateY;
        public string spritePath;
    }
}

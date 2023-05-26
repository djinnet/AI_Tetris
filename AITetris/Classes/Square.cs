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
            spritePath = "C:\\Users\\Sebas\\source\\repos\\AI_Tetris\\AI_Tetris\\AITetris\\Assets\\Sprits\\BlueGreenPrimary.png";
        }
        public int coordinateX;
        public int coordinateY;
        public string spritePath;
    }
}

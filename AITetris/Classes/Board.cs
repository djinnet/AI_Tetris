using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AITetris.Classes
{
    public class Board
    {
        public Board(int xLength, int yLength)
        {
            grid = new bool[xLength,yLength];
            squares = new Square[xLength * yLength];
        }

        public bool[,] grid;
        public Square[] squares;
    }
}

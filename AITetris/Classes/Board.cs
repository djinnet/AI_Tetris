using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AITetris.Classes
{
    public class Board
    {
        // Constructor for making a new Board.
        public Board(int xLength, int yLength)
        {
            squares = new List<Square>();
            gridSize= xLength * yLength;
        }

        public int gridSize;
        public List<Square> squares;
    }
}

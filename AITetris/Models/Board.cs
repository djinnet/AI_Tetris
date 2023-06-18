using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AITetris.Classes
{
    public class Board
    {
        /// <summary>
        /// The method for making a new Board.
        /// </summary>
        /// <param name="width">X length</param>
        /// <param name="height">Y Length</param>
        /// <returns>Board</returns>
        public static Board Create(int width, int height)
        {
            Board board = new()
            {
                GridSize = width * height
            };
            return board;
        }

        /// <summary>
        /// The size of the grid
        /// </summary>
        public int GridSize { get; set; }

        /// <summary>
        /// The squares for the grid
        /// </summary>
        public List<Square> Squares { get; set; } = new List<Square>();
    }
}

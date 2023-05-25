using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AITetris.Classes
{
    public class Figure
    {
        public Figure(int coordinateX, int coordinateY, FigureType figureType)
        {
            this.coordinateX = coordinateX;
            this.coordinateY = coordinateY;
            this.figureType = figureType;
            blocks = new Block[] { }; // TODO: Make this work.
            shape = setStartShape();
        }

        public int coordinateX;
        public int coordinateY;
        public bool[,] shape;
        public Block[] blocks;
        public FigureType figureType;

        private bool[,] setStartShape()
        {
            bool[,] result;
            switch (figureType)
            {
                case FigureType.I:
                    //  * * * *
                    //  x x x x
                    //  * * * *
                    //  * * * *
                    result = new bool[,] { { false, false, true, false },
                                           { false, false, true, false },
                                           { false, false, true, false },
                                           { false, false, true, false }
                                         };
                    break;
                case FigureType.J:
                    //  * * x
                    //  x x x
                    //  * * *
                    result = new bool[,] { { false, true, false },
                                           { false, true, false },
                                           { false, true, true }
                                         };
                    break;
                case FigureType.L:
                    //  x * *
                    //  x x x
                    //  * * *
                    result = new bool[,] { { false, true, true },
                                           { false, true, false },
                                           { false, true, false }
                                         };
                    break;
                case FigureType.O:
                    //  * x x *
                    //  * x x *
                    //  * * * *
                    result = new bool[,] { { false, false, false },
                                           { false, true, true, },
                                           { false, true, true, },
                                           { false, false, false }
                                         };
                    break;
                case FigureType.S:
                    //  * x x
                    //  x x *
                    //  * * *
                    result = new bool[,] { { false, true, false },
                                           { false, true, true },
                                           { false, false, true }
                                         };
                    break;
                case FigureType.T:
                    //  * x *
                    //  x x x
                    //  * * *
                    result = new bool[,] { { false, true, false },
                                           { false, true, true },
                                           { false, true, false }
                                         };
                    break;
                case FigureType.Z:
                    //  x x *
                    //  * x x
                    //  * * *
                    result = new bool[,] { { false, false, true },
                                           { false, true, true },
                                           { false, true, false }
                                         };
                    break;
                default:
                    result = new bool[0, 0];
                    break;
            }
            return result;
        }
    }

    public enum FigureType
    {
        I,
        J,
        L,
        O,
        S,
        T,
        Z
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AITetris.Classes
{
    public class TetrisFigure
    {
        public TetrisFigure(int[] coordinates, FigureType figureType)
        {
            this.figureType = figureType;
            shape = setStartShape(coordinates);
            squares = new Square[shape.GetLength(0)];
            for (int i = 0; i < shape.GetLength(0); i++)
            {
                squares[i] = new Square(shape[i, 0], shape[i, 1]);
            }
        }

        public int[,] shape;
        public Square[] squares;
        public FigureType figureType;

        private int[,] setStartShape(int[] coords)
        {
            int[,] result;
            switch (figureType)
            {
                case FigureType.I:
                    //  * * * *
                    //  x x x x
                    //  * * * *
                    //  * * * *
                    result = new int[,] { { -1, 0 }, { 0, 0 }, { 1, 0 }, { 2, 0 } };
                    //result = new bool[,] { { false, false, true, false },
                    //                       { false, false, true, false },
                    //                       { false, false, true, false },
                    //                       { false, false, true, false }
                    //                     };
                    break;
                case FigureType.J:
                    //  x * *
                    //  x x x
                    //  * * *
                    result = new int[,] { { -1, 1 }, { -1, 0 }, { 0, 0 }, { 1, 0 } };
                    //result = new bool[,] { { false, true, true },
                    //                       { false, true, false },
                    //                       { false, true, false }
                    //                     };
                    break;
                case FigureType.L:
                    //  * * x
                    //  x x x
                    //  * * *
                    result = new int[,] { { -1, 0 }, { 0, 0 }, { 1, 0 }, { 1, 1 } };
                    //result = new bool[,] { { false, true, false },
                    //                       { false, true, false },
                    //                       { false, true, true }
                    //                     };

                    break;
                case FigureType.O:
                    //  * x x *
                    //  * x x *
                    //  * * * *
                    result = new int[,] { { 0, 1 }, { 0, 0 }, { 1, 1 }, { 1, 0 } };
                    //result = new bool[,] { { false, false, false },
                    //                       { false, true, true, },
                    //                       { false, true, true, },
                    //                       { false, false, false }
                    //                     };
                    break;
                case FigureType.S:
                    //  * x x
                    //  x x *
                    //  * * *
                    result = new int[,] { { -1, 0 }, { 0, 0 }, { 0, 1 }, { 1, 1 } };
                    //result = new bool[,] { { false, true, false },
                    //                       { false, true, true },
                    //                       { false, false, true }
                    //                     };
                    break;
                case FigureType.T:
                    //  * x *
                    //  x x x
                    //  * * *
                    result = new int[,] { { -1, 0 }, { 0, 0 }, { 0, 1 }, { 1, 0 } };
                    //result = new bool[,] { { false, true, false },
                    //                       { false, true, true },
                    //                       { false, true, false }
                    //                     };
                    break;
                case FigureType.Z:
                    //  x x *
                    //  * x x
                    //  * * *
                    result = new int[,] { { -1, 1 }, { 0, 0 }, { 0, 1 }, { 1, 0 } };
                    //result = new bool[,] { { false, false, true },
                    //                       { false, true, true },
                    //                       { false, true, false }
                    //                     };
                    break;
                default:
                    result = new int[0, 0];
                    break;
            }
            return RelativeToAbsolute( coords, result);
        }

        private int[,] RelativeToAbsolute(int[] absolute, int[,] relative)
        {
            int[,] result = new int[4, 2];
            for (int i = 0; i < relative.GetLength(0); i++)
            {
                result[i,0] = relative[i,0] + absolute[0];
                result[i, 1] = relative[i, 1] + absolute[1];
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

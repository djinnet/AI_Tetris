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
            referenceCoords = coordinates;
            SetStartShape();
            squares = new Square[shape.GetLength(0)];

            for (int i = 0; i < shape.GetLength(0); i++)
            {
                squares[i] = new Square(shape[i, 0], shape[i, 1]);
            }

            ShapeToBoard();
        }

        public int[,] shape;
        public Square[] squares;
        public FigureType figureType;

        private int[] referenceCoords;

        public void Rotate()
        {
            for (int i = 0; i < shape.GetLength(0); i++)
            {
                var x = shape[i, 1];
                var y = -shape[i, 0];

                shape[i, 0] = x;
                shape[i, 1] = y;
            }
            ShapeToBoard();
        }

        private void SetStartShape()
        {
            switch (figureType)
            {
                case FigureType.I:
                    //  * * * *
                    //  x x x x
                    //  * * * *
                    //  * * * *
                    shape = new int[,] { { -1, 0 }, { 0, 0 }, { 1, 0 }, { 2, 0 } };
                    //shape = new bool[,] { { false, false, true, false },
                    //                       { false, false, true, false },
                    //                       { false, false, true, false },
                    //                       { false, false, true, false }
                    //                     };
                    break;
                case FigureType.J:
                    //  x * *
                    //  x x x
                    //  * * *
                    shape = new int[,] { { -1, 1 }, { -1, 0 }, { 0, 0 }, { 1, 0 } };
                    //shape = new bool[,] { { false, true, true },
                    //                       { false, true, false },
                    //                       { false, true, false }
                    //                     };
                    break;
                case FigureType.L:
                    //  * * x
                    //  x x x
                    //  * * *
                    shape = new int[,] { { -1, 0 }, { 0, 0 }, { 1, 0 }, { 1, 1 } };
                    //shape = new bool[,] { { false, true, false },
                    //                       { false, true, false },
                    //                       { false, true, true }
                    //                     };
                    break;
                case FigureType.O:
                    //  * x x *
                    //  * x x *
                    //  * * * *
                    shape = new int[,] { { 0, 1 }, { 0, 0 }, { 1, 1 }, { 1, 0 } };
                    //shape = new bool[,] { { false, false, false },
                    //                       { false, true, true, },
                    //                       { false, true, true, },
                    //                       { false, false, false }
                    //                     };
                    break;
                case FigureType.S:
                    //  * x x
                    //  x x *
                    //  * * *
                    shape = new int[,] { { -1, 0 }, { 0, 0 }, { 0, 1 }, { 1, 1 } };
                    //shape = new bool[,] { { false, true, false },
                    //                       { false, true, true },
                    //                       { false, false, true }
                    //                     };
                    break;
                case FigureType.T:
                    //  * x *
                    //  x x x
                    //  * * *
                    shape = new int[,] { { -1, 0 }, { 0, 0 }, { 0, 1 }, { 1, 0 } };
                    //shape = new bool[,] { { false, true, false },
                    //                       { false, true, true },
                    //                       { false, true, false }
                    //                     };
                    break;
                case FigureType.Z:
                    //  x x *
                    //  * x x
                    //  * * *
                    shape = new int[,] { { -1, 1 }, { 0, 0 }, { 0, 1 }, { 1, 0 } };
                    //shape = new bool[,] { { false, false, true },
                    //                       { false, true, true },
                    //                       { false, true, false }
                    //                     };
                    break;
                default:
                    shape = new int[0, 0];
                    break;
            }
        }

        private void ShapeToBoard()
        {
            for (int i = 0; i < squares.Length; i++)
            {
                squares[i].coordinateX = shape[i,0] + referenceCoords[0];
                squares[i].coordinateY = shape[i, 1] + referenceCoords[1];
            }
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

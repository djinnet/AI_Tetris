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
                squares[i] = new Square(shape[i, 0], shape[i, 1], color);
            }

            ShapeToBoard();
        }

        public int[,] shape;
        public Square[] squares;
        public FigureType figureType;

        private int[] referenceCoords;
        private string color;

        public void Rotate()
        {
            for (int i = 0; i < shape.GetLength(0); i++)
            {
                var x = -shape[i, 1];
                var y = shape[i, 0];

                shape[i, 0] = x;
                shape[i, 1] = y;
            }
            ShapeToBoard();
        }

        public void Move(string direction)
        {
            switch (direction)
            {
                case "left":
                    referenceCoords[0] -= 1;
                    break;
                case "right":
                    referenceCoords[0] += 1;
                    break;
                case "up":
                    referenceCoords[1] -= 1;
                    break;
                case "down":
                    referenceCoords[1] += 1;
                    break;
                default:
                    break;
            }
            ShapeToBoard();
        }

        public void MoveTo(int[] coords)
        {
            referenceCoords = coords;
            SetStartShape();
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
                    color = "GreenBluePrimary";

                    break;
                case FigureType.J:
                    //  x * *
                    //  x x x
                    //  * * *
                    shape = new int[,] { { -1, 1 }, { -1, 0 }, { 0, 0 }, { 1, 0 } };
                    color = "BluePrimary";

                    break;
                case FigureType.L:
                    //  * * x
                    //  x x x
                    //  * * *
                    shape = new int[,] { { -1, 0 }, { 0, 0 }, { 1, 0 }, { 1, 1 } };
                    color = "RedPrimary";

                    break;
                case FigureType.O:
                    //  * x x *
                    //  * x x *
                    //  * * * *
                    shape = new int[,] { { 0, 1 }, { 0, 0 }, { 1, 1 }, { 1, 0 } };
                    color = "PurplePrimary";

                    break;
                case FigureType.S:
                    //  * x x
                    //  x x *
                    //  * * *
                    shape = new int[,] { { -1, 0 }, { 0, 0 }, { 0, 1 }, { 1, 1 } };
                    color = "GreenPrimary";

                    break;
                case FigureType.T:
                    //  * x *
                    //  x x x
                    //  * * *
                    shape = new int[,] { { -1, 0 }, { 0, 0 }, { 0, 1 }, { 1, 0 } };
                    color = "BlueMaroonPrimary";

                    break;
                case FigureType.Z:
                    //  x x *
                    //  * x x
                    //  * * *
                    shape = new int[,] { { -1, 1 }, { 0, 0 }, { 0, 1 }, { 1, 0 } };
                    color = "RedGreenPrimary";

                    break;
                default:
                    shape = new int[0, 0];
                    color = "BluePrimary";
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

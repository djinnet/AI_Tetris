using AITetris.Classes;
using AITetris.Enums;
using AITetris.Stores;

namespace AITetris.Extensions;
public static class TetrisFigureExtensions
{
    public static void Rotate(this int[,] shape)
    {
        for (int i = 0; i < shape.GetLength(0); i++)
        {
            var x = -shape[i, 1];
            var y = shape[i, 0];

            shape[i, 0] = x;
            shape[i, 1] = y;
        }
    }

    public static Square[] GenerateSquares(this int[,] shape, string color)
    {
        var squares = new Square[shape.GetLength(0)];

        for (int i = 0; i < shape.GetLength(0); i++)
        {
            squares[i] = new Square(shape[i, 0], shape[i, 1], color);
        }

        return squares;
    }

    public static int[] Move(int[] coord, EDirection direction)
    {
        
        switch (direction)
        {
            case EDirection.left:
                coord[0] -= 1;
                break;
            case EDirection.right:
                coord[0] += 1;
                break;
            case EDirection.up:
                coord[1] -= 1;
                break;
            case EDirection.down:
                coord[1] += 1;
                break;
            default:
                break;
        }
        //parse perfectly
        return coord;
    }

    public static (int[,] shape, string color) SetStartShape(this EFigureType figureType)
    {
        int[,] shape;
        string color;
        switch (figureType)
        {
            case EFigureType.I:
                //  * * * *
                //  x x x x
                //  * * * *
                //  * * * *
                shape = new int[,] { { -1, 0 }, { 0, 0 }, { 1, 0 }, { 2, 0 } };
                color = ColorStore.GreenBluePrimary;

                break;
            case EFigureType.J:
                //  x * *
                //  x x x
                //  * * *
                shape = new int[,] { { -1, 1 }, { -1, 0 }, { 0, 0 }, { 1, 0 } };
                color = ColorStore.BluePrimary;

                break;
            case EFigureType.L:
                //  * * x
                //  x x x
                //  * * *
                shape = new int[,] { { -1, 0 }, { 0, 0 }, { 1, 0 }, { 1, 1 } };
                color = ColorStore.RedPrimary;

                break;
            case EFigureType.O:
                //  * x x *
                //  * x x *
                //  * * * *
                shape = new int[,] { { 0, 1 }, { 0, 0 }, { 1, 1 }, { 1, 0 } };
                color = ColorStore.PurplePrimary;

                break;
            case EFigureType.S:
                //  * x x
                //  x x *
                //  * * *
                shape = new int[,] { { -1, 0 }, { 0, 0 }, { 0, 1 }, { 1, 1 } };
                color = ColorStore.GreenPrimary;

                break;
            case EFigureType.T:
                //  * x *
                //  x x x
                //  * * *
                shape = new int[,] { { -1, 0 }, { 0, 0 }, { 0, 1 }, { 1, 0 } };
                color = ColorStore.BlueMaroonPrimary;

                break;
            case EFigureType.Z:
                //  x x *
                //  * x x
                //  * * *
                shape = new int[,] { { -1, 1 }, { 0, 0 }, { 0, 1 }, { 1, 0 } };
                color = ColorStore.RedGreenPrimary;

                break;
            default:
                shape = new int[0, 0];
                color = ColorStore.BluePrimary;
                break;
        }
        return (shape, color);
    }
}

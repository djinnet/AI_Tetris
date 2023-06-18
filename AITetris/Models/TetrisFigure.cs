using AITetris.Enums;
using AITetris.Extensions;

namespace AITetris.Classes
{
    public class TetrisFigure
    {
        // Constuctor for a TetrisFigure.
        public TetrisFigure(int[] coordinates, EFigureType figureType)
        {
            this.FigureType = figureType;
            ReferenceCoords = coordinates;
            SetStartShape();
            Squares = Shape.GenerateSquares(Color);

            ShapeToBoard();
        }

        public int[,] Shape { get; set; }
        public Square[] Squares { get; set; }
        public EFigureType FigureType { get; set; }

        private int[] ReferenceCoords { get; set; }
        private string Color { get; set; }

        // Calculates the new shape, through the formula: (x,y) => (y,-x)
        public void Rotate()
        {
            Shape.Rotate();
            ShapeToBoard();
        }

        // Changes the referenceCoords according to direction to move.
        public void Move(EDirection direction)
        {
            ReferenceCoords = TetrisFigureExtensions.Move(ReferenceCoords, direction);
            ShapeToBoard();
        }

        // Changes the referenceCoords to exact coordinates.
        public void MoveTo(int[] coords)
        {
            ReferenceCoords = coords;
            SetStartShape();
            ShapeToBoard();
        }

        // Sets shape to the default shape for the figure.
        private void SetStartShape()
        {
            (int[,] shape, string color) = FigureType.SetStartShape();
            Shape = shape;
            Color = color;
        }

        // Combines shape and referenceCoords to calculate square coordinates.
        private void ShapeToBoard()
        {
            for (int i = 0; i < Squares.Length; i++)
            {
                Squares[i].CoordinateX = Shape[i,0] + ReferenceCoords[0];
                Squares[i].CoordinateY = Shape[i, 1] + ReferenceCoords[1];
            }
        }
    }

    // Enumeration for the different types of figures.
    
}

using AITetris.Stores;
using System.Windows.Controls;

namespace AITetris.Classes
{
    public class Square
    {
        // Contructor for Square that assigns X and Y values, and determines the full image path.
        public Square(int coordinateX, int coordinateY, string color)
        {
            this.CoordinateX = coordinateX;
            this.CoordinateY = coordinateY;

            //SpritePath = color;
            //Fullpath = FileStore.ColorImage(color).AbsolutePath;

            Image = FileStore.GeneratedSpriteImage(color);

        }
        public int CoordinateX { get; set; }
        public int CoordinateY { get; set; }
        //private string SpritePath { get; set; }
        //private string Fullpath { get; set; }
        public Image Image { get; set; }
    }
}

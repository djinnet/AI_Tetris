using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media.Imaging;

namespace AITetris.Classes
{
    public class Square
    {
        // Contructor for Square that assigns X and Y values, and determines the full image path.
        public Square(int coordinateX, int coordinateY, string color)
        {
            this.coordinateX = coordinateX;
            this.coordinateY = coordinateY;

            spritePath = color;
            fullpath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Assets\\Sprites\\" + spritePath + ".png";

            image = new Image();
            image.Source = new BitmapImage(new Uri(fullpath, UriKind.Absolute));
        }
        public int coordinateX;
        public int coordinateY;
        public string spritePath;
        public string fullpath;
        public Image image;
    }
}

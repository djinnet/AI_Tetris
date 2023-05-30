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
        public Square(int coordinateX, int coordinateY)
        {
            this.coordinateX = coordinateX;
            this.coordinateY = coordinateY;

            spritePath = "\\Assets\\Sprits\\BlueGreenPrimary.png";
            fullpath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + spritePath;

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

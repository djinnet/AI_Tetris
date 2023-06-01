using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace AITetris.Classes
{
    public class Settings
    {
        public Settings()
        {
            gameSpeed = 10.0;
            startSpeed = 1000.0;
            enableSwapBlock = false;
            enableNextBlock = true;
            volume = 50;
            enableTraining = false;
            KeyBinds = new KeyBinds();
        }

        public double gameSpeed { get; set; }
        public double startSpeed { get; set; }
        public bool enableSwapBlock { get; set; }
        public bool enableNextBlock { get; set; }
        public int volume { get; set; }
        public bool enableTraining { get; set; }
        public KeyBinds KeyBinds { get; set; }
    }
}

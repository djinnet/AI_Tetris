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
            KeyBinds = new KeyBinds();
        }

        public double gameSpeed;
        public double startSpeed;
        public bool enableSwapBlock;
        public bool enableNextBlock;
        public int volume;
        public bool enableTraining;
        public KeyBinds KeyBinds;
    }
}

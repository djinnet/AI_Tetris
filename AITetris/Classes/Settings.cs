using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace AITetris.Classes
{
    internal class Settings
    {
        public Settings()
        {
            KeyBinds = new KeyBinds();
        }

        double gameSpeed;
        double startSpeed;
        bool enableSwapBlock;
        bool enableNextBlock;
        int volume;
        bool enableTraining;
        KeyBinds KeyBinds;
    }
}

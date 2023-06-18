using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AITetris.Classes
{
    public class Upgrades
    {
        // Constructor to initialize Upgrades and set default scoreMultiplier.
        public Upgrades()
        {
            ScoreMultiplier = 1;
        }
        public int Revive { get; set; }
        public double ScoreMultiplier { get; set; }
        public bool RemoveSwap { get; set; }
        public int EmergancyLineClear { get; set; }
        public int SlowTime { get; set; }
        public bool[] PurchasedUpgrades { get; set; }
    }
}

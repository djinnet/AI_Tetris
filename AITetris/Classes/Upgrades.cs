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
            scoreMultiplier = 1;
        }
        public int revive { get; set; }
        public double scoreMultiplier { get; set; }
        public bool removeSwap { get; set; }
        public int emergancyLineClear { get; set; }
        public int slowTime { get; set; }
        public bool[] purchasedUpgrades { get; set; }
    }
}

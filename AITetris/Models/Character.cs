using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AITetris.Classes
{
    public class Character
    {
        // Constructor for making a new Character.
        public Character(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// The character name
        /// </summary>
        public string Name { get; private set; }
    }
}

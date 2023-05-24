using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AITetris.Classes
{
    internal class Game
    {
        public Game(Board board, Character character, bool isPlayer, Settings settings)
        {
            this.board = board;
            this.character = character;
            this.points = 0;
            this.time = 0;
            this.isPlayer = isPlayer;
            this.settings = settings;
        }
        Board board;
        Character character;
        int points;
        int time;
        bool isPlayer;
        Settings settings;
    }
}

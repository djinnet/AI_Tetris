using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AITetris.Classes
{
    public class Game
    {
        public Game(Board board, Character character, Settings settings)
        {
            this.board = board;
            this.character = character;
            this.points = 0;
            this.time = 0;
            this.isPlayer = character.GetType() == typeof(Player);
            this.settings = settings;
        }

        public Game(Character character, int points, int time, bool isPlayer, int linesCleared)
        {
            this.character = character;
            this.points = points;
            this.time = time;
            this.isPlayer = isPlayer;
            this.linesCleared = linesCleared;
        }

        public Board board;
        public Character character;
        public int points;
        public int time;
        public int linesCleared;
        public bool isPlayer;
        public Settings settings;
    }
}

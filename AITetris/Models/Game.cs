using AITetris.Extensions;

namespace AITetris.Classes
{
    public class Game
    {
        // Constructor for making an entirely new Game.
        public Game()
        {
        }

        // Constructor for making a new Game from database information.
        public Game(Character character, int points, int linesCleared, int time, bool isPlayer, int rank)
        {
            this.Character = character;
            this.Points = points;
            this.Time = time;
            this.IsPlayer = isPlayer;
            this.LinesCleared = linesCleared;
            this.Rank = rank;
        }

        public static Game Create(Board board, Character character, Settings settings, Upgrades upgrades)
        {
            return new()
            {
                Board = board,
                Character = character,
                Settings = settings,
                Upgrades = upgrades,
                Points = 0,
                Time = 0,
                IsPlayer = character.IsPlayer()
            };
        }

        public Board? Board { get; private set; }
        public Character? Character { get; private set; }
        public int Points { get; set; }
        public int Time { get; set; }
        public int LinesCleared { get; set; }
        public bool IsPlayer { get; set; }
        public int Rank { get; set; }
        public Settings? Settings { get; set; }
        public Upgrades? Upgrades { get; private set; }
    }
}

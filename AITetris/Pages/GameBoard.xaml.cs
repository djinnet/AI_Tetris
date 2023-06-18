using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using AITetris.Classes;
using AITetris.Controllers;
using AITetris.Controls;
using AITetris.Enums;
using AITetris.Services;

namespace AITetris.Pages
{
    /// <summary>
    /// Interaction logic for GameBoard.xaml
    /// </summary>
    public partial class GameBoard : Page
    {
        // Menu variables
        public Game game;
        private PauseMenu pauseMenu;

        // Timer varibles for the scoreboard timer
        private DispatcherTimer scoreboardTimer;
        private DateTime startTime;
        private TimeSpan elapsedTime;
        private TimeSpan pausedTime;
        private bool isScoreboardTimerPaused;

        // Timer variables for the automovement
        private DispatcherTimer autoMoveTimer;
        private TimeSpan autoMoveTimerInterval;

        // Grid variables
        private int minHeight = 0;
        private int maxHeight = 20;
        private int minWidth = 0;
        private int maxWidth = 10;

        // Figure variables
        private TetrisFigure figure;
        private TetrisFigure nextFigure;
        private TetrisFigure swappedFigure;

        // Check variables
        private bool hasSwapped = false;
        private bool hasLost = false;
        private bool isPaused = false;
        private bool revived = false;

        //Execution directory
        private string exeDir;

        // Audio variables
        private MediaPlayer backgroundMusic = new MediaPlayer();
        private SoundPlayer gameOverMelody;
        private SoundPlayer SFXMove;
        private SoundPlayer SFXDrop;
        private SoundPlayer SFXLineClear;
        private SoundPlayer SFXTetrisClear;

        // Machine learning variables
        private int currentIndividual;
        private int currentChromosome;
        private double bestFitness;

        private Random rand;

        public GameBoard(Character character, Upgrades upgrades)
        {
            InitializeComponent();

            // Set scoreboard timer state to false to ensure it runs
            isScoreboardTimerPaused = false;

            // Set the path to the execution directory
            exeDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

            // Create a new instannce of game where gameboard is created, character is set and the settings is applied
            game = Game.Create(
                Board.Create(maxWidth + 2, maxHeight),
                character,
                JsonSerializer.Deserialize<Settings>(File.ReadAllText(exeDir + "/Assets/JSON/Settings.json")),
                upgrades);

            // Checks if the character is an AI and if so, sets the AI-specific variables.
            if (!game.IsPlayer)
            {
                currentIndividual = 0;
                currentChromosome = 0;
                bestFitness = 0.0;

                // Random is used to specifiy the order of figures. Using the same seed maked each game the same.
                rand = new Random((game.Character as AI).Seed);
            }
            else
            {
                rand = new Random();
            }


            // Set the playername in the scoreboard
            GameBoardScorePlayerLbl.Content = character.Name;

            // Set the AI panel labels
            GameBoardAINameLbl.Content = "AI Name: ";
            GameBoardGenerationLbl.Content = "Generation: ";
            GameBoardIndividualLbl.Content = "Individual: ";
            GameBoardCurrentFitnessLbl.Content = "Current Fitness: ";
            GameBoardBestFitnessLbl.Content = "Best Fitness: ";
            GameBoardLastFitnessLbl.Content = "Last Fitness: ";

            // Check if AI is enabled
            if (!game.IsPlayer)
            {
                // Set the AI panel labels
                GameBoardAINameLbl.Content = character.Name;
                GameBoardGenerationLbl.Content = "Generation: " + (game.Character as AI).GenerationNumber.ToString();
                GameBoardIndividualLbl.Content = "Individual: " + currentIndividual.ToString();
                GameBoardCurrentFitnessLbl.Content = "Current Fitness: " + (game.Character as AI).Population[currentIndividual].Fitness;
                GameBoardBestFitnessLbl.Content = "Best Fitness: " + bestFitness.ToString();
                GameBoardLastFitnessLbl.Content = "Last Fitness: " + (game.Character as AI).Population[currentIndividual].Fitness.ToString();
            }

            // Apply the settings set in the JSON settings file
            ApplySettings();
            
            // Set and start the scoreboard timer
            scoreboardTimer = new DispatcherTimer();
            StartTime(scoreboardTimer);

            // Set an invisible border for collision purposes.
            FillBoard();

            // Create a new gamegrid in the foreground
            CreateDynamicGameGrid(maxWidth, maxHeight);

            // Generate a random figure on the gameboard
            GenerateRandomFigure();

            // Generate and set the next block in the next block board
            NextToGame();

            //Music start
            MusicStart();

            // Initialize a pause menu in this game
            pauseMenu = new PauseMenu(this);

            if (game.Upgrades.EmergancyLineClear == 0)
            {
                GameBoardActionsConsumeOneBtn.IsEnabled = false;
            }
            if (game.Upgrades.SlowTime == 0)
            {
                GameBoardActionsConsumeTwoBtn.IsEnabled = false;
            }
            if (!game.Upgrades.RemoveSwap)
            {
                GameBoardActionsConsumeThreeBtn.IsEnabled = false;
            }
        }

        // A function that pauses the game and opens the pause menu
        public void TogglePauseGame()
        {
            // Checking if the game is paused or not
            if (!isPaused)
            {
                // Game timers is stopped
                autoMoveTimer.Stop();
                PauseTime(scoreboardTimer);

                // The bool that holds the pause state is set to true to indicate a pause is triggered
                isPaused = true;

                // Add the pausemenu to the gameboardmaingrid
                GameBoardMainGrid.Children.Add(pauseMenu);

                // Position the pause menu on the gameboardmaingrid
                Grid.SetColumn(pauseMenu, 1);
                Grid.SetRow(pauseMenu, 1);
                Grid.SetColumnSpan(pauseMenu, 5);
                Grid.SetRowSpan(pauseMenu, 7);
            }
            else
            {
                // The bool that holds the pause state is set to false to indicate a pause is stopped
                isPaused = false;

                // Resume and start the timers
                autoMoveTimer.Start();
                ResumeTime(scoreboardTimer);

                // Remove the pausemenu from the gameboardmaingrid
                GameBoardMainGrid.Children.Remove(pauseMenu);
            }
        }

        // A function that adds insivible blocks on the sides of the game grid. Used for collision.
        private void FillBoard()
        {
            // Makes sure the board is clear.
            game.Board.Squares.Clear();

            // Goes through the height of the board.
            for (int i = 0; i < maxHeight; i++)
            {
                // Adds a block to the left, just outside of the visual grid.
                game.Board.Squares.Add(new Square(-1, i, "BluePrimary"));

                // Adds a block to the right, just outside of the visual grid.
                game.Board.Squares.Add(new Square(maxWidth, i, "BluePrimary"));
            }
        }

        // Visualizes a list of squares in a specific grid.
        private void DrawSquares(Square[] squares, Grid grid)
        {
            // Goes through each square.
            foreach (Square square in squares)
            {
                // Sets the X and Y coodinates of the square to the grid.
                Grid.SetColumn(square.Image, square.CoordinateX);
                Grid.SetRow(square.Image, square.CoordinateY);
                
                // Adds the square to the grid.
                grid.Children.Add(square.Image);
            }
        }

        // Visually removes a list of squares from a specific grid.
        private void EraseSquares(Square[] squares, Grid grid)
        {
            // Goes through each square,
            foreach (Square square in squares)
            {
                // Removes it from the grid.
                grid.Children.Remove(square.Image);
            }
        }

        // Rotates the figure on the game board.
        private void RotateFigure()
        {
            // Visually removes the figure.
            EraseSquares(figure.Squares, GameBoardGameGrid);

            // Rotates the figure 90 degrees.
            figure.Rotate();

            // Checks if the figure shares space with a square on the board.
            if (!CanRotate())
            {
                // If the figure shouldn't be able to rotate, it rotates 270 degrees more, returning to its original position.
                figure.Rotate();
                figure.Rotate();
                figure.Rotate();
            }

            // Moves the figure if it is outside of bounds.
            Kickback();

            // Visualizes the figure.
            DrawSquares(figure.Squares, GameBoardGameGrid);
        }

        // A figure to move the figure on the game board.
        private void MoveFigure(EDirection direction)
        { 
            // Checks if the figure is able to move the direction.
            if (Collision(direction))
            {
                // Checks if the figure is able to move down.
                if (direction == EDirection.down)
                {
                    // Plays a sound and adds the figure to the board.
                    SFXDrop.Play();
                    FigureToBoard();
                }
                return;
            }

            // Removes the figure visually, moves it, then visualizes it again.
            EraseSquares(figure.Squares, GameBoardGameGrid);

            
            figure.Move(direction);
            DrawSquares(figure.Squares, GameBoardGameGrid);
        }

        // A function that moves the figure immediately to the bottom.
        private void InstaDrop()
        {
            // Continues to run until it collides.
            while (!Collision(EDirection.down))
            {
                // Removes the figure visually, moves it, then visualizes it again.
                EraseSquares(figure.Squares, GameBoardGameGrid);
                figure.Move(EDirection.down);
                DrawSquares(figure.Squares, GameBoardGameGrid);
            }

            // Moves the figure down once more, triggering anything that would normally happen when it moves down.
            MoveFigure(EDirection.down);
        }

        // A function that generates a random figure in the Next Block grid.
        private void GenerateRandomFigure()
        {
            // Generates a random number between 0 and thene amount of figure types.
            int randomNumber = rand.Next(0, Enum.GetNames(typeof(EFigureType)).Length);

            // Sets nextFigure to a new TetrisFigure with coords for the grid and the random number to specify the FigureType
            nextFigure = new TetrisFigure(new int[]{ 1,0},(EFigureType)randomNumber);

            // Visualizes the Next Block grid.
            DrawSquares(nextFigure.Squares, GameBoardNextGrid);
        }

        // A function that moves nextFigure to the Game Board.
        private void NextToGame()
        {
            // Removes visualization of the figure in the Next Block grid.
            EraseSquares(nextFigure.Squares, GameBoardNextGrid);

            // Sets the Game Board figure and moves it to the right position.
            figure = nextFigure;
            figure.MoveTo(new int[]{4,0});

            // Visualizes the figure.
            DrawSquares(figure.Squares, GameBoardGameGrid);

            // Checks if a figure have been swapped, otherwise start an automove timer.
            if (!hasSwapped) StartAutoMove();

            // Generates a new figure in the Next Block grid.
            GenerateRandomFigure();
        }

        // A function that swaps between the Swap Block grid and the Game Board.
        private void Swap()
        {
            TetrisFigure reserve;

            // Checks if a figure already have swapped or if it is disabled in the settings.
            if (!hasSwapped && game.Settings.EnableSwapBlock)
            {
                // Swapping is now happening.
                hasSwapped = true;

                // Removes the figure visually from the Game Board.
                EraseSquares(figure.Squares, GameBoardGameGrid);

                // Checks if the Swap Block grid is empty.
                if (swappedFigure == null)
                {
                    // Moves the figure into Swap Block and sets it coordinates.
                    swappedFigure = figure;
                    swappedFigure.MoveTo(new int[] { 1, 0 });

                    // Moves a figure from Next Block to Game Board.
                    NextToGame();
                }
                else
                {
                    // Removes visualization of the figure in the Swap Block grid.
                    EraseSquares(swappedFigure.Squares, GameBoardSwapGrid);

                    // Temporarily stores the Swap Block figure.
                    reserve = swappedFigure;

                    // Moves the figure into Swap Block and sets it coordinates
                    swappedFigure = figure;
                    swappedFigure.MoveTo(new int[] { 1, 0 });

                    // Moves the reserve into Game Board and sets it coordinates
                    figure = reserve;
                    figure.MoveTo(new int[] { 4, 0 });

                    // Visualizes the block in the Game Board
                    DrawSquares(figure.Squares, GameBoardGameGrid);
                }

                // Visualizes the block in the Swap Block grid.
                DrawSquares(swappedFigure.Squares, GameBoardSwapGrid);
            }
        }

        // A function that moves the squares of a figure to the Game Board.
        private void FigureToBoard()
        {
            // Adds the squares of the figure to the board.
            game.Board.Squares.AddRange(figure.Squares);

            // Checks if any lines are getting cleared.
            ClearLine();

            // Stops the autoMoveTimer and resets hasSwapped
            autoMoveTimer.Stop();
            hasSwapped = false;

            // Checks if the game is lost.
            LoseGame();

            // If the game is not lost or paused, then the game will continue.
            if (!hasLost && !isPaused)
            {
                NextToGame();
            }
        }

        // A function that checks collison on move.
        private bool Collision(EDirection move)
        {
            bool result = false;

            // Checks each square in the figure.
            foreach (Square square in figure.Squares)
            {
                // Checks which direction the figure is moving. Then checks if it goes outside of bounds or collides with a block.
                switch (move)
                {
                    case EDirection.left:
                        if (square.CoordinateX == minWidth || BlockCollision(square, move))
                        {
                            result = true;
                        }
                        break;
                    case EDirection.right:
                        if (square.CoordinateX == maxWidth - 1 || BlockCollision(square, move))
                        {
                            result = true;
                        }
                        break;
                    case EDirection.up:
                        if (square.CoordinateY == minHeight || BlockCollision(square, move))
                        {
                            result = true;
                        }
                        break;
                    case EDirection.down:
                        if (square.CoordinateY == maxHeight - 1 || BlockCollision(square, move))
                        {
                            result = true;
                        }
                        break;
                    default:
                        break;
                }
            }
            return result;
        }

        // A function that checks if a square is colliding with a block if it moves.
        private bool BlockCollision(Square square, EDirection direction)
        {
            bool result = false;

            int x = 0;
            int y = 0;

            // Checks what direction it moves, and sets which direction it needs to look out for.
            switch (direction)
            {
                case EDirection.left:
                    x = -1;
                    break;
                case EDirection.right:
                    x = 1;
                    break;
                case EDirection.up:
                    y = -1;
                    break;
                case EDirection.down:
                    y = 1;
                    break;
                default:
                    break;
            }

            // Checks if any square of the figure is moving into a square of the board.
            int collide = game.Board.Squares.Where(board => board.CoordinateX == square.CoordinateX + x && board.CoordinateY == square.CoordinateY + y).Count();
            if (collide != 0)
            {
                result = true;
            }

            return result;
        }

        // A function that checks if any squares of the figure overlaps with the board.
        private bool CanRotate()
        {
            // Goes through each square in the figure.
            foreach (Square square in figure.Squares)
            {
                // Checks if the square is overlapping with the board.
                int collide = game.Board.Squares.Where(board => board.CoordinateX == square.CoordinateX && board.CoordinateY == square.CoordinateY).Count();

                if (collide != 0)
                {
                    return false;
                }
            }
            return true;
        }

        // A function that moves the figure if it is outside of bounds.
        private void Kickback()
        {
            // Goes through each square of the figure.
            foreach (Square square in figure.Squares)
            {
                // If the square is outside of the board, then it is moved in the opposite direction.
                if (square.CoordinateX < minWidth)
                {
                    figure.Move(EDirection.right);
                }
                else if (square.CoordinateX > maxWidth - 1)
                {
                    figure.Move(EDirection.left);
                }
                else if (square.CoordinateY < minHeight)
                {
                    figure.Move(EDirection.down);
                }
                else if (square.CoordinateY > maxHeight - 1)
                {
                    figure.Move(EDirection.up);
                }
            }
        }

        // A function that creates a new gamegrid in the foreground with a specific grid size given
        private void CreateDynamicGameGrid(int cols, int rows)
        {
            // An instance of the gameboardgamegrid to add content to
            Grid gamegrid = GameBoardGameGrid;

            // Clear current gamegrid
            gamegrid.Children.Clear();

            // Remove current definitions
            gamegrid.RowDefinitions.Clear();
            gamegrid.ColumnDefinitions.Clear();

            // Create new row definitions based on the rows given
            for (int i = 0; i < rows; i++)
            {
                // Add a new row definition
                gamegrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            }

            // Create new row definitions based on the columns given
            for (int i = 0; i < cols; i++)
            {
                // Add a new column definition
                gamegrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }

            // Looping all rows
            for (int row = 0; row < rows; row++)
            {
                // Looping all columns
                for (int column = 0; column < cols; column++)
                {
                    // Creating a border
                    Border border = new Border();
                    border.BorderBrush = Brushes.Silver;
                    border.BorderThickness = new Thickness(1);

                    // Child alignment
                    border.HorizontalAlignment = HorizontalAlignment.Stretch;
                    border.VerticalAlignment = VerticalAlignment.Stretch;

                    // Add coordinates to the border
                    Grid.SetRow(border, row);
                    Grid.SetColumn(border, column);

                    // Add child to grid
                    gamegrid.Children.Add(border);
                }
            }
        }

        //Function to clear lines and call add points
        private void ClearLine()
        {
            //Lineclear variable
            int linesCleared = 0;

            //For loop that runs through each row in the gamegrid
            for (int i = 0; i < maxHeight; i++)
            {
                //Here we get the squares in the current row of the game grid
                List<Square> line = game.Board.Squares.Where(s => s.CoordinateY == i && s.CoordinateX >= 0 && s.CoordinateX < maxWidth).ToList();
                //If statment to check if the amount of square equal the max width
                if (line.Count() >= maxWidth)
                {
                    //Foreach loop to go through each square in the row
                    foreach (Square square in line)
                    {
                        //Remove the square from the board (functionaly)
                        game.Board.Squares.Remove(square);
                        //Remove the square from the board (visualy)
                        GameBoardGameGrid.Children.Remove(square.Image);
                    }
                    //Here we get the squares above the current row of the game grid
                    List<Square> above = game.Board.Squares.Where(s => s.CoordinateY < i && s.CoordinateX >= 0 && s.CoordinateX < maxWidth).ToList();
                    //Foreach loop to go through each sqaure in the above rows
                    foreach (Square square in above)
                    {
                        //Moves each square down one Y coordinate
                        square.CoordinateY++;
                    }
                    //Visualy erase all squares above
                    EraseSquares(above.ToArray(), GameBoardGameGrid);
                    //Visualy draw all squares above
                    DrawSquares(above.ToArray(), GameBoardGameGrid);

                    //Add line clear to total line clear
                    game.LinesCleared++;
                    //Call speed up function
                    SpeedUp();
                    //local line clear variable plus 1
                    linesCleared++;
                }
            }
            //If statment checking if local line clear variable is more then 0
            if (linesCleared > 0)
            {
                //Call add point function
                AddPoint(linesCleared, game.Upgrades.ScoreMultiplier);
            }
        }

        // A function that checks if the game is lost.
        private void LoseGame()
        {
            // Checks if there are more than 2 squares on the top of the board.
            if (game.Board.Squares.Where(s => s.CoordinateY == 0).Count() > 2)
            {
                // Checks if AI Training is enabled.
                if (game.Settings.EnableTraining)
                {
                    // Checks if the character is an AI.
                    if (!game.IsPlayer)
                    {
                        // Calculates fitness for the current individual.
                        double currentFitness = EvaluateFitness();
                        (game.Character as AI).Population[currentIndividual].Fitness = currentFitness;

                        // Checks and assigns the best fitness.
                        if (currentFitness > bestFitness)
                        {
                            bestFitness = currentFitness;
                        }

                        // Advances the current individual.
                        currentIndividual++;

                        // Checks if the current individual is at the end of the population.
                        if (currentIndividual == (game.Character as AI).Population.Count())
                        {
                            // Advances to the next generation.
                            NextGeneration();
                            currentIndividual = 0;
                            (game.Character as AI).GenerationNumber++;
                        }
                    }

                    // Clears all three boards and clears lines and points.
                    ClearBoard();
                    game.LinesCleared = 0;
                    game.Points = 0;

                    // Checks if the character is a player.
                    if (!game.IsPlayer)
                    {
                        double currentFitness = EvaluateFitness();

                        // Set the AI panel labels
                        GameBoardAINameLbl.Content = game.Character.Name;
                        GameBoardGenerationLbl.Content = "Generation: " + (game.Character as AI).GenerationNumber.ToString();
                        GameBoardIndividualLbl.Content = "Individual: " + currentIndividual.ToString();
                        GameBoardLastFitnessLbl.Content = "Last Fitness: " + currentFitness.ToString();
                        GameBoardBestFitnessLbl.Content = "Best Fitness: " + bestFitness.ToString();

                        // Resets the Random.
                        rand = new Random((game.Character as AI).Seed);
                    }
                    // Resets the timer.
                    scoreboardTimer = new DispatcherTimer();

                    // Sets the board.
                    SetBoard();
                }
                else
                {
                    // Ends the game.
                    GameOver();
                    hasLost = true;
                }
            }
        }

        // Public overload of LoseGame with manual lose condition.
        public void LoseGame(bool lose)
        {
            // Checks if the game is lost.
            if (lose)
            {
                // Ends the game.
                GameOver();
                hasLost = true;
            }
        }

        // A function that trigger a gameover event
        private void GameOver()
        {
        
            // Stop the current game timers
            
            game.Time = Convert.ToInt32(elapsedTime.TotalMilliseconds);
            if (!revived)
            {
                DatabaseService.CreateLeaderboardEntry(game);
            }
            else
            {
                DatabaseService.UpdateLeaderboardEntry(game);
            }
            
            PauseTime(scoreboardTimer);
            autoMoveTimer.Stop();

            // Start game over music
            gameOverMelody.Play();
            backgroundMusic.Stop();
            

            // Add the game over menu to the gameboardmaingrid
            
            GameOverMenu menu = new GameOverMenu(this);
            
            GameBoardMainGrid.Children.Add(menu);
            
            // Position the game over menu on the gamegboardmain grid
            Grid.SetColumn(menu, 1);
            Grid.SetRow(menu, 1);
            Grid.SetColumnSpan(menu, 5);
            Grid.SetRowSpan(menu, 7);

            // Resource cleanup
            ClearBoard();
        }

        // A function that cleans up the grids and sounds
        private void ClearBoard()
        {
            // Clear the gamboard of content
            GameBoardGameGrid.Children.Clear();
            figure = null;
            game.Board.Squares.Clear();
            GameBoardNextGrid.Children.Clear();
            nextFigure = null;
            GameBoardSwapGrid.Children.Clear();
            swappedFigure = null;
        }

        // A function that sets the board,
        private void SetBoard()
        {
            // Starts the timer.
            StartTime(scoreboardTimer);

            // Adds invisible borders to the board.
            FillBoard();

            // Creates the visual grid.
            CreateDynamicGameGrid(maxWidth,maxHeight);

            // Gets the first figure.
            GenerateRandomFigure();
        }

        // A function to move the AI.
        private void AIMove()
        {
            // Calculates the outputs.
            double calcMove = CalculateOutput();
            double calcDown = CalculateOutput();
            double calcRotate = CalculateOutput();
            double calcSwap = CalculateOutput();

            // Decides if the figure is going to move left, right, or not at all.
            if (calcMove < 0)
            {
                MoveFigure(EDirection.left);
            }
            else if (calcMove > 0)
            {
                MoveFigure(EDirection.right);
            }

            // Decides if the figure is going to go down slow, quick or not at all.
            if (calcDown < 0)
            {
                MoveFigure(EDirection.down);
            }
            else if(calcDown > 0)
            {
                InstaDrop();
            }

            // Decides if the figure is going to rotate.
            if (calcRotate > 0)
            {
                RotateFigure();
            }

            // Decides if the figure is going to swap.
            if (calcSwap > 0)
            {
                Swap();
            }

            currentChromosome = 0;
        }

        // A function to calculate the outputs of the AI.
        private double CalculateOutput()
        {
            Individual individual = (game.Character as AI).Population[currentIndividual];

            int output = 0;

            // Calculates the output with the following formula.
            // output_n = (input_x * chromosone[x]) + (input_x+1 * chromosone[x+1]) + (input_x+2 * chromosone[x+2]) +...

            // Goes through each square on the board.
            for (int i = 0; i < game.Board.Squares.Count(); i++)
            {
                // Adds to the output and increases the current chromosome.
                output += game.Board.Squares[i].CoordinateX * individual.Chromosomes[currentChromosome];
                output += game.Board.Squares[i].CoordinateY * individual.Chromosomes[currentChromosome];
                currentChromosome++;
            }

            // Skips chromosomes for each unused square on the board.
            currentChromosome += game.Board.GridSize - game.Board.Squares.Count();

            // Goes through each square in figure.
            for (int i = 0; i < figure.Squares.Length; i++)
            {
                // Adds to the output and increases the current chromosome.
                output += figure.Squares[i].CoordinateX * individual.Chromosomes[currentChromosome];
                output += figure.Squares[i].CoordinateY * individual.Chromosomes[currentChromosome];
                currentChromosome++;
            }

            // Goes through each square in nextFigure.
            for (int i = 0; i < nextFigure.Squares.Length; i++)
            {
                // Adds to the output and increases the current chromosome.
                output += nextFigure.Squares[i].CoordinateX * individual.Chromosomes[currentChromosome];
                output += nextFigure.Squares[i].CoordinateY * individual.Chromosomes[currentChromosome];
                currentChromosome++;
            }

            // Skips swappedFigure unless it is not null. 
            if (swappedFigure == null)
            {
                currentChromosome += 4;
            }
            else
            {
                // Goes through each square in swappedFigure.
                for (int i = 0; i < swappedFigure.Squares.Length; i++)
                {
                    // Adds to the output and increases the current chromosome.
                    output += swappedFigure.Squares[i].CoordinateX * individual.Chromosomes[currentChromosome];
                    output += swappedFigure.Squares[i].CoordinateY * individual.Chromosomes[currentChromosome];
                    currentChromosome++;
                }
            }

            return output;
        }

        // A function to determine the next generation.
        private void NextGeneration()
        {
            List<Individual> newPopulation = new List<Individual>();
            Individual[] population = (game.Character as AI).Population;

            // Decides on a random individual to pair with the best.
            Random random = new Random();
            Individual mate = population[random.Next(population.Length)];


            // Sets the individual with the highest fitness to be first in the next generation.
            newPopulation.Add(population.First(i => i.Fitness == population.Max(i => i.Fitness)));

            // Makes 4 pairings between the best and the mate.
            for (int i = 0; i < 4; i++)
            {
                newPopulation.Add(Reproduce(newPopulation[0], mate, random));
            }

            // Fills the rest of the array with completely new individuals.
            for (int i = newPopulation.Count(); i < population.Length; i++)
            {
                newPopulation.Add(new Individual(newPopulation[0].Chromosomes.Length));
            }

            (game.Character as AI).Population = newPopulation.ToArray();
        }

        // A function to make a pairing between two individuals.
        private Individual Reproduce(Individual first, Individual second, Random random)
        {
            // Designates the result to be a clone of the first.
            Individual result = new Individual(first.Chromosomes, 0.0);

            // Goes through half of the chromosomes.
            for (int i = 0; i < (first.Chromosomes.Length / 2); i++)
            {
                // Determines which chromosome is getting changed.
                int rng = random.Next(first.Chromosomes.Length);

                // Changes the chromosome to the second individuals.
                result.Chromosomes[rng] = second.Chromosomes[rng];
            }

            // Adds a single random mutation.
            result.Chromosomes[random.Next(first.Chromosomes.Length)] = result.RandomChromosome();

            return result;
        }

        // A function that evalutates the current fitness.
        private double EvaluateFitness()
        {
            return game.Points + elapsedTime.TotalSeconds;
        }

        // A function that starts the automove timer
        private void StartAutoMove()
        {
            // Instantiate a new dispatchertimer to run the automovement
            autoMoveTimer = new DispatcherTimer();

            // Set the interval of the timer in milliseconds
            autoMoveTimer.Interval = autoMoveTimerInterval;

            // Set the event that happends on tick
            autoMoveTimer.Tick += AutoMove_Tick;

            // Start the timer
            autoMoveTimer.Start();
        }

        // The automove event
        private void AutoMove_Tick(object sender, EventArgs e)
        {
            // Move the figure down
            MoveFigure(EDirection.down);
            // Checks if the character is an AI.
            if (!game.IsPlayer && !hasLost)
            {
                // Move the figure.
                AIMove();

                // Assing current fitnes and update the UI.
                (game.Character as AI).Population[currentIndividual].Fitness = EvaluateFitness();
                GameBoardCurrentFitnessLbl.Content = "Current Fitness: " + (game.Character as AI).Population[currentIndividual].Fitness;

            }
        }

        // A function that starts the score timer
        private void StartTime(DispatcherTimer timer)
        {
            // Set the interval of the timer in milliseconds
            timer.Interval = TimeSpan.FromMilliseconds(1);

            // Set the event that happends on tick
            timer.Tick += Timer_Tick;

            // Save the start time of the timer
            startTime = DateTime.Now;

            // Start the timer
            timer.Start();
        }

        // The score timer event
        private void Timer_Tick(object sender, EventArgs e)
        {
            // Check if the scoreboardtimer is paused
            if (!isScoreboardTimerPaused)
            {
                // Calculate the elapsed time
                TimeSpan currentTime = DateTime.Now - startTime;

                // Add the calculated time to the elapsed time
                elapsedTime = currentTime + pausedTime;

                // Update the timer label in the UI
                if (elapsedTime.TotalMilliseconds >= 0)
                {
                    // Custom formatting the timer
                    string formattedTime = $"{elapsedTime.Hours:D2}:{elapsedTime.Minutes:D2}:{elapsedTime.Seconds:D2}:{elapsedTime.Milliseconds:D3}";

                    // Add the timer to the scoreboard
                    GameBoardScoreTimeLbl.Content = formattedTime;
                }
                else
                {
                    // Backup if timer fails the formatting
                    GameBoardScoreTimeLbl.Content = "00:00:00:000";
                }
            }
        }

        // A function that pauses the scoretimer and preserves the timer
        private void PauseTime(DispatcherTimer timer)
        {
            // Change the scoreboardtimer status
            isScoreboardTimerPaused = true;

            // Stop the timer
            timer.Stop();

            // Increase the current pause time
            pausedTime += DateTime.Now - startTime;
        }

        // A function that resumes the scoretimer from where it was paused
        private void ResumeTime(DispatcherTimer timer)
        {
            // Change the scoreboardtimer status
            isScoreboardTimerPaused = false;

            // Update the starttime to the time of resume
            startTime = DateTime.Now;

            // Start the timer
            timer.Start();
        }

        //A function to load and start all music/sounds
        private void MusicStart()
        {
            //Load in background music
            backgroundMusic.Open(new Uri(exeDir + "\\Assets\\Sound\\Tetris99MainTheme.mp3", UriKind.Absolute));
            //add an event handler that triggers at the end of the song starting the song over
            backgroundMusic.MediaEnded += new EventHandler((sender, e) =>
            {
                ((MediaPlayer)sender).Position = TimeSpan.Zero;
            });
            //Start background music
            backgroundMusic.Play();

            //Load all the sound effects
            gameOverMelody = new SoundPlayer(exeDir + "\\Assets\\Sound\\MusicGameOver.wav");
            SFXMove = new SoundPlayer(exeDir + "\\Assets\\Sound\\SFXMove.wav");
            SFXDrop = new SoundPlayer(exeDir + "\\Assets\\Sound\\SFXDrop.wav");
            SFXLineClear = new SoundPlayer(exeDir + "\\Assets\\Sound\\SFXLineClear.wav");
            SFXTetrisClear = new SoundPlayer(exeDir + "\\Assets\\Sound\\SFXTetrisClear.wav");
        }

        // A function that speeds up the automove timer
        private void SpeedUp()
        {
        
            // Every ten lines cleared
            
            if (game.LinesCleared % 10 == 0 & game.LinesCleared != 0)
            
            {
                // Recalculating the automovetimerinterval setting the value lower to increase the tick speed
                autoMoveTimerInterval -= autoMoveTimerInterval * (game.Settings.GameSpeed / 100);
            }
        }

        // A function that adds points to the score
        private void AddPoint(int lines, double mulitplier)
        {
            // Sounds played on point gain
            if(lines == 4)
            {
                // Play this tetris clear sound when 4 lines is cleared
                SFXTetrisClear.Play();
            }
            else
            {
                // Play this SFX whenn 3 or less lines are cleared
                SFXLineClear.Play();
            }
                        
            // Recalculate the points and doubling the points added accordingly to the amount of lines cleared
            game.Points += Convert.ToInt32(((int)Math.Pow(2, lines) * 100) * mulitplier);

            // Update the scoreboard labels
            GameBoardScorePointLbl.Content = "Point: " + game.Points;
            GameBoardScoreLineClearedLbl.Content = "Lines: " + game.LinesCleared;
            
        }

        //A function that applies the current settings to the board
        private void ApplySettings()
        {
            //If statment to check what setting enable next block is and then sets the visibility to the respective options
            if (game.Settings.EnableNextBlock)
            {
                GameBoardNextBlockBorder.Visibility = Visibility.Visible;
            }
            else
            {
                GameBoardNextBlockBorder.Visibility = Visibility.Hidden;
            }

            //If statment to check what setting enable swap block is and then sets the visibility to the respective options
            if (game.Settings.EnableSwapBlock)
            {
                GameBoardSaveBlockBorder.Visibility = Visibility.Visible;
            }
            else
            {
                GameBoardSaveBlockBorder.Visibility = Visibility.Hidden;
            }

            //Sets the inital auto move timer interval
            autoMoveTimerInterval = TimeSpan.FromMilliseconds(game.Settings.StartSpeed);

            //sets the audio level of the background music
            backgroundMusic.Volume = Convert.ToDouble(game.Settings.Volume) / 100;

            // Setting the controls from the keybinds in the controls panel
            GameBoardControlsPause.Content = "Pause: " + game.Settings.KeyBinds.Pause.ToString();
            GameBoardControlsSave.Content = "Swap: " + game.Settings.KeyBinds.Swap.ToString();
            GameBoardControlsRotate.Content = "Rotate: " + game.Settings.KeyBinds.Rotate.ToString();
            GameBoardControlsLeft.Content = "Left: " + game.Settings.KeyBinds.Left.ToString();
            GameBoardControlsRight.Content = "Right: " + game.Settings.KeyBinds.Right.ToString();
            GameBoardControlsDown.Content = "Down: " + game.Settings.KeyBinds.Drop.ToString();
            GameBoardControlsInstantDown.Content = "Instant Down: " + game.Settings.KeyBinds.Insta.ToString();

            // Check if AI is enabled
            if (!game.IsPlayer)
            {
                // Removing the controls from the keybinds in the controls panel if the AI is active
                GameBoardControlsPause.Content = "Pause: ";
                GameBoardControlsSave.Content = "Swap: ";
                GameBoardControlsRotate.Content = "Rotate: ";
                GameBoardControlsLeft.Content = "Left: ";
                GameBoardControlsRight.Content = "Right: ";
                GameBoardControlsDown.Content = "Down: ";
                GameBoardControlsInstantDown.Content = "Instant Down: ";
            }
        }

        // A function that sets the incoming settings to the current settings and calls the private ApplySettings
        public void ApplySettings(Settings settings)
        {
            //sets the current settings
            game.Settings = settings;
            //Applies the current settings to the board
            ApplySettings();
        }

        public void Revive()
        {
            hasLost = false;
            revived = true;
            ClearBoard();
            game.Upgrades.Revive--;
            backgroundMusic.Play();
            SetBoard();
            ResumeTime(scoreboardTimer);
            NextToGame();
        }

        // A UI button that triggers the pause menu
        private void GameBoardActionsPauseBtn_Click(object sender, RoutedEventArgs e)
        {
            // Trigger the pause menu
            TogglePauseGame();
        }

        // UI button for using the not yet planned consumeable within the game
        private void GameBoardActionsConsumeOneBtn_Click(object sender, RoutedEventArgs e)
        {
            for (int y = maxHeight - 2; y < maxHeight; y++)
            {
                for (int x = 0; x < maxWidth; x++)
                {
                    game.Board.Squares.Add(new Square(x, y, "GreenPrimary"));
                }
            }
            double originalMultiplier = game.Upgrades.ScoreMultiplier;
            game.Upgrades.ScoreMultiplier = 0.5;
            ClearLine();
            game.Upgrades.ScoreMultiplier = originalMultiplier;
            game.Upgrades.EmergancyLineClear--;
            if(game.Upgrades.EmergancyLineClear == 0)
            {
                GameBoardActionsConsumeOneBtn.IsEnabled = false;
            }
        }

        // UI button for using the not yet planned consumeable within the game
        private void GameBoardActionsConsumeTwoBtn_Click(object sender, RoutedEventArgs e)
        {
            autoMoveTimer.Interval *= 2;
            game.Settings.StartSpeed *= 2;
            DispatcherTimer slowTime = new DispatcherTimer();
            slowTime.Interval = TimeSpan.FromSeconds(game.Upgrades.SlowTime);
            slowTime.Tick += (object sender, EventArgs e) =>
            {
                autoMoveTimer.Interval /=  2;
                game.Settings.StartSpeed /= 2;
                GameBoardActionsConsumeTwoBtn.IsEnabled = false;
                slowTime.Stop();
            };
            slowTime.Start();
        }

        // UI button for using the not yet planned consumeable within the game
        private void GameBoardActionsConsumeThreeBtn_Click(object sender, RoutedEventArgs e)
        {
            if (swappedFigure != null)
            {
                EraseSquares(swappedFigure.Squares, GameBoardSwapGrid);
                swappedFigure = null;
                hasSwapped = false;
                game.Upgrades.RemoveSwap = false;
                GameBoardActionsConsumeThreeBtn.IsEnabled = false;
            }
        }

        //A key down event handler
        public void GamePage_KeyDown(object sender, KeyEventArgs e)
        {
            //If statment checking if the player has lost and the player is a player
            if (!hasLost && game.IsPlayer)
            {
                //If statment checking if the inputed key is the pause keybind
                if(e.Key == game.Settings.KeyBinds.Pause)
                {
                    // Trigger the pause menu
                    TogglePauseGame();
                }
                //If statment checking if the game is paused
                if (!isPaused)
                {
                    //Switch case on the inputed key
                    switch (e.Key)
                    {
                        //Case When the inputed key = kebind left
                        case Key k when k == game.Settings.KeyBinds.Left:
                            //Play move sfx
                            SFXMove.Play();
                            //Call the move figure function
                            MoveFigure(EDirection.left);
                            break;
                        //Case When the inputed key = kebind right
                        case Key k when k == game.Settings.KeyBinds.Right:
                            //Play move sfx
                            SFXMove.Play();
                            //Call the move figure function
                            MoveFigure(EDirection.right);
                            break;
                        //Case When the inputed key = kebind rotate
                        case Key k when k == game.Settings.KeyBinds.Rotate:
                            //Call the Rotate Figure
                            RotateFigure();
                            break;
                        //Case When the inputed key = kebind drop
                        case Key k when k == game.Settings.KeyBinds.Drop:
                            //Call the move figure function
                            MoveFigure(EDirection.down);
                            break;
                        //Case When the inputed key = kebind insta
                        case Key k when k == game.Settings.KeyBinds.Insta:
                            //Call the insta drop function
                            InstaDrop();
                            break;
                        //Case When the inputed key = kebind swap
                        case Key k when k == game.Settings.KeyBinds.Swap:
                            //Call the swap figure function
                            Swap();
                            break;
                        default: break;
                    }
                }
            }
        }

        //This function adds the above keydown event to the window keydown event on page load
        private void GamePage_Loaded(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).KeyDown += GamePage_KeyDown;
        }
    }
}

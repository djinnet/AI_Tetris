using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
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

        private Random rand;

        public GameBoard(Character character)
        {
            InitializeComponent();

            // Set scoreboard timer state to false to ensure it runs
            isScoreboardTimerPaused = false;

            // Set the path to the execution directory
            exeDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

            // Create a new instannce of game where gameboard is created, character is set and the settings is applied
            game = new Game(
                new Board(maxWidth + 2, maxHeight),
                character,
                JsonSerializer.Deserialize<Settings>(File.ReadAllText(exeDir + "/Assets/JSON/Settings.json")));

            // Todo! - Sebastian - Dokumentation
            if (!game.isPlayer)
            {
                currentIndividual = 0;
                currentChromosome = 0;
                rand = new Random((game.character as AI).seed);
            }
            else
            {
                rand = new Random();
            }


            // Set the playername in the scoreboard
            GameBoardScorePlayerLbl.Content = character.name;

            // Set the AI panel labels
            GameBoardAINameLbl.Content = "";
            GameBoardGenerationLbl.Content = "";
            GameBoardIndividualLbl.Content = "";
            GameBoardLastFitnessLbl.Content = "";

            // Check if AI is enabled
            if (!game.isPlayer)
            {
                // Set the AI panel labels
                GameBoardAINameLbl.Content = character.name;
                GameBoardGenerationLbl.Content = "TO DO SEBASTIAN :)";
                GameBoardIndividualLbl.Content = currentIndividual.ToString();
                GameBoardLastFitnessLbl.Content = "TO DO SEBASTIAN :)";
            }

            // Apply the settings set in the JSON settings file
            ApplySettings();
            
            // Set and start the scoreboard timer
            scoreboardTimer = new DispatcherTimer();
            StartTime(scoreboardTimer);

            // Todo! - Sebastian - Dokumentation
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

        // Todo! - Sebastian - Dokumentation
        private void FillBoard()
        {
            game.board.squares.Clear();
            for (int i = 0; i < maxHeight; i++)
            {
                game.board.squares.Add(new Square(-1, i, "BluePrimary"));
                game.board.squares.Add(new Square(maxWidth, i, "BluePrimary"));
            }
        }

        // Todo! - Sebastian - Dokumentation
        private void DrawSquares(Square[] squares, Grid grid)
        {
            foreach (Square square in squares)
            {
                Grid.SetColumn(square.image, square.coordinateX);
                Grid.SetRow(square.image, square.coordinateY);
                
                grid.Children.Add(square.image);
            }
        }

        // Todo! - Sebastian - Dokumentation
        private void EraseSquares(Square[] squares, Grid grid)
        {
            foreach (Square square in squares)
            {
                grid.Children.Remove(square.image);
            }
        }

        // Todo! - Sebastian - Dokumentation
        private void RotateFigure()
        {
            
            if (CanRotate())
            {
                EraseSquares(figure.squares, GameBoardGameGrid);
                figure.Rotate();
                if (!CanRotate())
                {
                    figure.Rotate();
                    figure.Rotate();
                    figure.Rotate();
                }
                Kickback();
                DrawSquares(figure.squares, GameBoardGameGrid);
            }
        }

        // Todo! - Sebastian - Dokumentation
        private void MoveFigure(string destination)
        {
            if (Collision(destination))
            {
                if (destination == "down")
                {
                    SFXDrop.Play();
                    FigureToBoard();
                }
                return;
            }

            EraseSquares(figure.squares, GameBoardGameGrid);
            figure.Move(destination);
            DrawSquares(figure.squares, GameBoardGameGrid);
        }

        // Todo! - Sebastian - Dokumentation
        private void InstaDrop()
        {
            while (!Collision("down"))
            {
                EraseSquares(figure.squares, GameBoardGameGrid);
                figure.Move("down");
                DrawSquares(figure.squares, GameBoardGameGrid);
            }
            MoveFigure("down");
        }

        // Todo! - Sebastian - Dokumentation
        private void GenerateRandomFigure()
        {
            var i = rand.Next(0, Enum.GetNames(typeof(FigureType)).Length);
            nextFigure = new TetrisFigure(new int[]{ 1,0},(FigureType)i);

            DrawSquares(nextFigure.squares, GameBoardNextGrid);
        }

        // Todo! - Sebastian - Dokumentation
        private void NextToGame()
        {
            EraseSquares(nextFigure.squares, GameBoardNextGrid);
            figure = nextFigure;
            figure.MoveTo(new int[]{4,0});
            DrawSquares(figure.squares, GameBoardGameGrid);
            if (!hasSwapped) StartAutoMove();

            GenerateRandomFigure();
        }

        // Todo! - Sebastian - Dokumentation
        private void Swap()
        {
            TetrisFigure reserve;

            if (!hasSwapped && game.settings.enableSwapBlock)
            {
                hasSwapped = true;

                EraseSquares(figure.squares, GameBoardGameGrid);

                if (swappedFigure == null)
                {

                    swappedFigure = figure;
                    swappedFigure.MoveTo(new int[] { 1, 0 });

                    NextToGame();
                }
                else
                {
                    EraseSquares(swappedFigure.squares, GameBoardSwapGrid);

                    reserve = swappedFigure;

                    swappedFigure = figure;
                    swappedFigure.MoveTo(new int[] { 1, 0 });

                    figure = reserve;
                    figure.MoveTo(new int[] { 4, 0 });

                    DrawSquares(figure.squares, GameBoardGameGrid);
                }

                DrawSquares(swappedFigure.squares, GameBoardSwapGrid);
            }
        }

        // Todo! - Sebastian - Dokumentation
        private void FigureToBoard()
        {
            game.board.squares.AddRange(figure.squares);
            ClearLine();
            autoMoveTimer.Stop();
            hasSwapped = false;
            LoseGame();
            if (!hasLost && !isPaused)
            {
                NextToGame();
            }
        }

        // Todo! - Sebastian - Dokumentation
        private bool Collision(string move)
        {
            bool result = false;

            foreach (Square square in figure.squares)
            {
                switch (move)
                {
                    case "left":
                        if (square.coordinateX == minWidth || BlockCollision(square, move))
                        {
                            result = true;
                        }
                        break;
                    case "right":
                        if (square.coordinateX == maxWidth - 1 || BlockCollision(square, move))
                        {
                            result = true;
                        }
                        break;
                    case "up":
                        if (square.coordinateY == minHeight || BlockCollision(square, move))
                        {
                            result = true;
                        }
                        break;
                    case "down":
                        if (square.coordinateY == maxHeight - 1 || BlockCollision(square, move))
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

        // Todo! - Sebastian - Dokumentation
        private bool BlockCollision(Square square, string direction)
        {
            bool result = false;

            int x = 0;
            int y = 0;

            switch (direction)
            {
                case "left":
                    x = -1;
                    break;
                case "right":
                    x = 1;
                    break;
                case "up":
                    y = -1;
                    break;
                case "down":
                    y = 1;
                    break;
                default:
                    break;
            }

            int collide = game.board.squares.Where(board => board.coordinateX == square.coordinateX + x && board.coordinateY == square.coordinateY + y).Count();
            if (collide != 0)
            {
                result = true;
            }

            return result;
        }

        // Todo! - Sebastian - Dokumentation
        private bool CanRotate()
        {
            foreach (Square square in figure.squares)
            {
                int collide = game.board.squares.Where(board => board.coordinateX == square.coordinateX && board.coordinateY == square.coordinateY).Count();

                if (collide != 0)
                {
                    return false;
                }
            }
            return true;
        }

        // Todo! - Sebastian - Dokumentation
        private void Kickback()
        {
            foreach (Square square in figure.squares)
            {
                if (square.coordinateX < minWidth)
                {
                    figure.Move("right");
                }
                else if (square.coordinateX > maxWidth - 1)
                {
                    figure.Move("left");
                }
                else if (square.coordinateY < minHeight)
                {
                    figure.Move("down");
                }
                else if (square.coordinateY > maxHeight - 1)
                {
                    figure.Move("up");
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
                List<Square> line = game.board.squares.Where(s => s.coordinateY == i && s.coordinateX >= 0 && s.coordinateX < maxWidth).ToList();
                //If statment to check if the amount of square equal the max width
                if (line.Count() == maxWidth)
                {
                    //Foreach loop to go through each square in the row
                    foreach (Square square in line)
                    {
                        //Remove the square from the board (functionaly)
                        game.board.squares.Remove(square);
                        //Remove the square from the board (visualy)
                        GameBoardGameGrid.Children.Remove(square.image);
                    }
                    //Here we get the squares above the current row of the game grid
                    List<Square> above = game.board.squares.Where(s => s.coordinateY < i && s.coordinateX >= 0 && s.coordinateX < maxWidth).ToList();
                    //Foreach loop to go through each sqaure in the above rows
                    foreach (Square square in above)
                    {
                        //Moves each square down one Y coordinate
                        square.coordinateY++;
                    }
                    //Visualy erase all squares above
                    EraseSquares(above.ToArray(), GameBoardGameGrid);
                    //Visualy draw all squares above
                    DrawSquares(above.ToArray(), GameBoardGameGrid);

                    //Add line clear to total line clear
                    game.linesCleared++;
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
                AddPoint(linesCleared);
            }
        }

        // Todo! - Sebastian - Dokumentation
        private void LoseGame()
        {
            if (game.board.squares.Where(s => s.coordinateY == 0).Count() > 2)
            {
                if (game.settings.enableTraining)
                {
                    if (!game.isPlayer)
                    {
                        (game.character as AI).population[currentIndividual].fitness = EvaluateFitness();
                        Debug.WriteLine("Disengaging individual: " + currentIndividual);
                        Debug.WriteLine("Total Fitness: " + (game.character as AI).population[currentIndividual].fitness);
                        currentIndividual++;

                        if (currentIndividual == (game.character as AI).population.Count())
                        {
                            NextGeneration();
                            currentIndividual = 0;
                            (game.character as AI).generationNumber++;
                            Debug.WriteLine("Engaging generation: " + (game.character as AI).generationNumber);
                            Debug.WriteLine("Engaging individual: " + currentIndividual);
                        }
                        else
                        {
                            Debug.WriteLine("Engaging individual: " + currentIndividual);
                        }
                    }

                    ClearBoard();
                    game.linesCleared = 0;
                    game.points = 0;
                    rand = new Random((game.character as AI).seed);
                    SetBoard();
                }
                else
                {
                    GameOver();
                    hasLost = true;
                }
            }
        }

        // Todo! - Sebastian - Dokumentation
        public void LoseGame(bool lose)
        {
            if (lose)
            {
                GameOver();
                hasLost = true;
            }
        }

        // A function that trigger a gameover event
        private void GameOver()
        {
        
            // Stop the current game timers
            
            game.time = Convert.ToInt32(elapsedTime.TotalMilliseconds);
            SQLCalls.CreateLeaderboardEntry(game);
            
            StopTime(scoreboardTimer);
            autoMoveTimer.Stop();

            // Start game over music
            gameOverMelody.Play();
            

            // Add the game over menu to the gameboardmaingrid
            
            GameOverMenu menu = new GameOverMenu(game);
            
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
            GameBoardNextGrid.Children.Clear();
            nextFigure = null;
            GameBoardSwapGrid.Children.Clear();
            swappedFigure = null;
        }

        // Todo! - Sebastian - Dokumentation
        private void SetBoard()
        {
            scoreboardTimer = new DispatcherTimer();
            StartTime(scoreboardTimer);

            FillBoard();
            CreateDynamicGameGrid(maxWidth,maxHeight);

            GenerateRandomFigure();
        }

        // Todo! - Sebastian - Dokumentation
        private void AIMove()
        {

            double calcMove = CalculateOutput();
            double calcDown = CalculateOutput();
            double calcRotate = CalculateOutput();
            double calcSwap = CalculateOutput();

            //Debug.WriteLine("Move: " + calcMove);
            if (calcMove < 0)
            {
                MoveFigure("left");
            }
            else if (calcMove > 0)
            {
                MoveFigure("right");
            }


            if (calcDown < 0)
            {
                MoveFigure("down");
            }
            else if(calcDown > 0)
            {
                InstaDrop();
            }

            if (calcRotate > 0)
            {
                RotateFigure();
            }

            if (calcSwap > 0)
            {
                Swap();
            }

            currentChromosome = 0;
        }

        // Todo! - Sebastian - Dokumentation
        private double CalculateOutput()
        {
            Individual individual = (game.character as AI).population[currentIndividual];

            int output = 0;

            // output_n = (input_n * chromosone[n]) + (input_n+1 * chromosone[n+1]) + (input_n+2 * chromosone[n+2])


            for (int i = 0; i < game.board.squares.Count(); i++)
            {
                output += game.board.squares[i].coordinateX * individual.chromosomes[currentChromosome];
                output += game.board.squares[i].coordinateY * individual.chromosomes[currentChromosome];
                currentChromosome++;
            }

            currentChromosome += game.board.gridSize - game.board.squares.Count();

            for (int i = 0; i < figure.squares.Length; i++)
            {
                output += figure.squares[i].coordinateX * individual.chromosomes[currentChromosome];
                output += figure.squares[i].coordinateY * individual.chromosomes[currentChromosome];
                currentChromosome++;
            }

            for (int i = 0; i < nextFigure.squares.Length; i++)
            {
                output += nextFigure.squares[i].coordinateX * individual.chromosomes[currentChromosome];
                output += nextFigure.squares[i].coordinateY * individual.chromosomes[currentChromosome];
                currentChromosome++;
            }

            if (swappedFigure == null)
            {
                currentChromosome += 4;
            }
            else
            {
                for (int i = 0; i < swappedFigure.squares.Length; i++)
                {
                    output += swappedFigure.squares[i].coordinateX * individual.chromosomes[currentChromosome];
                    output += swappedFigure.squares[i].coordinateY * individual.chromosomes[currentChromosome];
                    currentChromosome++;
                }
            }

            return output;
        }

        private void Advance()
        {
            AI aI = (AI)game.character;

            if (currentChromosome == aI.population[currentIndividual].chromosomes.Count())
            {
                currentChromosome = 0;
            }
        }

        private void NextGeneration()
        {
            List<Individual> newPopulation = new List<Individual>();
            Individual[] population = (game.character as AI).population;
            Random random = new Random();
            Individual mate = population[random.Next(population.Length)];

            //Debug.WriteLine("New population: ");

            // 0. Best
            newPopulation.Add(population.MaxBy(i => i.fitness));
            //Debug.WriteLine("Fitness: " + newPopulation.Last());

            // 1. Child of best & random + mutation
            // 2. Child of best & random + mutation
            // 3. Child of best & random + mutation
            // 4. Child of best & random + mutation
            for (int i = 0; i < 4; i++)
            {
                newPopulation.Add(Reproduce(newPopulation[0], mate, random));
                //Debug.WriteLine("Fitness: " + newPopulation.Last());
            }

            // 5. New random
            // 6. New random
            // 7. New random
            // 8. New random
            // 9. New random

            for (int i = newPopulation.Count(); i < population.Length; i++)
            {
                newPopulation.Add(new Individual(newPopulation[0].chromosomes.Length));
                //Debug.WriteLine("Fitness: " + newPopulation.Last());
            }

            population = newPopulation.ToArray();
        }

        private Individual Reproduce(Individual best, Individual mate, Random random)
        {
            Individual result = new Individual(best.chromosomes, 0.0);

            for (int i = 0; i < (best.chromosomes.Length / 2); i++)
            {
                int rng = random.Next(best.chromosomes.Length);

                result.chromosomes[rng] = mate.chromosomes[rng];
            }

            result.chromosomes[random.Next(best.chromosomes.Length)] = result.RandomChromosome();

            return result;
        }

        private double EvaluateFitness()
        {
            return game.points + elapsedTime.TotalSeconds;
        }

        // A function that starts the automove timer
        private void StartAutoMove()
        {
            // Instantiate a new dispatchertimer to run the automovement
            autoMoveTimer = new DispatcherTimer();

            //Sets the inital auto move timer interval
            autoMoveTimerInterval = TimeSpan.FromMilliseconds(game.settings.startSpeed);

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
            MoveFigure("down");
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

                // Todo! - Sebastian - Dokumentation
                if (((int)elapsedTime.TotalMilliseconds) % 500 <= 10 && !game.isPlayer && !hasLost)
                {
                    AIMove();
                }
            }
        }

        // A function that stop and reset the score timer
        private void StopTime(DispatcherTimer timer)
        {
            // Stop the timer
            timer.Stop();

            // Resetting the elapsed time and the paused time
            elapsedTime = TimeSpan.Zero;
            pausedTime = TimeSpan.Zero;

            // Resetting the UI label
            GameBoardScoreTimeLbl.Content = "00:00:00:000";
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
            
            if (game.linesCleared % 10 == 0)
            
            {
                // Recalculating the automovetimerinterval setting the value lower to increase the tick speed
                autoMoveTimerInterval -= autoMoveTimerInterval * (game.settings.gameSpeed / 100);
            }
        }

        // A function that adds points to the score
        private void AddPoint(int lines)
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
            game.points += (int)Math.Pow(2, lines) * 100;

            // Update the scoreboard labels
            GameBoardScorePointLbl.Content = "Point: " + game.points;
            GameBoardScoreLineClearedLbl.Content = "Lines: " + game.linesCleared;
            
        }

        //A function that applies the current settings to the board
        private void ApplySettings()
        {
            //If statment to check what setting enable next block is and then sets the visibility to the respective options
            if (game.settings.enableNextBlock)
            {
                GameBoardNextBlockBorder.Visibility = Visibility.Visible;
            }
            else
            {
                GameBoardNextBlockBorder.Visibility = Visibility.Hidden;
            }

            //If statment to check what setting enable swap block is and then sets the visibility to the respective options
            if (game.settings.enableSwapBlock)
            {
                GameBoardSaveBlockBorder.Visibility = Visibility.Visible;
            }
            else
            {
                GameBoardSaveBlockBorder.Visibility = Visibility.Hidden;
            }

            //sets the audio level of the background music
            backgroundMusic.Volume = Convert.ToDouble(game.settings.volume) / 100;
        }

        // A function that sets the incoming settings to the current settings and calls the private ApplySettings
        public void ApplySettings(Settings settings)
        {
            //sets the current settings
            game.settings = settings;
            //Applies the current settings to the board
            ApplySettings();
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

        }

        // UI button for using the not yet planned consumeable within the game
        private void GameBoardActionsConsumeTwoBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        // UI button for using the not yet planned consumeable within the game
        private void GameBoardActionsConsumeThreeBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        //A key down event handler
        private void GamePage_KeyDown(object sender, KeyEventArgs e)
        {
            //If statment checking if the player has lost and the player is a player
            if (!hasLost && game.isPlayer)
            {
                //If statment checking if the inputed key is the pause keybind
                if(e.Key == game.settings.KeyBinds.pause)
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
                        case Key k when k == game.settings.KeyBinds.left:
                            //Play move sfx
                            SFXMove.Play();
                            //Call the move figure function
                            MoveFigure("left");
                            break;
                        //Case When the inputed key = kebind right
                        case Key k when k == game.settings.KeyBinds.right:
                            //Play move sfx
                            SFXMove.Play();
                            //Call the move figure function
                            MoveFigure("right");
                            break;
                        //Case When the inputed key = kebind rotate
                        case Key k when k == game.settings.KeyBinds.rotate:
                            //Call the Rotate Figure
                            RotateFigure();
                            break;
                        //Case When the inputed key = kebind drop
                        case Key k when k == game.settings.KeyBinds.drop:
                            //Call the move figure function
                            MoveFigure("down");
                            break;
                        //Case When the inputed key = kebind insta
                        case Key k when k == game.settings.KeyBinds.insta:
                            //Call the insta drop function
                            InstaDrop();
                            break;
                        //Case When the inputed key = kebind swap
                        case Key k when k == game.settings.KeyBinds.swap:
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

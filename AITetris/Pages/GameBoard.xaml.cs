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

        // Scores
        private int totalLinesCleared;
        private int points;

        // Execution directory
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

            // Initialize variables
            isScoreboardTimerPaused = false;
            exeDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

            game = new Game(
                new Board(maxWidth + 2, maxHeight),
                character,
                JsonSerializer.Deserialize<Settings>(File.ReadAllText(exeDir + "/Assets/JSON/Settings.json")));

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



            GameBoardScorePlayerLbl.Content = character.name;
            ApplySettings();
            
            // Scoreboard timer
            scoreboardTimer = new DispatcherTimer();
            StartTime(scoreboardTimer);
            FillBoard();
            CreateDynamicGameGrid(maxWidth, maxHeight);

            GenerateRandomFigure();
            NextToGame();

            //Music start
            MusicStart();

            pauseMenu = new PauseMenu(this);
        }

        public void TogglePauseGame()
        {
            if (!isPaused)
            {
                autoMoveTimer.Stop();
                PauseTime(scoreboardTimer);
                isPaused = true;
                GameBoardMainGrid.Children.Add(pauseMenu);
                Grid.SetColumn(pauseMenu, 1);
                Grid.SetRow(pauseMenu, 1);

                Grid.SetColumnSpan(pauseMenu, 5);
                Grid.SetRowSpan(pauseMenu, 7);
            }
            else
            {
                isPaused = false;
                autoMoveTimer.Start();
                ResumeTime(scoreboardTimer);
                GameBoardMainGrid.Children.Remove(pauseMenu);
            }
        }

        private void FillBoard()
        {
            game.board.squares.Clear();
            for (int i = 0; i < maxHeight; i++)
            {
                game.board.squares.Add(new Square(-1, i, "BluePrimary"));
                game.board.squares.Add(new Square(maxWidth, i, "BluePrimary"));
            }
        }

        private void DrawSquares(Square[] squares, Grid grid)
        {
            foreach (Square square in squares)
            {
                Grid.SetColumn(square.image, square.coordinateX);
                Grid.SetRow(square.image, square.coordinateY);
                
                grid.Children.Add(square.image);
            }
        }

        private void EraseSquares(Square[] squares, Grid grid)
        {
            foreach (Square square in squares)
            {
                grid.Children.Remove(square.image);
            }
        }

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

        private void GenerateRandomFigure()
        {
            var i = rand.Next(0, Enum.GetNames(typeof(FigureType)).Length);
            nextFigure = new TetrisFigure(new int[]{ 1,0},(FigureType)i);

            DrawSquares(nextFigure.squares, GameBoardNextGrid);
        }

        private void NextToGame()
        {
            EraseSquares(nextFigure.squares, GameBoardNextGrid);
            figure = nextFigure;
            figure.MoveTo(new int[]{4,0});
            DrawSquares(figure.squares, GameBoardGameGrid);
            if (!hasSwapped) StartAutoMove();

            GenerateRandomFigure();
        }

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

        private void CreateDynamicGameGrid(int cols, int rows)
        {
            // Gamegrid
            Grid gamegrid = GameBoardGameGrid;

            // Clear current gamegrid
            gamegrid.Children.Clear();

            // Remove current definitions
            gamegrid.RowDefinitions.Clear();
            gamegrid.ColumnDefinitions.Clear();

            // Create new row definitions
            for (int i = 0; i < rows; i++)
            {
                gamegrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            }

            // Create new row definitions
            for (int i = 0; i < cols; i++)
            {
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

        private void ClearLine()
        {
            int linesCleared = 0;

            // Check and clear the line
            for (int i = 0; i < maxHeight; i++)
            {
                List<Square> line = game.board.squares.Where(s => s.coordinateY == i && s.coordinateX >= 0 && s.coordinateX < maxWidth).ToList();
                if (line.Count() == maxWidth)
                {
                    foreach (Square square in line)
                    {
                        game.board.squares.Remove(square);
                        GameBoardGameGrid.Children.Remove(square.image);
                    }

                    List<Square> above = game.board.squares.Where(s => s.coordinateY < i && s.coordinateX >= 0 && s.coordinateX < maxWidth).ToList();
                    foreach (Square square in above)
                    {
                        square.coordinateY++;
                    }
                    EraseSquares(above.ToArray(), GameBoardGameGrid);
                    DrawSquares(above.ToArray(), GameBoardGameGrid);

                    totalLinesCleared++;
                    SpeedUp();
                    linesCleared++;
                }
            }

            if (linesCleared > 0)
            {
                AddPoint(linesCleared);
            }
        }

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

        public void LoseGame(bool lose)
        {
            if (lose)
            {
                GameOver();
                hasLost = true;
            }
        }

        private void GameOver()
        {
            StopTime(scoreboardTimer);
            autoMoveTimer.Stop();

            gameOverMelody.Play();
            GameOverMenu menu = new GameOverMenu();
            GameBoardMainGrid.Children.Add(menu);
            Grid.SetColumn(menu, 1);
            Grid.SetRow(menu, 1);

            Grid.SetColumnSpan(menu, 5);
            Grid.SetRowSpan(menu, 7);

            ClearBoard();
        }

        private void ClearBoard()
        {
            GameBoardGameGrid.Children.Clear();
            figure = null;
            GameBoardNextGrid.Children.Clear();
            nextFigure = null;
            GameBoardSwapGrid.Children.Clear();
            swappedFigure = null;
        }

        private void SetBoard()
        {
            scoreboardTimer = new DispatcherTimer();
            StartTime(scoreboardTimer);

            FillBoard();
            CreateDynamicGameGrid(maxWidth,maxHeight);

            GenerateRandomFigure();
        }

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

        private void StartAutoMove()
        {
            autoMoveTimer = new DispatcherTimer();

            // Set the interval of the timer in milliseconds
            autoMoveTimer.Interval = autoMoveTimerInterval;

            // Set the event that happends on tick
            autoMoveTimer.Tick += AutoMove_Tick;

            // Start the timer
            autoMoveTimer.Start();
        }

        private void AutoMove_Tick(object sender, EventArgs e)
        {
            MoveFigure("down");
        }

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


                if (((int)elapsedTime.TotalMilliseconds) % 500 <= 10 && !game.isPlayer && !hasLost)
                {
                    AIMove();
                }
            }
        }


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

        private void PauseTime(DispatcherTimer timer)
        {
            // Change the scoreboardtimer status
            isScoreboardTimerPaused = true;

            // Stop the timer
            timer.Stop();

            // Increase the current pause time
            pausedTime += DateTime.Now - startTime;
        }

        private void ResumeTime(DispatcherTimer timer)
        {
            // Change the scoreboardtimer status
            isScoreboardTimerPaused = false;

            // Update the starttime to the time of resume
            startTime = DateTime.Now;

            // Start the timer
            timer.Start();
        }    
        
        private void MusicStart()
        {
            backgroundMusic.Open(new Uri(exeDir + "\\Assets\\Sound\\Tetris99MainTheme.mp3", UriKind.Absolute));
            backgroundMusic.MediaEnded += new EventHandler((sender, e) =>
            {
                ((MediaPlayer)sender).Position = TimeSpan.Zero;
            });
            backgroundMusic.Play();
            gameOverMelody = new SoundPlayer(exeDir + "\\Assets\\Sound\\MusicGameOver.wav");
            SFXMove = new SoundPlayer(exeDir + "\\Assets\\Sound\\SFXMove.wav");
            SFXDrop = new SoundPlayer(exeDir + "\\Assets\\Sound\\SFXDrop.wav");
            SFXLineClear = new SoundPlayer(exeDir + "\\Assets\\Sound\\SFXLineClear.wav");
            SFXTetrisClear = new SoundPlayer(exeDir + "\\Assets\\Sound\\SFXTetrisClear.wav");
        }

        private void SpeedUp()
        {
            if (totalLinesCleared % 10 == 0)
            {
                autoMoveTimerInterval -= autoMoveTimerInterval * (game.settings.gameSpeed / 100);
            }
        }

        private void AddPoint(int lines)
        {
            if(lines == 4)
            {
                SFXTetrisClear.Play();
            }
            else
            {
                SFXLineClear.Play();
            }
            
            points += (int)Math.Pow(2, lines) * 100;

            GameBoardScorePointLbl.Content = "Point: " + points;
            GameBoardScoreLineClearedLbl.Content = "Lines: " + totalLinesCleared;
        }

        private void ApplySettings()
        {
            if (game.settings.enableNextBlock)
            {
                GameBoardNextBlockBorder.Visibility = Visibility.Visible;
            }
            else
            {
                GameBoardNextBlockBorder.Visibility = Visibility.Hidden;
            }

            if (game.settings.enableSwapBlock)
            {
                GameBoardSaveBlockBorder.Visibility = Visibility.Visible;
            }
            else
            {
                GameBoardSaveBlockBorder.Visibility = Visibility.Hidden;
            }

            autoMoveTimerInterval = TimeSpan.FromMilliseconds(game.settings.startSpeed);

            backgroundMusic.Volume = Convert.ToDouble(game.settings.volume) / 100;
        }

        public void ApplySettings(Settings settings)
        {
            game.settings = settings;
            ApplySettings();
        }


        private void GameBoardActionsPauseBtn_Click(object sender, RoutedEventArgs e)
        {
            TogglePauseGame();
        }

        private void GameBoardActionsConsumeOneBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void GameBoardActionsConsumeTwoBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void GameBoardActionsConsumeThreeBtn_Click(object sender, RoutedEventArgs e)
        {

        }
        
        private void GamePage_KeyDown(object sender, KeyEventArgs e)
        {
            if (!hasLost && game.isPlayer)
            {
                if(e.Key == game.settings.KeyBinds.pause)
                {
                    TogglePauseGame();
                }
                if (!isPaused)
                {
                    switch (e.Key)
                    {
                        case Key k when k == game.settings.KeyBinds.left:
                            SFXMove.Play();
                            MoveFigure("left");
                            break;
                        case Key k when k == game.settings.KeyBinds.right:
                            SFXMove.Play();
                            MoveFigure("right");
                            break;
                        case Key k when k == game.settings.KeyBinds.rotate:
                            RotateFigure();
                            break;
                        case Key k when k == game.settings.KeyBinds.drop:
                            MoveFigure("down");
                            break;
                        case Key k when k == game.settings.KeyBinds.insta:
                            InstaDrop();
                            break;
                        case Key k when k == game.settings.KeyBinds.swap:
                            Swap();
                            break;
                        default: break;
                    }
                }
            }
        }

        private void GamePage_Loaded(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).KeyDown += GamePage_KeyDown;
        }

        // Test buttons to control the timer

        //private void GameBoardActionsStartTimeTestBtn_Click(object sender, RoutedEventArgs e)
        //{
        //    StartTime(scoreboardTimer);
        //}

        //private void GameBoardActionsStopTimeTestBtn_Click(object sender, RoutedEventArgs e)
        //{
        //    StopTime(scoreboardTimer);
        //}

        //private void GameBoardActionsPauseTimeTestBtn_Click(object sender, RoutedEventArgs e)
        //{
        //    PauseTime(scoreboardTimer);
        //}

        //private void GameBoardActionsResumeTimeTestBtn_Click(object sender, RoutedEventArgs e)
        //{
        //    ResumeTime(scoreboardTimer);
        //}
    }
}

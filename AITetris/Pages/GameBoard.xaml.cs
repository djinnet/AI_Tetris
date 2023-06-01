using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
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

namespace AITetris.Pages
{
    /// <summary>
    /// Interaction logic for GameBoard.xaml
    /// </summary>
    public partial class GameBoard : Page
    {
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

        // Character variables
        private Character character;
        private Board board;
        private TetrisFigure figure;
        private TetrisFigure nextFigure;
        private TetrisFigure swappedFigure;
        private bool hasSwapped = false;

        private int totalLinesCleared;
        private int points;

        //Execution directory
        private string exeDir;

        //Audio variables
        private MediaPlayer backgroundMusic = new MediaPlayer();
        private SoundPlayer gameOverMelody;
        private SoundPlayer SFXMove;
        private SoundPlayer SFXDrop;
        private SoundPlayer SFXLineClear;
        private SoundPlayer SFXTetrisClear;

        //Settings variable
        private Settings settings;

        public GameBoard(Character character)
        {
            InitializeComponent();

            // Initialize variables
            isScoreboardTimerPaused = false;
            this.character = character;
            exeDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

            GameBoardScorePlayerLbl.Content = character.name;
            
            // Scoreboard timer
            scoreboardTimer = new DispatcherTimer();
            StartTime(scoreboardTimer);
            board = new Board(maxWidth+2, maxHeight);
            FillBoard();
            CreateDynamicGameGrid(maxWidth, maxHeight);

            GenerateRandomFigure();
            NextToGame();

            settings = JsonSerializer.Deserialize<Settings>(File.ReadAllText(exeDir + "/Assets/JSON/Settings.json"));
            autoMoveTimerInterval = TimeSpan.FromMilliseconds(settings.startSpeed);

            //Music start
            MusicStart();
        }

        private void FillBoard()
        {
            for (int i = 0; i < maxHeight; i++)
            {
                board.squares.Add(new Square(-1, i));
                board.squares.Add(new Square(maxWidth, i));
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
            Random rand = new Random();
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

        private void FigureToBoard()
        {
            board.squares.AddRange(figure.squares);
            ClearLine();
            StopAutoMove();
            hasSwapped = false;
            if (!LoseGame())
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

            int collide = board.squares.Where(board => board.coordinateX == square.coordinateX + x && board.coordinateY == square.coordinateY + y).Count();
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
                int collide = board.squares.Where(board => board.coordinateX == square.coordinateX && board.coordinateY == square.coordinateY).Count();

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
                List<Square> line = board.squares.Where(s => s.coordinateY == i && s.coordinateX >= 0 && s.coordinateX < maxWidth).ToList();
                if (line.Count() == maxWidth)
                {
                    foreach (Square square in line)
                    {
                        board.squares.Remove(square);
                        GameBoardGameGrid.Children.Remove(square.image);
                    }

                    List<Square> above = board.squares.Where(s => s.coordinateY < i && s.coordinateX >= 0 && s.coordinateX < maxWidth).ToList();
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

        private bool LoseGame()
        {
            bool result = false;
            if (board.squares.Where(s => s.coordinateY == 0).Count() > 2)
            {
                GameOver();
                Debug.WriteLine("You lose!");
                result = true;

            }
            return result;
        }

        private void GameOver()
        {
            GameOverMenu menu = new GameOverMenu();
            GameBoardMainGrid.Children.Add(menu);
            Grid.SetColumn(menu, 1);
            Grid.SetRow(menu, 1);

            Grid.SetColumnSpan(menu, 5);
            Grid.SetRowSpan(menu, 7);

            StopTime(scoreboardTimer);

            backgroundMusic.Close();
            gameOverMelody.Play();
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

        private void StopAutoMove()
        {
            // Stop the timer
            autoMoveTimer.Stop();
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
            backgroundMusic.Volume = Convert.ToDouble(settings.volume) / 100;
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
                autoMoveTimerInterval -= autoMoveTimerInterval * settings.gameSpeed / 100;
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


        private void GameBoardActionsPauseBtn_Click(object sender, RoutedEventArgs e)
        {

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
            switch (e.Key)
            {
                case Key k when k == settings.KeyBinds.left:
                    SFXMove.Play();
                    MoveFigure("left");
                    break;
                case Key k when k == settings.KeyBinds.right:
                    SFXMove.Play();
                    MoveFigure("right");
                    break;
                case Key k when k == settings.KeyBinds.rotate:
                    RotateFigure();
                    break;
                case Key k when k == settings.KeyBinds.drop:
                    MoveFigure("down");
                    break;
                case Key k when k == settings.KeyBinds.insta:
                    InstaDrop();
                    break;
                case Key k when k == settings.KeyBinds.pause:
                    break;
                case Key k when k == settings.KeyBinds.swap:
                    if (!hasSwapped)
                    {
                        hasSwapped = true;
                        Swap();
                    }
                    break;
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

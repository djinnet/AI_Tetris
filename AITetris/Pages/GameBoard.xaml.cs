using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
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

        // Grid variables
        private int minHeight = 0;
        private int maxHeight = 20;
        private int minWidth = 0;
        private int maxWidth = 10;

        // Character variables
        private Character character;
        private Board board;
        private TetrisFigure figure;
        private Image[] images = new Image[4];

        //
        public GameBoard(Character character)
        {
            InitializeComponent();

            // Initialize variables
            isScoreboardTimerPaused = false;
            this.character = character;
            
            GameBoardScorePlayerLbl.Content = character.name;
            
            // Scoreboard timer
            scoreboardTimer = new DispatcherTimer();
            StartTime(scoreboardTimer);
            board = new Board(maxWidth+2, maxHeight);
            FillBoard();
            CreateDynamicGameGrid(maxWidth, maxHeight);

            GenerateRandomFigure();
        }

        private void FillBoard()
        {
            for (int i = 0; i < maxHeight; i++)
            {
                board.squares.Add(new Square(-1, i));
                board.squares.Add(new Square(maxWidth, i));
            }
        }

        private void AddFigure()
        {
            for (int i = 0; i < figure.squares.Length; i++)
            {
                string imageSource = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + figure.squares[i].spritePath;
                images[i] = new Image();
                images[i].Source = new BitmapImage(new Uri(imageSource, UriKind.Absolute));
                Grid.SetColumn(images[i], figure.squares[i].coordinateX);
                Grid.SetRow(images[i], figure.squares[i].coordinateY);
                GameBoardGameGrid.Children.Add(images[i]);
            }
        }

        private void DeleteFigure()
        {
            foreach (Image image in images)
            {
                GameBoardGameGrid.Children.Remove(image);
            }
        }

        private void RotateFigure()
        {
            
            if (CanRotate())
            {
                DeleteFigure();
                figure.Rotate();
                if (!CanRotate())
                {
                    figure.Rotate();
                    figure.Rotate();
                    figure.Rotate();
                }
                Kickback();
                AddFigure();
            }
        }

        private void MoveFigure(string destination)
        {
            if (Collision(destination))
            {
                if (destination == "down")
                {
                    FigureToBoard();
                }
                return;
            }

            DeleteFigure();
            figure.Move(destination);
            AddFigure();
        }

        private void InstaDrop()
        {
            while (!Collision("down"))
            {
                DeleteFigure();
                figure.Move("down");
                AddFigure();
            }
            MoveFigure("down");
        }

        private void GenerateRandomFigure()
        {
            Debug.WriteLine("Generate");
            Random rand = new Random();
            var i = rand.Next(0, Enum.GetNames(typeof(FigureType)).Length);
            figure = new TetrisFigure(new int[]{ 4,0},(FigureType)i);

            AddFigure();
        }

        private void FigureToBoard()
        {
            board.squares.AddRange(figure.squares);
            GenerateRandomFigure();
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

        private void ClearLine()
        {
            int linesCleared = 0;
            int clearIfMax = 0;
            for(int y = board.grid.GetLength(1) - 1; y >= 0; y--)
            {
                Debug.WriteLine("this is y" + y);
                for (int x = 0; x < board.grid.GetLength(0); x++)
                {
                    Debug.WriteLine("this is x" + x);
                    if (board.grid[x, y])
                    {
                        clearIfMax++;
                    }
                    else
                    {
                        break;
                    }
                }
                if(clearIfMax == maxWidth)
                {
                    linesCleared++;
                    for(int i = 0; i < board.squares.Count; i++)
                    {
                        if(board.squares[i].coordinateY == y)
                        {
                            board.squares[i] = null;
                        }
                        if(board.squares[i].coordinateY > y)
                        {
                            board.squares[i].coordinateY--;
                        }
                    }
                }
                clearIfMax = 0;
            }
            if (linesCleared > 0)
            {
                AddPoint(linesCleared);
            }
        }

        private void AddPoint(int lines)
        {
            GameBoardScorePointLbl.Content = "Point: " + (Convert.ToInt32(((string)GameBoardScorePointLbl.Content).Remove(0,7)) + (Math.Pow(2, lines) * 100)).ToString();
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
                case Key.A:
                    MoveFigure("left");
                    break;
                case Key.D:
                    MoveFigure("right");
                    break;
                case Key.W:
                    // TODO: Added up button to W, need to remove that from the final build.
                    MoveFigure("up");
                    break;
                case Key.S:
                    MoveFigure("down");
                    break;
                case Key.Space:
                    InstaDrop();
                    break;
                case Key.Escape:
                    break;
                case Key.E:
                    //GenerateRandomFigure();
                    RotateFigure();
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

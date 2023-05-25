using System;
using System.Collections.Generic;
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
        private Character character;
        private Board board;
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
            board = new Board(10, 20);
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
            int clearIf10 = 0;
            for(int y = board.grid.GetLength(1) - 1; y >= 0; y--)
            {
                Debug.WriteLine("this is y" + y);
                for (int x = 0; x < board.grid.GetLength(0); x++)
                {
                    Debug.WriteLine("this is x" + x);
                    if (board.grid[x, y])
                    {
                        clearIf10++;
                    }
                    else
                    {
                        break;
                    }
                }
                if(clearIf10 == 10)
                {
                    linesCleared++;
                    foreach(Classes.Block block in board.blocks)
                    {
                        if(block.coordinateY > y)
                        {
                            block.coordinateY--;
                        }
                    }
                }
                clearIf10 = 0;
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

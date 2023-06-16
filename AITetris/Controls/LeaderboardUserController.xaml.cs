using AITetris.Classes;
using AITetris.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace AITetris.Controls
{
    /// <summary>
    /// Interaction logic for LeaderboardUserController.xaml
    /// </summary>
    public partial class LeaderboardUserController : UserControl
    {
        // Game variables
        GameBoard game;
        // A list of games played collected from the SQLCalls function GetLeaderboardTop10
        List<Game> scores = SQLCalls.GetLeaderboardTop10();
        bool isSerching = false;
        public LeaderboardUserController(GameBoard game)
        {
            InitializeComponent();

            // Set game to an instance to the current game
            this.game = game;

            // Fill leaderboard with a top 10 games played
            FillLeaderboard();
        }

        // A function that creates a leaderboard with a top 10 games played
        private void FillLeaderboard()
        {

            // Set leaderboardGrid to an instance of the leaderboardgrid UI element
            Grid leaderboardGrid = LeaderboardGrid;

            // Clear current leaderboard
            leaderboardGrid.Children.Clear();

            // Remove current definitions
            leaderboardGrid.RowDefinitions.Clear();
            leaderboardGrid.ColumnDefinitions.Clear();

            // Amount of rows in the leaderboard
            int rowCount = scores.Count + 1;

            // Amount of columns in the leaderboard
            int columnCount = 6;

            // Create new row definitions based on the lenght of the scores list
            for (int i = 0; i < rowCount; i++)
            {
                // Adding a new row definition
                leaderboardGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            }

            // Create new row definitions based on how many columns given
            for (int i = 0; i < columnCount; i++)
            {
                // Adding a new column definition
                leaderboardGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }

            // Running for each row in the leaderboard
            for (int i = 0; i < rowCount; i++)
            {
                // Running for each column in the leaderboard
                for (int j = 0; j < columnCount; j++)
                {
                    if (i == 0)
                    {
                        // Border for each label
                        Border border = new Border();

                        // Border styling
                        border.BorderBrush = Brushes.Silver;
                        border.BorderThickness = new Thickness(2);

                        // Creating a new label
                        Label label = new Label();

                        // Add content to the label
                        switch (j)
                        {
                            case 0:
                                label.Content = "Rank";
                                break;
                            case 1:
                                label.Content = "Name";
                                break;
                            case 2:
                                label.Content = "Point";
                                break;
                            case 3:
                                label.Content = "Lines";
                                break;
                            case 4:
                                label.Content = "Time";
                                break;
                            case 5:
                                label.Content = "Character";
                                break;

                            default:
                                break;
                        }

                        // Add styling to the label
                        label.HorizontalAlignment = HorizontalAlignment.Center;
                        label.VerticalAlignment = VerticalAlignment.Center;
                        label.FontFamily = new FontFamily("Tahomaa");
                        label.FontSize = 20;
                        label.FontWeight = FontWeights.Bold;

                        // Add label to border
                        border.Child = label;

                        // Add the border to the leaderboardgrid
                        Grid.SetColumn(border, j);
                        Grid.SetRow(border, i);
                        leaderboardGrid.Children.Add(border);
                    }
                    else
                    {
                        // Border for each label
                        Border border = new Border();

                        // Border styling
                        border.BorderBrush = Brushes.Silver;
                        border.BorderThickness = new Thickness(2);

                        // Creating a new label
                        Label label = new Label();

                        // Add content to the label
                        // Using a shorthand switchcase we add the leaderboard content for each game we pass through in the loop
                        label.Content = j switch
                        {
                            0 => scores[i - 1].rank,
                            1 => scores[i - 1].character.name,
                            2 => scores[i - 1].points,
                            3 => scores[i - 1].linesCleared,
                            4 => scores[i - 1].time,
                            5 => scores[i - 1].isPlayer ? "Player" : "AI",
                            _ => ""
                        };

                        // Add styling to the label
                        label.HorizontalAlignment = HorizontalAlignment.Center;
                        label.VerticalAlignment = VerticalAlignment.Center;
                        label.FontFamily = new FontFamily("Tahomaa");
                        label.FontSize = 15;
                        label.FontWeight = FontWeights.Bold;

                        // Add label to border
                        border.Child = label;

                        // Add the border to the leaderboardgrid
                        Grid.SetColumn(border, j);
                        Grid.SetRow(border, i);
                        leaderboardGrid.Children.Add(border);
                    }
                }
            }
        }

        // Navigation
        private void LeaderboardBackToPauseBtn_Click(object sender, RoutedEventArgs e)
        {
            // Navigation to the Main Menu
            game.GameBoardMainGrid.Children.Remove(this);
        }

        private void LeaderboardControlsFindPlayerBtn_Click(object sender, RoutedEventArgs e)
        {
            //call sql database with name from text box
            scores = SQLCalls.SearchLeaderboardOnName(LeaderboardControlsFindPlayerTxtbox.Text);
            //clear current leaderboard
            LeaderboardGrid.Children.Clear();
            //bool to note we are searching now
            isSerching = true;
            //fill the leaderboard with the gotten entries
            FillLeaderboard();
        }

        private void LeaderboardPreviousTenBtn_Click(object sender, RoutedEventArgs e)
        {
            //check if searching
            if (isSerching)
            {
                //check if searched name yeilded results
                if (scores.Count != 0)
                {
                    //check if there are any previous entries to get
                    if (SQLCalls.GetPrevious10Leaderboard(scores[0].rank, LeaderboardControlsFindPlayerTxtbox.Text).Count != 0)
                    {
                        //call sql database for previous 10 by rank with searched name
                        scores = SQLCalls.GetPrevious10Leaderboard(scores[0].rank, LeaderboardControlsFindPlayerTxtbox.Text);
                        //clear current leaderboard
                        LeaderboardGrid.Children.Clear();
                        //fill the leaderboard with the gotten entries
                        FillLeaderboard();
                        //enable LeaderboardNextTenBtn
                        LeaderboardNextTenBtn.IsEnabled = true;
                    }
                    else
                    {
                        //disable LeaderboardPrieviousTenBtn
                        LeaderboardPreviousTenBtn.IsEnabled = false;
                    }
                }
                else
                {
                    //disable LeaderboardPrieviousTenBtn
                    LeaderboardPreviousTenBtn.IsEnabled = false;
                }
            }
            else
            {
                //check if there are any previous entries to get
                if (SQLCalls.GetPrevious10Leaderboard(scores[0].rank).Count != 0)
                {
                    //call sql database for previous 10 by rank
                    scores = SQLCalls.GetPrevious10Leaderboard(scores[0].rank);
                    //clear current leaderboard
                    LeaderboardGrid.Children.Clear();
                    //fill the leaderboard with the gotten entries
                    FillLeaderboard();
                    //enable LeaderboardNextTenBtn
                    LeaderboardNextTenBtn.IsEnabled = true;
                }
                else
                {
                    //disable LeaderboardPrieviousTenBtn
                    LeaderboardPreviousTenBtn.IsEnabled = false;
                }
            }
        }

        //function that finds the next ten in the leaderboard
        private void LeaderboardNextTenBtn_Click(object sender, RoutedEventArgs e)
        {
            //check if searching
            if (isSerching)
            {
                //check if searched name yeilded results
                if (scores.Count != 0)
                {
                    //check if there are any next entries to get
                    if (SQLCalls.GetNext10Leaderboard(scores[scores.Count - 1].rank, LeaderboardControlsFindPlayerTxtbox.Text).Count != 0)
                    {
                        //call sql database for next 10 by rank with searched name
                        scores = SQLCalls.GetNext10Leaderboard(scores[scores.Count - 1].rank, LeaderboardControlsFindPlayerTxtbox.Text);
                        //clear current leaderboard
                        LeaderboardGrid.Children.Clear();
                        //fill the leaderboard with the gotten entries
                        FillLeaderboard();
                        //enable LeaderboardPrieviousTenBtn
                        LeaderboardPreviousTenBtn.IsEnabled = true;
                    }
                    else
                    {
                        //disable LeaderboardNextTenBtn
                        LeaderboardNextTenBtn.IsEnabled = false;
                    }
                }
                else
                {
                    //disable LeaderboardNextTenBtn
                    LeaderboardNextTenBtn.IsEnabled = false;
                }
            }
            else
            {
                //check if there are any next entries to get
                if (SQLCalls.GetNext10Leaderboard(scores[scores.Count - 1].rank).Count != 0)
                {
                    //call sql database for next 10 by rank
                    scores = SQLCalls.GetNext10Leaderboard(scores[scores.Count - 1].rank);
                    //clear current leaderboard
                    LeaderboardGrid.Children.Clear();
                    //fill the leaderboard with the gotten entries
                    FillLeaderboard();
                    //enable LeaderboardPrieviousTenBtn
                    LeaderboardPreviousTenBtn.IsEnabled = true;
                }
                else
                {
                    //disable LeaderboardNextTenBtn
                    LeaderboardNextTenBtn.IsEnabled = false;
                }
            }
        }

        //function that refresh the leaderboard
        private void LeaderboardRefreshBtn_Click(object sender, RoutedEventArgs e)
        {
            scores = SQLCalls.GetLeaderboardTop10();
            //clear current leaderboard
            LeaderboardGrid.Children.Clear();
            //bool to note we are not searching
            isSerching = false;
            //enable LeaderboardPrieviousTenBtn
            LeaderboardPreviousTenBtn.IsEnabled = true;
            //enable LeaderboardNextTenBtn
            LeaderboardNextTenBtn.IsEnabled = true;
            //fill the leaderboard with the gotten entries
            FillLeaderboard();
        }
    }
}

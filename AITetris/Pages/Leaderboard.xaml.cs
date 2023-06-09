using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using AITetris.Classes;

namespace AITetris.Pages
{
    /// <summary>
    /// Interaction logic for Leaderboard.xaml
    /// </summary>
    public partial class Leaderboard : Page
    {
        List<Game> scores = SQLCalls.GetLeaderboardTop10();
        bool isSerching = false;
        public Leaderboard()
        {
            InitializeComponent();

            // Create a list of games using the SQLCalls function GetLeaderboardTop10
            scores = SQLCalls.GetLeaderboardTop10();

            // Create and fill the leaderboard with contennt
            FillLeaderboard();
        }

        // A function that fills and creates the leaderboard
        private void FillLeaderboard()
        {
            // Get an instance of the leaderboard grid
            Grid leaderboardGrid = LeaderboardGrid;

            // Clear current leaderboard
            leaderboardGrid.Children.Clear();

            // Remove current definitions
            leaderboardGrid.RowDefinitions.Clear();
            leaderboardGrid.ColumnDefinitions.Clear();

            // Amount of rows in the leaderboard + 1 for headers
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
                    // Check if it is the first row then add the headers
                    if(i == 0)
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
        private void LeaderboardBackToMainBtn_Click(object sender, RoutedEventArgs e)
        {
            // Navigation to the Main Menu
            NavigationService.Navigate(new Uri("Pages/MainPage.xaml", UriKind.Relative));
        }

        // Todo! - Add function that finds a specific player using his name
        private void LeaderboardControlsFindPlayerBtn_Click(object sender, RoutedEventArgs e)
        {
            scores = SQLCalls.SearchLeaderboardOnName(LeaderboardControlsFindPlayerTxtbox.Text);
            LeaderboardGrid.Children.Clear();
            isSerching = true;
            FillLeaderboard();
        }

        // Todo! - Add function that finds the prievious ten in the leaderboard
        private void LeaderboardPrieviousTenBtn_Click(object sender, RoutedEventArgs e)
        {
            if(isSerching)
            {
                if (SQLCalls.GetPrevious10Leaderboard(scores[0].rank, LeaderboardControlsFindPlayerTxtbox.Text).Count != 0)
                {
                    scores = SQLCalls.GetPrevious10Leaderboard(scores[0].rank, LeaderboardControlsFindPlayerTxtbox.Text);
                    LeaderboardGrid.Children.Clear();
                    FillLeaderboard();
                    LeaderboardNextTenBtn.IsEnabled = true;
                }
                else
                {
                    LeaderboardPrieviousTenBtn.IsEnabled = false;
                }
            }
            else
            {
                if (SQLCalls.GetPrevious10Leaderboard(scores[0].rank).Count != 0 )
                {
                    scores = SQLCalls.GetPrevious10Leaderboard(scores[0].rank);
                    LeaderboardGrid.Children.Clear();
                    FillLeaderboard();
                    LeaderboardNextTenBtn.IsEnabled = true;
                }
                else
                {
                    LeaderboardPrieviousTenBtn.IsEnabled = false;
                }
            }
        }

        // Todo! - Add function that finds the next ten in the leaderboard
        private void LeaderboardNextTenBtn_Click(object sender, RoutedEventArgs e)
        {
            if (isSerching)
            {
                if (SQLCalls.GetNext10Leaderboard(scores[scores.Count - 1].rank, LeaderboardControlsFindPlayerTxtbox.Text).Count != 0)
                {
                    scores = SQLCalls.GetNext10Leaderboard(scores[scores.Count - 1].rank, LeaderboardControlsFindPlayerTxtbox.Text);
                    LeaderboardGrid.Children.Clear();
                    FillLeaderboard();
                    LeaderboardPrieviousTenBtn.IsEnabled = true;
                }
                else
                {
                    LeaderboardNextTenBtn.IsEnabled = false;
                }
            }
            else
            {
                if(SQLCalls.GetNext10Leaderboard(scores[scores.Count - 1].rank).Count != 0)
                {
                    scores = SQLCalls.GetNext10Leaderboard(scores[scores.Count - 1].rank);
                    LeaderboardGrid.Children.Clear();
                    FillLeaderboard();
                    LeaderboardPrieviousTenBtn.IsEnabled = true;
                }
                else
                {
                    LeaderboardNextTenBtn.IsEnabled = false;
                }
            }
        }

        // Todo! - Add function that refresh the leaderboard
        private void LeaderboardRefreshBtn_Click(object sender, RoutedEventArgs e)
        {
            scores = SQLCalls.GetLeaderboardTop10();
            LeaderboardGrid.Children.Clear();
            isSerching = false;
            LeaderboardPrieviousTenBtn.IsEnabled = true;
            LeaderboardNextTenBtn.IsEnabled = true;
            FillLeaderboard();
        }
    }
}

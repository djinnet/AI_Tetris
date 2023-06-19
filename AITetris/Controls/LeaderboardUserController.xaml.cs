using AITetris.Classes;
using AITetris.Pages;
using AITetris.Services;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace AITetris.Controls
{
    /// <summary>
    /// Interaction logic for LeaderboardUserController.xaml
    /// </summary>
    public partial class LeaderboardUserController : UserControl
    {
        // Game variables
        GameBoard game;
        // A list of games played collected from the DatabaseService function GetLeaderboardTop10
        List<Game> scores = DatabaseService.GetLeaderboardTop10();
        bool isSerching = false;
        public LeaderboardUserController(GameBoard game)
        {
            InitializeComponent();

            // Set game to an instance to the current game
            this.game = game;

            // Fill leaderboard with a top 10 games played
            LeaderboardGrid.FillLeaderboard(scores);
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
            scores = DatabaseService.SearchLeaderboardOnName(LeaderboardControlsFindPlayerTxtbox.Text);
            //clear current leaderboard
            LeaderboardGrid.Children.Clear();
            //bool to note we are searching now
            isSerching = true;
            //fill the leaderboard with the gotten entries
            LeaderboardGrid.FillLeaderboard(scores);
        }

        private void LeaderboardPreviousTenBtn_Click(object sender, RoutedEventArgs e)
        {
            (bool found, List<Game> SearchScores) = LeaderboardGrid.SearchWithinLeaderboard(scores, isSerching, LeaderboardControlsFindPlayerTxtbox.Text);
            scores = SearchScores;
            if (found)
            {
                LeaderboardNextTenBtn.IsEnabled = true;
            }
            else
            {
                LeaderboardPreviousTenBtn.IsEnabled = false;
            }
        }

        //function that finds the next ten in the leaderboard
        private void LeaderboardNextTenBtn_Click(object sender, RoutedEventArgs e)
        {
            (bool found, List<Game> SearchScores) = LeaderboardGrid.SearchWithinLeaderboard(scores, isSerching, LeaderboardControlsFindPlayerTxtbox.Text);
            scores = SearchScores;
            if (found)
            {
                LeaderboardPreviousTenBtn.IsEnabled = true;
            }
            else
            {
                LeaderboardNextTenBtn.IsEnabled = false;
            }
        }

        //function that refresh the leaderboard
        private void LeaderboardRefreshBtn_Click(object sender, RoutedEventArgs e)
        {
            scores = DatabaseService.GetLeaderboardTop10();
            //clear current leaderboard
            LeaderboardGrid.Children.Clear();
            //bool to note we are not searching
            isSerching = false;
            //enable LeaderboardPrieviousTenBtn
            LeaderboardPreviousTenBtn.IsEnabled = true;
            //enable LeaderboardNextTenBtn
            LeaderboardNextTenBtn.IsEnabled = true;
            //fill the leaderboard with the gotten entries
            LeaderboardGrid.FillLeaderboard(scores);
        }
    }
}

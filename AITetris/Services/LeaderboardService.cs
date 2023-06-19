using AITetris.Classes;
using AITetris.Enums;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows;

namespace AITetris.Services;
public static class LeaderboardService
{
    public static void FillLeaderboard(this Grid leaderboardGrid, List<Game> scores)
    {
        // Clear current leaderboard
        leaderboardGrid.Children.Clear();

        // Remove current definitions
        leaderboardGrid.RowDefinitions.Clear();
        leaderboardGrid.ColumnDefinitions.Clear();

        // Amount of rows in the leaderboard
        int rowCount = scores.Count + 1;

        // Amount of columns in the leaderboard
        int columnCount = 6;

        // Create new row definitions
        for (int i = 0; i < rowCount; i++)
        {
            leaderboardGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
        }

        // Create new row definitions
        for (int i = 0; i < columnCount; i++)
        {
            leaderboardGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        }

        GeneratedLeaderboardBody(scores, leaderboardGrid, rowCount, columnCount);
    }

    private static void GeneratedLeaderboardBody(List<Game> scores, Grid leaderboardGrid, int rowCount, int columnCount)
    {
        // Running for each row in the leaderboard
        for (int row = 0; row < rowCount; row++)
        {
            // Running for each column in the leaderboard
            for (int column = 0; column < columnCount; column++)
            {
                Border border;
                //if row is zero, generated headers
                if (row == 0)
                {
                    border = GameOverService.GenerateHeaders((EColumnsHeaderNames)column, row);
                }
                else
                {
                    border = scores.GeneratedNormalBorder(column, row);
                }
                leaderboardGrid.Children.Add(border);
            }
        }
    }

    public static (bool found, List<Game> SearchScores) SearchWithinLeaderboard(this Grid LeaderboardGrid, List<Game> scores, bool isSerching, string FindPlayerText)
    {
        //check if searching
        if (isSerching)
        {
            //check if searched name yeilded results
            if (scores.Count == 0)
            {
                //disable LeaderboardNextTenBtn
                return (false, scores);
            }

            //check if there are any next entries to get
            if (DatabaseService.GetNext10Leaderboard(scores[scores.Count - 1].Rank, FindPlayerText).Count == 0)
            {
                //disable LeaderboardNextTenBtn
                return (false, scores);
            }

            //call sql database for next 10 by rank with searched name
            scores = DatabaseService.GetNext10Leaderboard(scores[scores.Count - 1].Rank, FindPlayerText);
            //clear current leaderboard
            LeaderboardGrid.Children.Clear();
            //fill the leaderboard with the gotten entries
            LeaderboardGrid.FillLeaderboard(scores);
            //enable LeaderboardPrieviousTenBtn
            return (true, scores);
        }
        else
        {
            //check if there are any next entries to get
            if (DatabaseService.GetNext10Leaderboard(scores[scores.Count - 1].Rank).Count == 0)
            {
                //disable LeaderboardNextTenBtn
                return (false, scores);
            }

            //call sql database for next 10 by rank
            scores = DatabaseService.GetNext10Leaderboard(scores[scores.Count - 1].Rank);
            //clear current leaderboard
            LeaderboardGrid.Children.Clear();
            //fill the leaderboard with the gotten entries
            LeaderboardGrid.FillLeaderboard(scores);
            //enable LeaderboardPrieviousTenBtn
            return (true, scores);
        }
    }

}

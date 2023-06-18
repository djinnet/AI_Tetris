﻿using AITetris.Classes;
using AITetris.Pages;
using AITetris.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
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

namespace AITetris.Controllers
{
    /// <summary>
    /// Interaction logic for GameOverMenu.xaml
    /// </summary>
    public partial class GameOverMenu : UserControl
    {
        private GameBoard gameBoard;
        // Generation variables
        string generationName;
        int generationNumber;
        List<Individual> individuals;
        int seed = 0;
        private Game gameFromDB;

        public GameOverMenu(GameBoard gameBoard)
        {
            InitializeComponent();
            this.gameBoard = gameBoard;
            gameFromDB = DatabaseService.GetExactLeaderboardEntry(gameBoard.game);
            List<Game> leaderboard = DatabaseService.Get4AboveCurrentRank(gameFromDB.Rank);

            // Check to see if character is AI
            if(!gameBoard.game.IsPlayer)
            {
                // Set the generation variables
                generationName = gameBoard.game.Character.Name;
                generationNumber = (gameBoard.game.Character as AI).GenerationNumber;
                individuals = (gameBoard.game.Character as AI).Population.ToList();
                seed = (gameBoard.game.Character as AI).Seed;
            }

            // Default set buttons to true, then disable later
            GameOverMenuControlSaveAI.IsEnabled = true;
            GameOverMenuControlRevive.IsEnabled = true;
            FillLeaderboard(leaderboard);

            // Check if the game contains a player or AI
            if(gameBoard.game.IsPlayer == true)
            {
                // Disable save AI button for player
                GameOverMenuControlSaveAI.IsEnabled = false;

                // Enable or disable the upgrades bought
                if (gameBoard.game.Upgrades.Revive == 0)
                {
                    // Disable Revive button for Player if you have no revives
                    GameOverMenuControlRevive.IsEnabled = false;
                }
            }
            else
            {
                // Disable Revive button for AI
                GameOverMenuControlRevive.IsEnabled = false;
            }
        }
        private void FillLeaderboard(List<Game> scores)
        {
            // Leaderbordgrid
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

                        // Add content and styling to the label
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

                        // Add content and styling to the label

                        label.Content = j switch
                        {
                            0 => scores[i - 1].Rank,
                            1 => scores[i - 1].Character.Name,
                            2 => scores[i - 1].Points,
                            3 => scores[i - 1].LinesCleared,
                            4 => scores[i - 1].Time,
                            5 => scores[i - 1].IsPlayer ? "Player" : "AI",
                            _ => ""
                        };

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
        
        // UI buttons
        // A button that triggers a revive when upgrade is created
        private void GameOverMenuControlRevive_Click(object sender, RoutedEventArgs e)
        {
            gameBoard.game.rank = gameFromDB.Rank;
            gameBoard.Revive();
            gameBoard.GameBoardMainGrid.Children.Remove(this);
        }

        // A button that triggers a save of the AI in training
        private void GameOverMenuControlSaveAI_Click(object sender, RoutedEventArgs e)
        {
            // Saving the generation
            DatabaseService.CreateGenerationEntry(generationName, generationNumber, seed);

            // Saving the individuals
            DatabaseService.Create10IndividualsEntry(individuals, DatabaseService.GetLastGenerationID());

            // Navigating back to main menu
            ((NavigationWindow)Window.GetWindow(this)).NavigationService.Navigate(new Uri("Pages/MainPage.xaml", UriKind.Relative));
        }

        // Navigation
        private void GameOverMenuControlQuitGame_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).KeyDown -= gameBoard.GamePage_KeyDown;
            string exeDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            int metaCurrency = Convert.ToInt32(File.ReadAllText(exeDir + "/Assets/JSON/MetaCurrency.txt"));
            File.WriteAllText(exeDir + "/Assets/JSON/MetaCurrency.txt", ((gameBoard.game.Points / 100) + metaCurrency).ToString());
            // Navigating back to main menu
            ((NavigationWindow)Window.GetWindow(this)).NavigationService.Navigate(new Uri("Pages/MainPage.xaml", UriKind.Relative));
        }
    }
}

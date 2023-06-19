using AITetris.Classes;
using AITetris.Enums;
using AITetris.Pages;
using AITetris.Services;
using AITetris.Stores;
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
        private GameBoard gameBoard { get; set; }

        // Generation variables
        string generationName { get; set; }
        int generationNumber { get; set; }
        List<Individual> individuals { get; set; }
        int seed { get; set; } = 0;
        private Game gameFromDB { get; set; }

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
            LeaderboardGrid.FillLeaderboard(leaderboard);

            (bool SaveAi, bool Revive) = gameBoard.CheckPlayerStateOfSaveAiAndRevive();
            GameOverMenuControlSaveAI.IsEnabled = SaveAi;
            GameOverMenuControlRevive.IsEnabled = Revive;
        }
       
        // UI buttons
        // A button that triggers a revive when upgrade is created
        private void GameOverMenuControlRevive_Click(object sender, RoutedEventArgs e)
        {
            gameBoard.game.Rank = gameFromDB.Rank;
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
            ((NavigationWindow)Window.GetWindow(this)).NavigationService.Navigate(FileStore.MainMenu);
        }

        // Navigation
        private void GameOverMenuControlQuitGame_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).KeyDown -= gameBoard.GamePage_KeyDown;
            int metaCurrency = TxtIOService.Read;
            TxtIOService.Write(metaCurrency, gameBoard.game.Points);
            // Navigating back to main menu
            ((NavigationWindow)Window.GetWindow(this)).NavigationService.Navigate(FileStore.MainMenu);
        }
    }
}

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
using static System.Formats.Asn1.AsnWriter;

namespace AITetris.Controls
{
    /// <summary>
    /// Interaction logic for PauseMenu.xaml
    /// </summary>
    public partial class PauseMenu : UserControl
    {
        // Game variables
        private GameBoard game;

        public PauseMenu(GameBoard game)
        {
            InitializeComponent();

            // Set game to an instance of the current game
            this.game = game;

            AddUpgradeToPauseMenuUpgradesSP();
        }

        // Function that adds the active upgrades from the startmenu to the pause menu
        private void AddUpgradeToPauseMenuUpgradesSP()
        {
            // Clear stackpanel to avoid dublication
            PauseMenuUpgradesSP.Children.Clear();

            // Add amount of revives
            PauseMenuUpgradesSP.Children.Add(
                new Label { 
                    Content = "Revives left: " + game.game.Upgrades.Revive,
                    FontFamily = new FontFamily("Tahoma"),
                    FontSize = 20,
                    FontWeight = FontWeights.Bold,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(10, 10, 10, 10)
                });

            // Add amount of score multiplier
            PauseMenuUpgradesSP.Children.Add(
                new Label
                {
                    Content = "Score multiplier: " + game.game.Upgrades.ScoreMultiplier + "x",
                    FontFamily = new FontFamily("Tahoma"),
                    FontSize = 20,
                    FontWeight = FontWeights.Bold,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(10, 10, 10, 10)
                });

            // Add amount of remove swap
            PauseMenuUpgradesSP.Children.Add(
                new Label
                {
                    Content = "Remove swap left: " + (game.game.Upgrades.RemoveSwap ? "1":"0"),
                    FontFamily = new FontFamily("Tahoma"),
                    FontSize = 20,
                    FontWeight = FontWeights.Bold,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(10, 10, 10, 10)
                });

            // Add amount of emergency line clears
            PauseMenuUpgradesSP.Children.Add(
                new Label
                {
                    Content = "Emergency line clears left: " + game.game.Upgrades.EmergancyLineClear,
                    FontFamily = new FontFamily("Tahoma"),
                    FontSize = 20,
                    FontWeight = FontWeights.Bold,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(10, 10, 10, 10)
                });
            
            // Add amount of slow time
            PauseMenuUpgradesSP.Children.Add(
                new Label
                {
                    Content = "Slow time left: " + game.game.Upgrades.SlowTime,
                    FontFamily = new FontFamily("Tahoma"),
                    FontSize = 20,
                    FontWeight = FontWeights.Bold,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(10, 10, 10, 10)
                });
        }

        // UI buttons
        // Back to game
        private void PauseMenuResumeBtn_Click(object sender, RoutedEventArgs e)
        {
            // Toggle pause off
            game.TogglePauseGame();
        }

        // Open the settings menu
        private void PauseMenuSettingsBtn_Click(object sender, RoutedEventArgs e)
        {
            // Create an instance of the settings menu controller for the current game
            SettingsMenuUserController settings = new SettingsMenuUserController(game);

            // Add the settings menu to the gameboardmaingrid
            game.GameBoardMainGrid.Children.Add(settings);

            // Position the settings menu
            Grid.SetColumn(settings, 1);
            Grid.SetRow(settings, 1);
            Grid.SetColumnSpan(settings, 5);
            Grid.SetRowSpan(settings, 7);
        }

        // Return to the main menu by forfeit
        private void PauseMenuForfeitGame_Click(object sender, RoutedEventArgs e)
        {
            // Unpause the game
            game.TogglePauseGame();

            // Trigger the lose game event
            game.LoseGame(true);
        }

        // Open the leaaderboard menu
        private void PauseMenuLeaderboard_Click(object sender, RoutedEventArgs e)
        {
            // Create an instance of the leaderboard menu controller for the current game
            LeaderboardUserController leaderboard = new LeaderboardUserController(game);

            // Add the leaderboard menu to the gameboardmaingrid
            game.GameBoardMainGrid.Children.Add(leaderboard);

            // Position the leaderboard menu
            Grid.SetColumn(leaderboard, 1);
            Grid.SetRow(leaderboard, 1);
            Grid.SetColumnSpan(leaderboard, 5);
            Grid.SetRowSpan(leaderboard, 7);
        }
    }
}

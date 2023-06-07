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
    /// Interaction logic for PauseMenu.xaml
    /// </summary>
    public partial class PauseMenu : UserControl
    {
        private GameBoard game;
        public PauseMenu(GameBoard game)
        {
            InitializeComponent();
            this.game = game;
        }

        private void PauseMenuResumeBtn_Click(object sender, RoutedEventArgs e)
        {
            game.TogglePauseGame();
        }

        private void PauseMenuSettingsBtn_Click(object sender, RoutedEventArgs e)
        {
            SettingsMenuUserController settings = new SettingsMenuUserController(game);
            game.GameBoardMainGrid.Children.Add(settings);
            Grid.SetColumn(settings, 1);
            Grid.SetRow(settings, 1);

            Grid.SetColumnSpan(settings, 5);
            Grid.SetRowSpan(settings, 7);
        }

        private void PauseMenuForfeitGame_Click(object sender, RoutedEventArgs e)
        {
            game.TogglePauseGame();
            game.LoseGame(true);
        }

        private void PauseMenuLeaderboard_Click(object sender, RoutedEventArgs e)
        {
            LeaderboardUserController leaderboard = new LeaderboardUserController(game);
            game.GameBoardMainGrid.Children.Add(leaderboard);
            Grid.SetColumn(leaderboard, 1);
            Grid.SetRow(leaderboard, 1);

            Grid.SetColumnSpan(leaderboard, 5);
            Grid.SetRowSpan(leaderboard, 7);
        }
    }
}

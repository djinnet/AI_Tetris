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

namespace AITetris.Pages
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }

        // UI buttons
        // Navigation
        private void MainMenuStartGameBtn_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to the Start Game Menu
            NavigationService.Navigate(new Uri("Pages/StartGameMenu.xaml", UriKind.Relative));
        }

        // Navigation
        private void MainMenuLeaderboardBtn_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to the Leaderboard
            NavigationService.Navigate(new Uri("Pages/Leaderboard.xaml", UriKind.Relative));
        }

        // Navigation
        private void MainMenuPointShopBtn_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to the Point shop
            NavigationService.Navigate(new Uri("Pages/PointShop.xaml", UriKind.Relative));
        }

        // Navigation
        private void MainMenuSettingsBtn_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to the Settings Menu
            NavigationService.Navigate(new Uri("Pages/SettingsMenu.xaml", UriKind.Relative));
        }

        // Quit game
        private void MainMenuExitBtn_Click(object sender, RoutedEventArgs e)
        {
            // A popup is shown that allows you to take action
            MessageBoxResult result = MessageBox.Show("Do you really want to exit the game?", "Confirmation", MessageBoxButton.OKCancel);

            // Check if the response of the popup was OK
            if (result == MessageBoxResult.OK)
            {
                // Close the program
                Application.Current.Shutdown();
            }
        }
    }
}

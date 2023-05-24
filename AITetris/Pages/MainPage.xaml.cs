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

        //private void NavigateTestPage_Click(object sender, RoutedEventArgs e)
        //{
        //    NavigationService.Navigate(new Uri("Pages/TestPage.xaml", UriKind.Relative));
        //}

        private void MainMenuStartGameBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("Pages/StartGameMenu.xaml", UriKind.Relative));
        }

        private void MainMenuLeaderboardBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("Pages/Leaderboard.xaml", UriKind.Relative));
        }

        private void MainMenuPointShopBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("Pages/PointShop.xaml", UriKind.Relative));
        }

        private void MainMenuSettingsBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("Pages/Settings.xaml", UriKind.Relative));
        }

        private void MainMenuExitBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you really want to exit the game?", "Confirmation", MessageBoxButton.OKCancel);

            if (result == MessageBoxResult.OK)
            {
                Application.Current.Shutdown();
            }
        }
    }
}

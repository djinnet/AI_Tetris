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

namespace AITetris.Controllers
{
    /// <summary>
    /// Interaction logic for GameOverMenu.xaml
    /// </summary>
    public partial class GameOverMenu : UserControl
    {
        public GameOverMenu()
        {
            InitializeComponent();
        }
        
        // UI buttons
        // A button that triggers a revive when upgrade is created
        private void GameOverMenuControlRevive_Click(object sender, RoutedEventArgs e)
        {

        }

        // A button that triggers a save of the AI in training
        private void GameOverMenuControlSaveAI_Click(object sender, RoutedEventArgs e)
        {

        }

        // Navigation
        private void GameOverMenuControlQuitGame_Click(object sender, RoutedEventArgs e)
        {
            // Navigating back to main menu
            ((NavigationWindow)Window.GetWindow(this)).NavigationService.Navigate(new Uri("Pages/MainPage.xaml", UriKind.Relative));
        }
    }
}

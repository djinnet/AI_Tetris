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

        private void GameOverMenuControlRevive_Click(object sender, RoutedEventArgs e)
        {

        }

        private void GameOverMenuControlSaveAI_Click(object sender, RoutedEventArgs e)
        {

        }

        private void GameOverMenuControlQuitGame_Click(object sender, RoutedEventArgs e)
        {
            
            ((NavigationWindow)Window.GetWindow(this)).NavigationService.Navigate(new Uri("Pages/MainPage.xaml", UriKind.Relative));
        }
    }
}

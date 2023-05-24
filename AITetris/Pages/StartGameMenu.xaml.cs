using AITetris.Classes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Interaction logic for StartGameMenu.xaml
    /// </summary>
    public partial class StartGameMenu : Page
    {
        public StartGameMenu()
        {
            InitializeComponent();
        }

        private void PlayAsAichb_Click(object sender, RoutedEventArgs e)
        {
            PickAIcmb.IsEnabled = ((CheckBox)sender).IsChecked ?? false;
        }

        private void StartGamebnt_Click(object sender, RoutedEventArgs e)
        {
            if(Nametxtbox.Text.Length > 0)
            {
                if (PlayAsAichb.IsChecked ?? false)
                {
                    Player player = new Player(Nametxtbox.Text);
                    NavigationService.Navigate(new Uri("GameBoard.xaml", UriKind.Relative));
                }
                else
                {
                    //TODO AI create
                }
            }
            else
            {
                NoNamelbl.Visibility = Visibility.Visible;
            }
        }

        private void Cancelbtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}

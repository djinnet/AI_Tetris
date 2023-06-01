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
        public PauseMenu()
        {
            InitializeComponent();
        }

        private void PauseMenuResumeBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PauseMenuSettingsBtn_Click(object sender, RoutedEventArgs e)
        {
            ((NavigationWindow)Window.GetWindow(this)).NavigationService.Navigate(new Uri("Pages/Settings.xaml", UriKind.Relative));
        }

        private void PauseMenuForfeitGame_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

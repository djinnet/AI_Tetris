using AITetris.Services;
using AITetris.Stores;
using System.Windows;
using System.Windows.Navigation;

namespace AITetris
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : NavigationWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            // Event that maximize the window when program is launched
            Loaded += MainWindow_Loaded;


            if (!JsonIOService.CheckSetting())
            {
                //log the failed crash and stop the program, since it couldnt load or create the settings.
                return;
            }

            // Navigate to the first page of the program (Main menu)
            NavigationService.Navigate(FileStore.MainMenu);
        }

        // Event - Fullscreen on launch
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Overwrite the current windowstyle to none
            WindowStyle = WindowStyle.None;

            // Overwrite the current windowstate to maximized
            WindowState = WindowState.Maximized;
        }
    }
}

using AITetris.Classes;
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


            try
            {
                // Try to read the settings JSON file
                File.ReadAllText(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "/Assets/JSON/Settings.json");
            }
            catch
            {
                // Create the settings JSON file if it fails
                File.WriteAllText(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "/Assets/JSON/Settings.json", JsonSerializer.Serialize(new Settings(), new JsonSerializerOptions() { WriteIndented = true }));
            }

            // Navigate to the first page of the program (Main menu)
            NavigationService.Navigate(new Uri("Pages/MainPage.xaml", UriKind.Relative));
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

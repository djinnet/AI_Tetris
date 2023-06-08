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
using System.Text.Json;
using System.IO;
using AITetris.Classes;
using AITetris.Controllers;
using AITetris.Controls;

namespace AITetris.Pages
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class SettingsMenu : Page
    {
        // String for containing execution directory path
        private string exeDir;

        // Settings object for holding the settings in the settigns JSON file
        public Settings settings;

        public SettingsMenu()
        {
            InitializeComponent();

            // Set the execution directory path
            exeDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

            // Load in the settings JSON
            settings = JsonSerializer.Deserialize<Settings>(File.ReadAllText(exeDir + "/Assets/JSON/Settings.json"));

            // Set the settings controls using the settings
            SettingsSliderVolume.Value = settings.volume;
            SettingsSliderSpeed.Value = settings.startSpeed;
            SettingsSliderDeltaSpeed.Value = settings.gameSpeed;
            SettingsSliderAITraining.Value = Convert.ToInt32(settings.enableTraining);
            SettingsSliderSaveBlock.Value = Convert.ToInt32(settings.enableSwapBlock);
            SettingsSliderNextBlock.Value = Convert.ToInt32(settings.enableNextBlock);
        }

        // UI slider controls
        // Volume slider
        private void SettingsSliderVolume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // Set the settings.volume to the slider value
            settings.volume = (int)((Slider)sender).Value;
        }

        // Startspeed slider
        private void SettingsSliderSpeed_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // Set the settings.startspeed to the slider value
            settings.startSpeed = ((Slider)sender).Value;
        }

        // Deltaspeed slider
        private void SettingsSliderDeltaSpeed_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // Set the settings.deltaspeed to the slider value
            settings.gameSpeed = ((Slider)sender).Value;
        }

        // Toggle slider
        // AI training toggle
        private void SettingsSliderAITraining_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // Check if the setting is toggle on or off
            if (e.NewValue == 0)
            {
                // Toggle setting off
                settings.enableTraining = false;
                SettingsSliderValueAITraining.Content = "Off";
            }
            else
            {
                // Toggle setting on
                settings.enableTraining = true;
                SettingsSliderValueAITraining.Content = "On";
            }
        }

        // Save block toggle
        private void SettingsSliderSaveBlock_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // Check if the setting is toggle on or off
            if (e.NewValue == 0)
            {
                // Toggle setting off
                settings.enableSwapBlock = false;
                SettingsSliderValueSaveBlock.Content = "Off";
            }
            else
            {
                // Toggle setting on
                settings.enableSwapBlock = true;
                SettingsSliderValueSaveBlock.Content = "On";
            }
        }

        // Next block toggle
        private void SettingsSliderNextBlock_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // Check if the setting is toggle on or off
            if (e.NewValue == 0)
            {
                // Toggle setting off
                settings.enableNextBlock = false;
                SettingsSliderValueNextBlock.Content = "Off";
            }
            else
            {
                // Toggle setting on
                settings.enableNextBlock = true;
                SettingsSliderValueNextBlock.Content = "On";
            }
        }

        // UI buttons
        // Navigation, open the keybind user control
        private void SettingsControlsKeybinds_Click(object sender, RoutedEventArgs e)
        {
            // Set the keybindmenu
            KeybindsMenu menu = new KeybindsMenu(this);

            // Add the menu to the Settings menu grid
            SettingsMenuGrid.Children.Add(menu);

            // Locate and size the user control
            Grid.SetColumn(menu, 1);
            Grid.SetRow(menu, 1);
            Grid.SetColumnSpan(menu, 2);
            Grid.SetRowSpan(menu, 2);
        }

        // Apply / Save settings to settings JSON
        private void SettingsControlsApplySettings_Click(object sender, RoutedEventArgs e)
        {
            // Write new settings to the settings file
            File.WriteAllText(exeDir + "/Assets/JSON/Settings.json", JsonSerializer.Serialize(settings, new JsonSerializerOptions() { WriteIndented = true }));
        }

        // Navigation
        private void SettingsControlsMainMenu_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to the Main Menu
            NavigationService.Navigate(new Uri("Pages/MainPage.xaml", UriKind.Relative));
        }
    }
}

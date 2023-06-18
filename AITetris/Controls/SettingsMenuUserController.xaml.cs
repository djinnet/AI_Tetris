using AITetris.Classes;
using AITetris.Pages;
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

namespace AITetris.Controls
{
    /// <summary>
    /// Interaction logic for SettingsMenuUserController.xaml
    /// </summary>
    public partial class SettingsMenuUserController : UserControl
    {
        // Directory variables
        private string exeDir;
        
        // Menu variables
        public Settings settings;
        private GameBoard game;

        public SettingsMenuUserController(GameBoard game)
        {
            InitializeComponent();

            // Set the execution directory path
            exeDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

            // Set the current settings to the game settings
            settings = game.game.Settings;

            // Update the settings variables with the settings from the game settings
            SettingsSliderVolume.Value = settings.Volume;
            SettingsSliderSpeed.Value = settings.StartSpeed;
            SettingsSliderDeltaSpeed.Value = settings.GameSpeed;
            SettingsSliderAITraining.Value = Convert.ToInt32(settings.EnableTraining);
            SettingsSliderSaveBlock.Value = Convert.ToInt32(settings.EnableSwapBlock);
            SettingsSliderNextBlock.Value = Convert.ToInt32(settings.EnableNextBlock);

            // Set the game to the current game
            this.game = game;

            // Set the state to false since there is no new changes
            SettingsControlsApplySettings.IsEnabled = false;
        }

        //UI buttons
        // Close the settings menu return to pause menu
        private void SettingsControlsPauseMenu_Click(object sender, RoutedEventArgs e)
        {
            if (SettingsControlsApplySettings.IsEnabled == true)
            {
                // A popup is shown that allows you to take action
                MessageBoxResult result = MessageBox.Show("There is unapplied changes to the settings, do you want to apply the changes", "Warning", MessageBoxButton.YesNo);

                // Check if the response of the popup was OK
                if (result == MessageBoxResult.Yes)
                {
                    // Write new settings to the settings file
                    File.WriteAllText(exeDir + "/Assets/JSON/Settings.json", JsonSerializer.Serialize(settings, new JsonSerializerOptions() { WriteIndented = true }));

                    // Set the state to false since was just applied
                    SettingsControlsApplySettings.IsEnabled = false;
                }
            }

            // Removing the settings menu from the gameboardmaingrid to close it
            game.GameBoardMainGrid.Children.Remove(this);
        }

        // Write aand apply the newly set settings
        private void SettingsControlsApplySettings_Click(object sender, RoutedEventArgs e)
        {
            // Write the settings to the settings JSON file
            File.WriteAllText(exeDir + "/Assets/JSON/Settings.json", JsonSerializer.Serialize(settings, new JsonSerializerOptions() { WriteIndented = true }));

            // Apply the new settings to the game settings
            game.ApplySettings(settings);

            // Set the state to false since was just applied
            SettingsControlsApplySettings.IsEnabled = false;
        }

        // Open the keybinds menu from the Settings menu
        private void SettingsControlsKeybinds_Click(object sender, RoutedEventArgs e)
        {
            // Create a new keybind menu
            KeybindsMenu menu = new KeybindsMenu(game, settings);

            // Add the keybind menu to the gamebordmaingrid
            game.GameBoardMainGrid.Children.Add(menu);

            // Position the keybind menu
            Grid.SetColumn(menu, 1);
            Grid.SetRow(menu, 1);
            Grid.SetColumnSpan(menu, 5);
            Grid.SetRowSpan(menu, 7);

            // Set the state to true since there is new changes
            SettingsControlsApplySettings.IsEnabled = true;
        }

        // UI Slider controls
        // Next block
        private void SettingsSliderNextBlock_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // Check the slider value through sender
            if (e.NewValue == 0)
            {
                // Toggle the next block Off
                settings.EnableNextBlock = false;
                SettingsSliderValueNextBlock.Content = "Off";
            }
            else
            {
                // Toggle the next block On
                settings.EnableNextBlock = true;
                SettingsSliderValueNextBlock.Content = "On";
            }

            // Set the state to true since there is new changes
            SettingsControlsApplySettings.IsEnabled = true;
        }

        // Save block
        private void SettingsSliderSaveBlock_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // Check the slider value through sender
            if (e.NewValue == 0)
            {
                // Toggle the save block Off
                settings.EnableSwapBlock = false;
                SettingsSliderValueSaveBlock.Content = "Off";
            }
            else
            {
                // Toggle the save block On
                settings.EnableSwapBlock = true;
                SettingsSliderValueSaveBlock.Content = "On";
            }

            // Set the state to true since there is new changes
            SettingsControlsApplySettings.IsEnabled = true;
        }

        // AI training
        private void SettingsSliderAITraining_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // Check the slider value through sender
            if (e.NewValue == 0)
            {
                // Toggle the AI training Off
                settings.EnableTraining = false;
                SettingsSliderValueAITraining.Content = "Off";
            }
            else
            {
                // Toggle the AI training On
                settings.EnableTraining = true;
                SettingsSliderValueAITraining.Content = "On";
            }

            // Set the state to true since there is new changes
            SettingsControlsApplySettings.IsEnabled = true;
        }

        // Deltaspeed
        private void SettingsSliderDeltaSpeed_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // Set the gamespeed/deltaspeed to the current slider value
            settings.GameSpeed = ((Slider)sender).Value;

            // Set the state to true since there is new changes
            SettingsControlsApplySettings.IsEnabled = true;
        }

        // Default speed
        private void SettingsSliderSpeed_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // Set the Startspeed/Defaultspeed to the current slider value
            settings.StartSpeed = ((Slider)sender).Value;

            // Set the state to true since there is new changes
            SettingsControlsApplySettings.IsEnabled = true;
        }

        // Volume
        private void SettingsSliderVolume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // Set the volume to the current slider value
            settings.Volume = (int)((Slider)sender).Value;

            // Set the state to true since there is new changes
            SettingsControlsApplySettings.IsEnabled = true;
        }
    }
}

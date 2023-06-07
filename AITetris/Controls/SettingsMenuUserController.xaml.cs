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
        private string exeDir;
        public Settings settings;
        private GameBoard game;
        public SettingsMenuUserController(GameBoard game)
        {
            InitializeComponent();

            exeDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            settings = JsonSerializer.Deserialize<Settings>(File.ReadAllText(exeDir + "/Assets/JSON/Settings.json"));
            SettingsSliderVolume.Value = settings.volume;
            SettingsSliderSpeed.Value = settings.startSpeed;
            SettingsSliderDeltaSpeed.Value = settings.gameSpeed;
            SettingsSliderAITraining.Value = Convert.ToInt32(settings.enableTraining);
            SettingsSliderSaveBlock.Value = Convert.ToInt32(settings.enableSwapBlock);
            SettingsSliderNextBlock.Value = Convert.ToInt32(settings.enableNextBlock);
            this.game = game;
        }

        private void SettingsControlsPauseMenu_Click(object sender, RoutedEventArgs e)
        {
            game.GameBoardMainGrid.Children.Remove(this);
        }

        private void SettingsControlsApplySettings_Click(object sender, RoutedEventArgs e)
        {
            File.WriteAllText(exeDir + "/Assets/JSON/Settings.json", JsonSerializer.Serialize(settings, new JsonSerializerOptions() { WriteIndented = true }));
            game.ApplySettings(settings);
        }

        private void SettingsControlsKeybinds_Click(object sender, RoutedEventArgs e)
        {
            KeybindsMenu menu = new KeybindsMenu(game, settings);
            game.GameBoardMainGrid.Children.Add(menu);
            Grid.SetColumn(menu, 1);
            Grid.SetRow(menu, 1);

            Grid.SetColumnSpan(menu, 5);
            Grid.SetRowSpan(menu, 7);
        }

        private void SettingsSliderNextBlock_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (e.NewValue == 0)
            {
                settings.enableNextBlock = false;
                SettingsSliderValueNextBlock.Content = "Off";
            }
            else
            {
                settings.enableNextBlock = true;
                SettingsSliderValueNextBlock.Content = "On";
            }
        }

        private void SettingsSliderSaveBlock_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (e.NewValue == 0)
            {
                settings.enableSwapBlock = false;
                SettingsSliderValueSaveBlock.Content = "Off";
            }
            else
            {
                settings.enableSwapBlock = true;
                SettingsSliderValueSaveBlock.Content = "On";
            }
        }

        private void SettingsSliderAITraining_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (e.NewValue == 0)
            {
                settings.enableTraining = false;
                SettingsSliderValueAITraining.Content = "Off";
            }
            else
            {
                settings.enableTraining = true;
                SettingsSliderValueAITraining.Content = "On";
            }
        }

        private void SettingsSliderDeltaSpeed_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            settings.gameSpeed = ((Slider)sender).Value;
        }

        private void SettingsSliderSpeed_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            settings.startSpeed = ((Slider)sender).Value;
        }

        private void SettingsSliderVolume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            settings.volume = (int)((Slider)sender).Value;
        }
    }
}

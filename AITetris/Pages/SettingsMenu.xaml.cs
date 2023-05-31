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

namespace AITetris.Pages
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class SettingsMenu : Page
    {
        private string exeDir;
        private Settings settings;

        public SettingsMenu()
        {
            InitializeComponent();
            exeDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            Debug.WriteLine(File.ReadAllText(exeDir + "/Assets/JSON/Settings.json"));
            settings = JsonSerializer.Deserialize<Settings>(File.ReadAllText(exeDir + "/Assets/JSON/Settings.json"));
            SettingsSliderVolume.Value = settings.volume;
            SettingsSliderSpeed.Value = settings.startSpeed;
            SettingsSliderDeltaSpeed.Value = settings.gameSpeed;
            SettingsSliderAITraining.Value = Convert.ToInt32(settings.enableTraining);
            SettingsSliderSaveBlock.Value = Convert.ToInt32(settings.enableSwapBlock);
            SettingsSliderNextBlock.Value = Convert.ToInt32(settings.enableNextBlock);
        }

        private void SettingsSliderVolume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Debug.WriteLine(SettingsSliderVolume.Value);
        }

        private void SettingsSliderSpeed_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Debug.WriteLine(SettingsSliderSpeed.Value);
        }

        private void SettingsSliderDeltaSpeed_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Debug.WriteLine(SettingsSliderDeltaSpeed.Value);
        }

        private void SettingsSliderAITraining_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int value = (int)SettingsSliderAITraining.Value;

            if(value == 0)
            {
                SettingsSliderValueAITraining.Content = "Off";
            }
            else
            {
                SettingsSliderValueAITraining.Content = "On";
            }
        }

        private void SettingsSliderSaveBlock_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int value = (int)SettingsSliderSaveBlock.Value;

            if (value == 0)
            {
                SettingsSliderValueSaveBlock.Content = "Off";
            }
            else
            {
                SettingsSliderValueSaveBlock.Content = "On";
            }
        }

        private void SettingsSliderNextBlock_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int value = (int)SettingsSliderNextBlock.Value;

            if (value == 0)
            {
                SettingsSliderValueNextBlock.Content = "Off";
            }
            else
            {
                SettingsSliderValueNextBlock.Content = "On";
            }
        }

        private void SettingsControlsKeybinds_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SettingsControlsApplySettings_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SettingsControlsMainMenu_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("Pages/MainPage.xaml", UriKind.Relative));
        }
    }
}

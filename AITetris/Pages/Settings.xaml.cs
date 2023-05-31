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
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Page
    {
        public Settings()
        {
            InitializeComponent();
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;
using AITetris.Classes;
using AITetris.Pages;

namespace AITetris.Controls
{
    /// <summary>
    /// Interaction logic for KeybindsMenu.xaml
    /// </summary>
    public partial class KeybindsMenu : UserControl
    {
        SettingsMenu prev;
        private bool isListeningForKey = false;
        private TextBox textbox;
        private string typeOfKeybind;

        public KeybindsMenu(SettingsMenu prev)
        {
            InitializeComponent();

            this.prev = prev;

            // Set keybinds from settings
            KeybindsMenuKeybindPauseTxtbox.Text = prev.settings.KeyBinds.pause.ToString();
            KeybindsMenuKeybindSwitchTxtbox.Text = prev.settings.KeyBinds.swap.ToString();
            KeybindsMenuKeybindInstantDropTxtbox.Text = prev.settings.KeyBinds.insta.ToString();
            KeybindsMenuKeybindRotateTxtbox.Text = prev.settings.KeyBinds.rotate.ToString();
            KeybindsMenuKeybindLeftTxtbox.Text = prev.settings.KeyBinds.left.ToString();
            KeybindsMenuKeybindRightTxtbox.Text = prev.settings.KeyBinds.right.ToString();
            KeybindsMenuKeybindDownTxtbox.Text = prev.settings.KeyBinds.drop.ToString();
        }

        private void KeybindsMenuBackToSettings_Click(object sender, RoutedEventArgs e)
        {
            ((SettingsMenu)prev).SettingsMenuGrid.Children.Remove(this);
        }

        private void UserControl_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if(isListeningForKey == true)
            {
                Key pressedKey = e.Key;

                switch (typeOfKeybind)
                {
                    case "Pause":
                        prev.settings.KeyBinds.pause = pressedKey;
                        break;

                    case "Switch":
                        prev.settings.KeyBinds.swap = pressedKey;
                        break;

                    case "InstantDrop":
                        prev.settings.KeyBinds.insta = pressedKey;
                        break;

                    case "Rotate":
                        prev.settings.KeyBinds.rotate = pressedKey;
                        break;

                    case "Left":
                        prev.settings.KeyBinds.left = pressedKey;
                        break;

                    case "Right":
                        prev.settings.KeyBinds.right = pressedKey;
                        break;

                    case "Down":
                        prev.settings.KeyBinds.drop = pressedKey;
                        break;

                    default:
                        break;
                }

                textbox.Text = pressedKey.ToString();

                isListeningForKey = false;
            }
        }

        private void KeybindsMenuKeybindPauseBtn_Click(object sender, RoutedEventArgs e)
        {
            textbox = KeybindsMenuKeybindPauseTxtbox;
            typeOfKeybind = "Pause";
            isListeningForKey = true;
        }

        private void KeybindsMenuKeybindSwitchBtn_Click(object sender, RoutedEventArgs e)
        {
            textbox = KeybindsMenuKeybindSwitchTxtbox;
            typeOfKeybind = "Switch";
            isListeningForKey = true;
        }

        private void KeybindsMenuKeybindInstantDropBtn_Click(object sender, RoutedEventArgs e)
        {
            textbox = KeybindsMenuKeybindInstantDropTxtbox;
            typeOfKeybind = "InstantDrop";
            isListeningForKey = true;
        }

        private void KeybindsMenuKeybindRotateBtn_Click(object sender, RoutedEventArgs e)
        {
            textbox = KeybindsMenuKeybindRotateTxtbox;
            typeOfKeybind = "Rotate";
            isListeningForKey = true;
        }

        private void KeybindsMenuKeybindLeftBtn_Click(object sender, RoutedEventArgs e)
        {
            textbox = KeybindsMenuKeybindLeftTxtbox;
            typeOfKeybind = "Left";
            isListeningForKey = true;
        }

        private void KeybindsMenuKeybindRightBtn_Click(object sender, RoutedEventArgs e)
        {
            textbox = KeybindsMenuKeybindRightTxtbox;
            typeOfKeybind = "Right";
            isListeningForKey = true;
        }

        private void KeybindsMenuKeybindDownBtn_Click(object sender, RoutedEventArgs e)
        {
            textbox = KeybindsMenuKeybindDownTxtbox;
            typeOfKeybind = "Down";
            isListeningForKey = true;
        }
    }
}

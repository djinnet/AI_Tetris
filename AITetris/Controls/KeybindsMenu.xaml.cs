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
        // Menu variables
        SettingsMenu prev;
        GameBoard gamePrev;
        Settings pauseMenuSettings;

        // Variable that toggles listener
        private bool isListeningForKey = false;

        // Placeholder variables to assist with keybinding
        private TextBox textbox;
        private string typeOfKeybind;

        public KeybindsMenu(SettingsMenu prev)
        {
            InitializeComponent();

            // Set prev to an instance of the settings menu from main menu
            this.prev = prev;

            // Set keybinds from settings
            KeybindsMenuKeybindPauseTxtbox.Text = prev.settings.keyBinds.pause.ToString();
            KeybindsMenuKeybindSwitchTxtbox.Text = prev.settings.keyBinds.swap.ToString();
            KeybindsMenuKeybindInstantDropTxtbox.Text = prev.settings.keyBinds.insta.ToString();
            KeybindsMenuKeybindRotateTxtbox.Text = prev.settings.keyBinds.rotate.ToString();
            KeybindsMenuKeybindLeftTxtbox.Text = prev.settings.keyBinds.left.ToString();
            KeybindsMenuKeybindRightTxtbox.Text = prev.settings.keyBinds.right.ToString();
            KeybindsMenuKeybindDownTxtbox.Text = prev.settings.keyBinds.drop.ToString();
        }

        public KeybindsMenu(GameBoard prev, Settings pauseMenuSettings)
        {
            InitializeComponent();

            // Set prev to an instance of the settings menu from the pause menu
            gamePrev = prev;
            this.pauseMenuSettings = pauseMenuSettings;

            // Set keybinds from settings
            KeybindsMenuKeybindPauseTxtbox.Text = pauseMenuSettings.keyBinds.pause.ToString();
            KeybindsMenuKeybindSwitchTxtbox.Text = pauseMenuSettings.keyBinds.swap.ToString();
            KeybindsMenuKeybindInstantDropTxtbox.Text = pauseMenuSettings.keyBinds.insta.ToString();
            KeybindsMenuKeybindRotateTxtbox.Text = pauseMenuSettings.keyBinds.rotate.ToString();
            KeybindsMenuKeybindLeftTxtbox.Text = pauseMenuSettings.keyBinds.left.ToString();
            KeybindsMenuKeybindRightTxtbox.Text = pauseMenuSettings.keyBinds.right.ToString();
            KeybindsMenuKeybindDownTxtbox.Text = pauseMenuSettings.keyBinds.drop.ToString();
        }
        // UI buttons
        // Remove the keybind menu
        private void KeybindsMenuBackToSettings_Click(object sender, RoutedEventArgs e)
        {
            // Check if gameprev is set
            if(gamePrev != null)
            {
                // Remove the keybindmenu from the gameboardmaingrid
                gamePrev.GameBoardMainGrid.Children.Remove(this);
            }
            else
            {
                // Remove the keybindmenu from the settingsmenugrid
                prev.SettingsMenuGrid.Children.Remove(this);
            }
        }

        // A listener event on the UserControl UI element
        private void UserControl_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // Check if listening is active
            if(isListeningForKey == true)
            {
                // Listen for which key is pressed
                Key pressedKey = e.Key;

                // Check if it is from the pause menu or game settings menu keybind is set
                if(pauseMenuSettings != null)
                {
                    // Check what type of keybind is pressed in the pausemenu keybinds
                    switch (typeOfKeybind)
                    {
                        case "Pause":
                            pauseMenuSettings.keyBinds.pause = pressedKey;
                            break;

                        case "Switch":
                            pauseMenuSettings.keyBinds.swap = pressedKey;
                            break;

                        case "InstantDrop":
                            pauseMenuSettings.keyBinds.insta = pressedKey;
                            break;

                        case "Rotate":
                            pauseMenuSettings.keyBinds.rotate = pressedKey;
                            break;

                        case "Left":
                            pauseMenuSettings.keyBinds.left = pressedKey;
                            break;

                        case "Right":
                            pauseMenuSettings.keyBinds.right = pressedKey;
                            break;

                        case "Down":
                            pauseMenuSettings.keyBinds.drop = pressedKey;
                            break;

                        default:
                            break;
                    }
                }
                else
                {
                    // Check what type of keybind is pressed in the settings keybind menu
                    switch (typeOfKeybind)
                    {
                        case "Pause":
                            prev.settings.keyBinds.pause = pressedKey;
                            break;

                        case "Switch":
                            prev.settings.keyBinds.swap = pressedKey;
                            break;

                        case "InstantDrop":
                            prev.settings.keyBinds.insta = pressedKey;
                            break;

                        case "Rotate":
                            prev.settings.keyBinds.rotate = pressedKey;
                            break;

                        case "Left":
                            prev.settings.keyBinds.left = pressedKey;
                            break;

                        case "Right":
                            prev.settings.keyBinds.right = pressedKey;
                            break;

                        case "Down":
                            prev.settings.keyBinds.drop = pressedKey;
                            break;

                        default:
                            break;
                    }
                }

                // Add the key to the corrosponding textbox
                textbox.Text = pressedKey.ToString();

                // Set the listener to inactive to make it stop listning always
                isListeningForKey = false;
            }
        }

        // UI buttons
        // Set keybind Pause
        private void KeybindsMenuKeybindPauseBtn_Click(object sender, RoutedEventArgs e)
        {
            // Set placeholder variables
            textbox = KeybindsMenuKeybindPauseTxtbox;
            typeOfKeybind = "Pause";

            // Set listening to active
            isListeningForKey = true;
        }

        // Set keybind Switch block
        private void KeybindsMenuKeybindSwitchBtn_Click(object sender, RoutedEventArgs e)
        {
            // Set placeholder variables
            textbox = KeybindsMenuKeybindSwitchTxtbox;
            typeOfKeybind = "Switch";

            // Set listening to active
            isListeningForKey = true;
        }

        // Set keybind Instant down
        private void KeybindsMenuKeybindInstantDropBtn_Click(object sender, RoutedEventArgs e)
        {
            // Set placeholder variables
            textbox = KeybindsMenuKeybindInstantDropTxtbox;
            typeOfKeybind = "InstantDrop";

            // Set listening to active
            isListeningForKey = true;
        }

        // Set keybind Rotate
        private void KeybindsMenuKeybindRotateBtn_Click(object sender, RoutedEventArgs e)
        {
            // Set placeholder variables
            textbox = KeybindsMenuKeybindRotateTxtbox;
            typeOfKeybind = "Rotate";

            // Set listening to active
            isListeningForKey = true;
        }

        // Set keybind Left
        private void KeybindsMenuKeybindLeftBtn_Click(object sender, RoutedEventArgs e)
        {
            // Set placeholder variables
            textbox = KeybindsMenuKeybindLeftTxtbox;
            typeOfKeybind = "Left";

            // Set listening to active
            isListeningForKey = true;
        }

        // Set keybind Right
        private void KeybindsMenuKeybindRightBtn_Click(object sender, RoutedEventArgs e)
        {
            // Set placeholder variables
            textbox = KeybindsMenuKeybindRightTxtbox;
            typeOfKeybind = "Right";

            // Set listening to active
            isListeningForKey = true;
        }

        // Set keybind Down
        private void KeybindsMenuKeybindDownBtn_Click(object sender, RoutedEventArgs e)
        {
            // Set placeholder variables
            textbox = KeybindsMenuKeybindDownTxtbox;
            typeOfKeybind = "Down";

            // Set listening to active
            isListeningForKey = true;
        }
    }
}

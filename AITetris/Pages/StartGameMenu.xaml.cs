using AITetris.Classes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using AITetris.Classes;
using System.Reflection;

namespace AITetris.Pages
{
    /// <summary>
    /// Interaction logic for StartGameMenu.xaml
    /// </summary>
    public partial class StartGameMenu : Page
    {
        // Toggle state of the upgrade buttons
        private bool[] buttonStates = { false, false, false, false, false, false, false, false, false, false, false, false };

        private Upgrades activatedUpgrades;

        // Path
        string exeDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

        // Settings
        Settings settings;

        // AI toggle is a variable controlling if the AI is active or not, it has a starting value of 0/off
        private int AIToggle = 0;

        List<AI> generations;

        public StartGameMenu()
        {
            InitializeComponent();

            // Add a background to the page
            // Get path to background
            string imagePath = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Assets\\Sprites\\Background\\SirBackground2.png";

            // Set an image source
            ImageSource imageSource = new BitmapImage(new Uri(imagePath));

            // Add image source to image
            StartGameBackground.Source = imageSource;

            // Set the focus of the page to this textbox
            Nametxtbox.Focus();
            activatedUpgrades = JsonSerializer.Deserialize<Upgrades>(File.ReadAllText(exeDir + "/Assets/JSON/Upgrades.json"));

            // Get current set settings
            settings = JsonSerializer.Deserialize<Settings>(File.ReadAllText(exeDir + "/Assets/JSON/Settings.json"));

            // Set the slider value accordingly to the settings
            AITrainingOnOffSlider.Value = settings.enableTraining ? 1:0;

            for (int i = 0; i < StartGameMenuUpgradesGrid.Children.Count; i++)
            {
                if(StartGameMenuUpgradesGrid.Children[i] is Button button)
                {
                    button.IsEnabled = activatedUpgrades.purchasedUpgrades[i];
                }
            }

            PopulateAIDropdown();
        }

        private void PopulateAIDropdown()
        {
            ComboBox dropdown = StartGameMenuAIDropdown;

            generations = SQLCalls.Load10AIGenerations();

            foreach (AI ai in generations) 
            {
                dropdown.Items.Add(ai.name + " " + ai.generationNumber);
            }

            dropdown.SelectedItem = dropdown.Items[0];
        }

        // Change button state of button pressed
        private void ChangeButtonState(int buttonStateIndex, Button clickedButton)
        {
            // Check the state of the button
            if (buttonStates[buttonStateIndex] == false)
            {
                // Changing state to on
                buttonStates[buttonStateIndex] = true;

                clickedButton.Background = Brushes.LightGreen;
                
                switch(buttonStateIndex)
                {
                    case 0:
                    case 1:
                    case 2:
                        activatedUpgrades.revive++;
                        break;
                    case 3:
                    case 4:
                        activatedUpgrades.scoreMultiplier += 0.25;
                        break;
                    case 5:
                        activatedUpgrades.scoreMultiplier += 0.5;
                        break;
                    case 6:
                    case 7:
                    case 8:
                        activatedUpgrades.emergancyLineClear++;
                        break;
                    case 9:
                        activatedUpgrades.removeSwap = true;
                        break;
                    case 10:
                    case 11:
                        activatedUpgrades.slowTime += 30;
                        break;
                }
            }
            else
            {
                // Changing state to off
                buttonStates[buttonStateIndex] = false;

                clickedButton.Background = Brushes.LightGray;

                switch (buttonStateIndex)
                {
                    case 0:
                    case 1:
                    case 2:
                        activatedUpgrades.revive--;
                        break;
                    case 3:
                    case 4:
                        activatedUpgrades.scoreMultiplier -= 0.25;
                        break;
                    case 5:
                        activatedUpgrades.scoreMultiplier -= 0.5;
                        break;
                    case 6:
                    case 7:
                    case 8:
                        activatedUpgrades.emergancyLineClear--;
                        break;
                    case 9:
                        activatedUpgrades.removeSwap = false;
                        break;
                    case 10:
                    case 11:
                        activatedUpgrades.slowTime -= 30;
                        break;
                }
            }
        }

        // UI buttons
        // Toggle the upgrades 1 - 12
        private void Upgrade1_Click(object sender, RoutedEventArgs e)
        {
            // Toggle current buttonstate
            ChangeButtonState(0,(Button)sender);
        }

        private void Upgrade2_Click(object sender, RoutedEventArgs e)
        {
            // Toggle current buttonstate
            ChangeButtonState(1, (Button)sender);
        }

        private void Upgrade3_Click(object sender, RoutedEventArgs e)
        {
            // Toggle current buttonstate
            ChangeButtonState(2, (Button)sender);
        }

        private void Upgrade4_Click(object sender, RoutedEventArgs e)
        {
            // Toggle current buttonstate
            ChangeButtonState(3, (Button)sender);
        }

        private void Upgrade5_Click(object sender, RoutedEventArgs e)
        {
            // Toggle current buttonstate
            ChangeButtonState(4, (Button)sender);
        }

        private void Upgrade6_Click(object sender, RoutedEventArgs e)
        {
            // Toggle current buttonstate
            ChangeButtonState(5, (Button)sender);
        }

        private void Upgrade7_Click(object sender, RoutedEventArgs e)
        {
            // Toggle current buttonstate
            ChangeButtonState(6, (Button)sender);
        }

        private void Upgrade8_Click(object sender, RoutedEventArgs e)
        {
            // Toggle current buttonstate
            ChangeButtonState(7, (Button)sender);
        }

        private void Upgrade9_Click(object sender, RoutedEventArgs e)
        {
            // Toggle current buttonstate
            ChangeButtonState(8, (Button)sender);
        }

        private void Upgrade10_Click(object sender, RoutedEventArgs e)
        {
            // Toggle current buttonstate
            ChangeButtonState(9, (Button)sender);
        }

        private void Upgrade11_Click(object sender, RoutedEventArgs e)
        {
            // Toggle current buttonstate
            ChangeButtonState(10, (Button)sender);
        }

        private void Upgrade12_Click(object sender, RoutedEventArgs e)
        {
            // Toggle current buttonstate
            ChangeButtonState(11, (Button)sender);
        }

        // Start game
        private void StartGameMenuStartGameBtn_Click(object sender, RoutedEventArgs e)
        {
            // Check if the name textbox is empty by length
            if (Nametxtbox.Text.Length > 0)
            {
                // Check if the AI is on or off
                if (AIToggle == 0)
                {
                    // AI is off and continuing as a player

                    // Creating a player with the name from the name textbox
                    Player player = new Player(Nametxtbox.Text);

                    // Creating a gameboard and sending the player with it
                    GameBoard gameBoard = new GameBoard(player, activatedUpgrades);

                    // Navigate to the gameboard
                    NavigationService.Navigate(gameBoard);
                }
                else
                {
                    // AI is on and continuing as an AI

                    // PopulationSize is set here.
                    int populationSize = 10;

                    // Calculate output though the following formula;
                    //      (((xBoard.length + border) * yBoard.length) + gameFigure.squares + nextFigure.squares + swapFigure.squares) * outputAmount
                    int inputSize = (((10 + 2) * 20) + 4 + 4 + 4) * 4;

                    // Sets a new AI
                    AI ai = new AI(Nametxtbox.Text, populationSize, inputSize);

                    // Starts a new game and navigates to the gameBoard.
                    GameBoard gameBoard = new GameBoard(ai, activatedUpgrades);
                    NavigationService.Navigate(gameBoard);
                }
            }
            else if (Nametxtbox.Text.Length == 0 && AIToggle == 1)
            {
                // Sets a new game based on the loaded AI. Then navigates to the gameBoard.
                AI ai = generations[StartGameMenuAIDropdown.Items.IndexOf(StartGameMenuAIDropdown.SelectedItem)];
                GameBoard gameBoard = new GameBoard(ai, activatedUpgrades);
                NavigationService.Navigate(gameBoard);
            }
            else
            {
                // Error message in a messagebox due to name not entered
                MessageBox.Show("Name is missing, enter a name!", "Error");
            }
        }

        // Navigation
        private void StartGameMenuMainMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to the Main Menu
            NavigationService.Navigate(new Uri("Pages/MainPage.xaml", UriKind.Relative));
        }

        // UI slider
        private void PlayerOrAISlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // The value of the slider 0:Player - 1:AI
            int value = (int)PlayerOrAISlider.Value;

            // Set the AI toggle to the current value
            AIToggle = value;

            // Check to see if slider is toggled AI
            if (value == 1)
            {
                // Running through all upgrades
                for (int i = 0; i < StartGameMenuUpgradesGrid.Children.Count; i++)
                {
                    // If button is hit disable button
                    if (StartGameMenuUpgradesGrid.Children[i] is Button button)
                    {
                        // Button disabled
                        button.IsEnabled = false;

                        // Check to see if button is lightgreen
                        if(button.Background == Brushes.LightGreen)
                        {
                            // Changing button state
                            ChangeButtonState(i, button);
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < StartGameMenuUpgradesGrid.Children.Count; i++)
                {
                    if (StartGameMenuUpgradesGrid.Children[i] is Button button)
                    {
                        button.IsEnabled = activatedUpgrades.purchasedUpgrades[i];
                    }
                }
            }
        }

        private void AITrainingOnOffSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // Set the enableTraining to the value of the slider value
            settings.enableTraining = ((Slider)sender).Value == 1 ? true : false;

            // Write new settings to the settings file
            File.WriteAllText(exeDir + "/Assets/JSON/Settings.json", JsonSerializer.Serialize(settings, new JsonSerializerOptions() { WriteIndented = true }));
        }
    }
}

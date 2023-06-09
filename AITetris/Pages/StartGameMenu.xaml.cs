using AITetris.Classes;
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
    /// Interaction logic for StartGameMenu.xaml
    /// </summary>
    public partial class StartGameMenu : Page
    {
        // Toggle state of the upgrade buttons
        private bool[] buttonStates = { false, false, false, false, false, false, false, false, false, false, false, false };

        // AI toggle is a variable controlling if the AI is active or not, it has a starting value of 0/off
        private int AIToggle = 0;
        public StartGameMenu()
        {
            InitializeComponent();

            // Set the focus of the page to this textbox
            Nametxtbox.Focus();
        }

        // Change button state of button pressed
        private void ChangeButtonState(int buttonStateIndex)
        {
            // Check the state of the button
            if (buttonStates[buttonStateIndex] == false)
            {
                // Changing state to on
                buttonStates[buttonStateIndex] = true;

                Debug.WriteLine("Upgrade " + buttonStateIndex.ToString() + " toggled on!");
            }
            else
            {
                // Changing state to off
                buttonStates[buttonStateIndex] = false;

                Debug.WriteLine("Upgrade " + buttonStateIndex.ToString() + " toggled off!");
            }
        }

        // UI buttons
        // Toggle the upgrades 1 - 12
        private void Upgrade1_Click(object sender, RoutedEventArgs e)
        {
            // Toggle current buttonstate
            ChangeButtonState(0);
        }

        private void Upgrade2_Click(object sender, RoutedEventArgs e)
        {
            // Toggle current buttonstate
            ChangeButtonState(1);
        }

        private void Upgrade3_Click(object sender, RoutedEventArgs e)
        {
            // Toggle current buttonstate
            ChangeButtonState(2);
        }

        private void Upgrade4_Click(object sender, RoutedEventArgs e)
        {
            // Toggle current buttonstate
            ChangeButtonState(3);
        }

        private void Upgrade5_Click(object sender, RoutedEventArgs e)
        {
            // Toggle current buttonstate
            ChangeButtonState(4);
        }

        private void Upgrade6_Click(object sender, RoutedEventArgs e)
        {
            // Toggle current buttonstate
            ChangeButtonState(5);
        }

        private void Upgrade7_Click(object sender, RoutedEventArgs e)
        {
            // Toggle current buttonstate
            ChangeButtonState(6);
        }

        private void Upgrade8_Click(object sender, RoutedEventArgs e)
        {
            // Toggle current buttonstate
            ChangeButtonState(7);
        }

        private void Upgrade9_Click(object sender, RoutedEventArgs e)
        {
            // Toggle current buttonstate
            ChangeButtonState(8);
        }

        private void Upgrade10_Click(object sender, RoutedEventArgs e)
        {
            // Toggle current buttonstate
            ChangeButtonState(9);
        }

        private void Upgrade11_Click(object sender, RoutedEventArgs e)
        {
            // Toggle current buttonstate
            ChangeButtonState(10);
        }

        private void Upgrade12_Click(object sender, RoutedEventArgs e)
        {
            // Toggle current buttonstate
            ChangeButtonState(11);
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
                    GameBoard gameBoard = new GameBoard(player);

                    // Navigate to the gameboard
                    NavigationService.Navigate(gameBoard);
                }
                else
                {
                    // AI is on and continuing as an AI
                    // Todo! - Sebastian - Dokumentation

                    int populationSize = 10;
                    // (((xBoard.length + border) * yBoard.length) + gameFigure.squares + nextFigure.squares + swapFigure.squares) * outputAmount
                    int inputSize = (((10 + 2) * 20) + 4 + 4 + 4) * 4;
                    AI ai = new AI(Nametxtbox.Text, populationSize, inputSize);
                    GameBoard gameBoard = new GameBoard(ai);
                    NavigationService.Navigate(gameBoard);
                }
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

            // Check sider value
            if (value == 0)
            {
                // Player active
                Debug.WriteLine("Player is active!");
            }
            else
            {
                // AI active
                Debug.WriteLine("AI is active!");
            }

            // Set the AI toggle to the current value
            AIToggle = value;
        }
    }
}

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
        private bool[] buttonStates = { false, false, false, false, false, false, false, false, false, false, false, false };

        private int AIToggle = 0;
        public StartGameMenu()
        {
            InitializeComponent();

            // Set the focus of the page to this textbox
            Nametxtbox.Focus();
        }

        private void Upgrade1_Click(object sender, RoutedEventArgs e)
        {
            if (buttonStates[0] == false)
            {
                buttonStates[0] = true;

                Debug.WriteLine("Upgrade 1 toggled on!");
            }
            else
            {
                buttonStates[0] = false;

                Debug.WriteLine("Upgrade 1 toggled off!");
            }
        }

        private void Upgrade2_Click(object sender, RoutedEventArgs e)
        {
            if (buttonStates[1] == false)
            {
                buttonStates[1] = true;

                Debug.WriteLine("Upgrade 2 toggled on!");
            }
            else
            {
                buttonStates[1] = false;

                Debug.WriteLine("Upgrade 2 toggled off!");
            }
        }

        private void Upgrade3_Click(object sender, RoutedEventArgs e)
        {
            if (buttonStates[2] == false)
            {
                buttonStates[2] = true;

                Debug.WriteLine("Upgrade 3 toggled on!");
            }
            else
            {
                buttonStates[2] = false;

                Debug.WriteLine("Upgrade 3 toggled off!");
            }
        }

        private void Upgrade4_Click(object sender, RoutedEventArgs e)
        {
            if (buttonStates[3] == false)
            {
                buttonStates[3] = true;

                Debug.WriteLine("Upgrade 4 toggled on!");
            }
            else
            {
                buttonStates[3] = false;

                Debug.WriteLine("Upgrade 4 toggled off!");
            }
        }

        private void Upgrade5_Click(object sender, RoutedEventArgs e)
        {
            if (buttonStates[4] == false)
            {
                buttonStates[4] = true;

                Debug.WriteLine("Upgrade 5 toggled on!");
            }
            else
            {
                buttonStates[4] = false;

                Debug.WriteLine("Upgrade 5 toggled off!");
            }
        }

        private void Upgrade6_Click(object sender, RoutedEventArgs e)
        {
            if (buttonStates[5] == false)
            {
                buttonStates[5] = true;

                Debug.WriteLine("Upgrade 6 toggled on!");
            }
            else
            {
                buttonStates[5] = false;

                Debug.WriteLine("Upgrade 6 toggled off!");
            }
        }

        private void Upgrade7_Click(object sender, RoutedEventArgs e)
        {
            if (buttonStates[6] == false)
            {
                buttonStates[6] = true;

                Debug.WriteLine("Upgrade 7 toggled on!");
            }
            else
            {
                buttonStates[6] = false;

                Debug.WriteLine("Upgrade 7 toggled off!");
            }
        }

        private void Upgrade8_Click(object sender, RoutedEventArgs e)
        {
            if (buttonStates[7] == false)
            {
                buttonStates[7] = true;

                Debug.WriteLine("Upgrade 8 toggled on!");
            }
            else
            {
                buttonStates[7] = false;

                Debug.WriteLine("Upgrade 8 toggled off!");
            }
        }

        private void Upgrade9_Click(object sender, RoutedEventArgs e)
        {
            if (buttonStates[8] == false)
            {
                buttonStates[8] = true;

                Debug.WriteLine("Upgrade 9 toggled on!");
            }
            else
            {
                buttonStates[8] = false;

                Debug.WriteLine("Upgrade 9 toggled off!");
            }
        }

        private void Upgrade10_Click(object sender, RoutedEventArgs e)
        {
            if (buttonStates[9] == false)
            {
                buttonStates[9] = true;

                Debug.WriteLine("Upgrade 10 toggled on!");
            }
            else
            {
                buttonStates[9] = false;

                Debug.WriteLine("Upgrade 10 toggled off!");
            }
        }

        private void Upgrade11_Click(object sender, RoutedEventArgs e)
        {
            if (buttonStates[10] == false)
            {
                buttonStates[10] = true;

                Debug.WriteLine("Upgrade 11 toggled on!");
            }
            else
            {
                buttonStates[10] = false;

                Debug.WriteLine("Upgrade 11 toggled off!");
            }
        }

        private void Upgrade12_Click(object sender, RoutedEventArgs e)
        {
            if (buttonStates[11] == false)
            {
                buttonStates[11] = true;

                Debug.WriteLine("Upgrade 12 toggled on!");
            }
            else
            {
                buttonStates[11] = false;

                Debug.WriteLine("Upgrade 12 toggled off!");
            }
        }

        private void PlayerOrAISlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int value = (int)PlayerOrAISlider.Value;

            if (value == 0)
            {
                Debug.WriteLine("Player is active!");
            }
            else
            {
                Debug.WriteLine("AI is active!");
            }

            AIToggle = value;
        }

        private void StartGameMenuStartGameBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Nametxtbox.Text.Length > 0)
            {
                if (AIToggle == 0)
                {
                    Player player = new Player(Nametxtbox.Text);
                    GameBoard gameBoard = new GameBoard(player);
                    NavigationService.Navigate(gameBoard);
                }
                else
                {
                    int populationSize = 10;
                    // (((xBoard.length + border) * yBoard.length) + gameFigure.squares + nextFigure.squares + swapFigure.squares) * outputAmount
                    int inputSize = (((10 + 2) * 20) + 4 + 4 + 4) * 4;
                    AI ai = new AI(Nametxtbox.Text,populationSize,inputSize);
                    GameBoard gameBoard = new GameBoard(ai);
                    NavigationService.Navigate(gameBoard);
                }
            }
            else
            {
                MessageBox.Show("Name is missing, enter a name!", "Error");
            }
        }

        private void StartGameMenuMainMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("Pages/MainPage.xaml", UriKind.Relative));
        }

        //private void PlayAsAichb_Click(object sender, RoutedEventArgs e)
        //{
        //    PickAIcmb.IsEnabled = ((CheckBox)sender).IsChecked ?? false;
        //}

        //private void StartGamebnt_Click(object sender, RoutedEventArgs e)
        //{
        //    if(Nametxtbox.Text.Length > 0)
        //    {
        //        if (!PlayAsAichb.IsChecked ?? false)
        //        {
        //            Player player = new Player(Nametxtbox.Text);
        //            GameBoard gameBoard = new GameBoard(player);
        //            NavigationService.Navigate(gameBoard);
        //        }
        //        else
        //        {
        //            //TODO AI create
        //        }
        //    }
        //    else
        //    {
        //        NoNamelbl.Visibility = Visibility.Visible;
        //    }
        //}

        //private void Cancelbtn_Click(object sender, RoutedEventArgs e)
        //{
        //    NavigationService.GoBack();
        //}
    }
}

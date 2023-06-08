using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
    /// Interaction logic for PointShop.xaml
    /// </summary>
    public partial class PointShop : Page
    {
        // Variable used to store the accumulated metacurrency, this should be loaded in from a JSON 
        private int metaCurrency;

        // Bool array holding the upgrade status, this should be loaded in from a JSON 
        // Not owned = false, owned = true
        private bool[] upgradeStates = { false, false, false, false, false, false, false, false, false, false, false, false };

        public PointShop()
        {
            InitializeComponent();

            // Flip the upgradestates to toggle is enabled on the upgrade buttons
            ChangeButtonEnabledState();

            // Todo - Set this to current meta currency
            // This should be loaded in from a JSON 
            metaCurrency = 1000;

            // Update the UI label for metacurrency
            PointShopShopControlsMetaCurrency.Content = metaCurrency;
        }

        // A function that flips the state of the buttons, to enable those who has to be enabled from the JSON
        private void ChangeButtonEnabledState()
        {
            // Looping through the children of the Shop grid
            for (int i = 0; i < PointShopShopGrid.Children.Count; i++)
            {
                // Check if the child is a stackpanel
                if (PointShopShopGrid.Children[i] is StackPanel stackPanel)
                {
                    // If it wasa aa stackpanel loop through the childrend of the stackpanel
                    foreach (var child in stackPanel.Children)
                    {
                        // If a child is a button switch the state of the button
                        if (child is Button button)
                        {
                            // Switching the state of the button to ensure it is enabled or disabled
                            button.IsEnabled = !upgradeStates[i];
                        }
                    }
                }
            }
        }

        // A function that handles the purchase of the upgrade.
        private void BuyUpgrade(int index, int price, Button button)
        {
            // Check if the price is too high
            if (metaCurrency < price)
            {
                // Error message that tells the user in a messagebox they cant afford the upgrade
                MessageBox.Show("Price is too high, you cant afford it!", "Error");
            }
            else
            {
                // Recalculating the metacurrency by subtracting the price
                metaCurrency -= price;

                // Updating the UI Label that shows the meta currency
                PointShopShopControlsMetaCurrency.Content = metaCurrency;

                // Set the upgrade status to true to indicate an upgrade is bought
                upgradeStates[index] = true;

                // Disable the button and the the content of the button to Purchased
                button.IsEnabled = false;
                button.Content = "Purchased";
            }
        }

        // UI buttons for each upgrade 1 - 12
        private void Upgrade1_buyBtn_Click(object sender, RoutedEventArgs e)
        {
            // Price and index of the upgrade
            int price = 500;
            int index = 0;

            // Try to purchase the upgrade
            BuyUpgrade(index, price, (Button)sender);
        }

        private void Upgrade2_buyBtn_Click(object sender, RoutedEventArgs e)
        {
            // Price and index of the upgrade
            int price = 500;
            int index = 1;

            // Try to purchase the upgrade
            BuyUpgrade(index, price, (Button)sender);
        }

        private void Upgrade3_buyBtn_Click(object sender, RoutedEventArgs e)
        {
            // Price and index of the upgrade
            int price = 500;
            int index = 2;

            // Try to purchase the upgrade
            BuyUpgrade(index, price, (Button)sender);
        }

        private void Upgrade4_buyBtn_Click(object sender, RoutedEventArgs e)
        {
            // Price and index of the upgrade
            int price = 500;
            int index = 3;

            // Try to purchase the upgrade
            BuyUpgrade(index, price, (Button)sender);
        }

        private void Upgrade5_buyBtn_Click(object sender, RoutedEventArgs e)
        {
            // Price and index of the upgrade
            int price = 500;
            int index = 4;

            // Try to purchase the upgrade
            BuyUpgrade(index, price, (Button)sender);
        }

        private void Upgrade6_buyBtn_Click(object sender, RoutedEventArgs e)
        {
            // Price and index of the upgrade
            int price = 500;
            int index = 5;

            // Try to purchase the upgrade
            BuyUpgrade(index, price, (Button)sender);
        }

        private void Upgrade7_buyBtn_Click(object sender, RoutedEventArgs e)
        {
            // Price and index of the upgrade
            int price = 500;
            int index = 6;

            // Try to purchase the upgrade
            BuyUpgrade(index, price, (Button)sender);
        }

        private void Upgrade8_buyBtn_Click(object sender, RoutedEventArgs e)
        {
            // Price and index of the upgrade
            int price = 500;
            int index = 7;

            // Try to purchase the upgrade
            BuyUpgrade(index, price, (Button)sender);
        }

        private void Upgrade9_buyBtn_Click(object sender, RoutedEventArgs e)
        {
            // Price and index of the upgrade
            int price = 500;
            int index = 8;

            // Try to purchase the upgrade
            BuyUpgrade(index, price, (Button)sender);
        }

        private void Upgrade10_buyBtn_Click(object sender, RoutedEventArgs e)
        {
            // Price and index of the upgrade
            int price = 500;
            int index = 9;

            // Try to purchase the upgrade
            BuyUpgrade(index, price, (Button)sender);
        }

        private void Upgrade11_buyBtn_Click(object sender, RoutedEventArgs e)
        {
            // Price and index of the upgrade
            int price = 500;
            int index = 10;

            // Try to purchase the upgrade
            BuyUpgrade(index, price, (Button)sender);
        }

        private void Upgrade12_buyBtn_Click(object sender, RoutedEventArgs e)
        {
            // Price and index of the upgrade
            int price = 500;
            int index = 11;

            // Try to purchase the upgrade
            BuyUpgrade(index, price, (Button)sender);
        }

        // Navigation
        private void PointShopShopControlsBackToMainMenu_Click(object sender, RoutedEventArgs e)
        {
            // Navigate back to the Main Menu
            NavigationService.Navigate(new Uri("Pages/MainPage.xaml", UriKind.Relative));
        }
    }
}

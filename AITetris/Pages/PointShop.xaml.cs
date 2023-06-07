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
        private int metaCurrency;

        private bool[] upgradeStates = { false, false, false, false, false, false, false, false, false, false, false, false };
        public PointShop()
        {
            InitializeComponent();

            // Todo - Set this to current meta currency
            metaCurrency = 1000;
            PointShopShopControlsMetaCurrency.Content = metaCurrency;
        }

        private void BuyUpgrade(int index, int price, Button button)
        {
            if (metaCurrency < price)
            {
                MessageBox.Show("Price is too high, you cant afford it!", "Error");
            }
            else
            {
                metaCurrency -= price;
                PointShopShopControlsMetaCurrency.Content = metaCurrency;

                upgradeStates[index] = true;

                button.IsEnabled = false;
                button.Content = "Purchased";
            }
        }

        private void Upgrade1_buyBtn_Click(object sender, RoutedEventArgs e)
        {
            int price = 500;
            int index = 0;

            BuyUpgrade(index, price, (Button)sender);
        }

        private void Upgrade2_buyBtn_Click(object sender, RoutedEventArgs e)
        {
            int price = 500;
            int index = 1;

            BuyUpgrade(index, price, (Button)sender);
        }

        private void Upgrade3_buyBtn_Click(object sender, RoutedEventArgs e)
        {
            int price = 500;
            int index = 2;

            BuyUpgrade(index, price, (Button)sender);
        }

        private void Upgrade4_buyBtn_Click(object sender, RoutedEventArgs e)
        {
            int price = 500;
            int index = 3;

            BuyUpgrade(index, price, (Button)sender);
        }

        private void Upgrade5_buyBtn_Click(object sender, RoutedEventArgs e)
        {
            int price = 500;
            int index = 4;

            BuyUpgrade(index, price, (Button)sender);
        }

        private void Upgrade6_buyBtn_Click(object sender, RoutedEventArgs e)
        {
            int price = 500;
            int index = 5;

            BuyUpgrade(index, price, (Button)sender);
        }

        private void Upgrade7_buyBtn_Click(object sender, RoutedEventArgs e)
        {
            int price = 500;
            int index = 6;

            BuyUpgrade(index, price, (Button)sender);
        }

        private void Upgrade8_buyBtn_Click(object sender, RoutedEventArgs e)
        {
            int price = 500;
            int index = 7;

            BuyUpgrade(index, price, (Button)sender);
        }

        private void Upgrade9_buyBtn_Click(object sender, RoutedEventArgs e)
        {
            int price = 500;
            int index = 8;

            BuyUpgrade(index, price, (Button)sender);
        }

        private void Upgrade10_buyBtn_Click(object sender, RoutedEventArgs e)
        {
            int price = 500;
            int index = 9;

            BuyUpgrade(index, price, (Button)sender);
        }

        private void Upgrade11_buyBtn_Click(object sender, RoutedEventArgs e)
        {
            int price = 500;
            int index = 10;

            BuyUpgrade(index, price, (Button)sender);
        }

        private void Upgrade12_buyBtn_Click(object sender, RoutedEventArgs e)
        {
            int price = 500;
            int index = 11;

            BuyUpgrade(index, price, (Button)sender);
        }

        private void PointShopShopControlsBackToMainMenu_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("Pages/MainPage.xaml", UriKind.Relative));
        }
    }
}

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
using AITetris.Classes;

namespace AITetris.Pages
{
    /// <summary>
    /// Interaction logic for GameBoard.xaml
    /// </summary>
    public partial class GameBoard : Page
    {
        Character character;
        public GameBoard(Character character)
        {
            InitializeComponent();
            this.character = character;
            GameBoardScorePlayerLbl.Content = character.name;
            AddPoint(1);
        }

        private void AddPoint(int lines)
        {
            GameBoardScorePointLbl.Content = "Point: " + (Convert.ToInt32(((string)GameBoardScorePointLbl.Content).Remove(0,7)) + (Math.Pow(2, lines) * 100)).ToString();
        }

        private void GameBoardActionsPauseBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void GameBoardActionsConsumeOneBtn_Click(object sender, RoutedEventArgs e)
        {
            AddPoint(2);
        }

        private void GameBoardActionsConsumeTwoBtn_Click(object sender, RoutedEventArgs e)
        {
            AddPoint(3);
        }

        private void GameBoardActionsConsumeThreeBtn_Click(object sender, RoutedEventArgs e)
        {
            AddPoint(4);
        }
    }
}

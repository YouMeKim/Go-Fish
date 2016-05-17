using GoFish.Models;
using GoFish.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;

namespace GoFish.Views
{
    /// <summary>
    /// Interaction logic for PlayerTurnWindow.xaml
    /// </summary>
    public partial class PlayerTurnWindow : Window
    {
        public GameBoardViewModel GameBoardVM;
        public PlayerTurnWindow()
        {
            InitializeComponent();
        }
        public PlayerTurnWindow(GameBoardViewModel gameBoardVM)
        {
            GameBoardVM = gameBoardVM;
            Console.WriteLine("Initializing PlayerTurnWindow");
            InitializeComponent();
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            e.Cancel = true;
        }
        public void Reload() {
            // remove all elements from card and player choices
            CardChoice.Items.Clear();
            PlayerChoice.Items.Clear();

            // add all players besides current player into player choice combobox
            for (int i = 1; i < GameBoardVM.Players.Count; i++)
            {
                Player player = GameBoardVM.Players[i];
                PlayerChoice.Items.Add(player.Name);
            }

            // add all current players hand cards into card choice combobox
            foreach (Card card in GameBoardVM.Player.Hand)
            {
                CardChoice.Items.Add(card.CardValueStr);
            }
        }

        private void PlayerAskButton_Click(object sender, RoutedEventArgs e)
        {
            if (CardChoice.SelectedIndex >= 0 && 
                PlayerChoice.SelectedIndex >= 0) {
                GameBoardVM.PlayerChoiceCard = CardChoice.SelectedIndex;
                GameBoardVM.PlayerChoicePlayer = PlayerChoice.SelectedIndex + 1;
                this.Hide();
            }
        }
    }
}

using GoFish.Model.Enum;
using GoFish.Models;
using GoFish.ViewModels;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        public String PlayerName;
        public GameBoardViewModel GameBoardVM;
        public GameOverWindow GameOverWin;

        const int CARD_WIDTH = 57;
        const int CARD_HEIGHT = 80;
        const int CARD_WIDTH_PLAYER = 71;
        const int CARD_HEIGHT_PLAYER = 100;
        public GameWindow()
        {
            InitializeComponent();
        }
        public GameWindow(String playerName)
        {
            PlayerName = playerName;
            InitializeComponent();
            PlayerNameBlock.Text = PlayerName + "'s Hand";
            PlayerPairsNameBlock.Text = PlayerName + "'s Completed Sets";
            CreateNewGame();
        }
        private void CreateNewGame()
        {
            GameBoardVM = new GameBoardViewModel(this, PlayerName);
            UpdateAllFields();
        }
        public void UpdateAllFields()
        {
            UpdateField(GameBoardVM.Player, PlayerHandField, PlayerSetsField, PlayerScoreBlock);
            UpdateField(GameBoardVM.Player1, Player1HandField, Player1SetsField, Player1ScoreBlock);
            UpdateField(GameBoardVM.Player2, Player2HandField, Player2SetsField, Player2ScoreBlock);
        }
        private void UpdateField(Player player, StackPanel handField, StackPanel setsField, TextBlock scoreBlock)
        {
            ClearField(handField);
            ClearField(setsField);

            foreach (Card card in player.Hand) {
                Image CardImage = new Image();
                if (player.Name == "Player 1" || 
                    player.Name == "Player 2") {
                    CardImage.Height = CARD_HEIGHT;
                    CardImage.Width = CARD_WIDTH;
                    CardImage.Source = new BitmapImage(new Uri("/Assets/CardBack.png", UriKind.Relative));
                    // CardImage.Source = new BitmapImage(new Uri(card.ImageLocation)); // for testing purposes, we will show opponent hands
                } else {
                    // only the player's cards should be visible and larger
                    CardImage.Height = CARD_HEIGHT_PLAYER;
                    CardImage.Width = CARD_WIDTH_PLAYER;
                    CardImage.Source = new BitmapImage(new Uri(card.ImageLocation));
                }
                handField.Children.Add(CardImage);
            }

            foreach (Card card in player.Sets)
            {
                if (card.CardSuit == Suit.Spades)
                {
                    Image CardImage = new Image();
                    if (player.Name == "Player 1" ||
                        player.Name == "Player 2")
                    {
                        CardImage.Height = CARD_HEIGHT;
                        CardImage.Width = CARD_WIDTH;
                    } else {
                        CardImage.Height = CARD_HEIGHT_PLAYER;
                        CardImage.Width = CARD_WIDTH_PLAYER;
                    }
                    CardImage.Source = new BitmapImage(new Uri(card.ImageLocation));
                    setsField.Children.Add(CardImage);
                }
            }

            player.Score = player.Sets.Count;
            scoreBlock.Text = "Score: " + player.Score / 4;
        }
        private void ClearField(StackPanel field)
        {
            field.Children.RemoveRange(0, field.Children.Count);
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            int player = GameBoardVM.CurrentPlayer;
            InstructionsBlock.Text = GameBoardVM.NextStep();

            UpdateAllFields();

            if (GameBoardVM.CheckEnd())
            {
                NextButton.IsEnabled = false;

                string winner = "";
                int score = 0;
                bool won = false;

                foreach (Player p in GameBoardVM.Players)
                {
                    if (p.Score > score) {
                        winner = p.Name;
                        score = p.Score;
                    }
                }

                if (winner == PlayerName) {
                    won = true;
                }

                GameOverWin = new GameOverWindow(winner, score, won);
            }
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            Application.Current.Shutdown();
        }
    }
}

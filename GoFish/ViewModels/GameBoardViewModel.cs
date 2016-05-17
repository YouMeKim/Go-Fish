using GoFish.Models;
using GoFish.Services;
using GoFish.Services.Requests;
using GoFish.Services.Responses;
using GoFish.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GoFish.ViewModels
{
    public class GameBoardViewModel
    {
        #region Game State Properties
        public string PlayerName;
        public int CurrentPlayer;
        public List<Player> Players { get; set; }
        public Player Player;
        public Player Player1;
        public Player Player2;
        public string DeckId;

        public int PlayerChoiceCard;
        public int PlayerChoicePlayer;
        #endregion
        DataService dataService;
        GameWindow GameWin;
        PlayerTurnWindow PlayerTurnWin;
        Random random;
        int numberOfCardsRemaining;
        public GameBoardViewModel(GameWindow gameWin, string playerName)
        {
            GameWin = gameWin;
            PlayerTurnWin = new PlayerTurnWindow(this);
            random = new Random();
            PlayerName = playerName;
            dataService = new DataService();
            Players = new List<Player>();
            NewGame();
        }

        private void NewGame()
        {
            CurrentPlayer = -1;
            DeckId = dataService.NewDecks(1);
            Player = new Player(PlayerName);
            Players.Add(Player);
            Player1 = new Player("Player 1");
            Players.Add(Player1);
            Player2 = new Player("Player 2");
            Players.Add(Player2);
            DealInitialHands();
        }

        public string NextStep()
        {
            // When 
            if (CurrentPlayer == Players.Count - 1) {
                CurrentPlayer = 0;
            } else {
                CurrentPlayer++;
            }

            if (CurrentPlayer > 0) {
                // AI is playing
                return PlayAI(Players[CurrentPlayer]);
            } else {
                // Player is playing
                return PlayerTurn();
            }
        }

        private void DealInitialHands()
        {
            Player.Hand = Draw(5);
            Player1.Hand = Draw(5);
            Player2.Hand = Draw(5);
        }

        private List<Card> Draw(int numberOfCards)
        {
            DrawRequest drawReq = new DrawRequest();
            drawReq.DeckID = DeckId;
            drawReq.NumberOfCards = numberOfCards;
            DrawResponse drawResponse = dataService.Draw(drawReq);
            numberOfCardsRemaining = drawResponse.RemainingCards;
            return drawResponse.Cards;
        }

        private string PlayerTurn()
        {
            Player player = Player;
            string updates = Player.Name + "'s turn\r\n";
            bool hasCard = true;
            bool first = true;

            if (player.Hand.Count > 0) {
                while (hasCard)
                {
                    if (!first)
                    {
                        updates += " | ";
                    }

                    PlayerTurnWin.Reload();
                    PlayerTurnWin.ShowDialog();

                    Card card = player.Hand[PlayerChoiceCard];

                    hasCard = CheckPlayerHand(player, Players[PlayerChoicePlayer], card.CardValueStr);

                    GameWin.UpdateAllFields();

                    updates += Players[PlayerChoicePlayer].Name + ", do you have a " + card.CardValueStr + " = " + hasCard;
                    first = false;
                }
            }
            else
            {
                updates += "Skipping " + player.Name + "'s turn.";
            }

            updates += CheckSets(player);

            if (player.Hand.Count <= 0)
            {
                if (numberOfCardsRemaining >= 5) {
                    updates += "\r\n" + player.Name + " ran out of cards and drew 5 cards.";
                    player.Hand.AddRange(Draw(5));
                } else if (numberOfCardsRemaining > 0) {
                    updates += "\r\n" + player.Name + " ran out of cards and drew " + numberOfCardsRemaining + " cards, which was the rest of the deck.";
                    player.Hand.AddRange(Draw(numberOfCardsRemaining));
                } else {
                    updates += "\r\n" + player.Name + " cannot draw 5 cards because there are no cards left.";
                }
            }
            else
            {
                if (numberOfCardsRemaining > 0)
                {
                    updates += "\r\nGo Fish! " + player.Name + " drew a card.";
                    player.Hand.Add(Draw(1)[0]);
                } else {
                    updates += "\r\nGo Fish! " + player.Name + " cannot draw a card because there are no cards left";
                }
            }

            updates += "\r\nThere are " + numberOfCardsRemaining + " cards left in the deck.";

            return updates;
        }

        private string PlayAI(Player player) {
            string updates = player.Name + "'s turn\r\n";
            bool hasCard = true;
            bool first = true;

            if (player.Hand.Count > 0) {
                while (hasCard)
                {
                    if (!first) {
                        updates += " | ";
                    }
                    int cardIndex = random.Next(0, player.Hand.Count);
                    int playerIndex = random.Next(0, 2);
                    if (player.Name == "Player 1" && playerIndex == 1)
                    {
                        playerIndex = 2;
                    }
                    Card card = player.Hand[cardIndex];

                    hasCard = CheckPlayerHand(player, Players[playerIndex], card.CardValueStr);

                    updates += Players[playerIndex].Name + ", do you have a " + card.CardValueStr + " = " + hasCard;
                    first = false;
                }
            }
            else
            {
                updates += "Skipping " + player.Name + "'s turn.";
            }

            updates += CheckSets(player);

            if (player.Hand.Count <= 0) {
                if (numberOfCardsRemaining >= 5)
                {
                    updates += "\r\n" + player.Name + " ran out of cards and drew 5 cards.";
                    player.Hand.AddRange(Draw(5));
                }
                else if (numberOfCardsRemaining > 0)
                {
                    updates += "\r\n" + player.Name + " ran out of cards and drew " + numberOfCardsRemaining + " cards, which was the rest of the deck.";
                    player.Hand.AddRange(Draw(numberOfCardsRemaining));
                }
                else
                {
                    updates += "\r\n" + player.Name + " cannot draw 5 cards because there are no cards left.";
                }
            } else {
                if (numberOfCardsRemaining > 0)
                {
                    updates += "\r\nGo Fish! " + player.Name + " drew a card.";
                    player.Hand.Add(Draw(1)[0]);
                }
                else
                {
                    updates += "\r\nGo Fish! " + player.Name + " cannot draw a card because there are no cards left.";
                }
            }

            updates += "\r\nThere are " + numberOfCardsRemaining + " cards left in the deck.";

            return updates;
        }

        private bool CheckPlayerHand(Player asker, Player to, string value)
        {
            foreach (Card card in to.Hand)
            {
                if (card.CardValueStr == value) {
                    TransferCard(to, asker, card);
                    return true;
                }
            }

            return false;
        }

        private void TransferCard(Player from, Player to, Card card)
        {
            string cardVal = card.CardValueStr;
            for (int i = from.Hand.Count - 1; i >= 0; i-- )
            {
                if (from.Hand[i].CardValueStr == cardVal) {
                    Card theCard = from.Hand[i];
                    from.Hand.Remove(theCard);
                    to.Hand.Add(theCard);
                }
            }
        }

        private string CheckSets(Player player)
        {
            string updates = "";
            List<String> values = new List<String>();
            List<String> sets = new List<String>();

            foreach (Card card in player.Hand)
            {
                if (!values.Contains(card.CardValueStr)) {
                    values.Add(card.CardValueStr);
                }
            }

            foreach (String value in values) {
                int count = 0;
                foreach (Card card in player.Hand) {
                    if (card.CardValueStr == value) {
                        count++;
                    }
                }
                if (count >= 4) {
                    sets.Add(value);
                    updates += "\r\n" + player.Name + " has a set of " + value +"'s";
                }
            }

            for (int i = player.Hand.Count - 1; i >= 0; i--)
            {
                Card card = player.Hand[i];
                if (sets.Contains(card.CardValueStr))
                {
                    player.Hand.Remove(card);
                    player.Sets.Add(card);
                }
            }       

            return updates;
        }

        public bool CheckEnd()
        {
            if (numberOfCardsRemaining <= 0 && 
                Player.Hand.Count <= 0 &&
                Player1.Hand.Count <= 0 && 
                Player2.Hand.Count <= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

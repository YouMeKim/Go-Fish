using GoFish.Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GoFish.Models
{
    [DataContract]
    public class Card
    {
        private int cardValue;
        public int CardValue { get; set; }
        public Suit CardSuit
        {
            get
            {
                Suit suit;
                switch (SuitValueStr)
                {
                    case "CLUBS":
                        suit = Suit.Clubs;
                        break;
                    case "DIAMONDS":
                        suit = Suit.Diamonds;
                        break;
                    case "HEARTS":
                        suit = Suit.Hearts;
                        break;
                    case "SPADES":
                        suit = Suit.Spades;
                        break;
                    default:
                        suit = Suit.Spades;
                        break;
                }
                return suit;
            }
            set { }
        }
        [DataMember(Name = "image")]
        public string ImageLocation { get; set; }
        [DataMember(Name = "value")]
        public string CardValueStr { get; set; }
        [DataMember(Name = "suit")]
        public string SuitValueStr { get; set; }
        public string ToString()
        {
            return CardValueStr + " of " + CardSuit;
        }
    }
}

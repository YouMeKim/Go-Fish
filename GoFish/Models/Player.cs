using GoFish.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GoFish.Models
{
    public class Player
    {
        public string Name { get; set; }
        public int Score { get; set; }
        public List<Card> Hand { get; set; }
        public List<Card> Sets { get; set; }
        public Player(string name)
        {
            Name = name;
            Score = 0;
            Hand = new List<Card>();
            Sets = new List<Card>();
        }
    }
}

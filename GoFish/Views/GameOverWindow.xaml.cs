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
    /// Interaction logic for GameOverWindow.xaml
    /// </summary>
    public partial class GameOverWindow : Window
    {
        string Winner;
        int Score;
        bool Won;
        public GameOverWindow()
        {
            InitializeComponent();
        }
        public GameOverWindow(string winner, int score, bool won)
        {
            InitializeComponent();
            Winner = winner;
            Score = score;
            Won = won;

            if (won) {
                MessageBlock.Text = "Congratulations!";
            } else {
                MessageBlock.Text = "Sorry!";
            }

            SubMessageBlock.Text = Winner + " won with " + Score + " points!";
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            Application.Current.Shutdown();
        }
    }
}

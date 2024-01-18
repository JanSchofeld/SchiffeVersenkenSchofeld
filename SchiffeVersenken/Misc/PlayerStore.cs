using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchiffeVersenken
{
    public class PlayerStore
    {
        private Player playerOne = new Player("");

        public Player PlayerOne
        {
            get { return playerOne; }
            set
            {
                playerOne = value;
            }
        }

        private Player playerTwo = new Player("");

        public Player PlayerTwo
        {
            get { return playerTwo; }
            set
            {
                playerTwo = value;
            }
        }

        public bool isSecondPlayer = false;
    }
}

using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.ObjectModel;

namespace SchiffeVersenken
{
    public partial class Player : ObservableObject
    {
        public GameBoard gameBoard {  get; set; }

        public ObservableCollection<Ship> ships { get; set; }

        public string Name {  get; set; }

        [ObservableProperty]
        public bool isPlayersTurn = false;

        public Player(string name)
        {
            Name = name;

            ships = new ObservableCollection<Ship>()
            {
                new UBoot(),
                new UBoot(),
                new Zerstörer(),
                new Zerstörer(),
                new Kreuzer(),
                new Schlachtschiff(),
                new Flugzeugträger()
            };

            gameBoard = new GameBoard();
        }
    }
}

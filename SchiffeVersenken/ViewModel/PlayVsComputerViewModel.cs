using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SchiffeVersenken
{
    public partial class PlayVsComputerViewModel : ObservableObject, INotifyPropertyChanged
    {
        private NavigationStore navigation;
        private IServiceProvider services;
        private PlayerStore playerStore;

        public event PropertyChangedEventHandler? PropertyChanged;

        [ObservableProperty]
        private Player player;

        [ObservableProperty]
        private GameBoard playerBoard;

        [ObservableProperty]
        private GameBoard computerBoard;


        private bool isGameOver = false;
        public bool IsGameOver
        {
            get { return isGameOver; }
            set
            {
                isGameOver = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsGameOver)));
            }
        }

        private string gameMessage;
        public string GameMessage
        {
            get { return gameMessage; }
            set
            {
                gameMessage = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(GameMessage)));
            }
        }

        private bool isPlayersTurn;
        public bool IsPlayersTurn
        {
            get { return isPlayersTurn; }
            set
            {
                isPlayersTurn = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsPlayersTurn)));
            }
        }

        private Random rnd;

        public PlayVsComputerViewModel(IServiceProvider services)
        {
            navigation = services.GetRequiredService<NavigationStore>();
            playerStore = services.GetRequiredService<PlayerStore>();
            this.services = services;

            rnd = new Random();

            Player = playerStore.PlayerOne;

            ComputerBoard = new GameBoard();
            PlayerBoard = Player.gameBoard;
            FillComputerGameBoardRandomly();
            IsPlayersTurn = true;
        }


        [RelayCommand]
        private async void PlayerAttackComputer(Coordinates coordinates)
        {
            int attackIndex = coordinates.Row * 10 + coordinates.Column;

            switch (ComputerBoard.GameBoardPanels[attackIndex].PanelType)
            {
                case PanelType.Hit:
                case PanelType.Miss:
                    GameMessage = "Koordinaten wurden bereits angegriffen!";
                    return;

                case PanelType.Wasser:
                    GameMessage = ($"{Player.Name} hat verfehlt!");
                    ComputerBoard.GameBoardPanels[attackIndex].PanelType = PanelType.Miss;
                    break;

                default:
                    GameMessage = ($"{Player.Name} hat {ComputerBoard.GameBoardPanels[attackIndex].PanelType} getroffen!");
                    ComputerBoard.GameBoardPanels[attackIndex].PanelType = PanelType.Hit;
                    break;
            }


            if (PlayerHasWon())
            {
                GameMessage = ($"{Player.Name} hat gewonnen!");
            }
            else
            {
                IsPlayersTurn = false;
                await ComputerTurn();
                if (!IsGameOver)
                {
                    IsPlayersTurn = true;
                }
            }
        }

        private async Task ComputerTurn()
        {
            int attackCoordinates = rnd.Next(0, 99);

            while (PlayerBoard.GameBoardPanels[attackCoordinates].PanelType == PanelType.Hit || PlayerBoard.GameBoardPanels[attackCoordinates].PanelType == PanelType.Miss)
            {
                attackCoordinates = rnd.Next(0, 99);
            }

            await Task.Delay(1200);


            if (PlayerBoard.GameBoardPanels[attackCoordinates].PanelType == PanelType.Wasser)
            {
                PlayerBoard.GameBoardPanels[attackCoordinates].PanelType = PanelType.Miss;
                GameMessage = "Computer hat verfehlt!";
            }
            else
            {
                GameMessage = ($"Computer hat {PlayerBoard.GameBoardPanels[attackCoordinates].PanelType} getroffen!");
                PlayerBoard.GameBoardPanels[attackCoordinates].PanelType = PanelType.Hit;
            }

            if(ComputerHasWon())
            {
                GameMessage = ("Computer hat gewonnen!");
                //IsGameOver = true;
            }
        }


        private bool PlayerHasWon()
        {
            foreach(var gameBoardPanel in ComputerBoard.GameBoardPanels)
            {
                if (gameBoardPanel.isUnsunkShip())
                {
                    return false;
                }
            }
            IsGameOver = true;
            return true;
        }

        private bool ComputerHasWon()
        {
            foreach (var gameBoardPanel in PlayerBoard.GameBoardPanels)
            {
                if (gameBoardPanel.isUnsunkShip())
                {
                    return false;
                }
            }
            IsGameOver = true;
            return true;
        }

        private void FillComputerGameBoardRandomly()
        {
            var computerShips = new ObservableCollection<Ship>()
            {
                new UBoot(),
                new UBoot(),
                new Zerstörer(),
                new Zerstörer(),
                new Kreuzer(),
                new Schlachtschiff(),
                new Flugzeugträger()
            };

            bool shipSet = false;
            int tryCoordinates;
            bool isHorizontal;
            int shipWidth;

            foreach (Ship ship in computerShips)
            {
                shipWidth = ship.width;

                isHorizontal = rnd.Next(1, 10) > 5;


                while (!shipSet)
                {
                    tryCoordinates = rnd.Next(0, 99);

                    if(isHorizontal)
                    {
                        if (TrySetShipHorizontal(tryCoordinates, shipWidth))
                        {
                            for (int i = 0; i < shipWidth; i++)
                            {
                                ComputerBoard.GameBoardPanels[tryCoordinates + i].PanelType = ship.panelType;
                            }
                            shipSet = true;
                        }
                    }
                    else
                    {
                        if (TrySetShipVertical(tryCoordinates, shipWidth))
                        {
                            for (int i = 0; i < shipWidth; i++)
                            {
                                ComputerBoard.GameBoardPanels[tryCoordinates + (i * 10)].PanelType = ship.panelType;
                            }
                            shipSet = true;
                        }
                    }
                }
                shipSet = false;
            }
        }

        private bool TrySetShipHorizontal (int coordinates, int shipWidth)
        {
            if(coordinates % 10 + shipWidth > 10)
            {
                return false;
            }

            for (int i = 0; i < shipWidth; i++)
            {
                if (ComputerBoard.GameBoardPanels[coordinates + i].PanelType != PanelType.Wasser)
                {
                    return false;
                }
            }
            return true;
        }

        private bool TrySetShipVertical(int coordinates, int shipHeight)
        {
            if (coordinates / 10 + shipHeight > 10)
            {
                return false;
            }

            for (int i = 0; i < shipHeight; i++)
            {
                if (ComputerBoard.GameBoardPanels[coordinates + (i * 10)].PanelType != PanelType.Wasser)
                {
                    return false;
                }
            }
            return true;
        }

        [RelayCommand]
        private void PlayAgain()
        {
            playerStore.PlayerOne = new Player(Player.Name);

            navigation.NavigateTo(services.GetRequiredService<SetShipsViewModel>());
        }

        [RelayCommand]
        private void BackToMenu()
        {
            playerStore.PlayerOne = new Player("");

            navigation.NavigateTo(services.GetRequiredService<MenuViewModel>());
        }
    }
}

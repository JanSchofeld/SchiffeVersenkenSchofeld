using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SchiffeVersenken
{
    public partial class SetShipsViewModel : ObservableObject
    {
        private NavigationStore navigation;
        private IServiceProvider services;
        private PlayerStore playerStore;

        [ObservableProperty]
        public Player player;

        [ObservableProperty]
        public Ship currentShip;

        [ObservableProperty]
        public GameBoard gameBoard;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ConfirmCommand))]
        private bool hasSetAllShips = false;


        private int currentIndex = 0;

        private bool isPlayingAgainstComputer;



        public SetShipsViewModel(IServiceProvider services)
        {
            this.services = services;
            playerStore = services.GetRequiredService<PlayerStore>();
            navigation = services.GetRequiredService<NavigationStore>();


            if(!playerStore.isSecondPlayer)
            {
                Player = playerStore.PlayerOne;
            }
            else
            {
                Player = playerStore.PlayerTwo;
            }

            var chooseOpponent = services.GetRequiredService<ChooseOpponentViewModel>();
            this.isPlayingAgainstComputer = chooseOpponent.isPlayingAgainstComputer;

            GameBoard = Player.gameBoard;

            CurrentShip = Player.ships[0];
        }


        [RelayCommand]
        private void SetShip(Coordinates coordinates)
        {
            if (currentIndex < Player.ships.Count)
            {
                if (CurrentShip.isHorizontal)
                {
                    SetShipHorizontal(coordinates);
                }
                else
                {
                    SetShipVertical(coordinates);
                }

                if(currentIndex < Player.ships.Count)
                {
                    CurrentShip = Player.ships[currentIndex];
                }

                if(currentIndex > Player.ships.Count - 1)
                {
                    HasSetAllShips = true;
                }
                
            }
            else
            {
                MessageBox.Show("Alle Schiffe gesetzt!");
            }
        }

        private void SetShipHorizontal(Coordinates coordinates)
        {
            int shipWidth = CurrentShip.width;

            if (coordinates.Column + shipWidth > 10)
            {
                MessageBox.Show("Schiff zu breit!");
                return;
            }

            for (int i = 0; i < shipWidth; i++)
            {
                if (GameBoard.GameBoardPanels[coordinates.Row * 10 + coordinates.Column + i].PanelType != PanelType.Wasser)
                {
                    MessageBox.Show("Schiff kreuzt anderes Schiff!");
                    return;
                }
            }

            for (int i = 0; i < shipWidth; i++)
            {
                GameBoard.GameBoardPanels[coordinates.Row * 10 + coordinates.Column + i].PanelType = CurrentShip.panelType;
                CurrentShip.IsSet = true;
            }

            currentIndex++;
        }

        private void SetShipVertical(Coordinates coordinates)
        {
            int shipHeight = CurrentShip.width;

            if (coordinates.Row + shipHeight > 10)
            {
                MessageBox.Show("Schiff zu lang!");
                return;
            }

            for (int i = 0; i < shipHeight; i++)
            {
                if (GameBoard.GameBoardPanels[(coordinates.Row + i) * 10 + coordinates.Column].PanelType != PanelType.Wasser)
                {
                    MessageBox.Show("Schiff kreuzt anderes Schiff!");
                    return;
                }
            }

            for (int i = 0; i < shipHeight; i++)
            {
                GameBoard.GameBoardPanels[(coordinates.Row + i) * 10 + coordinates.Column].PanelType = CurrentShip.panelType;
                CurrentShip.IsSet = true;
            }

            currentIndex++;
        }

        [RelayCommand]
        private void TurnShip()
        {
            CurrentShip.isHorizontal = !CurrentShip.isHorizontal;
        }

        [RelayCommand(CanExecute = nameof(HasSetAllShips))]
        private void Confirm()
        {
            if (isPlayingAgainstComputer)
            {
                playerStore.PlayerOne = Player;

                navigation.NavigateTo(services.GetRequiredService<PlayVsComputerViewModel>());
            }
            else if (!playerStore.isSecondPlayer)
            {
                playerStore.PlayerOne = Player;

                playerStore.isSecondPlayer = true;


                if (playerStore.PlayerTwo.Name != "")
                {
                    navigation.NavigateTo(services.GetRequiredService<SetShipsViewModel>());
                }
                else
                { 
                    navigation.NavigateTo(services.GetRequiredService<ChooseNameViewModel>());
                }
            }
            else
            {
                playerStore.PlayerTwo = Player;
                playerStore.isSecondPlayer = false;
                navigation.NavigateTo(services.GetRequiredService<PlayerVsPlayerViewModel>());
            }
        }
    }
}

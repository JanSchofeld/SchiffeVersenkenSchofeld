using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SchiffeVersenken
{
    public partial class PlayerVsPlayerViewModel : ObservableObject
    {
        private NavigationStore navigation;
        private IServiceProvider services;
        private PlayerStore playerStore;

        [ObservableProperty]
        private Player playerOne;

        [ObservableProperty]
        private Player playerTwo;

        [ObservableProperty]
        private GameBoard playerOneBoard;

        [ObservableProperty]
        private GameBoard playerTwoBoard;

        [ObservableProperty]
        private bool isPlayerOneTurn = false;

        [ObservableProperty]
        private bool isPlayerTwoTurn = false;

        [ObservableProperty]
        private string gameMessage = "";

        [ObservableProperty]
        private bool isGameOver = false;


        public PlayerVsPlayerViewModel(IServiceProvider services)
        {
            navigation = services.GetRequiredService<NavigationStore>();
            playerStore = services.GetRequiredService<PlayerStore>();
            this.services = services;


            PlayerOne = playerStore.PlayerOne;
            PlayerTwo = playerStore.PlayerTwo;

            PlayerOneBoard = PlayerOne.gameBoard;
            PlayerTwoBoard = PlayerTwo.gameBoard;

            GameMessage = ($"{PlayerOne.Name} fängt an!");

            isPlayerOneTurn = true;
        }
        
        [RelayCommand]
        private void PlayerOneGameBoardButton(Coordinates coordinates)
        {
            int attackIndex = coordinates.Row * 10 + coordinates.Column;

            if(TryAttackCoordinates(attackIndex, PlayerOneBoard))
            {
                Attack(PlayerTwo, attackIndex, PlayerOneBoard);
            }
            else
            {
                return;
            }

            if (HasPlayerLost(PlayerOne))
            {
                IsGameOver = true;
                IsPlayerOneTurn = false;
                IsPlayerTwoTurn = false;
                GameMessage = ($"{PlayerTwo.Name} hat gewonnen!");
                return;
            }

            IsPlayerTwoTurn = false;
            IsPlayerOneTurn = true;
        }

        [RelayCommand]
        private void PlayerTwoGameBoardButton(Coordinates coordinates)
        {
            int attackIndex = coordinates.Row * 10 + coordinates.Column;

            if(TryAttackCoordinates(attackIndex, PlayerTwoBoard))
            {
                Attack(PlayerOne, attackIndex, PlayerTwoBoard);
            }
            else
            {
                return;
            }

            if (HasPlayerLost(PlayerTwo))
            {
                IsGameOver = true;
                IsPlayerOneTurn = false;
                IsPlayerTwoTurn = false;
                GameMessage = ($"{PlayerOne.Name} hat gewonnen!");
                return;
            }

            IsPlayerOneTurn = false;
            IsPlayerTwoTurn = true;
        }

        private bool TryAttackCoordinates(int attackIndex, GameBoard gameBoard)
        {
            if (gameBoard.GameBoardPanels[attackIndex].PanelType == PanelType.Hit || gameBoard.GameBoardPanels[attackIndex].PanelType == PanelType.Miss)
            {
                GameMessage = ("Koordinaten bereits angegriffen!");
                return false;
            }
            return true;
        }

        private void Attack(Player attacker, int attackIndex, GameBoard gameBoard)
        {
            if (gameBoard.GameBoardPanels[attackIndex].PanelType == PanelType.Wasser)
            {
                GameMessage = ($"{attacker.Name} hat verfehlt!");
                gameBoard.GameBoardPanels[attackIndex].PanelType = PanelType.Miss;
            }
            else
            {
                GameMessage = ($"{attacker.Name} hat {gameBoard.GameBoardPanels[attackIndex].PanelType} getroffen!");
                gameBoard.GameBoardPanels[attackIndex].PanelType = PanelType.Hit;
            }
        }

        private bool HasPlayerLost(Player player)
        {
            foreach(var panel in player.gameBoard.GameBoardPanels)
            {
                if(panel.isUnsunkShip())
                {
                    return false;
                }
            }
            return true;
        }

        [RelayCommand]
        private void PlayAgain()
        {
            playerStore.PlayerOne = new Player(PlayerOne.Name);
            playerStore.PlayerTwo = new Player(PlayerTwo.Name);

            navigation.NavigateTo(services.GetRequiredService<SetShipsViewModel>());
        }

        [RelayCommand]
        private void BackToMenu()
        {
            playerStore.PlayerOne = new Player("");
            playerStore.PlayerTwo = new Player("");

            navigation.NavigateTo(services.GetRequiredService<MenuViewModel>());
        }
    }
}

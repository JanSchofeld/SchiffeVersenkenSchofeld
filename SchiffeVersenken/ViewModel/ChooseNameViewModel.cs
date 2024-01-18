using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SchiffeVersenken
{
    public partial class ChooseNameViewModel : ObservableValidator
    {
        private IServiceProvider services;
        private NavigationStore navigation;
        private PlayerStore playerStore;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [MinLength(2), MaxLength(15)]
        [NotifyCanExecuteChangedFor(nameof(SetNameCommand))]
        private string name = "Player";


        public ChooseNameViewModel(IServiceProvider services)
        {
            navigation = services.GetRequiredService<NavigationStore>();
            playerStore = services.GetRequiredService<PlayerStore>();
            this.services = services;
        }

        public bool NameIsPossible { get { return !HasErrors; } }

        [RelayCommand(CanExecute = (nameof(NameIsPossible)))]
        private void SetName()
        {
            if (!playerStore.isSecondPlayer)
            {
                playerStore.PlayerOne.Name = Name;
                navigation.NavigateTo(services.GetRequiredService<SetShipsViewModel>());
            }
            else if (Name != playerStore.PlayerOne.Name)
            {
                playerStore.PlayerTwo.Name = Name;
                navigation.NavigateTo(services.GetRequiredService<SetShipsViewModel>());
            }
            else
            {
                MessageBox.Show("Anderen Namen eingeben!");
                return;
            }

        }
    }
}

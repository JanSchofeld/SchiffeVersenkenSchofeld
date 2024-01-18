using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System;


namespace SchiffeVersenken
{
    public partial class ChooseOpponentViewModel : ObservableObject
    {
        private IServiceProvider services;
        private NavigationStore navigation;
        private PlayerStore playerStore;

        public bool isPlayingAgainstComputer;

        public ChooseOpponentViewModel(IServiceProvider services)
        {
            navigation = services.GetRequiredService<NavigationStore>();
            playerStore = services.GetRequiredService<PlayerStore>();
            this.services = services;
        }


        [RelayCommand]
        private void VsComputer() 
        {
            isPlayingAgainstComputer = true;
            navigation.NavigateTo(services.GetRequiredService<ChooseNameViewModel>());
        }

        [RelayCommand]
        private void VsPlayer() 
        {
            isPlayingAgainstComputer = false;
            navigation.NavigateTo(services.GetRequiredService<ChooseNameViewModel>());
        }

        [RelayCommand]
        private void BackToMenu()
        {
            navigation.NavigateTo(services.GetRequiredService<MenuViewModel>());
        }
    }
}
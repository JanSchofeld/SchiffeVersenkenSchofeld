using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace SchiffeVersenken
{
    public partial class MenuViewModel : ObservableObject
    {
        private IServiceProvider services;
        private NavigationStore navigation;


        public MenuViewModel(IServiceProvider services)
        {
            navigation = services.GetRequiredService<NavigationStore>();
            this.services = services;
        }

        [RelayCommand]
        private void Play()
        {
            navigation.NavigateTo(services.GetRequiredService<ChooseOpponentViewModel>());
        }

        [RelayCommand]
        private void Quit()
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}
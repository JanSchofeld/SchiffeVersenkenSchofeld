using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace SchiffeVersenken
{
    public partial class MainViewModel : ObservableObject
    {
        private IServiceProvider services;
        private NavigationStore navigation;

        [ObservableProperty]
        private ObservableObject currentViewModel;

        public MainViewModel(IServiceProvider services)
        {
            this.services = services;
            navigation = services.GetRequiredService<NavigationStore>();

            CurrentViewModel = navigation.CurrentViewModel;

            navigation.CurrentViewModelChanged += () =>
            {
                CurrentViewModel = navigation.CurrentViewModel;
            };
        }
    }
}
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;


namespace SchiffeVersenken
{
    public partial class MainWindow : Window
    {
        private NavigationStore navigation;
        public ObservableObject CurrentViewModel { get; set; }

        public MainWindow(IServiceProvider services)
        {
            InitializeComponent();
            
            DataContext = services.GetRequiredService<MainViewModel>();
            navigation = services.GetRequiredService<NavigationStore>();
            CurrentViewModel = navigation.CurrentViewModel;
            navigation.CurrentViewModelChanged += Navigation_CurrentViewModelChanged;
        }

        private void Navigation_CurrentViewModelChanged()
        {
            CurrentViewModel = navigation.CurrentViewModel;
        }
    }
}
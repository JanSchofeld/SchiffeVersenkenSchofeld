using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Windows;

namespace SchiffeVersenken
{
    public partial class App : Application
    {
        private static IHost host;

        [STAThread]
        public static void Main(string[] args)
        {
            var builder = new HostBuilder();

            builder.ConfigureServices((context, services) =>
            {
                services.AddSingleton<NavigationStore>();
                services.AddSingleton<PlayerStore>();
                services.AddSingleton<MainWindow>();
                services.AddSingleton<MainViewModel>();
                services.AddSingleton<ChooseOpponentViewModel>();
                services.AddTransient<MenuViewModel>();
                services.AddTransient<ChooseNameViewModel>();
                services.AddTransient<PlayVsComputerViewModel>();
                services.AddTransient<SetShipsViewModel>();
                services.AddTransient<PlayerVsPlayerViewModel>();
            });

            host = builder.Build();
            host.Start();

            var app = new App();
            app.Run();
        }

        public App()
        {
            var navigation = host.Services.GetRequiredService<NavigationStore>();
            navigation.CurrentViewModel = host.Services.GetRequiredService<MenuViewModel>();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            host.Services.GetRequiredService<MainWindow>().Show();
            base.OnStartup(e);
        }
    }
}
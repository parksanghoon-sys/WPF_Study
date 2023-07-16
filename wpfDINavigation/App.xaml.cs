using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;
using wpfDINavigation.Servieces;
using wpfDINavigation.Stores;
using wpfDINavigation.ViewModels;
using wpfDINavigation.ViewModels.MainControlViewModels;
using wpfDINavigation.ViewModels.TestControlViewModels;
using wpfDINavigation.Views;

namespace wpfDINavigation
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public new static App Current => (App)Application.Current;        
        public App()
        {
            Services = ConfigureServieces();
            var mainView = Services.GetRequiredService<MainView>();
            mainView.Show();
        }
        private IServiceProvider ConfigureServieces()
        {
            var services = new ServiceCollection();

            services.AddSingleton<MainNavigationStore>();
            services.AddSingleton<SignupStore>();
            services.AddSingleton<RightStore>();
            services.AddSingleton<LeftStore>();

            services.AddSingleton<INavigationService, NavigationService>();

            services.AddSingleton<MainViewModel>();
            services.AddSingleton<LoginViewModel>();
            services.AddSingleton<SignupViewModel>();
            services.AddSingleton<TestViewModel>();

            services.AddSingleton<LeftViewModel>();
            services.AddSingleton<RightViewModel>();

            services.AddSingleton(s => new MainView()
            {
                DataContext = s.GetRequiredService<MainViewModel>()
            });
            return services.BuildServiceProvider();
        }
        public IServiceProvider Services { get; }
    }
}

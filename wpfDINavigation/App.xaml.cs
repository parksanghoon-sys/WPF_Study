using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using wpfDINavigation.Servieces;
using wpfDINavigation.Stores;
using wpfDINavigation.ViewModels;
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

            services.AddSingleton<INavigationService, NavigationService>();

            services.AddSingleton<MainViewModel>();
            services.AddSingleton<LoginViewModel>();
            services.AddSingleton<SignupViewModel>();
            services.AddSingleton<TestViewModel>();

            services.AddSingleton(s => new MainView()
            {
                DataContext = s.GetRequiredService<MainViewModel>()
            });
            return services.BuildServiceProvider();
        }
        public IServiceProvider Services { get; }
    }
}

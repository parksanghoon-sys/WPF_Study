using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;

using wpfPopup.Global.Interface;
using wpfPopup.Popup.Local.Services;
using wpfPopup.Popup.Local.ViewModels;
using wpfPopup.Popup.UI.Units;
using wpfPopup.Popup.UI.View;

namespace wpfPopup
{
    internal class App : Application
    {
        public new static App Current => (App)Application.Current;

        public IServiceProvider Services { get; }
        public App()
        {
            //Services = ConfigureServices();
            new Bootstrapper();

        }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            MainWindow window = new();

            window.ShowDialog();
        }
        private static IServiceProvider ConfigureServices()
        {
            ServiceCollection services = new ServiceCollection();
            // Services
            services.AddTransient<IDialogService, DialogService>();

            // Popup View
            //services.AddTransient<MainPopupWindow>();

            // Viewmodels
            services.AddTransient<MainPopupWindow>();
            services.AddTransient<MainViewModel>();
            return services.BuildServiceProvider();
        }

    }
}

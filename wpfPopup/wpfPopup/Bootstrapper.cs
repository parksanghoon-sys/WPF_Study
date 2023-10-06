using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;

using wpfPopup.Global.Interface;
using wpfPopup.Local.Interface;
using wpfPopup.Popup.Local.Services;
using wpfPopup.Popup.Local.ViewModels;
using wpfPopup.Popup.UI.Units;

namespace wpfPopup
{
    internal class Bootstrapper
    {
        public Bootstrapper()
        {
            var services = ConfigureServices();
            Ioc.Default.ConfigureServices(services);
        }
        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();
            //services.AddTransient<IView, FramePopup>();

            //// Viewmodels
            //services.AddTransient<MainViewModel>();
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

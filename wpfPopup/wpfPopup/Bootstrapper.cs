using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wpfPopup.Frame.Local.ViewModels;
using wpfPopup.Frame.UI.Units;
using wpfPopup.Local.Interface;

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
            services.AddTransient<IView, FramePopup>();

            // Viewmodels
            services.AddTransient<MainViewModel>();
            

            return services.BuildServiceProvider();
        }
    }
}

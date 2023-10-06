using CirclularGage.Main.Common;
using Log.Common;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.Windows;

namespace CirclularGage.Main
{
    /// <summary>
    /// App.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider Service;
        public App()
        {
            LogBase.AddFileListener();
            ExTrace.Print("App Start");
            Service = ConfigureServices();

            //ExDebug.Print("Debug : Start");
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            MainViewModel mainViewmodel = Service.GetService<MainViewModel>();
            var window = new MainWindow();
            window.DataContext = mainViewmodel;
            window.ShowDialog();
            ExTrace.Print("Window Start");
        }
        protected override void OnExit(ExitEventArgs e)
        {     
            base.OnExit(e);

        }
        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            // Viewmodels
            services.AddSingleton<MainViewModel>();
            services.AddTransient<IServiceTest, ServiceTest>();

            return services.BuildServiceProvider();
        }
    }
}

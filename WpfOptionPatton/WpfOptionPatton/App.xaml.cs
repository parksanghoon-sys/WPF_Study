using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Reflection;
using System.Windows;
using WpfOptionPatton.Configurations.Mailing;
using WpfOptionPatton.Services;

namespace WpfOptionPatton
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IServiceProvider _service;
        private MainViewModel _configuration;

        public App()
        {
            _service = ConfigureServices();
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            var mainWindowVmFactory = _service.GetRequiredService<IFactory<MainViewModel>>();
            _configuration = mainWindowVmFactory.New();

            var mainWindow = new MainWindow
            {
                DataContext = _configuration,
            };
            mainWindow.Show();            

            base.OnStartup(e);

        }
        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            services.AddTransient(typeof(IFactory<>), typeof(Factory<>));
            services.AddSingleton<IIntService, IntManager>();

            var configuration = new ConfigurationBuilder()
                        //.SetBasePath(Directory.GetCurrentDirectory())       
                        .AddJsonFile("appsettings.json")
                        .Build();

            services.AddSingleton<IConfiguration>(configuration);
            //string path = Directory.GetCurrentDirectory();
            //string baseLogDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            //string appName = Assembly.GetExecutingAssembly().GetName().Name!;
            //baseLogDir = Path.Combine(baseLogDir, appName);
            //configuration["baseLogDir"] = baseLogDir;

            var mailingOptions = new MailingOptions();
            configuration.Bind(nameof(MailingOptions), mailingOptions);
            services.AddSingleton(Options.Create(mailingOptions));
            // Viewmodels
            //services.AddSingleton<MainViewModel>();
            //services.AddTransient<IServiceTest, ServiceTest>();

            return services.BuildServiceProvider();
        }
    }
}

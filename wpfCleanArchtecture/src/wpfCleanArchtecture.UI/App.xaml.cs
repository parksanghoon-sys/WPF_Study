using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Windows;
using wpfCleanArchtecture.Application;
using wpfCleanArchtecture.Infrastructure;
using wpfCleanArchtecture.Presentation.ViewModels;

namespace wpfCleanArchtecture.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        private IHost? _host;

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Host 설정
            _host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    // Infrastructure 계층 의존성 등록
                    services.AddInfrastructure();

                    // Application 계층 의존성 등록
                    services.AddScoped<ITodoService, TodoService>();

                    // Presentation 계층 의존성 등록
                    services.AddTransient<MainViewModel>();
                    services.AddTransient<MainWindow>();

                    // 로깅 설정
                    services.AddLogging(builder =>
                    {
                        builder.AddConsole();
                        builder.AddDebug();
                    });
                })
                .Build();

            await _host.StartAsync();

            // MainWindow 생성 및 표시
            var mainWindow = _host.Services.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            if (_host != null)
            {
                await _host.StopAsync();
                _host.Dispose();
            }

            base.OnExit(e);
        }
    }

}

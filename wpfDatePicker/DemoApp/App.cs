using System.Windows;
using DemoApp.UI.Views;

namespace DemoApp
{
    internal class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            DateWindow dateWindow = new DateWindow();
            dateWindow.ShowDialog();
        }
    }
}

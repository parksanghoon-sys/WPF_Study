using System.Windows;
using wpfDatePicker.UI.Views;

namespace wpfDatePicker
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

using System.Windows;
using WpfExplorer.Support.UI.Units;

namespace WpfExplorer
{
    internal class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            DarkWindow window = new DarkWindow();
            window.ShowDialog();
        }
    }
}

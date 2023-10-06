using CommunityToolkit.Mvvm.DependencyInjection;
using System.Windows;

using wpfPopup.Local.Interface;
using wpfPopup.Popup.Local.ViewModels;

namespace wpfPopup.Popup.UI.View
{

    public class MainWindow : Window
    {
        static MainWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MainWindow), new FrameworkPropertyMetadata(typeof(MainWindow)));
        }
        public MainWindow()
        {
            this.DataContext = Ioc.Default.GetService<MainViewModel>();
        }     
    }
}

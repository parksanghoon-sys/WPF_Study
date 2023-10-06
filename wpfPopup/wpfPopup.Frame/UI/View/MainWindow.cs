using System.Windows;
using wpfPopup.Frame.Local.ViewModels;
using wpfPopup.Frame.UI.Units;
using wpfPopup.Local.Interface;

namespace wpfPopup.Frame.UI.View
{

    public class MainWindow : Window, IMainView
    {
        static MainWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MainWindow), new FrameworkPropertyMetadata(typeof(MainWindow)));
        }
        public MainWindow()
        {
            this.DataContext = new MainViewModel(this);
        }
        public bool? ShowPopupWindow()
        {
            FramePopup framePopup = new FramePopup();
            return framePopup.ShowDialog();
        }
    }
}

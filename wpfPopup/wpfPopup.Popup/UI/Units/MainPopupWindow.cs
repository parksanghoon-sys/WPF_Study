using System.Windows;
using wpfPopup.Global.Interface;
using wpfPopup.Popup.Local.ViewModels;

namespace wpfPopup.Popup.UI.Units
{

    public class MainPopupWindow : Window, IDialog
    {
        static MainPopupWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MainPopupWindow), new FrameworkPropertyMetadata(typeof(MainPopupWindow)));
        }
        public MainPopupWindow()
        {
            this.DataContext = new PopupMainViewModel();
        }
    }   
}

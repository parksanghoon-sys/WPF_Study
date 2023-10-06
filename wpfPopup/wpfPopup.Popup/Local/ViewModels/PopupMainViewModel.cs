using wpfPopup.Global;

namespace wpfPopup.Popup.Local.ViewModels
{
    public class PopupMainViewModel : ViewModelBase
    {
        private ViewModelBase _popupVM;
        public ViewModelBase PopupVM 
        { 
            get => _popupVM;
            set => this.SetProperty(ref _popupVM, value);
        }
    }
}

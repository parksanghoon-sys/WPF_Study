using wpfPopup.Global.Interface;
using wpfPopup.Global;
using CommunityToolkit.Mvvm.DependencyInjection;
using wpfPopup.Popup.Local.ViewModels;
using wpfPopup.Popup.UI.Units;

namespace wpfPopup.Popup.Local.Services
{
    public class DialogService : IDialogService
    {
        private IDialog _popupWindow;
        public DialogService()
        {
            _popupWindow = Ioc.Default.GetService<MainPopupWindow>();
        }

        public IDialog Dialog => _popupWindow;

        public void SetVM(ViewModelBase vm)
        {
            if (_popupWindow.DataContext is PopupMainViewModel viewModel)
            {
                viewModel.PopupVM = vm;
            }
        }
    }
}

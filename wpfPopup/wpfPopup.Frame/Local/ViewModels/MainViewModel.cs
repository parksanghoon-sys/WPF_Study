using CommunityToolkit.Mvvm.Input;
using wpfPopup.Global;
using wpfPopup.Local.Interface;

namespace wpfPopup.Frame.Local.ViewModels
{
    public class MainViewModel : ViewModelBase<IMainView>
    {
        public MainViewModel(IMainView view) :
            base(view)
        {
            
        }
        private RelayCommand _popUpCommand;
        public RelayCommand PopUpCommand
        {
            get
            {
                return _popUpCommand ??
                    (_popUpCommand = new RelayCommand(() =>
                    {
                        base.View.ShowPopupWindow();
                    },
                    null));
            }
        }
    }
}

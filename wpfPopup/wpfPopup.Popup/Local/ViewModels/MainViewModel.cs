using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using wpfPopup.Global;
using wpfPopup.Global.Interface;

namespace wpfPopup.Popup.Local.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IDialogService _dialogService;
        public MainViewModel(IDialogService dialogService)
        {
            _dialogService = dialogService;
        }
        private RelayCommand _popUpCommand;
        public ICommand PopUpCommand => _popUpCommand ?? new RelayCommand(PopUp);

        private void PopUp()
        {
            _dialogService.SetVM(new Popup1ViewModel());
            _dialogService.Dialog.ShowDialog();
        }
    }
}

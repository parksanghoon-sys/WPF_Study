using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace wpfHintTextBox
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool _isNameAccept = false;
        [ObservableProperty]
        private bool _isPswAccept = false;

        private RelayCommand _onClickCommand;
        public ICommand OnClickCommand => _onClickCommand ?? new RelayCommand(OnClick);        
        private void OnClick()
        {
            IsNameAccept = !IsNameAccept;
            IsPswAccept = !IsPswAccept;
        }
    }
}

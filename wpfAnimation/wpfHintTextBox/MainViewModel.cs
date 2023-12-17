using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace wpfHintTextBox
{
    public partial class MainViewModel : ObservableObject
    {
        private bool _isNameAccept;

        public bool IsNameAccept
        {
            get { return _isNameAccept; }
            set { SetProperty(ref _isNameAccept, value);  }
        }
        private bool _isPswAccept;

        public bool IsPswAccept
        {
            get { return _isPswAccept; }
            set { SetProperty(ref _isPswAccept, value); }
        }

        private RelayCommand _onClickCommand;
        public MainViewModel()
        {
            IsNameAccept = false;
            IsPswAccept = false;
        }
        public ICommand OnClickCommand => _onClickCommand ?? new RelayCommand(OnClick);        
        private void OnClick()
        {
            IsNameAccept = !IsNameAccept;
            IsPswAccept = !IsPswAccept;
        }
    }
}

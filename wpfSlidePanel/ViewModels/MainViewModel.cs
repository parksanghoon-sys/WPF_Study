using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using wpfSlidePanel.Commands;

namespace wpfSlidePanel.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private bool _isSignupModalOpen;
        public ICommand SignupCommand => new RelayCommand(_ => IsSignupModalOpen = true);
        public bool IsSignupModalOpen
        {
            get => _isSignupModalOpen;
            set
            {
                _isSignupModalOpen = value;
                OnPropertyChanged(nameof(IsSignupModalOpen));
            }
        }
    }
}

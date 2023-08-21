using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using wpfLockButton.Commands;

namespace wpfLockButton.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private bool _isLocked;
        public bool IsLocked
        {
            get => _isLocked;
            set
            {
                if (_isLocked != value)
                {
                    _isLocked = value;
                    OnPropertyChangeed();
                    OnPropertyChangeed(nameof(IsViewEnabled));
                }
            }
        }
        public MainViewModel()
        {
            IsLocked = true;
        }
        public bool IsViewEnabled => !IsLocked;
        public ICommand ToggleLockCommand => new RelayCommand<object>(_ => IsLocked = !IsLocked);
        private void OnPropertyChangeed([CallerMemberName]string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

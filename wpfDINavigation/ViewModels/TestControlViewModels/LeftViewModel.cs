using System.Windows.Input;
using wpfDINavigation.Commands;
using wpfDINavigation.Models;
using wpfDINavigation.Stores;

namespace wpfDINavigation.ViewModels.TestControlViewModels
{
    public class LeftViewModel : ViewModelBase
    {
        private readonly LeftStore _leftStore;
        private readonly RightStore _rightStore;
        private string _id = "";
        private string _password = "";
        private string _name = "";
        private string _email = "";

        private Account CurrentAccount => _leftStore.CurrentAccount!;
        private void Initialize()
        {
            Id = CurrentAccount.Id;
            Password = CurrentAccount.Password;
            Name = CurrentAccount.Name;
            Email = CurrentAccount.Email;
        }
        private void SendAccountToLeft(object obj)
        {
            _rightStore.CurrentAccount = new Account
            {
                Id = Id,
                Email = Email,
                Name = Name,
                Password = Password
            };
        }
        public LeftViewModel(LeftStore leftStore, RightStore rightStore)
        {
            _leftStore = leftStore;
            _rightStore = rightStore;
            Initialize();
            _leftStore.CurrentAccountChanged += CurrentAccountChanged;
            SendAccountToLeftCommand = new RelayCommand<object>(SendAccountToLeft);
        }

        private void CurrentAccountChanged(Account obj)
        {
            Id = obj.Id;
            Password = obj.Password;
            Name = obj.Name;
            Email = obj.Email;
        }

        public string Id
        {
            get { return _id; }
            set
            {
                if (_id != value)
                {
                    _id = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                if (_password != value)
                {
                    _password = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Email
        {
            get { return _email; }
            set
            {
                if (_email != value)
                {
                    _email = value;
                    OnPropertyChanged();
                }
            }
        }
        public ICommand SendAccountToLeftCommand { get; set; }
    }
}

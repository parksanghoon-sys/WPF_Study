using System.Windows.Input;
using wpfDINavigation.Commands;
using wpfDINavigation.Models;
using wpfDINavigation.Stores;

namespace wpfDINavigation.ViewModels.TestControlViewModels
{
    public class RightViewModel : ViewModelBase
    {
        private readonly RightStore _rightStore;
        private readonly LeftStore _leftStore;
        private string _id = "";
        private string _password = "";
        private string _name = "";
        private string _email = "";

        private Account CurrentAccount => _rightStore.CurrentAccount!;
        private void Initialize()
        {
 

        }
        public RightViewModel(RightStore rightStore, LeftStore leftStore)
        {
            _rightStore = rightStore;
            _leftStore = leftStore;

            _rightStore.CurrentAccountChanged += CurrentAccountChanged;
            SendAccountToRightCommand = new RelayCommand<object>(SendAccountToRight);
            Initialize();
        }

        private void CurrentAccountChanged(Account account)
        {
            Id = account.Id;
            Password = account.Password;
            Name = account.Name;
            Email = account.Email;
        }

        private void SendAccountToRight(object obj)
        {
            _leftStore.CurrentAccount = new Account
            {
                Id = Id,
                Password = Password,
                Name = Name,
                Email = Email
            };
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
        public ICommand SendAccountToRightCommand { get; set; }
    }
}

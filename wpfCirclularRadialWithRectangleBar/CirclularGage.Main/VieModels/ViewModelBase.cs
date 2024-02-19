using CirclularGage.Main.Helpers;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CirclularGage.Main
{
    public class ViewModelBase : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChagned([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

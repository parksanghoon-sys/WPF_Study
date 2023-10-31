using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfTranasparentWindow
{
    public class NotifyPropertyChanged : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void ClearAllPropertyChangedHandlers()
        {
            if (this.PropertyChanged == null)
            {
                return;
            }

            foreach (PropertyChangedEventHandler handler in this.PropertyChanged.GetInvocationList())
            {
                this.PropertyChanged -= handler;
            }
        }
        #endregion
    }
}

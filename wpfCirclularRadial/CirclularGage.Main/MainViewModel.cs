using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CirclularGage.Main
{
    internal class MainViewModel : INotifyPropertyChanged
    {        
        public event PropertyChangedEventHandler PropertyChanged;

        private double _airPortHeadingAngle;

        public double AirPortHeadingAngle
        {
            get { return _airPortHeadingAngle; }
            set { _airPortHeadingAngle = value % 360; OnPropertyChagned(); }
        }
        private double _startSafeZon;

        public double StartSafeZon
        {
            get { return _startSafeZon; }
            set { _startSafeZon = Convert.ToDouble(value); OnPropertyChagned(); }
        }
        private double _endtSafeZon;

        public double EndSafeZon
        {
            get { return _endtSafeZon; }
            set { _endtSafeZon = Convert.ToDouble(value); OnPropertyChagned(); }
        }
        private double _score;

        public double Score
        {
            get { return _score; }
            set { _score = Convert.ToDouble(value); OnPropertyChagned(); }
        }

        private ICommand _sendHeading;
        public ICommand SendHeading
        {
            get { return _sendHeading; }
            private set { _sendHeading = value;  }
        }
        
        public MainViewModel()
        {
            AirPortHeadingAngle = 12;
            EndSafeZon = 0;
            StartSafeZon = 0;
            SendHeading = new ParameterRelayCommand(btn => SendHeadingCommand(btn));
        }

        private void SendHeadingCommand(object btn)
        {
            if(btn is string  value)
            {
                AirPortHeadingAngle = Convert.ToDouble(value);
            }
        }

        private void OnPropertyChagned([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

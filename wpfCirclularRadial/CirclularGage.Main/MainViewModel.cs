using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public ObservableCollection<RectItem> RectItems { get; set; }
        public MainViewModel()
        {
            AirPortHeadingAngle = 12;
            EndSafeZon = 0;
            StartSafeZon = 0;
            SendHeading = new ParameterRelayCommand(btn => SendHeadingCommand(btn));

            RectItems = new ObservableCollection<RectItem>();
            RectItems.Add(RectItem.RectItemFactory(200, 50, 20, 30));
            RectItems.Add(RectItem.RectItemFactory(200, 80, 10, 30));
            RectItems.Add(RectItem.RectItemFactory(200, 20, 30, 30));
            RectItems.Add(RectItem.RectItemFactory(200, 200, 30, 30));
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

    public class RectItem
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public static RectItem RectItemFactory(double X, double Y, double width, double height)
        {
            return new RectItem { X = X, Y = Y, Width = width, Height = height };
        }
    }

}

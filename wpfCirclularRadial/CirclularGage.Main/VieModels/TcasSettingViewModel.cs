using CirclularGage.Location.Local.Enums;
using CirclularGage.Main.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;

namespace CirclularGage.Main
{
    public class TcasSettingViewModel : ViewModelBase
    {
        private TcasSymbol _intruderType;
        private double _range;
        private double _altritude;
        private IntruderVerticalSenseState _intruderVerticalMoveMentState;
        private double _bearing;

        public TcasSymbol IntruderType
        {
            get { return _intruderType; }
            set { _intruderType = value; OnPropertyChagned(); }
        }
        public IntruderVerticalSenseState IntruderVerticalMoveMentState
        {
            get { return _intruderVerticalMoveMentState; }
            set { _intruderVerticalMoveMentState = value; OnPropertyChagned(); }
        }
        public double Range
        {
            get { return _range; }
            set { _range = value; OnPropertyChagned(); }
        }
        public double Bearing
        {
            get { return _bearing; }
            set { _bearing = value; OnPropertyChagned(); }
        }
        public double Altitude
        {
            get { return _altritude; }
            set
            {
                if (_altritude != value)
                {
                    _altritude = value;
                    OnPropertyChagned();
                }
            }
        }
        public Array IntruderTypes
        {
            get { return Enum.GetValues(typeof(TcasSymbol)); }
        }
        public Array IntruderVerticalSenseStates
        {
            get { return Enum.GetValues(typeof(IntruderVerticalSenseState)); }
        }
        private ICommand _intruderItemSendCommand;
        public ICommand IntruderItemSendCommand
        {
            get => _intruderItemSendCommand;
            private set { _intruderItemSendCommand = value; }
        }
        public TcasSettingViewModel()
        {
            IntruderItemSendCommand = new ParameterRelayCommand(btn => OnIntruderItemSend(btn), obj => NotIntruderItemSend(obj));
            Messenger.Register<ObservableCollection<IntruderModel>>(this, OnViewModel2MessageReceived);
        }

        private bool NotIntruderItemSend(object _)
        {
            //if ((Range != 0) && (Altitude != 0) && (IntruderType != TcasSymbol.Nodata) && (IntruderVerticalMoveMentState != IntruderVerticalSenseState.NoData) && (_bearing != 0))
            //    return true;
            //return false;
            return true;
        }

        private void OnIntruderItemSend(object _)
        {
            var IntruderItem = new IntruderModel
            {
                Range = Range,
                Altitude = Altitude,
                IntruderType = IntruderType,
                IntruderVerticalMoveMentState = IntruderVerticalMoveMentState,
                Bearing = Bearing
            };
            Messenger.Send(IntruderItem);
        }

        private void OnViewModel2MessageReceived(ObservableCollection<IntruderModel> obj)
        {
            throw new NotImplementedException();
        }
    }
}

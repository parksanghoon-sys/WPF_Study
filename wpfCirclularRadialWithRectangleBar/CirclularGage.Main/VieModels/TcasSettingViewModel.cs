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
        private IntruderModel _currentIntruder;
        private TcasSymbol _intruderType;
        #region Properties
        public TcasSymbol IntruderType
        {
            get { return _intruderType; }
            set { _intruderType = value; OnPropertyChagned(); }
        }
        public IntruderVerticalSenseState IntruderVerticalMoveMentState
        {
            get { return _currentIntruder.IntruderVerticalMoveMentState; }
            set { _currentIntruder.IntruderVerticalMoveMentState = value; OnPropertyChagned(); }
        }
        public double Range
        {
            get { return _currentIntruder.Range; }
            set
            {
                _currentIntruder.Range = value;
                OnPropertyChagned();
            }
        }
        public double Bearing
        {
            get { return _currentIntruder.Bearing; }
            set
            {
                _currentIntruder.Bearing = value;
                OnPropertyChagned();
            }
        }
        public double Altitude
        {
            get { return _currentIntruder.Altitude; }
            set
            {
                if (_currentIntruder.Altitude != value)
                {
                    _currentIntruder.Altitude = value;
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
        #endregion


        #region  Commands
        private ICommand _intruderItemSendCommand;
        private ICommand _intruderItemUpdateCommand;
        public ICommand IntruderItemSendCommand
        {
            get => _intruderItemSendCommand;
            private set { _intruderItemSendCommand = value; }
        }
        public ICommand IntruderItemUpdateCommand
        {
            get { return _intruderItemUpdateCommand; }
            private set { _intruderItemUpdateCommand = value; }
        } 
        #endregion

        public TcasSettingViewModel()
        {
            IntruderItemSendCommand = new ParameterRelayCommand(obj => OnIntruderItemSend(obj), obj => NotIntruderItemSend(obj));
            IntruderItemUpdateCommand = new ParameterRelayCommand(obj => OnIntruderItemUpdate(obj));

            Messenger.Register<IntruderModel>( nameof(TcasSettingViewModel), OnViewModelMessageReceived);            
            _currentIntruder = new IntruderModel();
        }
        #region Commands Excute Method
        private void OnIntruderItemUpdate(object _)
        {
            Messenger.Send(nameof(MainViewModel), _currentIntruder);
        }

        private void OnIntruderItemSend(object _)
        {
            Messenger.Send(nameof(MainViewModel), _currentIntruder);
        }
        private bool NotIntruderItemSend(object _)
        {
            //if ((Range != 0) && (Altitude != 0) && (IntruderType != TcasSymbol.Nodata) && (IntruderVerticalMoveMentState != IntruderVerticalSenseState.NoData) && (_bearing != 0))
            //    return true;
            //return false;
            return true;
        }
        #endregion

        #region Methods
        private void OnViewModelMessageReceived(IntruderModel model)
        {
            _currentIntruder.Number = model.Number;
            Bearing = model.Bearing;
            IntruderType = model.IntruderType;
            Altitude = model.Altitude;
            Range = model.Range;
            IntruderVerticalMoveMentState = model.IntruderVerticalMoveMentState;
        }
        #endregion

    }
}

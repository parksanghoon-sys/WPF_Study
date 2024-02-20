
using CirclularGage.Location.Local.Enums;
using CirclularGage.Main.Common;
using CirclularGage.Main.Model;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace CirclularGage.Main
{
    internal class MainViewModel : ViewModelBase
    {       
        private double _airPortHeadingAngle;
        private double _startSafeZoon;
        private double _endtSafeZoon;
        private double _startWarningZoon;
        private double _endWarningZoon;
        private double _score;
        private double _gaugeRadius;
        private bool _riseLedOn;        
        private bool _fallLedOn;        
        private double _airPortSymbolBackgroundWidth;
        private double _airPortSymbolBackgroundHeight;
        private IntruderModel _intruderItem;
        private TcasDisplayRange _tcasDisplayRange = TcasDisplayRange.Rate5;
        private TcasAltitudeType _tcasAltitudeType;
        private TcasDisplayAboveBelow _tcasDisplayAboveBelow;     
        private ICommand _startTcasItemsRandom;
        private ICommand _clearTcasItems;
        private ICommand _createTcasItems;

        private readonly IServiceTest _serviceTest;
        #region Properties
        public double AirPortHeadingAngle
        {
            get { return _airPortHeadingAngle; }
            set { _airPortHeadingAngle = value % 360; OnPropertyChagned(); }
        }
        public double AirPortSymbolBackgroundWidth
        {
            get { return _airPortSymbolBackgroundWidth; }
            set { _airPortSymbolBackgroundWidth = value; OnPropertyChagned(); }
        }
        public double AirPortSymbolBackgroundHeight
        {
            get { return _airPortSymbolBackgroundHeight; }
            set { _airPortSymbolBackgroundHeight = value; OnPropertyChagned(); }
        }

        public double StartSafeZoon
        {
            get { return _startSafeZoon; }
            set { _startSafeZoon = value; OnPropertyChagned(); }
        }
        public double EndSafeZoon
        {
            get { return _endtSafeZoon; }
            set { _endtSafeZoon = value; OnPropertyChagned(); }
        }
        public double Score
        {
            get { return _score; }
            set { _score = Convert.ToDouble(value); OnPropertyChagned(); }
        }
        public double GaugeRadius
        {
            get { return _gaugeRadius; }
            set { _gaugeRadius = value; OnPropertyChagned(); }
        }
        public double StartWarningZoon
        {
            get { return _startWarningZoon; }
            set { _startWarningZoon = value; OnPropertyChagned(); }
        }
        public double EndWarningZoon
        {
            get { return _endWarningZoon; }
            set { _endWarningZoon = value; OnPropertyChagned(); }
        }
        public bool RiseLedOn
        {
            get { return _riseLedOn; }
            set { _riseLedOn = value; OnPropertyChagned(); }
        }
        public bool FallLedOn
        {
            get { return _fallLedOn; }
            set { _fallLedOn = value; OnPropertyChagned(); }
        }
        public TcasDisplayRange SelectedTcasDisplayRange
        {
            get { return _tcasDisplayRange; }
            set
            {
                if (_tcasDisplayRange != value)
                {
                    _tcasDisplayRange = value;
                    SetAirPortBackgroundSetting(_tcasDisplayRange);
                    OnPropertyChagned();
                }

            }
        }
        public TcasAltitudeType TcasAltitudeType
        {
            get { return _tcasAltitudeType; }
            set { _tcasAltitudeType = value; OnPropertyChagned(); }
        }
        public TcasDisplayAboveBelow TcasDisplayAboveBelow
        {
            get { return _tcasDisplayAboveBelow; }
            set { _tcasDisplayAboveBelow = value; OnPropertyChagned(); }
        }
        public IntruderModel IntruderItem
        {
            get => _intruderItem;
            set
            {
                if(_intruderItem != value)
                {
                    CancelOldIntruderChecked(_intruderItem);
                    _intruderItem = value;

                    var intruderItem = IntruderItems.FirstOrDefault(number => _intruderItem.Equals(number));
                    if (intruderItem != null)
                    {
                        intruderItem.IsSelected = true;
                    }
                    Messenger.Send(nameof(TcasSettingViewModel), IntruderItem);
                }
                
            }
        }
        public Array TcasAltitudeTypes
        {
            get { return Enum.GetValues(typeof(TcasAltitudeType)); }
        }
        public Array TcasDisplayAboveBelows
        {
            get { return Enum.GetValues(typeof(TcasDisplayAboveBelow)); }
        }
        public Array TcasDisplayRanges
        {
            get { return Enum.GetValues(typeof(TcasDisplayRange)); }
        }
        public ObservableCollection<IntruderModel> IntruderItems { get; set; }
        #endregion
        #region Commands

        public ICommand StartTcasItemsRandom
        {
            get { return _startTcasItemsRandom; }
            private set { _startTcasItemsRandom = value; }
        }
        public ICommand ClearTcasItems
        {
            get { return _clearTcasItems; }
            private set { _clearTcasItems = value; }
        }
        public ICommand CreateTcasItems
        {
            get { return _createTcasItems; }
            private set { _createTcasItems = value; }
        }
        

        #endregion
        public MainViewModel(IServiceTest serviceTest)
        {
            InitVariables();
            InitCommands();
            //CreateMaxIntruderItems();

            Messenger.Register<IntruderModel>(nameof(MainViewModel), OnIntruderModelMessageReceived);
            Messenger.Register<TcasDisplayRange>(nameof(MainViewModel), OnTcasDisplayRangeMessageReceived);
            _serviceTest = serviceTest;
        }

   

        #region Commands Excuate Methods

        private void StartTcasItemsRandomCommand(object _)
        {
            _serviceTest.Test();
            Random random = new Random();
            foreach (var item in IntruderItems)
            {
                Array values2 = Enum.GetValues(typeof(IntruderVerticalSenseState));
                var randomIntruderVerticalSenseState = (IntruderVerticalSenseState)values2.GetValue(random.Next(values2.Length));
                Array values3 = Enum.GetValues(typeof(DisplayMatrix));
                var randomDisplayMatrix = (DisplayMatrix)values3.GetValue(random.Next(values3.Length));

                item.IntruderDisplay = randomDisplayMatrix;
                item.IntruderVerticalMoveMentState = randomIntruderVerticalSenseState;
                item.Bearing = random.Next(0, 360);
                item.Altitude = random.Next(-127, 127);
                item.Range = random.Next(0, 120);
                
            }
        }
        private void ClearItemsRandom(object _)
        {
            if (IntruderItems.Count != 0)
                IntruderItems.Clear();
        }
        private void CreateItemsRandom(object _)
        {
            CreateMaxIntruderItems();
        }
        #endregion

        #region Methods
        private void InitVariables()
        {
            GaugeRadius = 100;
            AirPortHeadingAngle = 0;
            AirPortSymbolBackgroundWidth = 0;
            SelectedTcasDisplayRange = TcasDisplayRange.Rate5;
            SetAirPortBackgroundSetting(SelectedTcasDisplayRange);
            Score = double.MinValue;
            RiseLedOn = true;
            FallLedOn = false;
            IntruderItems = new ObservableCollection<IntruderModel>();
        }

        private void SetAirPortBackgroundSetting(TcasDisplayRange tcasDisplayRange)
        {
            switch (tcasDisplayRange)
            {
                case TcasDisplayRange.Rate5:
                    AirPortSymbolBackgroundHeight = 108.1690141;
                    AirPortSymbolBackgroundWidth = 100.1450474;
                    break;
                case TcasDisplayRange.Rate10:
                    AirPortSymbolBackgroundHeight = 54.08450704;
                    AirPortSymbolBackgroundWidth = 50.07252371;
                    break;
                case TcasDisplayRange.Rate20:
                    AirPortSymbolBackgroundHeight = 27.04225352;
                    AirPortSymbolBackgroundWidth = 25.03626185;
                    break;
                case TcasDisplayRange.Rate40:
                    AirPortSymbolBackgroundHeight = 0;
                    AirPortSymbolBackgroundWidth = 0;
                    break;

            }
        }

        private void InitCommands()
        {
            StartTcasItemsRandom = new ParameterRelayCommand(btn => StartTcasItemsRandomCommand(btn));
            ClearTcasItems = new ParameterRelayCommand(btn => ClearItemsRandom(btn));
            CreateTcasItems = new ParameterRelayCommand(btn => CreateItemsRandom(btn));
        }
        private void CreateMaxIntruderItems()
        {
            for (int i = 0; i < 5; i++)
            {
                var random = new Random();
                Array values2 = Enum.GetValues(typeof(IntruderVerticalSenseState));
                var randomIntruderVerticalSenseState = (IntruderVerticalSenseState)values2.GetValue(random.Next(values2.Length));
                Array values3 = Enum.GetValues(typeof(DisplayMatrix));
                var randomDisplayMatrix = (DisplayMatrix)values3.GetValue(random.Next(values3.Length));
                var altitue = random.Next(-1200, 1200);
                var bearing = 360;
                var range = 2;

                OnIntruderModelMessageReceived(IntruderModel.IntruderModelFactory(i + 1, range,
                    altitue, randomIntruderVerticalSenseState, TcasIntruderSymbol.ProximateTraffic, randomDisplayMatrix,SelectedTcasDisplayRange, bearing/(i+10)));
                Thread.Sleep(10);
            }
        }
        private void OnIntruderModelMessageReceived(IntruderModel model)
        {
            if (IntruderItems.Count > 30)
            {
                string message = "더 이상 불가";
                var result = MessageBox.Show(message);
                return;
            }

            if (IntruderItems.Contains(model))
            {
                var item = IntruderItems.Where(k => k.Equals(model)).FirstOrDefault();
                item.IntruderVerticalMoveMentState = model.IntruderVerticalMoveMentState;               
                item.IntruderDisplay = model.IntruderDisplay;
                item.Bearing = model.Bearing;
                item.Altitude = model.Altitude;
                item.Range = model.Range;
            }
            else
            {                
                IntruderItems.Add(model);
            }
       
        }
        private void OnTcasDisplayRangeMessageReceived(TcasDisplayRange range)
        {
            SelectedTcasDisplayRange = range;
        }
        private bool CancelOldIntruderChecked(IntruderModel model)
        {
            if (model != null)
            {
                IntruderItems.Where(number => model.Equals(number)).ToList().ForEach(number => number.IsSelected = false);                
            }
            return true;
        }
        #endregion


    }
    

}

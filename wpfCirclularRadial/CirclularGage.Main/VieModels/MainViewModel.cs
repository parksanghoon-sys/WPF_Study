
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
        private double _score;
        private double _gaugeRadius;
        private double _endWarningZoon;        
        private IntruderModel _intruderItem;
        private TcasDisplayRange _tcasDisplayRange;
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
        public TcasDisplayRange TcasDisplayRange
        {
            get { return _tcasDisplayRange; }
            set
            {
                if (_tcasDisplayRange != value)
                {
                    _tcasDisplayRange = value;                 
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
            CreateMaxIntruderItems();

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
            GaugeRadius = 110;
            AirPortHeadingAngle = 0;
         
            TcasDisplayRange = TcasDisplayRange.RangeNone;
            Score = double.MinValue;

            IntruderItems = new ObservableCollection<IntruderModel>();
        }
        private void InitCommands()
        {
            StartTcasItemsRandom = new ParameterRelayCommand(btn => StartTcasItemsRandomCommand(btn));
            ClearTcasItems = new ParameterRelayCommand(btn => ClearItemsRandom(btn));
            CreateTcasItems = new ParameterRelayCommand(btn => CreateItemsRandom(btn));
        }
        private void CreateMaxIntruderItems()
        {
            for (int i = 0; i < 30; i++)
            {
                var random = new Random();
                Array values2 = Enum.GetValues(typeof(IntruderVerticalSenseState));
                var randomIntruderVerticalSenseState = (IntruderVerticalSenseState)values2.GetValue(random.Next(values2.Length));
                Array values3 = Enum.GetValues(typeof(DisplayMatrix));
                var randomDisplayMatrix = (DisplayMatrix)values3.GetValue(random.Next(values3.Length));

                OnIntruderModelMessageReceived(IntruderModel.IntruderModelFactory(i + 1, 120,
                    random.Next(-1200, 1200), randomIntruderVerticalSenseState, TcasSymbol.ProximateTraffic, randomDisplayMatrix, 360 / 30 * (i + 1)));
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
            TcasDisplayRange = range;
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

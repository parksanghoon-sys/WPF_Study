
using CirclularGage.Location.Local.Enums;
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
        private ICommand _startTcasItemsRandom;

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
        private int _tCasDisplayRange;

        public int TCasDisplayRange
        {
            get { return _tCasDisplayRange; }
            set { _tCasDisplayRange = value; OnPropertyChagned(); }
        }        
        public TcasDisplayRange TcasDisplayRangeValue
        {
            get { return _tcasDisplayRange; }
            set
            {
                if (_tcasDisplayRange != value)
                {
                    _tcasDisplayRange = value;
                    switch (value)
                    {
                        case TcasDisplayRange.TcasDisplayRangeNone:
                            TCasDisplayRange = 1;
                            break;
                        case TcasDisplayRange.TcasDisplayRange10nm:
                            TCasDisplayRange = 10;
                            break;
                        case TcasDisplayRange.TcasDisplayRange20nm:
                            TCasDisplayRange = 20;
                            break;
                        case TcasDisplayRange.TcasDisplayRange40nm:
                            TCasDisplayRange = 40;
                            break;
                        case TcasDisplayRange.TcasDisplayRange80nm:
                            TCasDisplayRange = 80;
                            break;
                    }
                    OnPropertyChagned();
                }

            }
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
        public ObservableCollection<IntruderModel> IntruderItems { get; set; }
        #endregion
        #region Commands

        public ICommand StartTcasItemsRandom
        {
            get { return _startTcasItemsRandom; }
            private set { _startTcasItemsRandom = value; }
        } 
        #endregion
        public MainViewModel()
        {
            GaugeRadius = 140;
            AirPortHeadingAngle = 0;

            StartSafeZoon = 0;
            EndSafeZoon = 3;

            StartWarningZoon = -5;
            EndWarningZoon = -4;

            TCasDisplayRange = 1;
            TcasDisplayRangeValue = TcasDisplayRange.TcasDisplayRange20nm;
            Score = 0;

            IntruderItems = new ObservableCollection<IntruderModel>();
            StartTcasItemsRandom = new ParameterRelayCommand(btn => StartTcasItemsRandomCommand(btn));            
            Messenger.Register<IntruderModel>(nameof(MainViewModel), OnIntruderModelMessageReceived);
                       
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
        #region Commands Excuate Methods
        private void StartTcasItemsRandomCommand(object _)
        {
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
        #endregion

        #region Methods
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

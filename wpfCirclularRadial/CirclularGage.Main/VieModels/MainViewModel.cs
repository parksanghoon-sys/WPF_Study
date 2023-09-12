
using CirclularGage.Location.Local.Enums;
using CirclularGage.Main.Model;
using System;
using System.Collections.ObjectModel;
using System.Linq;
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
        private int _gaugeRadius;
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
            set { _startSafeZoon = Convert.ToDouble(value); OnPropertyChagned(); }
        }
        public double EndSafeZoon
        {
            get { return _endtSafeZoon; }
            set { _endtSafeZoon = Convert.ToDouble(value); OnPropertyChagned(); }
        }
        public double Score
        {
            get { return _score; }
            set { _score = Convert.ToDouble(value); OnPropertyChagned(); }
        }
        public int GaugeRadius
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
                if (_intruderItem != value)
                {
                    _intruderItem = value;
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
            AirPortHeadingAngle = 12;
            StartSafeZoon = -1.5;
            EndSafeZoon = 1.0;
            StartWarningZoon = -5;
            EndWarningZoon = -2;
            TCasDisplayRange = 1;
            TcasDisplayRangeValue = TcasDisplayRange.TcasDisplayRangeNone;

            StartTcasItemsRandom = new ParameterRelayCommand(btn => StartTcasItemsRandomCommand(btn));
            GaugeRadius = 140;
            Messenger.Register<IntruderModel>(nameof(MainViewModel), OnIntruderModelMessageReceived);
            Score = 0;
            IntruderItems = new ObservableCollection<IntruderModel>();
            
            //for (int i = 0; i < 30; i ++)
            //{
            //    var random = new Random();
            //    Array values2 = Enum.GetValues(typeof(IntruderVerticalSenseState));
            //    var randomIntruderVerticalSenseState = (IntruderVerticalSenseState)values2.GetValue(random.Next(values2.Length));
            //    Array values3 = Enum.GetValues(typeof(DisplayMatrix));
            //    var randomDisplayMatrix = (DisplayMatrix)values3.GetValue(random.Next(values3.Length));

            //    OnIntruderModelMessageReceived(IntruderModel.IntruderModelFactory(i+1, 120, random.Next(-1200,1200), randomIntruderVerticalSenseState, TcasSymbol.ProximateTraffic, randomDisplayMatrix, 360/30 * (i+1)));
            //}
          
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
        #endregion


    }
    

}

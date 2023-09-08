
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

        #region fildVariable
        private double _airPortHeadingAngle;
        private double _startSafeZoon;
        private double _endtSafeZoon;
        private double _startWarningZoon;
        private double _score;
        private int _gaugeRadius;
        private double _endWarningZoon;
        private TcasDisplayRange _tcasDisplayRange;
        private ICommand _startTcasItemsRandom;
        #endregion
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

        public ICommand StartTcasItemsRandom
        {
            get { return _startTcasItemsRandom; }
            private set { _startTcasItemsRandom = value; }
        }

        public ObservableCollection<IntruderModel> IntruderItems { get; set; }
        private IntruderModel _intruderItem;
        public IntruderModel IntruderItem 
        { 
            get => _intruderItem;
            set
            {
                if( _intruderItem != value)
                {
                    _intruderItem = value;
                    Messenger.Send("TcasSettingViewModel", IntruderItem);
                }
            } 
        }
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
            Messenger.Register<IntruderModel>("MainViewModel", OnIntruderModelMessageReceived);
            Score = 0;
            IntruderItems = new ObservableCollection<IntruderModel>();

            OnIntruderModelMessageReceived(IntruderModel.IntruderModelFactory(1, 120, -100, IntruderVerticalSenseState.Climbing, TcasSymbol.ProximateTraffic, DisplayMatrix.NoThreat, 10));
            OnIntruderModelMessageReceived(IntruderModel.IntruderModelFactory(3,120, 45, IntruderVerticalSenseState.Climbing, TcasSymbol.ProximateTraffic, DisplayMatrix.NoThreat, 50));
            OnIntruderModelMessageReceived(IntruderModel.IntruderModelFactory(5, 120, 50, IntruderVerticalSenseState.Climbing, TcasSymbol.ProximateTraffic, DisplayMatrix.NoThreat, 120));
            OnIntruderModelMessageReceived(IntruderModel.IntruderModelFactory(6, 120, -150, IntruderVerticalSenseState.Climbing, TcasSymbol.ProximateTraffic, DisplayMatrix.NoThreat, 170));
            OnIntruderModelMessageReceived(IntruderModel.IntruderModelFactory(1, 120, -100, IntruderVerticalSenseState.Climbing, TcasSymbol.ProximateTraffic, DisplayMatrix.NoThreat,180));

            OnIntruderModelMessageReceived(IntruderModel.IntruderModelFactory(3, 120, 45, IntruderVerticalSenseState.Climbing, TcasSymbol.ProximateTraffic, DisplayMatrix.NoThreat, -50));
            OnIntruderModelMessageReceived(IntruderModel.IntruderModelFactory(5, 120, 50, IntruderVerticalSenseState.Descending, TcasSymbol.ProximateTraffic, DisplayMatrix.NoThreat, -100));
            OnIntruderModelMessageReceived(IntruderModel.IntruderModelFactory(6, 120, -150, IntruderVerticalSenseState.NoData, TcasSymbol.ProximateTraffic, DisplayMatrix.NoThreat, -150));

        }

        private void OnIntruderModelMessageReceived(IntruderModel model)
        {
            if(IntruderItems.Count > 30)
            {
                string message = "더 이상 불가";
                var result = MessageBox.Show(message);
                return;
            }

            if (IntruderItems.Contains(model))
            {
                var item = IntruderItems.Where(k => k.Equals(model)).FirstOrDefault();                
                //var angle = model.Bearing - 90;
                //var radianAngle = (angle * Math.PI) / 180;
                //double x = (model.Range * Math.Cos(radianAngle) * 1);
                //double y = (model.Range * Math.Sin(radianAngle) * 1);
                //item.Altitude = y;
                //item.Range = x;
                item.IntruderVerticalMoveMentState = model.IntruderVerticalMoveMentState;
                item.IntruderType = model.IntruderType;
                item.Bearing = model.Bearing;
                item.Altitude = model.Altitude;
                item.Range = model.Range;
             
            }
            else
            {
                model.Number = IntruderItems.Count + 1;
                //var angle = model.Bearing - 90;
                //var radianAngle = (angle * Math.PI) / 180;
                //double x = (model.Range * Math.Cos(radianAngle) * 1);
                //double y = (model.Range * Math.Sin(radianAngle) * 1);
                //model.Altitude = y;
                //model.Range = x;                
                IntruderItems.Add(model);
            }

            //IntruderItems.Add(IntruderModel.IntruderModelFactory(IntruderItems.Count + 1, x, y,
            //    model.IntruderVerticalMoveMentState, model.IntruderType, DisplayMatrix.NoThreat, model.Bearing));
        }
        //private void CalculateIntruderPoint(IntruderModel model)
        //{            
        //    var angle = model.Bearing - 90;
        //    var radianAngle = (angle * Math.PI) / 180;
        //    if (model.Number == 1)
        //    {
        //        model.Number = 1;
        //    }
        //    double x = (model.Range * Math.Cos(radianAngle) * 1);
        //    double y = (model.Range * Math.Sin(radianAngle) * 1);
        //    model.Altitude = y;
        //    model.Range = x;
          
        //}
        private void StartTcasItemsRandomCommand(object _)
        {
            Random random = new Random();
            foreach (var item in IntruderItems)
            {
                Array values1 = Enum.GetValues(typeof(TcasSymbol));                
                TcasSymbol randomTcasSymbol = (TcasSymbol)values1.GetValue(random.Next(values1.Length));

                Array values2 = Enum.GetValues(typeof(IntruderVerticalSenseState));
                var randomIntruderVerticalSenseState = (IntruderVerticalSenseState)values2.GetValue(random.Next(values2.Length));

                item.IntruderType = randomTcasSymbol;
                item.IntruderVerticalMoveMentState = randomIntruderVerticalSenseState;
                item.Bearing = random.Next(0,360);
                item.Altitude = random.Next(-127, 127);
                item.Range = random.Next(0,120);                

            }
            //if (btn is string value)
            //{
            //    AirPortHeadingAngle = Convert.ToDouble(value);
            //}
            //foreach (IntruderModel model in IntruderItems)
            //{
            //    if (model.Number == 3)
            //    {
            //        model.IntruderVerticalMoveMentState = IntruderVerticalSenseState.NoVerticalRate;
            //    }
            //}
            //Messenger.Send("TcasSettingViewModel", IntruderItems);
        }

    }
    

}

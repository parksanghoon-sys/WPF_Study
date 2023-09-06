
using CirclularGage.Location.Local.Enums;
using CirclularGage.Main.Model;
using System;
using System.Collections.ObjectModel;
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
        private ICommand _sendHeading; 
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
        private TcasDisplayRange _tcasDisplayRange;

        public TcasDisplayRange TcasDisplayRangeValue
        {
            get { return _tcasDisplayRange; }
            set 
            { 
                if(_tcasDisplayRange != value)
                {
                    _tcasDisplayRange = value;
                    switch(value)
                    {
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

        public ICommand SendHeading
        {
            get { return _sendHeading; }
            private set { _sendHeading = value; }
        }

        public ObservableCollection<IntruderModel> IntruderItems { get; set; }
        public IntruderModel IntruderItem { get; set; }
        public MainViewModel()
        {
            AirPortHeadingAngle = 12;
            StartSafeZoon = -1.5;
            EndSafeZoon = 1.0;
            StartWarningZoon = -5;
            EndWarningZoon = -2;
            TCasDisplayRange = 1;
            TcasDisplayRangeValue = TcasDisplayRange.TcasDisplayRange40nm;

            SendHeading = new ParameterRelayCommand(btn => SendHeadingCommand(btn));
            GaugeRadius = 140;
            Messenger.Register<IntruderModel>(this, OnIntruderModelMessageReceived);
            Score = 0;
            IntruderItems = new ObservableCollection<IntruderModel>();

            OnIntruderModelMessageReceived(IntruderModel.IntruderModelFactory(1, 50, -100, IntruderVerticalSenseState.Climbing, TcasSymbol.ProximateTraffic, DisplayMatrix.NoThreat, 10));
            OnIntruderModelMessageReceived(IntruderModel.IntruderModelFactory(3, 50, 45, IntruderVerticalSenseState.Climbing, TcasSymbol.ProximateTraffic, DisplayMatrix.NoThreat, 50));
            OnIntruderModelMessageReceived(IntruderModel.IntruderModelFactory(5, 50, 50, IntruderVerticalSenseState.Climbing, TcasSymbol.ProximateTraffic, DisplayMatrix.NoThreat, 120));
            OnIntruderModelMessageReceived(IntruderModel.IntruderModelFactory(6, 50, -150, IntruderVerticalSenseState.Climbing, TcasSymbol.ProximateTraffic, DisplayMatrix.NoThreat, 170));
            OnIntruderModelMessageReceived(IntruderModel.IntruderModelFactory(1, 50, -100, IntruderVerticalSenseState.Climbing, TcasSymbol.ProximateTraffic, DisplayMatrix.NoThreat));

            OnIntruderModelMessageReceived(IntruderModel.IntruderModelFactory(3, 50, 45, IntruderVerticalSenseState.Climbing, TcasSymbol.ProximateTraffic, DisplayMatrix.NoThreat, -50));
            OnIntruderModelMessageReceived(IntruderModel.IntruderModelFactory(5, 50, 50, IntruderVerticalSenseState.Descending, TcasSymbol.ProximateTraffic, DisplayMatrix.NoThreat, -100));
            OnIntruderModelMessageReceived(IntruderModel.IntruderModelFactory(6, 50, -150, IntruderVerticalSenseState.NoData, TcasSymbol.ProximateTraffic, DisplayMatrix.NoThreat, -150));

            //OnIntruderModelMessageReceived(IntruderModel.IntruderModelFactory(1, 100, 100, IntruderVerticalSenseState.Climbing, TcasSymbol.ProximateTraffic, DisplayMatrix.NoThreat,0));
        }

        private void OnIntruderModelMessageReceived(IntruderModel model)
        {
            var angle = model.Bearing - 90;
            var radianAngle = (angle * Math.PI) / 180;
            double x = (model.Range * Math.Cos(radianAngle) * 1);
            double y = (model.Range * Math.Sin(radianAngle) * 1);
            //if (Math.Abs(x) < GaugeRadius)
            //    return;
            IntruderItems.Add(IntruderModel.IntruderModelFactory(IntruderItems.Count + 1, x, y,
                model.IntruderVerticalMoveMentState, model.IntruderType, DisplayMatrix.NoThreat,model.Bearing));
        }

        private void SendHeadingCommand(object btn)
        {
            if (btn is string value)
            {
                AirPortHeadingAngle = Convert.ToDouble(value);
            }
            foreach (IntruderModel model in IntruderItems)
            {
                if (model.Number == 3)
                {
                    model.IntruderVerticalMoveMentState = IntruderVerticalSenseState.NoVerticalRate;
                }
            }
            Messenger.Send(IntruderItems);
        }

    }
    

}

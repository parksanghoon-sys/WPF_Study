
using CirclularGage.Location.Local.Enums;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CirclularGage.Main
{
    internal class MainViewModel : ViewModelBase
    {

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
        private int _gaugeRadius;

        public int GaugeRadius
        {
            get { return _gaugeRadius; }
            set { _gaugeRadius = value; OnPropertyChagned(); }
        }

        private ICommand _sendHeading;
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
            EndSafeZon = 0;
            StartSafeZon = 0;
            SendHeading = new ParameterRelayCommand(btn => SendHeadingCommand(btn));
            GaugeRadius = 190;

            IntruderItems = new ObservableCollection<IntruderModel>();
            var angle = 10;
            var radianAngle = (angle * Math.PI) / 180;
            double x = (GaugeRadius * Math.Cos(radianAngle) * 1);
            double y = (GaugeRadius * Math.Sin(radianAngle) * 1);

            //IntruderItems.Add(IntruderModel.IntruderModelFactory(1, 15, -100, IntruderVerticalSenseState.NoVerticalRate, TcasSymbol.OtherTraffic, DisplayMatrix.NoThreat));
            //IntruderItems.Add(IntruderModel.IntruderModelFactory(3, -50, 45, IntruderVerticalSenseState.Climbing, TcasSymbol.ProximateTraffic, DisplayMatrix.NoThreat));
            //IntruderItems.Add(IntruderModel.IntruderModelFactory(5, 55, 50, IntruderVerticalSenseState.Descending, TcasSymbol.TrafficAdvisory, DisplayMatrix.NoThreat));
            IntruderItems.Add(IntruderModel.IntruderModelFactory(6, x, y, IntruderVerticalSenseState.NoData, TcasSymbol.ResolutionAdvisorty, DisplayMatrix.NoThreat));
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
        }

    }
    public class IntruderModel : ViewModelBase
    {
        public double X => Range;
        public double Y => Altitude;
        /// <summary>
        /// Intruder Index ? 
        /// </summary>
        public int Number { get; set; }
        /// <summary>
        /// 수평거리
        /// </summary>
        public double Range { get; set; }
        /// <summary>
        /// 상대 고도
        /// </summary>
        public double Altitude { get; set; }
        /// <summary>
        /// Intruder 수직 상태 이동
        /// </summary>
        private IntruderVerticalSenseState _intruderVerticalMoveMentState;

        public IntruderVerticalSenseState IntruderVerticalMoveMentState
        {
            get { return _intruderVerticalMoveMentState; }
            set { _intruderVerticalMoveMentState = value; OnPropertyChagned(); }
        }
        /// <summary>
        /// 항고기 Heading 기준 Intruder 위치 각도??
        /// </summary>
        private double _bearing;

        public double Bearing
        {
            get { return _bearing; }
            set { _bearing = value; OnPropertyChagned(); }
        }

        /// <summary>
        /// Intruder 종류
        /// </summary>
        public TcasSymbol IntruderType { get; set; }
        /// <summary>
        /// Intruder 무전 타입?
        /// </summary>
        public DisplayMatrix IntruderDisplay { get; set; }
        
        
        public static IntruderModel IntruderModelFactory(int index, double rnagne, double altitude, IntruderVerticalSenseState verticalState,
            TcasSymbol symbol, DisplayMatrix matrix, double bearing = 0)
            => new IntruderModel()
            {
                Number = index,
                Range = rnagne,
                Altitude = altitude,
                IntruderVerticalMoveMentState = verticalState,
                IntruderType = symbol,
                IntruderDisplay = matrix,
                Bearing = bearing
            };
    }

}

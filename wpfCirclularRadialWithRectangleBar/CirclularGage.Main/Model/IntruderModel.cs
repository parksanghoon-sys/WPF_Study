using CirclularGage.Location.Local.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CirclularGage.Main.Model
{
    public class IntruderModel : ViewModelBase
    {
        private readonly double _centerAdjustmentYAxis = 40.84507042;
        //private readonly double _centerAdjustmentYAxis = 0;
        private readonly double _airportSymbolSize = 30;
        private readonly double _bearingNotValidXAdjustmentXAxis = -19;

        private double _intruderSymbolCenterAdjustX;
        private double _intruderSymbolCenterAdjustY;

        private double _bearing;
        private double _range;
        private double _altritude;
        private IntruderVerticalSenseState _intruderVerticalMoveMentState;
        //private TcasSymbol _intruderType;
        private DisplayMatrix _intruderDisplay;
        private TcasDisplayRange _tcasDisplayRange;
        /// <summary>
        /// ItemModel의 X축 위치
        /// </summary>
        public double X => CalculateIntruderXPoint();
        /// <summary>
        /// ItemModel의 Y축 위치
        /// </summary>
        public double Y => CalculateIntruderYPoint();
        /// <summary>
        /// Intruder 종류
        /// </summary>
        public TcasIntruderSymbol IntruderType => CalculateIntruderType();
        //{
        //    get { return _intruderType; }
        //    set { _intruderType = value; OnPropertyChagned(); }
        //}
        #region Properties

        /// <summary>
        /// Intruder Index 
        /// </summary>
        public int Number { get; set; }
        /// <summary>
        /// 수평거리
        /// </summary>
        public double Range
        {
            get { return _range; }
            set
            {
                if (_range != value)
                {
                    _range = value;                    
                    OnPropertyChagned();
                    MovePointIntruder();              
                }
            }
        }
        /// <summary>
        /// 상대 고도
        /// </summary>
        public double Altitude
        {
            get { return _altritude; }
            set 
            { 
                _altritude = value;
                if(_altritude < 0)
                {
                    _intruderSymbolCenterAdjustY = 6;
                }
                else
                {
                    _intruderSymbolCenterAdjustY = -6;
                }
                OnPropertyChagned();
                MovePointIntruder();
            }
        }
        /// <summary>
        /// Intruder 수직 상태 이동
        /// </summary>
        public IntruderVerticalSenseState IntruderVerticalMoveMentState
        {
            get { return _intruderVerticalMoveMentState; }
            set
            {
                _intruderVerticalMoveMentState = value;
                if (_intruderVerticalMoveMentState == IntruderVerticalSenseState.Climbing 
                    || _intruderVerticalMoveMentState == IntruderVerticalSenseState.Descending)
                {
                    _intruderSymbolCenterAdjustX = 5;
                }
                else
                {
                    _intruderSymbolCenterAdjustX = 0;
                }
                OnPropertyChagned();
                MovePointIntruder();
            }
        }
        /// <summary>
        /// 항고기 Heading 기준 Intruder 위치 각도??
        /// </summary>
        public double Bearing
        {
            get { return _bearing; }
            set
            {
                if (_bearing != value)
                {
                    _bearing = value;                    
                    OnPropertyChagned();
                    MovePointIntruder();
                }
            }
        }
        
        /// <summary>
        /// Intruder 무전 타입?
        /// </summary>
        public DisplayMatrix IntruderDisplay
        {
            get => _intruderDisplay;
            set
            {
                if (_intruderDisplay != value)
                {
                    _intruderDisplay = value;
                    OnPropertyChagned(nameof(IntruderType));
                    OnPropertyChagned();
                }
            }
        }
        public TcasDisplayRange TcasDisplayRange
        {
            get => _tcasDisplayRange;
            set
            {
                _tcasDisplayRange = value;
                MovePointIntruder();
            }
        }      
        private bool _isSelected;

        public bool IsSelected
        {
            get { return _isSelected; }
            set { _isSelected = value; OnPropertyChagned(); }
        }

        #endregion

        #region constructor

        public static IntruderModel IntruderModelFactory(int index, double rnagne, double altitude, IntruderVerticalSenseState verticalState,
         TcasIntruderSymbol symbol, DisplayMatrix matrix, TcasDisplayRange tcasDisplayRange, double bearing = 0)
         => new IntruderModel()
         {
             Bearing = bearing,
             Number = index,
             Range = rnagne,
             Altitude = altitude,
             IntruderVerticalMoveMentState = verticalState,             
             IntruderDisplay = matrix,
             TcasDisplayRange = tcasDisplayRange
         };
        #endregion
        #region Methods
        private double CalculateIntruderXPoint()
        {
            var angle = Bearing - 90;
            var radianAngle = (angle * Math.PI) / 180;
            var xPoint = (Range * Math.Cos(radianAngle) * TcasRangeDisplayRatitoX(TcasDisplayRange)) + _intruderSymbolCenterAdjustX;
            return xPoint;
        }
        private double CalculateIntruderYPoint()
        {
            var angle = Bearing - 90;
            var radianAngle = (angle * Math.PI) / 180;
            var yPoint = (_centerAdjustmentYAxis + (Range  * Math.Sin(radianAngle)) * TcasRangeDisplayRatitoY(TcasDisplayRange)) + _intruderSymbolCenterAdjustY;
            return yPoint;
        }

        private double TcasRangeDisplayRatitoY(TcasDisplayRange tcasDisplayRange)
        {
            double ratito;
            switch (tcasDisplayRange)
            {
                case TcasDisplayRange.Rate5:
                    ratito = 27.04225352;
                    break;
                case TcasDisplayRange.Rate10:
                    ratito = 13.52112676;
                    break;
                case TcasDisplayRange.Rate20:
                    ratito = 6.76056338;
                    break;
                case TcasDisplayRange.Rate40:
                    ratito = 3.38028169;
                    break;
                default:
                    ratito = 1;
                    break;
            }
            return ratito;
        }
        private double TcasRangeDisplayRatitoX(TcasDisplayRange tcasDisplayRange)
        {
            double ratito;
            switch (tcasDisplayRange)
            {
                case TcasDisplayRange.Rate5:
                    ratito = 25.03626185;
                    break;
                case TcasDisplayRange.Rate10:
                    ratito = 13.71428571;
                    break;
                case TcasDisplayRange.Rate20:
                    ratito = 6.857142857;
                    break;
                case TcasDisplayRange.Rate40:
                    ratito = 3.428571429;
                    break;
                default:
                    ratito = 1;
                    break;
            }
            return ratito;
        }

        private TcasIntruderSymbol CalculateIntruderType()
        {
            if (IntruderDisplay == DisplayMatrix.RA)
                return TcasIntruderSymbol.ResolutionAdvisorty;

            if (IntruderDisplay == DisplayMatrix.TA)
                return TcasIntruderSymbol.TrafficAdvisory;

            if (IntruderDisplay == DisplayMatrix.NoThreat)
                return TcasIntruderSymbol.OtherTraffic;

            if (IntruderDisplay == DisplayMatrix.ProximateTime)
                return TcasIntruderSymbol.ProximateTraffic;

            return TcasIntruderSymbol.OtherTraffic;
        }
        private void MovePointIntruder()
        {
            OnPropertyChagned(nameof(X));
            OnPropertyChagned(nameof(Y));
        }
        private void ChangeIntruderType()
        {
            OnPropertyChagned(nameof(IntruderType));
        }
        #endregion

        #region Override
        public override bool Equals(object obj)
        {
            if (obj is IntruderModel moxel)
            {
                return moxel.Number == this.Number;
            }
            return false;

        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        } 
        #endregion
     
    }
}

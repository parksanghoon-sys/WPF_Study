using CirclularGage.Location.Local.Enums;
using CirclularGage.Main.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CirclularGage.Main.Model
{
    public class IntruderModel : ViewModelBase
    {

        private IntruderModelMonitoring _intruderModelMonitoring;

        private double _bearing;
        private double _range;
        private double _altritude;
        private IntruderVerticalSenseState _intruderVerticalMoveMentState;
        //private TcasSymbol _intruderType;
        private DisplayMatrix _intruderDisplay;
        private TcasDisplayRange _tcasDisplayRange;
        #region Properties
        /// <summary>
        /// Intruder 종류
        /// </summary>
        public TcasIntruderSymbol IntruderType => CalculateIntruderType();
        public IntruderModelMonitoring IntruderModelMonitoring
        {
            get { return _intruderModelMonitoring; }          
        }
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
                    IntruderModelMonitoring._intruderSymbolCenterAdjustY = 6;
                }
                else
                {
                    IntruderModelMonitoring._intruderSymbolCenterAdjustY = -6;
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
                    IntruderModelMonitoring._intruderSymbolCenterAdjustX = 7;
                }
                else
                {
                    IntruderModelMonitoring._intruderSymbolCenterAdjustX = 0;
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
        public IntruderModel()
        {
            _intruderModelMonitoring = new IntruderModelMonitoring(this);
        }
        #region constructor

        public static IntruderModel IntruderModelFactory(int index, double rnagne, double altitude, IntruderVerticalSenseState verticalState,
         TcasIntruderSymbol symbol, DisplayMatrix matrix, TcasDisplayRange tcasDisplayRange, double bearing = 0)
         => new IntruderModel()
         {
             Bearing = CalculateBearing(bearing),
             Number = index,
             Range = rnagne,
             Altitude = (altitude),
             IntruderVerticalMoveMentState = verticalState,             
             IntruderDisplay = matrix,
             TcasDisplayRange = tcasDisplayRange
         };
        #endregion
        #region Methods
        private static double CalculateBearing(double bearing)
        {
            double result = 0.0;
            if(bearing < 0)
            {
                result = (bearing + 180) * -1;
            }else
            {
                result = bearing;
            }
            return result;
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
            //OnPropertyChagned(nameof(X));
            //OnPropertyChagned(nameof(Y));
            OnPropertyChagned(nameof(IntruderModelMonitoring));

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

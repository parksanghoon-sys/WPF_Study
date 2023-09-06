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
        public double X => Range;

        public double Y => Altitude;
        /// <summary>
        /// Intruder Index ? 
        /// </summary>
        public int Number { get; set; }
        /// <summary>
        /// 수평거리
        /// </summary>
        private double _range;

        public double Range
        {
            get { return _range; }
            set { _range = value; OnPropertyChagned("X"); }
        }
        /// <summary>
        /// 상대 고도
        /// </summary>
        private double _altritude;

        public double Altitude
        {
            get { return _altritude; }
            set { _altritude = value; OnPropertyChagned(nameof(Y)); }
        }

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
        private TcasSymbol _intruderType;

        public TcasSymbol IntruderType
        {
            get { return _intruderType; }
            set { _intruderType = value; OnPropertyChagned(); }
        }

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

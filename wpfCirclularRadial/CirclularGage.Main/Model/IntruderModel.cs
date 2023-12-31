﻿using CirclularGage.Location.Local.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CirclularGage.Main.Model
{
    public class IntruderModel : ViewModelBase
    {
        private readonly int _centerAdjustmentYAxis = 50;
        private double _bearing;
        private double _range;
        private double _altritude;
        private IntruderVerticalSenseState _intruderVerticalMoveMentState;
        //private TcasSymbol _intruderType;
        private DisplayMatrix _intruderDisplay;
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
        public TcasSymbol IntruderType => CalculateIntruderType();
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
                    ChangeIntruderType();
                }
            }
        }
        /// <summary>
        /// 상대 고도
        /// </summary>
        public double Altitude
        {
            get { return _altritude; }
            set { _altritude = value; OnPropertyChagned(); ChangeIntruderType(); }
        }
        /// <summary>
        /// Intruder 수직 상태 이동
        /// </summary>
        public IntruderVerticalSenseState IntruderVerticalMoveMentState
        {
            get { return _intruderVerticalMoveMentState; }
            set { _intruderVerticalMoveMentState = value; OnPropertyChagned(); }
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
        private double _displayRangeRatio = 1;

        public double DisplayRangeRatio
        {   
            get { return _displayRangeRatio; }
            set 
            { 
                if(_displayRangeRatio != value)
                {
                    _displayRangeRatio = value;

                    OnPropertyChagned();
                    MovePointIntruder();
                }
                    
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
         TcasSymbol symbol, DisplayMatrix matrix, double bearing = 0)
         => new IntruderModel()
         {
             Bearing = bearing,
             Number = index,
             Range = rnagne,
             Altitude = altitude,
             IntruderVerticalMoveMentState = verticalState,             
             IntruderDisplay = matrix

         };
        #endregion
        #region Methods
        private double CalculateIntruderXPoint()
        {
            var angle = Bearing - 90;
            var radianAngle = (angle * Math.PI) / 180;
            return (Range * Math.Cos(radianAngle) * DisplayRangeRatio);            
        }
        private double CalculateIntruderYPoint()
        {
            var angle = Bearing - 90;
            var radianAngle = (angle * Math.PI) / 180;
            return (_centerAdjustmentYAxis + Range  * Math.Sin(radianAngle) * DisplayRangeRatio);
        }
        private TcasSymbol CalculateIntruderType()
        {
            if (IntruderDisplay == DisplayMatrix.RA)
                return TcasSymbol.ResolutionAdvisorty;

            if (IntruderDisplay == DisplayMatrix.TA)
                return TcasSymbol.TrafficAdvisory;

            if ((Range >= 60) && (50 >= Math.Abs(Altitude)))
                return TcasSymbol.OtherTraffic;

            if ((Range < 60) && (50 < Math.Abs(Altitude)))
                return TcasSymbol.ProximateTraffic;

            return TcasSymbol.OtherTraffic;
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

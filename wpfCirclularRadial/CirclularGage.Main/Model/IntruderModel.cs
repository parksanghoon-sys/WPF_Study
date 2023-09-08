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
        #region Properties
        private double _bearing;
        private double _range;
        private double _altritude;
        private IntruderVerticalSenseState _intruderVerticalMoveMentState;
        private TcasSymbol _intruderType;

        public double X => CalculateIntruderXPoint();
        public double Y => CalculateIntruderYPoint();
        /// <summary>
        /// Intruder Index ? 
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
                    OnPropertyChagned(nameof(X));
                    OnPropertyChagned(nameof(Y));
                    OnPropertyChagned();
                }
            }
        }
        /// <summary>
        /// 상대 고도
        /// </summary>
        public double Altitude
        {
            get { return _altritude; }
            set { _altritude = value; OnPropertyChagned(); }
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
                    OnPropertyChagned(nameof(X));
                    OnPropertyChagned(nameof(Y));
                    OnPropertyChagned();
                }
            }
        }
        /// <summary>
        /// Intruder 종류
        /// </summary>
        public TcasSymbol IntruderType
        {
            get { return _intruderType; }
            set { _intruderType = value; OnPropertyChagned(); }
        }
        /// <summary>
        /// Intruder 무전 타입?
        /// </summary>
        public DisplayMatrix IntruderDisplay { get; set; }
        #endregion

        #region constructor
        public IntruderModel()
        {

        }
        public static IntruderModel IntruderModelFactory(int index, double rnagne, double altitude, IntruderVerticalSenseState verticalState,
         TcasSymbol symbol, DisplayMatrix matrix, double bearing = 0)
         => new IntruderModel()
         {
             Bearing = bearing,
             Number = index,
             Range = rnagne,
             Altitude = altitude,
             IntruderVerticalMoveMentState = verticalState,
             IntruderType = symbol,
             IntruderDisplay = matrix

         };
        #endregion
        #region Methods
        private double CalculateIntruderXPoint()
        {
            var angle = Bearing - 90;
            var radianAngle = (angle * Math.PI) / 180;
            var x = (Range * Math.Cos(radianAngle) * 1);
            return x;
        }
        private double CalculateIntruderYPoint()
        {
            var angle = Bearing - 90;
            var radianAngle = (angle * Math.PI) / 180;
            var y = (Range * Math.Sin(radianAngle) * 1);
            return y;
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

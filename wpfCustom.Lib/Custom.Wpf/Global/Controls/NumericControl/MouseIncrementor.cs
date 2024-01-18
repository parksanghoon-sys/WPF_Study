using System;
using System.Windows;

namespace Custom.Wpf.Global.Controls.NumericControl
{
    public enum MouseDirections
    {
        /// <summary>
        /// A mouse movement was not detected.
        /// </summary>
        None,

        /// <summary>
        /// Mouse was moved horizontally rather than vertically.
        /// </summary>
        LeftRight,

        /// <summary>
        /// Mouse was moved vertically rather than horizontally.
        /// </summary>
        UpDown
    }
    public class MouseIncrementor
    {
        #region fields
        private MouseDirections _enumMouseDirection = MouseDirections.None;
        private Point _objPoint;

        private readonly Point _initialPoint;
        #endregion fields

        #region Ctors
        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="objPoint"></param>
        /// <param name="enumMouseDirection"></param>
        public MouseIncrementor(Point objPoint, MouseDirections enumMouseDirection)
        {
            _objPoint = objPoint;
            _initialPoint = _objPoint;
            _enumMouseDirection = enumMouseDirection;
        }
        #endregion Ctors

        #region properties
        /// <summary>
        /// Gets/sets the direction in which a mouse was seen to be moved
        /// when comparing 2 points.
        /// </summary>
        public MouseDirections MouseDirection
        {
            get
            {
                return _enumMouseDirection;
            }

            protected set
            {
                _enumMouseDirection = value;
            }
        }

        public Point InitialPoint
        {
            get
            {
                return _initialPoint;
            }
        }

        public Point Point
        {
            get
            {
                return _objPoint;
            }

            set
            {
                _objPoint = value;
            }
        }

        internal MouseDirections SetMouseDirection(Point pos)
        {
            double deltaX = this.Point.X - pos.X;
            double deltaY = this.Point.Y - pos.Y;

            if (Math.Abs(deltaX) > Math.Abs(deltaY))
                MouseDirection = MouseDirections.LeftRight;
            else
            {
                if (Math.Abs(deltaX) < Math.Abs(deltaY))
                    MouseDirection = MouseDirections.UpDown;
            }

            return MouseDirection;
        }
        #endregion properties
    }
}

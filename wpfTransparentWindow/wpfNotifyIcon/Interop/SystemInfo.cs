using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;

namespace wpfNotifyIcon.Interop
{
    public static class SystemInfo
    {
        /// <summary>
        /// Returns the DPI X Factor
        /// </summary>
        public static double DpiFactorX { get; private set; } = 1;

        /// <summary>
        /// Returns the DPI Y Factor
        /// </summary>
        public static double DpiFactorY { get; private set; } = 1;
        static SystemInfo()
        {
            UpdateDpiFactors();
        }

        internal static void UpdateDpiFactors()
        {
            using (var source = new HwndSource(new HwndSourceParameters()))
            {
                if(source.CompositionTarget?.TransformToDevice.M11 != null)
                {
                    DpiFactorX = source.CompositionTarget.TransformToDevice.M11;
                    DpiFactorY = source.CompositionTarget.TransformToDevice.M22;
                    return;
                }
            }
            DpiFactorX = DpiFactorY = 1;
        }
        /// <summary>
        /// Scale the supplied point to the current DPI settings
        /// </summary>
        /// <param name="point"></param>
        /// <returns>Point</returns>
        [Pure]
        public static Point ScaleWithDpi(this Point point)
        {
            return new Point
            {
                X = (int)(point.X / DpiFactorX),
                Y = (int)(point.Y / DpiFactorY)
            };
        }
        /// <summary>
        /// Scale the supplied size to the current DPI settings
        /// </summary>
        /// <param name="size"></param>
        /// <returns>Size</returns>
        [Pure]
        public static Size ScaleWithDpi(this Size size)
        {
            return new Size
            {
                Height = (int)(size.Height / DpiFactorY),
                Width = (int)(size.Width / DpiFactorX)
            };
        }
        private static Size? _smallIconSize = null;

        private const int CXSMICON = 49;
        private const int CYSMICON = 50;
        public static Size SmallIconSize
        {
            get
            {
                if (!_smallIconSize.HasValue)
                {
                    Size smallIconSize = new Size
                    {
                        Height = WinApi.GetSystemMetrics(CYSMICON),
                        Width = WinApi.GetSystemMetrics(CXSMICON)
                    };
                    _smallIconSize = smallIconSize.ScaleWithDpi();
                }

                return _smallIconSize.Value;
            }
        }
    }
}

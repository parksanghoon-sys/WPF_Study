using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace wpfLockButton
{
    internal class StSafetyButton : ContentControl
    {
        private Timer safetyTimer;
        public bool LockStatus
        {
            get { return (bool)GetValue(LockStatusProperty); }
            set { SetValue(LockStatusProperty, value); }
        }


        public int UnLockInterval
        {
            get { return (int)GetValue(UnLockIntervalProperty); }
            set { SetValue(UnLockIntervalProperty, value); }
        }

        // Using a DependencyProperty as the backing store for UnLockInterval.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UnLockIntervalProperty =
            DependencyProperty.Register("UnLockInterval", typeof(int), typeof(StSafetyButton), new PropertyMetadata(6000));


        // Using a DependencyProperty as the backing store for LockStatus.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LockStatusProperty =
            DependencyProperty.Register("LockStatus", typeof(bool), typeof(StSafetyButton), new PropertyMetadata(true));
        public StSafetyButton()
        {
            base.PreviewMouseLeftButtonDown += StSafetyButton_PreviewMouseLeftButtonDown;
        }

        private void StSafetyButton_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(!(e.OriginalSource is Image))
                return;
            var rectangle = e.OriginalSource as Image;
            if (!(rectangle.Name == "imgLock"))
            {
                return;
            }
            if (LockStatus)
            {
                rectangle.Source = new BitmapImage(new Uri("/wpfLockButton;component/Lock/btn_emergency_light_grey.png", UriKind.Relative));
                LockStatus = false;
                safetyTimer = new Timer(delegate
                {
                    base.Dispatcher.BeginInvoke((Action)delegate
                    {
                        if (!LockStatus)
                        {
                            LockStatus = true;
                            rectangle.Source = new BitmapImage(new Uri("/wpfLockButton;component/Lock/btn_emergency_red.png", UriKind.Relative));
                            safetyTimer.Dispose();
                        }
                    });
                }, null, UnLockInterval, 0);
            }
            else
            {
                rectangle.Source = new BitmapImage(new Uri("/wpfLockButton;component/Lock/btn_emergency_red.png", UriKind.Relative));
                LockStatus = true;
            }
        }
    }
}

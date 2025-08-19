using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace wpfCleanArchtecture.UI
{
    public class AnimatedContentControl : ContentControl
    {
        static AnimatedContentControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AnimatedContentControl),
                new FrameworkPropertyMetadata(typeof(AnimatedContentControl)));
        }

        public static readonly DependencyProperty AnimationStateProperty =
            DependencyProperty.Register(nameof(AnimationState), typeof(string),
                typeof(AnimatedContentControl), new PropertyMetadata("Normal", OnAnimationStateChanged));

        public string AnimationState
        {
            get => (string)GetValue(AnimationStateProperty);
            set => SetValue(AnimationStateProperty, value);
        }

        private static void OnAnimationStateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is AnimatedContentControl control)
            {
                VisualStateManager.GoToState(control, (string)e.NewValue, true);
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            VisualStateManager.GoToState(this, AnimationState, false);
        }
    }
}

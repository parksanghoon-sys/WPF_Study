using System;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media.Animation;

namespace wpfControls.Utilites
{
    public static class AnimationUtils
    {
        public static Storyboard CreateLeftPropertyStoryboard(this FrameworkElement frameworkElement, double from, double to, double ms)
        {
            Storyboard storyboard = new Storyboard();
            DoubleAnimation animation = new DoubleAnimation();
            animation.From = from;
            animation.To = to;
            animation.Duration = new Duration(TimeSpan.FromMilliseconds(ms));
            storyboard.Children.Add(animation);

            Storyboard.SetTarget(animation, frameworkElement);
            Storyboard.SetTargetProperty(animation, new PropertyPath(Canvas.LeftProperty));
            return storyboard;            
        }
    }
}

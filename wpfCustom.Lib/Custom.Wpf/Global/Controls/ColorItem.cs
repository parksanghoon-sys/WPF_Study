using System.Windows;
using System.Windows.Media.Animation;

namespace Custom.Wpf.Global.Controls
{
    public class ColorItem : ColorAnimation
    {
        public static readonly DependencyProperty TargetNameProperty = DependencyProperty.Register("TargetName", typeof(string), typeof(ColorItem), new PropertyMetadata(null, OnTargetNameChanged));

        public static readonly DependencyProperty PropertyProperty = DependencyProperty.Register("Property", typeof(PropertyPath), typeof(ColorItem), new PropertyMetadata(null, OnPropertyChanged));

        public static readonly DependencyProperty ModeProperty = DependencyProperty.Register("Mode", typeof(EasingMode), typeof(ColorItem), new PropertyMetadata(EasingMode.EaseOut, OnEasingModeChanged));

        public string TargetName
        {
            get
            {
                return (string)GetValue(TargetNameProperty);
            }
            set
            {
                SetValue(TargetNameProperty, value);
            }
        }

        public PropertyPath Property
        {
            get
            {
                return (PropertyPath)GetValue(PropertyProperty);
            }
            set
            {
                SetValue(PropertyProperty, value);
            }
        }

        public EasingMode Mode
        {
            get
            {
                return (EasingMode)GetValue(ModeProperty);
            }
            set
            {
                SetValue(ModeProperty, value);
            }
        }

        private static void OnTargetNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ColorItem element = (ColorItem)d;
            string name = (string)e.NewValue;
            Storyboard.SetTargetName(element, name);
        }

        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ColorItem element = (ColorItem)d;
            PropertyPath path = (PropertyPath)e.NewValue;
            Storyboard.SetTargetProperty(element, path);
        }

        private static void OnEasingModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ColorItem colorItem = (ColorItem)d;
            EasingMode easingMode = (EasingMode)e.NewValue;
            CubicEase cubicEase = colorItem.EasingFunction as CubicEase;
            if (cubicEase != null)
            {
                cubicEase.EasingMode = easingMode;
                return;
            }

            colorItem.EasingFunction = new CubicEase
            {
                EasingMode = easingMode
            };
        }
    }
}

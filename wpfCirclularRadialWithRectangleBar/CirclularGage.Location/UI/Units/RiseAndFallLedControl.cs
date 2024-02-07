using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.Xml.Linq;
using System.Windows.Shapes;

namespace CirclularGage.Location.UI.Units
{
    [TemplatePart(Name = "symbol", Type = typeof(Path))]
    internal class RiseAndFallLedControl : ContentControl
    {
        private Path symbol;
        public static readonly DependencyProperty TryangleLedAngleProperty =
            DependencyProperty.Register("TryangleLedAngle", typeof(double), typeof(RiseAndFallLedControl), new PropertyMetadata(null));

        public static readonly DependencyProperty TriangleLedColorProperty =
                    DependencyProperty.Register("TriangleLedColor", typeof(Color), typeof(RiseAndFallLedControl), null);

        public static readonly DependencyProperty IsTryangleLedOnProperty =
            DependencyProperty.Register("IsTryangleLedOn", typeof(bool), typeof(RiseAndFallLedControl), new PropertyMetadata(false, OnPropertyChangedLedOn));
        public Color TriangleLedColor
        {
            get { return (Color)GetValue(TriangleLedColorProperty); }
            set { SetValue(TriangleLedColorProperty, value); }
        }
        public bool IsTryangleLedOn
        {
            get { return (bool)GetValue(IsTryangleLedOnProperty); }
            set { SetValue(IsTryangleLedOnProperty, value); }
        }
        public double TryangleLedAngle
        {
            get { return (double)GetValue(TryangleLedAngleProperty); }
            set { SetValue(TryangleLedAngleProperty, value); }
        }
        static RiseAndFallLedControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RiseAndFallLedControl), new FrameworkPropertyMetadata(typeof(RiseAndFallLedControl)));
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            symbol = GetTemplateChild("symbol") as Path;
        }
        private static void OnPropertyChangedLedOn(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as RiseAndFallLedControl;
            control.OnLedOnValueChanged(e);

        }

        private void OnLedOnValueChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue != e.NewValue)
            {
                if (symbol != null)
                {
                    if (IsTryangleLedOn == true)
                    {
                        symbol.Fill = new SolidColorBrush(TriangleLedColor);
                    }
                    else
                    {
                        symbol.Fill = new SolidColorBrush(Colors.Transparent);
                    }
                }
            }
        }
    }
}

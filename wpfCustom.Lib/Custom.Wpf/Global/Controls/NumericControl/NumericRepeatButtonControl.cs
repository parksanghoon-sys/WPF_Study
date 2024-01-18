using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace Custom.Wpf.Global.Controls.NumericControl
{
    public class NumericRepeatButtonControl : RepeatButton
    {
        public static readonly DependencyProperty RepeatButtonContentProperty =
            DependencyProperty.Register("RepeatButtonContent", typeof(int), typeof(NumericRepeatButtonControl), new PropertyMetadata(null));

        public Geometry RepeatButtonContent
        {
            get { return (Geometry)GetValue(RepeatButtonContentProperty); }
            set { SetValue(RepeatButtonContentProperty, value); }
        }



        public ICommand ClickCommand
        {
            get { return (ICommand)GetValue(ClickCommandProperty); }
            set { SetValue(ClickCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ClickCommandProperty =
            DependencyProperty.Register("ClickCommand", typeof(ICommand), typeof(NumericRepeatButtonControl),new PropertyMetadata(null));


        static NumericRepeatButtonControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NumericRepeatButtonControl), new FrameworkPropertyMetadata(typeof(NumericRepeatButtonControl)));
        }

    }
}

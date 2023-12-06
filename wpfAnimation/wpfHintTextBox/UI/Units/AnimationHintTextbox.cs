using Custom.Wpf.Global.Controls;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;


namespace wpfHintTextBox.UI.Units
{
    public class AnimationHintTextbox : TextBox
    {


        public bool IsNotAcceptValue
        {
            get { return (bool)GetValue(IsNotAcceptValueProperty); }
            set { SetValue(IsNotAcceptValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsNotAcceptValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsNotAcceptValueProperty =
            DependencyProperty.Register("IsNotAcceptValue", typeof(bool), typeof(AnimationHintTextbox), new PropertyMetadata(false, new PropertyChangedCallback(OnChanged)));

        private static void OnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var root = d as AnimationHintTextbox;
            if (root != null)
            {
                if(e.NewValue == e.OldValue)
                {
                    
                }
            }
        }

        public SolidColorBrush HintTextColor
        {
            get { return (SolidColorBrush)GetValue(HintTextColorProperty); }
            set { SetValue(HintTextColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HintTextColorProperty =
            DependencyProperty.Register("HintTextColor", typeof(SolidColorBrush), typeof(AnimationHintTextbox), new PropertyMetadata(null));



        public string HintText
        {
            get { return (string)GetValue(HintTextProperty); }
            set { SetValue(HintTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HintText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HintTextProperty =
            DependencyProperty.Register("HintText", typeof(string), typeof(AnimationHintTextbox));


        static AnimationHintTextbox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AnimationHintTextbox), new FrameworkPropertyMetadata(typeof(AnimationHintTextbox)));
        }
        public AnimationHintTextbox()
        {
            this.GotFocus += AnimationHintTextbox_GotFocus;
            this.LostFocus += AnimationHintTextbox_LostFocus;
        }
        Viewbox vb;
        Border bdr;
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            vb = GetTemplateChild("PART_HintArea") as Viewbox;
            bdr = GetTemplateChild("PART_BDR") as Border;

            //this.CaretBrush = this.Foreground;
        }
        double topPoint = 0.0;
        double widthPoint = 0;
        double originWidthPoint = 0;
        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            widthPoint = (sizeInfo.NewSize.Width / 1.5);
            originWidthPoint = sizeInfo.NewSize.Width;
            topPoint = -(sizeInfo.NewSize.Height / 2);
        }
        private void AnimationHintTextbox_LostFocus(object sender, RoutedEventArgs e)
        {
            this.FocusedExitAction();
        }

        private void FocusedExitAction()
        {
            if(string.IsNullOrEmpty(this.Text) == false && Canvas.GetTop(this.vb) == topPoint)
            {
                return;
            }
            var sb = new Storyboard();
            ValueItem topMove = GetValueItem(From: topPoint, path: new PropertyPath(Canvas.TopProperty));
            ValueItem widthSize = GetValueItem(From: widthPoint, To: originWidthPoint, path: new PropertyPath(Viewbox.WidthProperty));

            sb.Children.Add(topMove);
            sb.Children.Add(widthSize);

            sb.Begin(this.vb);
            this.bdr.BorderBrush = HintTextColor;
        }

        private void AnimationHintTextbox_GotFocus(object sender, RoutedEventArgs e)
        {
            this.FocusEnterAction();
        }

        private void FocusEnterAction()
        {
            if (String.IsNullOrEmpty(this.Text) == false && Canvas.GetTop(this.vb) == topPoint)
                return;
            var sb = new Storyboard();

            ValueItem TopMove = GetValueItem(To: topPoint,
                                            path: new PropertyPath(Canvas.TopProperty));
            ValueItem WidthSize = GetValueItem(From: originWidthPoint,
                                                To: widthPoint,
                                                path: new PropertyPath(Viewbox.WidthProperty));
            sb.Children.Add(TopMove);
            sb.Children.Add(WidthSize);
            sb.Begin(this.vb);


            this.bdr.BorderBrush = this.Foreground;
        }
        private ValueItem GetValueItem(PropertyPath path, double From = 0.0, double To = 0.0)
        {
            ValueItem valueItem = new();
            valueItem.TargetName = this.vb.Name;
            valueItem.Property = path;
            valueItem.From = From;
            valueItem.To = To;
            valueItem.Duration = new Duration(new TimeSpan(0, 0, 0, 0, 500));
            valueItem.Mode = Custom.Wpf.Global.Enums.EasingFunctionBaseMode.ExponentialEaseOut;
            return valueItem;
        }
    }
}

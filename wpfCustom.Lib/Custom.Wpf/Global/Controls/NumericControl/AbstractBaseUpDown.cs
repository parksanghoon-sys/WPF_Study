using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using YamlDotNet.Core.Tokens;

namespace Custom.Wpf.Global.Controls.NumericControl
{
    public abstract class AbstractBaseUpDown<T> : InputBaseUpDown, ICommandSource
    {
        public const string Part_TextBoxName = "PART_TextBox";
        public const string PART_IncrementButton = "PART_IncrementButton";
        public const string PART_DecrementButton = "PART_DecrementButton";
        protected static T _MinValue = default(T);
        protected static T _MaxValue = default(T);
        private bool _IsDataValid = true;

        protected bool UserInput { get; set; }
        protected TextBox _PART_TextBox;
        private RepeatButton _PART_DecrementButton;
        private RepeatButton _PART_IncrementButton;
        private MouseIncrementor _objMouseIncr;

        public static readonly DependencyProperty IsIncDecButtonsVisibleProperty = 
            DependencyProperty.Register("IsIncDecButtonsVisible", typeof(bool), typeof(AbstractBaseUpDown<T>), new PropertyMetadata(true));

        protected static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(T), typeof(AbstractBaseUpDown<T>), new PropertyMetadata(_MinValue, new PropertyChangedCallback(OnValueChanged), new CoerceValueCallback(CoerceValue)));
        private static void OnValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var control = obj as AbstractBaseUpDown<T>;

            if (control != null && args != null)
            {
                RoutedPropertyChangedEventArgs<T> e = new RoutedPropertyChangedEventArgs<T>((T)args.OldValue, (T)args.NewValue, ValueChangedEvent);
                control.OnValueChanged(e);

                AbstractBaseUpDown<T>.CoerceValue(obj, args.NewValue);
            }
        }
        protected virtual void OnValueChanged(RoutedPropertyChangedEventArgs<T> args)
        {
            if (_PART_TextBox != null)
            {
                _PART_TextBox.Text = FormatNumber(Value);
                LastEditingNumericValue = Value;
            }
            CommandExecute(Command);
            this.RaiseEvent(args);
        }

        private static object CoerceValue(DependencyObject element, object value)
        {
            var control = element as AbstractBaseUpDown<T>;

            try
            {
                T newValue = (T)value;

                if (control != null)
                    return control.CoerceValue(newValue);
            }
            catch
            {
            }

            return control.Value;
        }

        protected static readonly DependencyProperty MinValueProperty =
          DependencyProperty.Register("MinValue", typeof(T), typeof(AbstractBaseUpDown<T>), new PropertyMetadata(_MinValue, new PropertyChangedCallback(OnMinValueChanged), new CoerceValueCallback(CoerceMinValue)));

        private static void OnMinValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var control = obj as AbstractBaseUpDown<T>;

            if (control != null && args != null)
            {
                RoutedPropertyChangedEventArgs<T> e = new RoutedPropertyChangedEventArgs<T>((T)args.OldValue, (T)args.NewValue, MinValueChangedEvent);
                control.OnMinValueChanged(e);

                AbstractBaseUpDown<T>.CoerceMinValue(obj, args.NewValue);
            }
        }

        private static object CoerceMinValue(DependencyObject element, object value)
        {
            var control = element as AbstractBaseUpDown<T>;

            try
            {
                T newValue = (T)value;

                if (control != null)
                    return control.CoerceMinValue(newValue);
            }
            catch
            {
            }

            return control.Value;
        }        
        protected virtual void OnMinValueChanged(RoutedPropertyChangedEventArgs<T> args)
        {
            this.RaiseEvent(args);
        }
        protected static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register("MaxValue", typeof(T), typeof(AbstractBaseUpDown<T>), new PropertyMetadata(_MaxValue, new PropertyChangedCallback(OnMaxValueChanged), new CoerceValueCallback(CoerceMaxValue)));

        private static void OnMaxValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var control = obj as AbstractBaseUpDown<T>;

            if (control != null && args != null)
            {
                RoutedPropertyChangedEventArgs<T> e = new RoutedPropertyChangedEventArgs<T>((T)args.OldValue, (T)args.NewValue, MaxValueChangedEvent);
                control.OnMaxValueChanged(e);

                AbstractBaseUpDown<T>.CoerceMaxValue(obj, args.NewValue);
            }
        }
        protected virtual void OnMaxValueChanged(RoutedPropertyChangedEventArgs<T> args)
        {
            this.RaiseEvent(args);
        }
        private static object CoerceMaxValue(DependencyObject element, object value)
        {
            var control = element as AbstractBaseUpDown<T>;

            try
            {
                T newValue = (T)value;

                if (control != null)
                    return control.CoerceMaxValue(newValue);

                return newValue;
            }
            catch (Exception)
            {
            }

            return control.MaxValue;
        }
        

        protected static readonly DependencyProperty FormatStringProperty =
             DependencyProperty.Register("FormatString", typeof(string), typeof(AbstractBaseUpDown<T>), new PropertyMetadata("G", OnIsFormatStringChanged));

        private static void OnIsFormatStringChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is AbstractBaseUpDown<T> control && control._PART_TextBox != null && e.NewValue is string)
                control._PART_TextBox.Text = control.FormatNumber(control.Value);
        }

        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register("CommandParameter", typeof(object), typeof(AbstractBaseUpDown<T>));

        public static readonly DependencyProperty InputElementProperty =
            DependencyProperty.Register("CommandTarget", typeof(IInputElement), typeof(AbstractBaseUpDown<T>));

        protected static readonly RoutedEvent ValueChangedEvent =
            EventManager.RegisterRoutedEvent("ValueChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<T>), typeof(AbstractBaseUpDown<T>));
        
        protected static readonly RoutedEvent MinValueChangedEvent = 
            EventManager.RegisterRoutedEvent("MinValueChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<T>), typeof(AbstractBaseUpDown<T>));

        protected static readonly RoutedEvent MaxValueChangedEvent =
            EventManager.RegisterRoutedEvent("MaxValueChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<T>), typeof(AbstractBaseUpDown<T>));

        public static readonly DependencyProperty MouseWheelAccelaratorKeyProperty =
            DependencyProperty.Register("MouseWheelAccelaratorKey", typeof(ModifierKeys), typeof(AbstractBaseUpDown<T>), new PropertyMetadata(ModifierKeys.Control));

        public static readonly DependencyProperty MouseWheelEnabledProperty =
            DependencyProperty.Register("MouseWheelEnabled", typeof(bool), typeof(AbstractBaseUpDown<T>), new PropertyMetadata(true));

        public static readonly DependencyProperty CommandProperty =
           DependencyProperty.Register("Command", typeof(ICommand), typeof(AbstractBaseUpDown<T>), new PropertyMetadata(null, new PropertyChangedCallback(CommandChangedCallBack)));

        public bool IsIncDecButtonsVisible
        {
            get { return (bool)GetValue(IsIncDecButtonsVisibleProperty); }
            set { SetValue(IsIncDecButtonsVisibleProperty, value); }
        }
        public T Value
        {
            get { return (T)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }        
        public T MinValue
        {
            get { return (T)GetValue(MinValueProperty); }
            set { SetValue(MinValueProperty, value); }
        }
        
        public T MaxValue
        {
            get { return (T)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }
        public string FormatString
        {
            get
            {
                string fsp = (string)GetValue(FormatStringProperty);
                if (fsp == "G" && (NumberStyle == System.Globalization.NumberStyles.HexNumber) ||
                    (NumberStyle == System.Globalization.NumberStyles.AllowHexSpecifier))
                {
                    fsp = "X";
                }
                return fsp;
            }
            set { SetValue(FormatStringProperty, value); }
        }
        public ModifierKeys MouseWheelAccelaratorKey
        {
            get { return (ModifierKeys)GetValue(MouseWheelAccelaratorKeyProperty); }
            set { SetValue(MouseWheelAccelaratorKeyProperty, value); }
        }
        public bool MouseWheelEnabled
        {
            get { return (bool)GetValue(MouseWheelEnabledProperty); }
            set { SetValue(MouseWheelEnabledProperty, value); }
        }

        public abstract T StepSize { get; set; }
        public bool IsValueValid
        {
            get { return _IsDataValid; }
            protected set
            {
                if (_IsDataValid != value)
                {
                    _IsDataValid = value;

                    EditingColorBrush = _IsDataValid ?
                        new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Green) :
                        new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Red);
        
                }
            }
        }
        private T lastEditingNumericValue;
        public T LastEditingNumericValue
        {
            get { return lastEditingNumericValue; }
            protected set
            {
                lastEditingNumericValue = value;          
            }
        }

        public event RoutedPropertyChangedEventHandler<T> ValueChanged
        {
            add { AddHandler(ValueChangedEvent, value); }
            remove { RemoveHandler(ValueChangedEvent, value); }
        }
        public event RoutedPropertyChangedEventHandler<T> MinValueChanged
        {
            add { AddHandler(MinValueChangedEvent, value); }
            remove { RemoveHandler(MinValueChangedEvent, value); }
        }

        public event RoutedPropertyChangedEventHandler<T> MaxValueChanged
        {
            add { AddHandler(MaxValueChangedEvent, value); }
            remove { RemoveHandler(MaxValueChangedEvent, value); }
        }

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public object CommandParameter
        {
            get { return (object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public IInputElement CommandTarget
        {
            get { return (IInputElement)GetValue(InputElementProperty); }
            set { SetValue(InputElementProperty, value); }
        }
        static AbstractBaseUpDown()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AbstractBaseUpDown<T>),
                       new FrameworkPropertyMetadata(typeof(AbstractBaseUpDown<T>)));
        }
        public AbstractBaseUpDown()
            : base()
        {
            UserInput = false;
        }
        abstract protected bool OnIncrement(T stepValue);
        abstract protected bool OnDecrement(T stepValue);
        protected abstract bool ParseText(string text, out T number);
        protected abstract T CoerceValue(T NewValue);
        protected abstract T CoerceMinValue(T NewValue);
        protected abstract T CoerceMaxValue(T NewValue);

        private static void CommandChangedCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is AbstractBaseUpDown<T> nud)
            {
                ICommand oldCommand = e.OldValue as ICommand;
                ICommand newCommand = e.NewValue as ICommand;
                nud.HookUpCommand(oldCommand, newCommand);
            }
        }
        private void CommandExecute(ICommand cmd)
        {
            if (cmd is RoutedCommand command)
                command.Execute(CommandParameter, CommandTarget);
            else if (cmd != null)
                cmd.Execute(CommandParameter);
        }
        private void HookUpCommand(ICommand? oldCommand, ICommand? newCommand)
        {
            if (oldCommand != null)
            {
                oldCommand.CanExecuteChanged -= CanExecuteChanged;
            }
            if (newCommand != null)
            {
                newCommand.CanExecuteChanged += CanExecuteChanged;
            }
        }
        private void CanExecuteChanged(object sender, EventArgs e)
        {
            if (this.Command is RoutedCommand command)
            {
                this.IsEnabled = command.CanExecute(CommandParameter, CommandTarget);
            }
            else if (this.Command != null)
            {
                this.IsEnabled = this.Command.CanExecute(CommandParameter);
            }
        }
       

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _PART_TextBox = this.GetTemplateChild(Part_TextBoxName) as TextBox;
            _PART_DecrementButton = this.GetTemplateChild(PART_DecrementButton) as RepeatButton;
            _PART_IncrementButton = this.GetTemplateChild(PART_IncrementButton) as RepeatButton;

            if(_PART_TextBox != null )
            {
                FormatText(_PART_TextBox.Text);
                _PART_TextBox.TextChanged += _PART_TextBox_TextChanged;
                _PART_TextBox.PreviewKeyDown += textBox_PreviewKeyDown;
                _PART_TextBox.PreviewTextInput += textBox_PreviewTextInput;
                DataObject.AddPastingHandler(_PART_TextBox, textBox_TextPasted);
            }
            if (_PART_DecrementButton != null)
                _PART_DecrementButton.PreviewKeyDown += IncDecButton_PreviewKeyDown;

            if (_PART_IncrementButton != null)
                _PART_IncrementButton.PreviewKeyDown += IncDecButton_PreviewKeyDown;

            this.IsVisibleChanged += new DependencyPropertyChangedEventHandler(this_IsVisibleChanged);
        }
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);

            if (!MouseWheelEnabled)
                return;
            if (e.Handled == false)
            {
                if (e.Delta != 0)
                {
                    if (e.Delta < 0 && CanDecreaseCommand() == true)
                    {      
                        OnDecrease();
                        e.Handled = true;
                    }
                    else
                    {
                        if (e.Delta > 0 && CanIncreaseCommand() == true)
                        {         
                            OnIncrease();
                            e.Handled = true;
                        }
                    }
                }
            }
        }
        private void this_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.ContextIdle, new Action(delegate ()
            {
                if (this.IsKeyboardFocused)
                {
                    Keyboard.ClearFocus();
                }

                _objMouseIncr = null;
            }));
        }

        private void IncDecButton_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Escape)
            {
                Keyboard.ClearFocus();
                e.Handled = true;
                return;
            }
        }

        private void textBox_TextPasted(object sender, DataObjectPastingEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (e.SourceDataObject.GetDataPresent(DataFormats.Text, true) == false)
            {
                return;
            }

            UserInput = true;
        }

        private void textBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            UserInput = true;
        }

        private void textBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            UserInput = true;
            if(e.Key == System.Windows.Input.Key.Enter)
            {
                _PART_TextBox.Text = FormatNumber(Value);
                _PART_TextBox.SelectionStart = 0;
                Keyboard.ClearFocus();
                e.Handled = true;
                return;
            }
            if (e.Key == System.Windows.Input.Key.Up )
            {
                if (CanIncreaseCommand() == true)
                    IncreaseCommand.Execute(null, this);

                e.Handled = true;
                return;
            }
            
            if (e.Key == System.Windows.Input.Key.Down)
            {
                if (CanDecreaseCommand() == true)
                    DecreaseCommand.Execute(null, this);

                e.Handled = true;
                return;
            }
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                if (_PART_TextBox != null)
                {
                    if (!IsValueValid)
                    {
                        e.Handled = true;
                        return;
                    }
                    T OldValue = Value;
                    Value = FormatText(_PART_TextBox.Text, true);

                    // force to raise value changed event to tigger re write /re-set for application
                    if (OldValue.Equals(Value))
                    {
                        System.Diagnostics.Debug.WriteLine("ValueChanged forced by user");
                        OnValueChanged(this, new DependencyPropertyChangedEventArgs(ValueProperty, Value, Value));
                    }

                    e.Handled = true;
                }
                return;
            }

        }
     

        private void _PART_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_PART_TextBox != null)
            {
                if(UserInput == true)
                {
                    T temp = LastEditingNumericValue;
                    IsValueValid = VerifyText(_PART_TextBox.Text, ref temp);
                    if (!LastEditingNumericValue.Equals(temp))
                    {
                        LastEditingNumericValue = temp;
                    }
                }
                else
                {
                    FormatText(_PART_TextBox.Text);
                }
            }
        }
        protected abstract bool VerifyText(string text, ref T tempValue);

        protected T FormatText(string text,bool formatNumber = true)
        {
            if (_PART_TextBox == null)
                return Value;
            T number = default;
            if (ParseText(text, out number))
            {
                number = CoerceValue(number);

                _PART_TextBox.Text = FormatNumber(number);
                _PART_TextBox.SelectionStart = 0;

                return number;
            }
            _PART_TextBox.Text = FormatNumber(Value);
            return LastEditingNumericValue;
        }


        protected string FormatNumber(T number)
        {
            string format = "{0}";
            var form = (string)GetValue(FormatStringProperty);
            if (string.IsNullOrEmpty(this.FormatString) == false)
            {
                format = !FormatString.StartsWith("{")
                    ? "{0:" + this.FormatString + "}"
                    : FormatString;
            }

            return string.Format(format, number);
        }
    }

}

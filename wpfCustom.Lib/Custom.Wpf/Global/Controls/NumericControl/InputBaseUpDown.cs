using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Custom.Wpf.Global.Controls.NumericControl
{
    public abstract class InputBaseUpDown : Control
    {
        public static readonly DependencyProperty IsReadOnlyProperty = 
            DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(InputBaseUpDown), new PropertyMetadata(true));

        public static readonly DependencyProperty NumberStyleProperty = 
            DependencyProperty.Register("NumberStyle", typeof(NumberStyles), typeof(InputBaseUpDown), new PropertyMetadata(NumberStyles.Any));

        public static readonly DependencyProperty EditingColorBrushProperty =
            DependencyProperty.Register("EditingColorBrush", typeof(System.Windows.Media.SolidColorBrush),
                typeof(InputBaseUpDown), new PropertyMetadata(new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Green)));

        private static RoutedCommand _IncreaseCommand;
        private static RoutedCommand _DecreaseCommand;

        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, value); }
        }
        public NumberStyles NumberStyle
        {
            get { return (NumberStyles)GetValue(NumberStyleProperty); }
            set { SetValue(NumberStyleProperty, value); }
        }
        protected System.Windows.Media.SolidColorBrush EditingColorBrush
        {
            get { return (System.Windows.Media.SolidColorBrush)GetValue(EditingColorBrushProperty); }
            set { SetValue(EditingColorBrushProperty, value); }
        }
        public static RoutedCommand IncreaseCommand
        {
            get
            {
                return _IncreaseCommand;
            }
        }

        /// <summary>
        /// Expose the decrease value command via <seealso cref="RoutedCommand"/> property.
        /// </summary>
        public static RoutedCommand DecreaseCommand
        {
            get
            {
                return _DecreaseCommand;
            }
        }

        #region methods        
        /// <summary>
        /// Increase the displayed integer value
        /// </summary>
        protected abstract void OnIncrease();

        /// <summary>
        /// Determines whether the increase command is available or not.
        /// </summary>
        protected abstract bool CanIncreaseCommand();

        /// <summary>
        /// Decrease the displayed integer value
        /// </summary>
        protected abstract void OnDecrease();

        /// <summary>
        /// Determines whether the decrease command is available or not.
        /// </summary>
        protected abstract bool CanDecreaseCommand();

        public InputBaseUpDown()
        {
            this.InitializeCommands();
        }
        /// <summary>
        /// Initialize up down/button commands and key gestures for up/down cursor keys
        /// </summary>
        private void InitializeCommands()
        {
            InputBaseUpDown._IncreaseCommand = new RoutedCommand("IncreaseCommand", typeof(InputBaseUpDown));
            CommandManager.RegisterClassCommandBinding(typeof(InputBaseUpDown), new CommandBinding(_IncreaseCommand, OnIncreaseCommand, OnCanIncreaseCommand));

            CommandManager.RegisterClassInputBinding(typeof(InputBaseUpDown), new InputBinding(_IncreaseCommand, new KeyGesture(Key.Up)));

            InputBaseUpDown._DecreaseCommand = new RoutedCommand("DecreaseCommand", typeof(InputBaseUpDown));

            CommandManager.RegisterClassCommandBinding(typeof(InputBaseUpDown), new CommandBinding(_DecreaseCommand, OnDecreaseCommand, OnCanDecreaseCommand));
        }

        /// <summary>
        /// Determine whether the IncreaseCommand can be executed or not and return the result
        /// in the <see cref="CanExecuteRoutedEventArgs.CanExecute"/> property of the given
        /// <paramref name="e"/>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnCanIncreaseCommand(object sender, CanExecuteRoutedEventArgs e)
        {
            var control = sender as InputBaseUpDown;
            if (control != null)
            {
                e.CanExecute = control.CanIncreaseCommand();
                e.Handled = true;
            }
        }

        /// <summary>
        /// Execute the increase value command
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnIncreaseCommand(object sender, ExecutedRoutedEventArgs e)
        {
            var control = sender as InputBaseUpDown;
            if (control != null)
            {
                control.OnIncrease();
                e.Handled = true;
            }
        }

        /// <summary>
        /// Execute the decrease value command
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnDecreaseCommand(object sender, ExecutedRoutedEventArgs e)
        {
            var control = sender as InputBaseUpDown;
            if (control != null)
            {
                control.OnDecrease();
                e.Handled = true;
            }
        }

        /// <summary>
        /// Determine whether the DecreaseCommand can be executed or not and return the result
        /// in the <see cref="CanExecuteRoutedEventArgs.CanExecute"/> property of the given
        /// <paramref name="e"/>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnCanDecreaseCommand(object sender, CanExecuteRoutedEventArgs e)
        {
            var control = sender as InputBaseUpDown;
            if (control != null)
            {
                e.CanExecute = control.CanDecreaseCommand();
                e.Handled = true;
            }
        }

        #endregion methods
    }
}

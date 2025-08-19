using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Controls;

// Behaviors/TextBoxFocusVisualStateBehavior.cs - TextBox VSM 관리

namespace wpfCleanArchtecture.UI.Behaviors
{
    public partial class VisualStateManagerBehavior : Behavior<FrameworkElement>
    {
        public static readonly DependencyProperty StateNameProperty =
            DependencyProperty.Register(nameof(StateName), typeof(string),
                typeof(VisualStateManagerBehavior), new PropertyMetadata(OnStateNameChanged));

        public static readonly DependencyProperty UseTransitionsProperty =
            DependencyProperty.Register(nameof(UseTransitions), typeof(bool),
                typeof(VisualStateManagerBehavior), new PropertyMetadata(true));

        public string? StateName
        {
            get => (string?)GetValue(StateNameProperty);
            set => SetValue(StateNameProperty, value);
        }

        public bool UseTransitions
        {
            get => (bool)GetValue(UseTransitionsProperty);
            set => SetValue(UseTransitionsProperty, value);
        }

        private static void OnStateNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var behavior = (VisualStateManagerBehavior)d;
            behavior.UpdateVisualState();
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            UpdateVisualState();
        }

        private void UpdateVisualState()
        {
            if (AssociatedObject != null && !string.IsNullOrEmpty(StateName))
            {
                VisualStateManager.GoToState(AssociatedObject as Control ??
                    AssociatedObject as UserControl, StateName, UseTransitions);
            }
        }
    }
}

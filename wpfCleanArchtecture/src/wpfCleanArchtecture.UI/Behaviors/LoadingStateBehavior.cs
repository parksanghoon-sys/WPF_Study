using Microsoft.Xaml.Behaviors;
using System.Windows;

// Behaviors/TextBoxFocusVisualStateBehavior.cs - TextBox VSM 관리

namespace wpfCleanArchtecture.UI.Behaviors
{
    public partial class VisualStateManagerBehavior
    {
        public class LoadingStateBehavior : Behavior<FrameworkElement>
    {
        public static readonly DependencyProperty IsLoadingProperty =
            DependencyProperty.Register(nameof(IsLoading), typeof(bool),
                typeof(LoadingStateBehavior), new PropertyMetadata(false, OnIsLoadingChanged));

        public bool IsLoading
        {
            get => (bool)GetValue(IsLoadingProperty);
            set => SetValue(IsLoadingProperty, value);
        }

        private static void OnIsLoadingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var behavior = (LoadingStateBehavior)d;
            behavior.UpdateLoadingState();
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            UpdateLoadingState();
        }

        private void UpdateLoadingState()
        {
            if (AssociatedObject == null) return;

            string stateName = IsLoading ? "Loading" : "NotLoading";
            VisualStateManager.GoToElementState(AssociatedObject, stateName, true);
        }
    }
}

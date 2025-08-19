using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Controls;
using wpfCleanArchtecture.Domain;
using wpfCleanArchtecture.Presentation.ViewModels;

namespace wpfCleanArchtecture.UI.Behaviors
{
    public class TodoItemVisualStateBehavior : Behavior<Border>
    {
        public static readonly DependencyProperty TodoViewModelProperty =
            DependencyProperty.Register(nameof(TodoViewModel), typeof(TodoItemViewModel),
                typeof(TodoItemVisualStateBehavior), new PropertyMetadata(OnTodoViewModelChanged));

        public TodoItemViewModel? TodoViewModel
        {
            get => (TodoItemViewModel?)GetValue(TodoViewModelProperty);
            set => SetValue(TodoViewModelProperty, value);
        }

        protected override void OnAttached()
        {
            base.OnAttached();

            // 마우스 이벤트 구독
            AssociatedObject.MouseEnter += OnMouseEnter;
            AssociatedObject.MouseLeave += OnMouseLeave;
            AssociatedObject.PreviewMouseLeftButtonDown += OnPreviewMouseLeftButtonDown;
            AssociatedObject.PreviewMouseLeftButtonUp += OnPreviewMouseLeftButtonUp;

            UpdateVisualStates();
        }

        protected override void OnDetaching()
        {
            if (AssociatedObject != null)
            {
                AssociatedObject.MouseEnter -= OnMouseEnter;
                AssociatedObject.MouseLeave -= OnMouseLeave;
                AssociatedObject.PreviewMouseLeftButtonDown -= OnPreviewMouseLeftButtonDown;
                AssociatedObject.PreviewMouseLeftButtonUp -= OnPreviewMouseLeftButtonUp;
            }

            if (TodoViewModel != null)
            {
                TodoViewModel.PropertyChanged -= OnTodoViewModelPropertyChanged;
            }

            base.OnDetaching();
        }

        private static void OnTodoViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var behavior = (TodoItemVisualStateBehavior)d;

            if (e.OldValue is TodoItemViewModel oldViewModel)
            {
                oldViewModel.PropertyChanged -= behavior.OnTodoViewModelPropertyChanged;
            }

            if (e.NewValue is TodoItemViewModel newViewModel)
            {
                newViewModel.PropertyChanged += behavior.OnTodoViewModelPropertyChanged;
            }

            behavior.UpdateVisualStates();
        }

        private void OnTodoViewModelPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(TodoItemViewModel.IsCompleted) ||
                e.PropertyName == nameof(TodoItemViewModel.Priority))
            {
                UpdateVisualStates();
            }
        }

        private void OnMouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            VisualStateManager.GoToState(AssociatedObject, "MouseOver", true);
        }

        private void OnMouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            VisualStateManager.GoToState(AssociatedObject, "Normal", true);
        }

        private void OnPreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            VisualStateManager.GoToState(AssociatedObject, "Pressed", true);
        }

        private void OnPreviewMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            VisualStateManager.GoToState(AssociatedObject, "Normal", true);
        }

        private void UpdateVisualStates()
        {
            if (AssociatedObject == null || TodoViewModel == null)
                return;

            // 완료 상태
            string completionState = TodoViewModel.IsCompleted ? "Completed" : "Pending";
            VisualStateManager.GoToState(AssociatedObject, completionState, true);

            // 우선순위 상태
            string priorityState = TodoViewModel.Priority switch
            {
                Priority.Low => "LowPriority",
                Priority.Medium => "MediumPriority",
                Priority.High => "HighPriority",
                _ => "MediumPriority"
            };
            VisualStateManager.GoToState(AssociatedObject, priorityState, true);
        }
    }
}

// Behaviors/TextBoxFocusVisualStateBehavior.cs - TextBox VSM 관리

namespace wpfCleanArchtecture.UI.Behaviors
{
    public class TextBoxFocusVisualStateBehavior : Behavior<TextBox>
    {
        public static readonly DependencyProperty IsValidProperty =
            DependencyProperty.Register(nameof(IsValid), typeof(bool),
                typeof(TextBoxFocusVisualStateBehavior), new PropertyMetadata(true, OnIsValidChanged));

        public bool IsValid
        {
            get => (bool)GetValue(IsValidProperty);
            set => SetValue(IsValidProperty, value);
        }

        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.GotFocus += OnGotFocus;
            AssociatedObject.LostFocus += OnLostFocus;
            AssociatedObject.TextChanged += OnTextChanged;

            UpdateValidationState();
        }

        protected override void OnDetaching()
        {
            if (AssociatedObject != null)
            {
                AssociatedObject.GotFocus -= OnGotFocus;
                AssociatedObject.LostFocus -= OnLostFocus;
                AssociatedObject.TextChanged -= OnTextChanged;
            }

            base.OnDetaching();
        }

        private static void OnIsValidChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var behavior = (TextBoxFocusVisualStateBehavior)d;
            behavior.UpdateValidationState();
        }

        private void OnGotFocus(object sender, RoutedEventArgs e)
        {
            VisualStateManager.GoToState(AssociatedObject, "Focused", true);
            UpdateValidationState();
        }

        private void OnLostFocus(object sender, RoutedEventArgs e)
        {
            VisualStateManager.GoToState(AssociatedObject, "Unfocused", true);
            UpdateValidationState();
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            // 실시간 유효성 검사 (예: 빈 문자열 체크)
            if (AssociatedObject != null)
            {
                IsValid = !string.IsNullOrWhiteSpace(AssociatedObject.Text);
            }
        }

        private void UpdateValidationState()
        {
            if (AssociatedObject == null)
                return;

            if (!IsValid && AssociatedObject.IsFocused)
            {
                VisualStateManager.GoToState(AssociatedObject, "InvalidFocused", true);
            }
            else
            {
                VisualStateManager.GoToState(AssociatedObject, "Valid", true);
            }
        }
    }
}

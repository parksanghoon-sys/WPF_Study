using System.Windows;
using System.Windows.Controls;
using wpfCleanArchtecture.Domain;
using wpfCleanArchtecture.UI.Extensions;

namespace wpfCleanArchtecture.UI.AttachedProperties
{
    public static class VisualStateManagerAttachedProperties
    {
        // IsCompleted 첨부 속성
        public static readonly DependencyProperty IsCompletedProperty =
            DependencyProperty.RegisterAttached("IsCompleted", typeof(bool), typeof(VisualStateManagerAttachedProperties),
                new PropertyMetadata(false, OnIsCompletedChanged));

        public static bool GetIsCompleted(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsCompletedProperty);
        }

        public static void SetIsCompleted(DependencyObject obj, bool value)
        {
            obj.SetValue(IsCompletedProperty, value);
        }

        // Priority 첨부 속성
        public static readonly DependencyProperty PriorityProperty =
            DependencyProperty.RegisterAttached("Priority", typeof(Priority), typeof(VisualStateManagerAttachedProperties),
                new PropertyMetadata(Priority.Medium, OnPriorityChanged));

        public static Priority GetPriority(DependencyObject obj)
        {
            return (Priority)obj.GetValue(PriorityProperty);
        }

        public static void SetPriority(DependencyObject obj, Priority value)
        {
            obj.SetValue(PriorityProperty, value);
        }

        private static void OnIsCompletedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Border border)
            {
                UpdateTodoItemStates(border);
            }
        }

        private static void OnPriorityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Border border)
            {
                UpdateTodoItemStates(border);
            }
        }

        private static void UpdateTodoItemStates(Border border)
        {
            bool isCompleted = GetIsCompleted(border);
            Priority priority = GetPriority(border);
            VisualStateManagerExtensions.UpdateTodoItemStates(border, isCompleted, priority);
        }
    }
}

using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using wpfCleanArchtecture.Domain;

namespace wpfCleanArchtecture.UI.Extensions
{
    public static class VisualStateManagerExtensions
    {
        public static bool GoToElementState(FrameworkElement element, string stateName, bool useTransitions)
        {
            return VisualStateManager.GoToElementState(element, stateName, useTransitions);
        }

        public static void UpdateTodoItemStates(Border container, bool isCompleted, Priority priority)
        {
            // 완료 상태 업데이트
            string completionState = isCompleted ? "Completed" : "Pending";
            VisualStateManager.GoToState(container, completionState, true);

            // 우선순위 상태 업데이트
            string priorityState = priority switch
            {
                Priority.Low => "LowPriority",
                Priority.Medium => "MediumPriority",
                Priority.High => "HighPriority",
                _ => "MediumPriority"
            };
            VisualStateManager.GoToState(container, priorityState, true);
        }
    }
    public class VisualStateExtension : MarkupExtension
    {
        public string? StateName { get; set; }
        public bool UseTransitions { get; set; } = true;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return new VisualStateAction(StateName, UseTransitions);
        }
    }

    public class VisualStateAction
    {
        private readonly string? _stateName;
        private readonly bool _useTransitions;

        public VisualStateAction(string? stateName, bool useTransitions)
        {
            _stateName = stateName;
            _useTransitions = useTransitions;
        }

        public void Execute(FrameworkElement element)
        {
            if (!string.IsNullOrEmpty(_stateName))
            {
                VisualStateManager.GoToState(element as Control, _stateName, _useTransitions);
            }
        }
    }
}

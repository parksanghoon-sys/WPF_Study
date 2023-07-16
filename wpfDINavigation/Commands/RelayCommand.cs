using System;
using System.Windows.Input;

namespace wpfDINavigation.Commands
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T>? _excute;
        private readonly Predicate<T>? _canExcuate;

        public RelayCommand(Action<T>? excute,Predicate<T>? canExcute = null)            
        {
            _excute = excute;
            _canExcuate = canExcute;
        }

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object? parameter)
        {
            return _canExcuate?.Invoke((T)parameter!) ?? true;
        }

        public void Execute(object? parameter)
        {
            _excute?.Invoke((T)parameter!);
        }
    }
}

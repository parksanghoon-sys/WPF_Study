using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace wpfSlidePanel.Commands
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Predicate<object>? _candicate;

        public event EventHandler? CanExecuteChanged;
        public RelayCommand(Action<object> excute)
            :this(excute,null)
        {
            
        }
        public RelayCommand(Action<object> excute, Predicate<object>? canExcute)
        {
            _execute = excute ?? throw new ArgumentNullException(nameof(excute));
            _candicate = canExcute;
        }
        public bool CanExecute(object? parameter)
        {
            return _candicate?.Invoke(parameter!) ?? true;
        }

        public void Execute(object? parameter)
        {
            _execute(parameter!);
        }
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this,EventArgs.Empty);
        }
    }
}

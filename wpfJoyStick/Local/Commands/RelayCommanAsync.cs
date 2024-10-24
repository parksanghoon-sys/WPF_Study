using System.Windows.Input;

namespace wpfJoyStick.Local.Commands
{
    public class RelayCommanAsync : ICommand
    {
        private readonly Func<Task> _execute;
        private readonly Func<bool> _canExecute;
        private readonly Action<Exception> _errorHandler;
        private bool _isExecuting;
        private CancellationTokenSource _cts;

        public RelayCommanAsync(
            Func<Task> execute,
            Func<bool> canExecute = null,
            Action<Exception> errorHandler = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
            _errorHandler = errorHandler;
            _cts = new CancellationTokenSource();
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool IsExecuting
        {
            get => _isExecuting;
            private set
            {
                _isExecuting = value;
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public bool CanExecute(object parameter)
        {
            return !IsExecuting && (_canExecute?.Invoke() ?? true);
        }

        public async void Execute(object parameter)
        {
            if (!CanExecute(parameter))
                return;

            IsExecuting = true;

            try
            {
                await ExecuteAsync();
            }
            catch (OperationCanceledException)
            {
                // 작업이 취소된 경우
            }
            catch (Exception ex)
            {
                _errorHandler?.Invoke(ex);
            }
            finally
            {
                IsExecuting = false;
            }
        }

        private async Task ExecuteAsync()
        {
            await _execute();
        }

        public void Cancel()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = new CancellationTokenSource();
        }
    }
    public class RelayCommanAsync<T> : ICommand
    {
        private readonly Func<T, CancellationToken, Task> _excute;
        private readonly Func<T, bool> _canExecute;
        private readonly Action<Exception> _errorHandler;
        private bool _isExecuting;
        private CancellationTokenSource _cts;
        public RelayCommanAsync(Func<T, CancellationToken, Task> execute,
            Func<T, bool> canExecute = null,
            Action<Exception> errorHandler = null)
        {
            _excute = execute ?? throw new ArgumentException(nameof(_excute));
            _canExecute = canExecute;
            _errorHandler = errorHandler;
            _cts  = new CancellationTokenSource();
        }
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool IsExecuting
        {
            get => _isExecuting;
            private set
            {
                _isExecuting = value;
                CommandManager.InvalidateRequerySuggested();
            }
        }
        public bool CanExecute(object parameter)
        {
            return !IsExecuting && (_canExecute?.Invoke((T)parameter) ?? true);
        }

        public async void Execute(object parameter)
        {
            if (!CanExecute(parameter))
                return;

            IsExecuting = true;

            try
            {
                await ExecuteAsync((T)parameter);
            }
            catch (OperationCanceledException)
            {
                // 작업이 취소된 경우
            }
            catch (Exception ex)
            {
                _errorHandler?.Invoke(ex);
            }
            finally
            {
                IsExecuting = false;
            }
        }

        private async Task ExecuteAsync(T parameter)
        {
            await _excute(parameter, _cts.Token);
        }

        public void Cancel()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = new CancellationTokenSource();
        }
    }
}

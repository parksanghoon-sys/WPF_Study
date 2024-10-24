using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using wpfJoyStick.Local.Commands;
using wpfJoyStick.Local.Models;

namespace wpfJoyStick.Local.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public ICommand JoystickMovedCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        private JoystickPosition _currentPosition;

        public JoystickPosition CurrentPosition
        {
            get { return _currentPosition; }
            set { _currentPosition = value; OnPropertyChanged(); }
        }


        public MainViewModel()
        {            
            JoystickMovedCommand = new RelayCommand<JoystickPosition>(
                (joystricPositon) =>
                {
                    CurrentPosition = new (joystricPositon.X, joystricPositon.Y);
                    Debug.WriteLine($"position X: {joystricPositon.X}");
                }
            );
            CancelCommand = new RelayCommand(CancelOperations);
        }

        // 비동기 명령어 예제
        private RelayCommanAsync<JoystickPosition> _savePositionCommand;
        private RelayCommanAsync _loadDataCommand;

        public ICommand SavePositionCommand =>
            _savePositionCommand ??= new RelayCommanAsync<JoystickPosition>(
                // 실행 함수
                async (position, cancellationToken) =>
                {
                    await Task.Delay(1000, cancellationToken); // 시뮬레이션된 지연
                    await SaveToDatabase(position);
                },
                // Can Execute 함수
                (position) => position != null,
                // 에러 핸들러
                (ex) => Debug.WriteLine($"Error saving position: {ex.Message}")
            );

        public ICommand LoadDataCommand =>
            _loadDataCommand ??= new RelayCommanAsync(
                async () =>
                {
                    await Task.Delay(10000); // 시뮬레이션된 지연
                    await LoadFromDatabase();
                },
                () => !_loadDataCommand.IsExecuting,
                (ex) => MessageBox.Show($"Error loading data: {ex.Message}")
            );

        // XAML에서 사용할 IsLoading 속성
        public bool IsLoading => _loadDataCommand.IsExecuting;

        private async Task SaveToDatabase(JoystickPosition position)
        {         
            await Task.Delay(100); // 시뮬레이션
        }

        private async Task LoadFromDatabase()
        {            
            await Task.Delay(100); // 시뮬레이션
        }

        // 작업 취소가 필요한 경우
        public void CancelOperations()
        {
            _savePositionCommand.Cancel();
            _loadDataCommand.Cancel();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}

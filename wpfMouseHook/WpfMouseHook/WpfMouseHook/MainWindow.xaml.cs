using Gma.System.MouseKeyHook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WpfMouseHook
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer _inactivityTimer;
        private DateTime _lastActivityTime;
        private IKeyboardMouseEvents _globalHook;
        public MainWindow()
        {
            InitializeComponent();
            // 사용자의 마지막 활동 시간을 현재 시간으로 초기화합니다.
            _lastActivityTime = DateTime.Now;

            // 타이머를 생성하고, 1분마다 시간을 확인하도록 설정합니다.
            _inactivityTimer = new DispatcherTimer();
            _inactivityTimer.Interval = TimeSpan.FromMinutes(1);
            _inactivityTimer.Tick += CheckInactivity;
            _inactivityTimer.Start();

            // 글로벌 훅을 설정하고, 마우스 이벤트와 키보드 이벤트를 구독합니다.
            _globalHook = Hook.GlobalEvents();
            _globalHook.MouseClick += (sender, e) => UpdateLastActivityTime();
            _globalHook.KeyDown += (sender, e) => UpdateLastActivityTime();
        }
        private void CheckInactivity(object sender, EventArgs e)
        {
            // 현재 시간과 마지막 활동 시간이 1시간 이상 차이가 나는지 확인합니다.
            if (DateTime.Now - _lastActivityTime > TimeSpan.FromHours(1))
            {
                // 1시간 이상 활동이 없는 경우, 경고 메시지를 표시하거나 원하는 작업을 수행합니다.
                MessageBox.Show("장시간 사용하지 않았습니다. 자동으로 종료합니다.");
                Application.Current.Shutdown();
            }
        }

        // 사용자의 활동에 따라 마지막 활동 시간을 업데이트하는 메서드를 구현합니다.
        private void UpdateLastActivityTime()
        {
            _lastActivityTime = DateTime.Now;
        }

        protected override void OnClosed(EventArgs e)
        {
            // 창이 닫힐 때 글로벌 훅 리소스를 해제합니다.
            _globalHook.Dispose();
            base.OnClosed(e);
        }
    }
}

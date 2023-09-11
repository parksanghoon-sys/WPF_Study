using Log.Common;
using System.Diagnostics;
using System.Windows;

namespace CirclularGage.Main
{
    /// <summary>
    /// App.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            LogBase.AddFileListener();
            ExTrace.Print("Start");
            
            //ExDebug.Print("Debug : Start");
        }
        protected override void OnExit(ExitEventArgs e)
        {
            int x = 1200;
            double y = 3.14;
            string z = "hello";
            ExTrace.Warning("1. {0:D3}", x, y, z);
            ExTrace.Warning("1. {0:D5}", x, y, z);
            ExTrace.Warning("1. {0:D6}", x, y, z);
            ExTrace.Warning("1. {0:D8}", x, y, z);
            base.OnExit(e);

        }
    }
}

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
            base.OnExit(e);

        }
    }
}

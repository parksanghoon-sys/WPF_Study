using LogHelper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly Lazy<ILog> _log = new Lazy<ILog>(() => LogHelper.LogHelp.Log(LogHelperType.Default));
        private const string _APPLICATION_NAME_ = "LogTest";
        private static readonly string _title = "Log Test";
        public App()
        {
            this.LogHelperConfigSetting();
            
        }
        /// <summary>
        /// 프로그램 타이틀
        /// </summary>
        public static string ProductTitle
        {
            get { return _title; }
        }

        public static ILog Log
        {
            get { return _log.Value; }
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Current.ShutdownMode = ShutdownMode.OnLastWindowClose;
            App.Log.Write("!!START");
        }
        private void LogHelperConfigSetting()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            Version version = assembly.GetName().Version;

            // 1. 로그헬퍼 환경설정 읽기
            // 1-1. 클라이언트에서 xml파일로 환경설정 읽기
            //LogHelper.XMLConfiguratorLoader.Loader("LogHelperXMLConfigure_bak.xml");
            // 1-1. 리소스에서 xml 읽기
            XMLConfiguratorLoader.LoaderByXML(global::WpfApp2.Properties.Resource.LogHelperXMLConfigure);

            // 2. 프로그램 버전 설정
            XMLConfiguratorLoader.ProgramVersion = version.ToString();

            LogHelper.LogHelp.AddLogHelper(LogHelperType.Default, "__WPFLog__");
        }
    }
}

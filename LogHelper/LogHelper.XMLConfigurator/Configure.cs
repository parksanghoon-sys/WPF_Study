using LogHelper.Appender;
using System.ComponentModel;
using System.Text;

namespace LogHelper.XMLConfigurator
{
    public class Configure
    {
        private string _logHelperXMLConfigurePath;
        private static Lazy<Configure> _lazy = new Lazy<Configure>((Func<Configure>)(() => new Configure()));
        public static Configure Instance => Configure._lazy.Value;

        public Dictionary<string, AppenderBase> LogAppender { get; private set; }
        public string ProgramVersion { get; set; }

        public string UserID { get; set; }

        public string strXMLConfigure { get; private set; }
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void XmlLoadByFile(string LogHelperXMLCOnfigurePath)
        {
            this._logHelperXMLConfigurePath = LogHelperXMLCOnfigurePath;
            this.BindingAppenderClass();
        }
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void XmlLoad(string XMLConfigure) => this.BindingAppenderClass(XMLConfigure);
        private void BindingAppenderClass()
        {
            try
            {
                this.strXMLConfigure = new StreamReader(this._logHelperXMLConfigurePath, Encoding.Default).ReadToEnd();
                this.LogAppender = new XMLConfigureParserFactory().ExcuteParser(this.strXMLConfigure);
            }
            catch (FileNotFoundException ex)
            {
                throw new Exception("환경설정 파일을 찾을 수 없습니다.");                
            }
        }
        private void BindingAppenderClass(string xMLConfigure) => this.LogAppender = new XMLConfigureParserFactory().ExcuteParser(xMLConfigure);
    }
}

using LogHelper.Utill;
using LogHelper.XMLConfigurator;

namespace LogHelper.Appender
{
    internal class RollingFile : AppenderBase
    {
        public string FolderPath { get; set; } = string.Empty;
        public string FolderNameType { get; set; } = string.Empty;
        public int DeleteDay { get; set; } 
        public bool ComputerConfigure { get; set; } 
        public string FileNameType { get; set; } = string.Empty;
        public string Header { get; set; } = string.Empty;
        ~ RollingFile() => this.DoDispose(false);
        public override void Init()
        {
            this.FolderPath = string.Format("{0}{1}\\{2}", (object) AppDomain.CurrentDomain.BaseDirectory,(object) this.FolderPath, (object)Configure.Instance.UserID);
            IOUtil.CreateDirectory(this.FolderPath, "");
            IOUtil.DeleteDirectoryToLate(this.FolderPath, "", this.DeleteDay);
            if (this.ComputerConfigure == false)
                return;
            new ComputerConfigureWrite((ILog)this).Write(this.FolderPath);

        }
        public override void Error(string channel, Exception ex)
        {
            this.Write(channel, ex.Message, new object[0]);
            this.Write(channel, ex.StackTrace, new object[0]);
        }

        public override void Write(string channel, string strFormat, params object[] listObj)
        {
            if (channel == null)
                channel = this.FileNameType;
            IOUtil.DeleteDirectoryToLate(this.FolderPath, "", this.DeleteDay);
            string directoryName = string.Format("{0}", (object)DateTime.Now.ToString("yyyy-MM-dd"));
            IOUtil.CreateDirectory(this.FolderPath, directoryName);
            string message = LogHelperUtil.GetMessage(true, this.Header, strFormat, listObj);
            IOUtil.WriteFile(this.FolderPath, directoryName, channel, message.ToString());
        }

        public override void Debug(string channel, string strFormat, params object[] listObj)
        {
            IOUtil.DeleteDirectoryToLate(this.FolderPath, "", this.DeleteDay);
            string directoryName = string.Format("{0}\\{1}", (object)DateTime.Now.ToString("yyyy-MM-dd"), (object)nameof(Debug));
            IOUtil.CreateDirectory(this.FolderPath, directoryName);
            string message = LogHelperUtil.GetMessage(true, this.Header, strFormat, listObj);
            IOUtil.WriteFile(this.FolderPath, directoryName, channel, message.ToString());
        }

        protected override void DoDispose(bool disposing) => base.DoDispose(disposing);
    }
}

using LogHelper.Appender;
using LogHelper.Utill;
using LogHelper.XMLConfigurator;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogHelper
{
    internal class DefaultLogHelper : ILog, IDisposable
    {
        public DefaultLogHelper()
        {
            if (Configure.Instance.LogAppender == null)
                return;
            Configure.Instance.LogAppender.ToList<KeyValuePair<string, AppenderBase>>().ForEach((Action<KeyValuePair<string, AppenderBase>>)(item => item.Value.Init()));
        }

        public ILogMsgReturn LogMsgReturn { get; set; }

        public int BeginTaskCount => Configure.Instance.LogAppender.Count > 0 ? Configure.Instance.LogAppender.ElementAt<KeyValuePair<string, AppenderBase>>(0).Value.BeginTaskCount : 0;

        public void Error(string channel, Exception ex)
        {
            Configure.Instance.LogAppender.ToList<KeyValuePair<string, AppenderBase>>().ForEach((Action<KeyValuePair<string, AppenderBase>>)(item => item.Value.Error(channel, ex)));
            if (this.LogMsgReturn == null)
                return;
            this.LogMsgReturn.Error(channel, ex);
        }

        public void Error(Exception ex) => this.Error((string)null, ex);

        public void Write(string channel, string strFormat, params object[] listObj)
        {
            Configure.Instance.LogAppender.ToList<KeyValuePair<string, AppenderBase>>().ForEach((Action<KeyValuePair<string, AppenderBase>>)(item => item.Value.Write(channel, strFormat, listObj)));
            string message = LogHelperUtil.GetMessage(true, string.Empty, strFormat, listObj);
            if (this.LogMsgReturn != null)
                this.LogMsgReturn.Write(channel, message);
            Trace.WriteLine(message);
            Console.WriteLine(message);
        }

        public void Write(string strFormat, params object[] listObj) => this.Write((string)null, strFormat, listObj);

        public void Debug(string channel, string strFormat, params object[] listObj)
        {
            Configure.Instance.LogAppender.ToList<KeyValuePair<string, AppenderBase>>().ForEach((Action<KeyValuePair<string, AppenderBase>>)(item => item.Value.Debug(channel, strFormat, listObj)));
            string message = LogHelperUtil.GetMessage(true, string.Empty, strFormat, listObj);
            if (this.LogMsgReturn != null)
                this.LogMsgReturn.Debug(channel, message);
            Trace.WriteLine(message);
            Console.WriteLine(message);
        }

        public void Debug(string strFormat, params object[] listObj) => this.Debug("Debug.txt", strFormat, listObj);

        public void BeginError(string channel, Exception ex)
        {
            Configure.Instance.LogAppender.ToList<KeyValuePair<string, AppenderBase>>().ForEach((Action<KeyValuePair<string, AppenderBase>>)(item => item.Value.BeginError(channel, ex)));
            if (this.LogMsgReturn == null)
                return;
            this.LogMsgReturn.Error(channel, ex);
        }

        public void BeginError(Exception ex) => this.BeginError((string)null, ex);

        public void BeginWrite(string channel, string strFormat, params object[] listObj)
        {
            Configure.Instance.LogAppender.ToList<KeyValuePair<string, AppenderBase>>().ForEach((Action<KeyValuePair<string, AppenderBase>>)(item => item.Value.BeginWrite(channel, strFormat, listObj)));
            string message = LogHelperUtil.GetMessage(true, string.Empty, strFormat, listObj);
            if (this.LogMsgReturn != null)
                this.LogMsgReturn.Write(channel, message);
            Trace.WriteLine(message);
            Console.WriteLine(message);
        }

        public void BeginWrite(string strFormat, params object[] listObj) => this.BeginWrite((string)null, strFormat, listObj);

        public void BeginDebug(string channel, string strFormat, params object[] listObj)
        {
            Configure.Instance.LogAppender.ToList<KeyValuePair<string, AppenderBase>>().ForEach((Action<KeyValuePair<string, AppenderBase>>)(item => item.Value.BeginDebug(channel, strFormat, listObj)));
            string message = LogHelperUtil.GetMessage(true, string.Empty, strFormat, listObj);
            if (this.LogMsgReturn != null)
                this.LogMsgReturn.Debug(channel, message);
            Trace.WriteLine(message);
            Console.WriteLine(message);
        }

        public void BeginDebug(string strFormat, params object[] listObj) => this.BeginDebug("Debug.txt", strFormat, listObj);

        public void Dispose() => Configure.Instance.LogAppender.ToList<KeyValuePair<string, AppenderBase>>().ForEach((Action<KeyValuePair<string, AppenderBase>>)(item => item.Value.Dispose()));
    }
}

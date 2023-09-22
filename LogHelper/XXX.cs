using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogHelper
{
    internal class XXX : ILog, IDisposable
    {
        private DefaultLogHelper _defaultLogHelper;

        public XXX() => this._defaultLogHelper = new DefaultLogHelper();

        public ILogMsgReturn LogMsgReturn { get; set; }

        public int BeginTaskCount => this._defaultLogHelper.BeginTaskCount;

        public void Error(string channel, Exception ex) => this._defaultLogHelper.Error(channel, ex);

        public void Error(Exception ex) => this._defaultLogHelper.Error(ex);

        public void Write(string channel, string strFormat, params object[] listObj) => this._defaultLogHelper.Write(channel, strFormat, listObj);

        public void Write(string strFormat, params object[] listObj) => this._defaultLogHelper.Write(strFormat, listObj);

        public void Debug(string channel, string strFormat, params object[] listObj) => this._defaultLogHelper.Debug(channel, strFormat, listObj);

        public void Debug(string strFormat, params object[] listObj) => this._defaultLogHelper.Debug(strFormat, listObj);

        public void BeginError(string channel, Exception ex) => this._defaultLogHelper.BeginError(channel, ex);

        public void BeginError(Exception ex) => this._defaultLogHelper.BeginError(ex);

        public void BeginWrite(string channel, string strFormat, params object[] listObj) => this._defaultLogHelper.BeginWrite(channel, strFormat, listObj);

        public void BeginWrite(string strFormat, params object[] listObj) => this._defaultLogHelper.BeginWrite(strFormat, listObj);

        public void BeginDebug(string channel, string strFormat, params object[] listObj) => this._defaultLogHelper.BeginDebug(channel, strFormat, listObj);

        public void BeginDebug(string strFormat, params object[] listObj) => this._defaultLogHelper.BeginDebug(strFormat, listObj);

        public void Dispose() => this._defaultLogHelper.Dispose();
    }
}

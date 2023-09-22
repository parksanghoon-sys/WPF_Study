namespace LogHelper
{
    public interface ILog : IDisposable
    {
        int BeginTaskCount { get; }

        void Error(string channel, Exception ex);

        void Error(Exception ex);

        void Write(string channel, string strFormat, params object[] listObj);

        void Write(string strFormat, params object[] listObj);

        void Debug(string channel, string strFormat, params object[] listObj);

        void Debug(string strFormat, params object[] listObj);

        void BeginError(string channel, Exception ex);

        void BeginError(Exception ex);

        void BeginWrite(string channel, string strFormat, params object[] listObj);

        void BeginWrite(string strFormat, params object[] listObj);

        void BeginDebug(string channel, string strFormat, params object[] listObj);

        void BeginDebug(string strFormat, params object[] listObj);
    }
}

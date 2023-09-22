namespace LogHelper
{
    public interface ILogMsgReturn
    {
        void Error(string channel, Exception ex);

        void Write(string channel, string logMessage);

        void Debug(string channel, string logMessage);
    }
}

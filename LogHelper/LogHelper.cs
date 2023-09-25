using LogHelper.XMLConfigurator;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogHelper
{
    public class LogHelp : IDisposable
    {
        private static readonly object _padlock = new object();
        private static Dictionary<LogHelperType, ILog> _logHelpers = new Dictionary<LogHelperType, ILog>();

        public static void AddLogHelper(LogHelperType lht, string userID)
        {
            Configure.Instance.UserID = userID;
            if (LogHelp._logHelpers.ContainsKey(lht))
                return;
            ILog log;
            switch (lht)
            {
                case LogHelperType.Default:
                    log = (ILog)new DefaultLogHelper();
                    break;
                case LogHelperType.XXX:
                    log = (ILog)new XXX();
                    break;
                default:
                    log = (ILog)new DefaultLogHelper();
                    break;
            }
            LogHelp._logHelpers.Add(lht, log);
        }

        public static void AddLogHelper<T>(LogHelperType lht, T logMsgReturn, string userID) where T : ILogMsgReturn, new()
        {
            Configure.Instance.UserID = userID;
            if (LogHelp._logHelpers.ContainsKey(lht))
                return;
            logMsgReturn = new T();
            ILog log;
            switch (lht)
            {
                case LogHelperType.Default:
                    log = (ILog)new DefaultLogHelper()
                    {
                        LogMsgReturn = (ILogMsgReturn)logMsgReturn
                    };
                    break;
                case LogHelperType.XXX:
                    log = (ILog)new XXX()
                    {
                        LogMsgReturn = (ILogMsgReturn)logMsgReturn
                    };
                    break;
                default:
                    log = (ILog)new DefaultLogHelper()
                    {
                        LogMsgReturn = (ILogMsgReturn)logMsgReturn
                    };
                    break;
            }
            LogHelp._logHelpers.Add(lht, log);
        }

        public static ILog Log(int index)
        {
            int num = 0;
            ILog log1 = (ILog)null;
            foreach (ILog log2 in LogHelp._logHelpers.Values)
            {
                if (num == index)
                {
                    log1 = log2;
                    break;
                }
            }
            if (log1 == null)
                log1 = (ILog)new DefaultLogHelper();
            return log1;
        }

        public static ILog Log(LogHelperType lht) => LogHelp._logHelpers.ContainsKey(lht) ? LogHelp._logHelpers[lht] : (ILog)new DefaultLogHelper();

        public void Dispose()
        {
            foreach (IDisposable disposable in LogHelp._logHelpers.Values)
                disposable.Dispose();
        }
    }
}

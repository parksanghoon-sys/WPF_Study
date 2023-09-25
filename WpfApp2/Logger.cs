using LogHelper;

namespace WpfApp2
{
    public static class Logger
    {
        public static ILog Log
        {
            get => LogHelper.LogHelp.Log(LogHelperType.Default);
        }
    }
}

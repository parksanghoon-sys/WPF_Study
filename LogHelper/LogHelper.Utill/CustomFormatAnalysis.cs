using System.Diagnostics;

namespace LogHelper.Utill
{
    internal class CustomFormatAnalysis
    {
        public static string Analysis(string customFormat)
        {
            if (string.IsNullOrWhiteSpace(customFormat))
                return DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff tt");
            int stackFrame = CustomFormatAnalysis.GetStackFrame();
            string str1 = customFormat;
            string str2 = customFormat;
            char[] separator = new char[1] { '}' };
            foreach (string str3 in str2.Split(separator, StringSplitOptions.RemoveEmptyEntries))
            {
                if (str3.IndexOf('{') >= 0)
                {
                    string format = str3.Substring(str3.IndexOf('{'), str3.Length - str3.IndexOf('{')).Replace("{", "");
                    switch (format)
                    {
                        case "M":
                            str1 = str1.Replace(string.Format("{{{0}}}", (object)format), new StackTrace(true).GetFrame(stackFrame).GetMethod().DeclaringType.ToString() + "." + new StackTrace(true).GetFrame(stackFrame).GetMethod().Name);
                            continue;
                        case "L":
                            str1 = str1.Replace(string.Format("{{{0}}}", (object)format), new StackTrace(true).GetFrame(stackFrame).GetFileLineNumber().ToString());
                            continue;
                        default:
                            try
                            {
                                str1 = str1.Replace(string.Format("{{{0}}}", (object)format), DateTime.Now.ToString(format));
                                continue;
                            }
                            catch
                            {
                                continue;
                            }
                    }
                }
            }
            return str1;
        }

        private static int GetStackFrame()
        {
            int stackFrame = 0;
            for (int index = 0; index < new StackTrace(true).FrameCount; ++index)
            {
                if (new StackTrace(true).GetFrame(index).GetMethod().DeclaringType.Name == "DefaultLogHelper" && new StackTrace(true).GetFrame(index + 1).GetMethod().DeclaringType.Name != "DefaultLogHelper")
                {
                    stackFrame = index;
                    break;
                }
            }
            return stackFrame;
        }
    }
}

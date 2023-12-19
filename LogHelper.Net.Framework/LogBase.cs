using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogHelper.Net.Framework
{
    public class LogBase
    {
        private const string _fileExtension = ".log";
        private static readonly string _lineIndent = $"{Environment.NewLine}\t\t\t\t\t";
        private static string _fileName;
        
        
        private static int _logStackFrame;
        private static int _callerStackFrame;

        private static readonly object _syncRoot = new Object();
        private static Queue<string> _logQueue = new Queue<string>();
        private static bool _isExecuting = false;
        public static FileMode FileMode { get; set; } = FileMode.Create;
        public static string LogLineHeaderFormat { get; set; } = "{0:yy-MM-dd HH:mm:ss.ffff}\t{1}\t{2}\t{3}\t{4}()\t";
        public static void AddFileListener(string fileName = null)
        {
            _fileName = fileName ?? AppDomain.CurrentDomain.BaseDirectory + "\\"
                + AppDomain.CurrentDomain.FriendlyName + _fileExtension;
            try
            {
                // Specifies that the operating system should create a new file. This requires Write permission.
                // If the file already exists, an IOException exception is thrown.
                if (FileMode is FileMode.CreateNew && File.Exists(_fileName)) File.Delete(_fileName);
                using (var file = File.Open(_fileName, FileMode)) ;
            }
            catch
            {
                Trace.TraceWarning(Texts.FileNotAvailable, _fileName);
                _fileName = null;                
            }
        }

        /// <summary>Displays a message box, prints logs and exits.</summary>
        /// <param name="format">Message format</param>
        /// <param name="messages">Messages</param>
        protected static async Task ExFail(string format, params object[] messages)
            => Trace.Fail( await PrintLog(format, messages));

        /// <summary>Output logs.</summary>
        /// <param name="format">Message format</param>
        /// <param name="messages">Messages</param>
        protected static void ExPrint(string format, params object[] messages)
            => Trace.WriteLine(PrintLog(format, messages));

        private static async Task<string> PrintLog(string format, params object[] messages)
        {
            var stackTrace = new StackTrace();
            if (_logStackFrame == 0)
            {
                for (var i = 1; i < stackTrace.FrameCount; i++)
                {
                    var className = stackTrace.GetFrame(i)?.GetMethod()?.DeclaringType?.Name;
                    if ((className == "ExTrace") || (className == "ExDebug"))
                    {
                        _logStackFrame = i;
                        _callerStackFrame = i + 1;
                        break;
                    }
                }
            }
            var log = stackTrace.GetFrame(_logStackFrame)?.GetMethod();
            var caller = stackTrace.GetFrame(_callerStackFrame)?.GetMethod();
            var text = new StringBuilder()
                .AppendFormat(LogLineHeaderFormat, DateTime.Now,
                    log?.DeclaringType?.Name, log?.Name, caller?.DeclaringType?.Name, caller?.Name)
                .AppendFormat(format, messages).Replace(Environment.NewLine, _lineIndent);

            if (_fileName is null == false)
            {
                _logQueue.Enqueue(text.ToString());
                if (!_isExecuting)
                {
                    _isExecuting = true;
                    _ = await WriteToFileAsync();
                }
                //File.AppendAllText(_fileName, text.ToString() + Environment.NewLine);
            }

            return text.ToString();
        }

        private static async Task WriteToFileAsync()
        {
            while (_logQueue.Count > 0)
            {
                string item;
                lock (_syncRoot)
                {
                    item = _logQueue.Dequeue();
                }

                using (StreamWriter logWriter = File.AppendText(_fileName))
                {
                    await logWriter.WriteLineAsync($"{item}");
                }               

            }
            lock (_syncRoot)
            {
                if (_logQueue.Count > 0)
                {
                    _ = WriteToFileAsync();
                    return;
                }
                else
                {
                    _isExecuting = false;
                }
            }
        }
    }
}

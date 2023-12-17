using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogHelper.Net.Framework
{
    public class ExTrace : LogBase
    {
        public static void Assert(bool condition, string format = "", params object[] messages)
        {
            if (!condition) ExFail(format, messages);
        }

        /// <summary>the message are displayed in a message box and exits.</summary>
        /// <param name="format">Message format</param>
        /// <param name="messages">Messages</param>
        public static void Fail(string format, params object[] messages) => ExFail(format, messages);

        /// <summary>the message and exception are displayed in a message box and exits.</summary>
        /// <param name="ex">Exception.</param>
        /// <param name="format">Message format</param>
        /// <param name="messages">Messages</param>
        public static void Error(Exception ex, string format = "", params object[] messages)
             => ExFail($"{format}{Environment.NewLine}{ex}", messages);

        /// <summary>Prints logs.</summary>
        /// <param name="format">Message format</param>
        /// <param name="messages">Messages</param>
        public static void Print(string format, params object[] messages) => ExPrint(format, messages);
        public static void PrintDDS(byte[] bytes)
        {
            var binaryString = "Binary :" + ByteToBinaryString(bytes);
            var hexString = "Hex :" + ByteToHexString(bytes);
            ExPrint(binaryString);
            ExPrint(hexString);
        }
        private static string ByteToBinaryString(byte[] bytes)
        {
            return string.Join(" ", bytes.Select(b => Convert.ToString(b, 2).PadLeft(8, '0')));
        }

        
        private static string ByteToHexString(byte[] bytes)
        {
            return string.Join(" ", bytes.Select(b => b.ToString("X2")));
        }

        /// <summary>Prints logs, if the condition is <c>true</c>.</summary>
        /// <param name="condition">Condition.</param>
        /// <param name="format">Message format</param>
        /// <param name="messages">Messages</param>
        public static void PrintIf(bool condition, string format, params object[] messages)
        {
            if (!condition) ExPrint(format, messages);
        }

        /// <summary>Prints warning logs.</summary>
        /// <param name="format">Message format</param>
        /// <param name="messages">Messages</param>
        public static void Warning(string format, params object[] messages) => ExPrint(format, messages);

        /// <summary>Prints warning logs, if the condition is <c>true</c>.</summary>
        /// <param name="condition">Condition.</param>
        /// <param name="format">Message format</param>
        /// <param name="messages">Messages</param>
        public static void WarningIf(bool condition, string format, params object[] messages)
        {
            if (!condition) ExPrint(format, messages);
        }
    }
}

using System.Collections;
using System.Text;

namespace LogHelper.Utill
{
    public class IOUtil
    {
        private static Hashtable _hashLock = new Hashtable();
        public static void CreateDirectory(string rootDirectoryPath, string directoryName)
        {
            if (Directory.Exists(rootDirectoryPath + "\\" + directoryName))
                return;
            Directory.CreateDirectory(rootDirectoryPath + "\\" + directoryName);
        }
        public static void DeleteDirectoryToLate(string rootDirectoryPath, string directoryName, int day)
        {
            foreach(DirectoryInfo directory in new DirectoryInfo(rootDirectoryPath + "\\" + directoryName).GetDirectories())
            {
                if (directory.LastWriteTime < DateTime.Now.AddDays((double)-day))
                    directory.Delete(true);
            }
        }
        public static void WriteFile(string rootDirectoryPath, string directoryName,string fileName, string msg)
        {
            ReaderWriterLock readerWriterLock = (ReaderWriterLock)null;
            lock (IOUtil._hashLock.SyncRoot)
            {
                if (IOUtil._hashLock.Contains((object)fileName))
                {
                    readerWriterLock = (ReaderWriterLock)IOUtil._hashLock[(object)fileName];
                }
                else
                {
                    readerWriterLock = new ReaderWriterLock();
                    IOUtil._hashLock.Add((object)fileName, (object)readerWriterLock);
                }
            }
            try
            {
                string path = rootDirectoryPath + "\\" + directoryName + "\\" + fileName;
                readerWriterLock.AcquireWriterLock(-1);
                try
                {
                    File.AppendAllText(path, msg, Encoding.UTF8);
                }
                finally
                {
                    readerWriterLock.ReleaseReaderLock();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("WriteFile Exception", ex);
            }
        }
        public static bool Exists(string rootDirectoryPath, string directoryName, string fileName)
        {
            string path = rootDirectoryPath + "\\" + directoryName + "\\" + fileName;
            if (string.IsNullOrEmpty(directoryName))
                path = rootDirectoryPath + "\\" + fileName;
            return File.Exists(path);
        }
    }
}

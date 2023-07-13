using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpfLazyTreeNode.Utils
{
    public class DirectoryUtils
    {
        public static bool IsDirectoryOnFileExists(string path)
        {
            IEnumerable<DirectoryInfo> directoryInfos = GetDirectories(path);
            if(directoryInfos.Any()) { return true; }

            IEnumerable<FileInfo> files = GetFiles(path);
            if(files.Any()) { return true; }
            return false;
        }

        public static IEnumerable<FileInfo> GetFiles(string path)
        {
            DirectoryInfo di = new DirectoryInfo(path);
            if(di.Exists)
            {
                return di.GetFiles("*",SearchOption.TopDirectoryOnly).Where(IsValidDirectoriesOrFiles);
            }
            return Enumerable.Empty<FileInfo>();
        }

        public static IEnumerable<DirectoryInfo> GetDirectories(string path)
        {
            DirectoryInfo di = new DirectoryInfo(path);
            if(di.Exists)
            {
                return di.GetDirectories("*",SearchOption.TopDirectoryOnly).Where(IsValidDirectoriesOrFiles);
            }
            return Enumerable.Empty<DirectoryInfo>();
        }

        private static bool IsValidDirectoriesOrFiles(FileSystemInfo arg)
        {
            try
            {
                // 숨겨진 파일 확인
                bool isHidden = (arg.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden;
                // 권한 폴더 체크
                bool isSystem = (arg.Attributes & FileAttributes.System) == FileAttributes.System;
                return !(isHidden || isSystem);
            }
            catch (UnauthorizedAccessException)
            {
                return false;

                throw;
            }
        }
    }
}

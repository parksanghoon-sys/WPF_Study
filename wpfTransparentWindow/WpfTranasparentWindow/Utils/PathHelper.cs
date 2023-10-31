namespace WpfTranasparentWindow.Utils
{
    public class PathHelper
    {
        public static string GetLocalDirectory(string subDir, string fileName)
        {
            return $"{System.AppDomain.CurrentDomain.BaseDirectory}\\{subDir}\\{fileName}";
        }
        public static string GetLocalImagesDirectory()
        {
            return $"{System.AppDomain.CurrentDomain.BaseDirectory}\\Images\\";
        }
    }
}

public class AppDomainHelper
    {
        public static string CurrentDirectoryPath()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }
    }
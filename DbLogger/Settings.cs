namespace DbLogger
{
    public class Settings
    {
        public static string LogFile { get; set; }
        public static string LogId { get; set; }
        public static bool OnlyConsoleOutput { get; set; }
        public static int DebugLevel { get; set; }

        // Do this when you start your application
        public static int mainThreadId;
        // If called in the non main thread, will return false;
        public static bool IsMainThread
        {
            get { return System.Threading.Thread.CurrentThread.ManagedThreadId == mainThreadId; }
        }
    }
}

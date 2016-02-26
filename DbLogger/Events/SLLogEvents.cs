using DbLogger.Models;

namespace DbLogger.Events
{
    public class SLLogEvents
    {
        public delegate void LogEventHandler(SLLogEventArgs args);

        public static event LogEventHandler ShowBallonTipp;
        public static event LogEventHandler ShowLogFile;

        public static void FireShowBallonTipp(SLLogEventArgs args)
        {
            if (ShowBallonTipp != null) ShowBallonTipp(args);
        }

        public static void FireShowLogFile()
        {
            if (ShowLogFile != null) ShowLogFile(new SLLogEventArgs());
        }
    }
}

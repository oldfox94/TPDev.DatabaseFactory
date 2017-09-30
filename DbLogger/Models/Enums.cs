namespace DbLogger.Models
{
    public enum LogType
    {
        Info,
        Warning,
        Error,
    }

    public class DebugLevelConstants
    {
        public const int NoLog = 0;
        public const int Low = 3;
        public const int Medium = 5;
        public const int High = 8;
        public const int VeryHigh = 10;
        public const int Unknow = 99;
    }
}

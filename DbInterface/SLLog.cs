using DbLogger.Models;
using System;

namespace DbInterface
{
    public class SLLog
    {
        public static DbLogger.DbLogger Logger { get; set; }

        public static void WriteInfo(string function, string message, bool onlyBallonTipp = false, string logId = "", bool onlyConsoleOutput = false)
        {
            if (Logger == null)
                Logger = new DbLogger.DbLogger(Environment.CurrentDirectory, "DbFactory", logId, onlyConsoleOutput);

            var data = new LogData
            {
                FunctionName = function,
                Message = message,
            };

            Logger.WriteInfo(data, onlyBallonTipp);
        }

        public static void WriteWarning(string function, string source, string message, string logId = "", bool onlyConsoleOutput = false)
        {
            if (Logger == null)
                Logger = new DbLogger.DbLogger(Environment.CurrentDirectory, "DbFactory", logId, onlyConsoleOutput);

            var data = new LogData
            {
                FunctionName = function,
                Source = source,
                Message = message,
            };

            Logger.WriteWarnng(data);
        }

        public static void WriteError(LogData data, string logId = "", bool onlyConsoleOutput = false)
        {
            if (Logger == null)
                Logger = new DbLogger.DbLogger(Environment.CurrentDirectory, "DbFactory", logId, onlyConsoleOutput);

            Logger.WriteError(data);
        }
    }
}

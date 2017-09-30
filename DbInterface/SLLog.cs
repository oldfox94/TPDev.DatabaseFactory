using DbLogger.Models;
using System;

namespace DbInterface
{
    public class SLLog
    {
        public static DbLogger.DbLogger Logger { get; set; }

        public static void WriteInfo(string function, string message, bool onlyBallonTipp = false, int debugLevel = DebugLevelConstants.Unknow, string logId = "", int initialDebugLevel = DebugLevelConstants.Medium, bool onlyConsoleOutput = false)
        {
            if (Logger == null)
                Logger = new DbLogger.DbLogger(Environment.CurrentDirectory, "DbFactory", logId, initialDebugLevel, onlyConsoleOutput);

            var data = new LogData
            {
                FunctionName = function,
                Message = message,
            };

            Logger.WriteInfo(data, onlyBallonTipp, debugLevel);
        }

        public static void WriteWarning(string function, string source, string message, int debugLevel = DebugLevelConstants.Unknow, string logId = "", int initialDebugLevel = DebugLevelConstants.Medium, bool onlyConsoleOutput = false)
        {
            if (Logger == null)
                Logger = new DbLogger.DbLogger(Environment.CurrentDirectory, "DbFactory", logId, initialDebugLevel, onlyConsoleOutput);

            var data = new LogData
            {
                FunctionName = function,
                Source = source,
                Message = message,
            };

            Logger.WriteWarnng(data, debugLevel);
        }

        public static void WriteError(LogData data, int debugLevel = DebugLevelConstants.Unknow, string logId = "", int initialDebugLevel = DebugLevelConstants.Medium, bool onlyConsoleOutput = false)
        {
            if (Logger == null)
                Logger = new DbLogger.DbLogger(Environment.CurrentDirectory, "DbFactory", logId, initialDebugLevel, onlyConsoleOutput);

            Logger.WriteError(data, debugLevel);
        }
    }
}

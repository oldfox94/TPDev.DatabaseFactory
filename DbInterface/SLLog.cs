using DbLogger.Models;
using System;

namespace DbInterface
{
    public class SLLog
    {
        public static DbLogger.DbLogger Logger { get; set; }

        public static LogData WriteInfo(string function, string message, bool onlyBallonTipp = false, int debugLevel = DebugLevelConstants.Unknow, string logId = "", int initialDebugLevel = DebugLevelConstants.Medium, bool onlyConsoleOutput = false, bool onlyReturnLogData = false, string logFileName = "")
        {
            if (Logger == null)
                Logger = new DbLogger.DbLogger(Environment.CurrentDirectory, "DbFactory", logId, initialDebugLevel, onlyConsoleOutput);

            var data = new LogData
            {
                FunctionName = function,
                Message = message,
                LogFileName = logFileName,
            };

            return Logger.WriteInfo(data, onlyBallonTipp, debugLevel, onlyReturnLogData);
        }

        public static LogData WriteWarning(string function, string source, string message, int debugLevel = DebugLevelConstants.Unknow, string logId = "", int initialDebugLevel = DebugLevelConstants.Medium, bool onlyConsoleOutput = false, bool onlyReturnLogData  = false, string logFileName = "")
        {
            if (Logger == null)
                Logger = new DbLogger.DbLogger(Environment.CurrentDirectory, "DbFactory", logId, initialDebugLevel, onlyConsoleOutput);

            var data = new LogData
            {
                FunctionName = function,
                Source = source,
                Message = message,
                LogFileName = logFileName,
            };

            return Logger.WriteWarnng(data, debugLevel, onlyReturnLogData);
        }

        public static LogData WriteError(LogData data, int debugLevel = DebugLevelConstants.Unknow, string logId = "", int initialDebugLevel = DebugLevelConstants.Medium, bool onlyConsoleOutput = false, bool onlyReturnLogData = false)
        {
            if (Logger == null)
                Logger = new DbLogger.DbLogger(Environment.CurrentDirectory, "DbFactory", logId, initialDebugLevel, onlyConsoleOutput);

            return Logger.WriteError(data, debugLevel, onlyReturnLogData);
        }
    }
}

using DbLogger.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbInterface
{
    public class SLLog
    {
        public static DbLogger.DbLogger Logger { get; set; }

        public static void WriteInfo(string function, string message)
        {
            if (Logger == null)
                Logger = new DbLogger.DbLogger(Environment.CurrentDirectory, "DbFactory");

            var data = new LogData
            {
                FunctionName = function,
                Message = message,
            };

            Logger.WriteInfo(data);
        }

        public static void WriteWarning(string function, string source, string message)
        {
            if (Logger == null)
                Logger = new DbLogger.DbLogger(Environment.CurrentDirectory, "DbFactory");

            var data = new LogData
            {
                FunctionName = function,
                Source = source,
                Message = message,
            };

            Logger.WriteWarnng(data);
        }

        public static void WriteError(LogData data)
        {
            if (Logger == null)
                Logger = new DbLogger.DbLogger(Environment.CurrentDirectory, "DbFactory");

            Logger.WriteError(data);
        }
    }
}

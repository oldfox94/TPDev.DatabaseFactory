using DbLogger.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace DbLogger
{
    public class DbLogger
    {
        private List<LogData> LogDataList { get; set; }
        public DbLogger(string logPath, string logFileName, string logId = "NoId")
        {
            LogDataList = new List<LogData>();

            Settings.LogId = logId;
            Settings.LogFile = Path.Combine(logPath, logFileName + ".log");
            if(!Directory.Exists(logPath))
            {
                Directory.CreateDirectory(logPath);
            }
        }

        public void WriteInfo(LogData data)
        {
            data.Type = LogType.Info;
            data.ExDate = DateTime.Now;

            LogToFile();
        }

        public void WriteWarnng(LogData data)
        {
            data.Type = LogType.Warning;
            data.ExDate = DateTime.Now;

            LogToFile();
        }

        public void WriteError(LogData data)
        {
            data.Type = LogType.Error;
            data.ExDate = DateTime.Now;

            if (data.Ex != null)
            {
                data.StackTrace = data.Ex.StackTrace;
                data.Message = data.Ex.Message;
            }

            LogToFile();
        }


        private void LogToFile()
        {

            var file = new StreamWriter(Settings.LogFile);
            foreach(var logEntry in LogDataList)
            {
                if (logEntry.IsInLogFile) continue;

                var line = string.Empty;
                try
                {
                    switch(logEntry.Type)
                    {
                        case LogType.Info:
                            line = string.Format(@"[Info]{1} => {2}: {0}Function: {3} {0}Message: {4}{0}{0}",
                                    Environment.NewLine, Settings.LogId, logEntry.ExDate.ToString(), logEntry.FunctionName, logEntry.Message);
                            break;

                        case LogType.Warning:
                            line = string.Format(@"[Warning]{1} => {2}: {0}Function: {3} {0}Source: {4} {0}Message: {5}{0}{0}",
                                    Environment.NewLine, Settings.LogId, logEntry.ExDate.ToString(), logEntry.FunctionName, logEntry.Source, logEntry.Message);
                            break;

                        case LogType.Error:
                            line = string.Format(@"[Error]{1} => {2}: {0}Function: {3} {0}Source: {4} {0}Message: {5} {0}StackTrace: {6}{0}{0}",
                                    Environment.NewLine, Settings.LogId, logEntry.ExDate.ToString(), logEntry.FunctionName, logEntry.Source, logEntry.Message, 
                                    logEntry.StackTrace);
                            break;
                    }

                    logEntry.IsInLogFile = true;
                }
                catch(Exception)
                {

                    logEntry.IsInLogFile = false;
                }
            }
        }
    }
}

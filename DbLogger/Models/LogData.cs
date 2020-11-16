﻿using System;

namespace DbLogger.Models
{
    public class LogData
    {
        public LogType Type { get; set; }

        public DateTime ExDate { get; set; }

        public string FunctionName { get; set; }
        public string Source { get; set; }
        public string StackTrace { get; set; }
        public string Message { get; set; }
        public string AdditionalMessage { get; set; }
        public int LineNumber { get; set; }

        public Exception Ex { get; set; }

        public string LogFileName { get; set; }
        public bool IsInLogFile { get; set; }
    }
}

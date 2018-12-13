using DbLogger.Events;
using DbLogger.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DbLogger
{
    public class DbLogger
    {
        private List<LogData> m_LogDataList { get; set; }
        public DbLogger(string logPath, string logFileName, string logId = "NoId", int debugLevel = DebugLevelConstants.Medium,
                        bool onlyConsoleOutput = false, int maxLogMBSize = 10)
        {
            m_LogDataList = new List<LogData>();

            Settings.mainThreadId = Thread.CurrentThread.ManagedThreadId;
            Settings.LogId = logId;
            Settings.DebugLevel = debugLevel;
            Settings.MaxLogFileSize = maxLogMBSize;

            Settings.LogFile = Path.Combine(logPath, logFileName + ".log");
            if(!Directory.Exists(logPath))
            {
                Directory.CreateDirectory(logPath);
            }

            SLLogEvents.ShowLogFile += OnShowLogFile;
        }

        private void OnShowLogFile(SLLogEventArgs args)
        {
            if (!File.Exists(Settings.LogFile)) return;
            Process.Start(Settings.LogFile);
        }

        public LogData WriteInfo(LogData data, bool onlyToolTipp = false, int debugLevel = DebugLevelConstants.Unknow, bool onlyReturnLogData = false)
        {
            if (Settings.DebugLevel < debugLevel && debugLevel != DebugLevelConstants.Unknow) return data;
            if (Settings.DebugLevel < DebugLevelConstants.High) return data;

            data.Type = LogType.Info;
            data.ExDate = DateTime.Now;

            if (onlyReturnLogData) return data;

            CreateBallonTipp(data);
            if (!onlyToolTipp)
            {
                m_LogDataList.Add(data);
                LogToFile();
            }
            return data;
        }

        public LogData WriteWarnng(LogData data, int debugLevel = DebugLevelConstants.Unknow, bool onlyReturnLogData = false)
        {
            if (Settings.DebugLevel < debugLevel && debugLevel != DebugLevelConstants.Unknow) return data;
            if (Settings.DebugLevel < DebugLevelConstants.Medium) return data;

            data.Type = LogType.Warning;
            data.ExDate = DateTime.Now;

            if (onlyReturnLogData) return data;

            m_LogDataList.Add(data);
            CreateBallonTipp(data);

            LogToFile();
            return data;
        }

        public LogData WriteError(LogData data, int debugLevel = DebugLevelConstants.Unknow, bool onlyReturnLogData = false)
        {
            if (Settings.DebugLevel < debugLevel && debugLevel != DebugLevelConstants.Unknow) return data;
            if (Settings.DebugLevel < DebugLevelConstants.Low) return data;

            data.Type = LogType.Error;
            data.ExDate = DateTime.Now;

            if (data.Ex != null)
            {
                data.StackTrace = data.Ex.StackTrace;
                data.Message = data.Ex.Message;

                // Get stack trace for the exception with source file information
                var st = new StackTrace(data.Ex, true);
                if (st != null)
                {
                    // Get the top stack frame
                    var frame = st.GetFrame(0);
                    if (frame != null) data.LineNumber = frame.GetFileLineNumber();
                }
            }

            if (onlyReturnLogData) return data;

            m_LogDataList.Add(data);
            CreateBallonTipp(data);

            LogToFile();
            return data;
        }

        private void CreateBallonTipp(LogData data)
        {
            if (Settings.OnlyConsoleOutput) return;

            var title = string.Empty;
            if (Settings.IsMainThread)
                SLLogEvents.FireShowBallonTipp(new SLLogEventArgs { Type = data.Type, Titel = "DatabaseFactory " + data.Type.ToString() + "!", LogMessage = data.Message });
        }

        private void LogToFile()
        {
            WriteToFile();
        }

        public string GetLogText(LogData logEntry)
        {
            switch (logEntry.Type)
            {
                case LogType.Info:
                    return string.Format(@"[Info]{1} => {2}: {0}Function: {3} {0}Message: {4}{5}{6}{0}{0}",
                            Environment.NewLine, Settings.LogId, logEntry.ExDate.ToString() + string.Format(" [{0}]", logEntry.ExDate.Millisecond), logEntry.FunctionName, logEntry.Message,
                            !string.IsNullOrEmpty(logEntry.AdditionalMessage)
                                ? $"{Environment.NewLine}Extra Msg.: {logEntry.AdditionalMessage}" : string.Empty,
                            logEntry.LineNumber != 0
                                ? string.Format(" [Line: {0}]", logEntry.LineNumber) : string.Empty);

                case LogType.Warning:
                    return string.Format(@"[Warning]{1} => {2}: {0}Function: {3} {0}Source: {4} {0}Message: {5}{6}{7}{0}{0}",
                            Environment.NewLine, Settings.LogId, logEntry.ExDate.ToString() + string.Format(" [{0}]", logEntry.ExDate.Millisecond), logEntry.FunctionName, logEntry.Source, logEntry.Message,
                            !string.IsNullOrEmpty(logEntry.AdditionalMessage)
                                ? $"{Environment.NewLine}Extra Msg.: {logEntry.AdditionalMessage}" : string.Empty,
                            logEntry.LineNumber != 0 
                                ? string.Format(" [Line: {0}]", logEntry.LineNumber) 
                                : string.Empty);

                case LogType.Error:
                    return string.Format(@"[Error]{1} => {2}: {0}Function: {3} {0}Source: {4} {0}Message: {5}{7} {0}StackTrace: {6}{8}{0}{0}",
                            Environment.NewLine, Settings.LogId, logEntry.ExDate.ToString() + string.Format(" [{0}]", logEntry.ExDate.Millisecond), logEntry.FunctionName, logEntry.Source, logEntry.Message,
                            logEntry.StackTrace,
                            !string.IsNullOrEmpty(logEntry.AdditionalMessage)
                                ? $"{Environment.NewLine}Extra Msg.: {logEntry.AdditionalMessage}" : string.Empty,
                            logEntry.LineNumber != 0
                                ? string.Format(" [Line: {0}]", logEntry.LineNumber) : string.Empty);
            }
            return string.Empty;
        }

        private DateTime m_LastCleanUp { get; set; }
        private void WriteToFile()
        {
            try
            {
                var currentList = new List<LogData>();
                currentList.AddRange(m_LogDataList);

                foreach (var logEntry in currentList.Where(x => x.IsInLogFile == false))
                {
                    if (logEntry.IsInLogFile) continue; //Sicherheitshalber => Sollten jedoch durch WHERE schon gefiltert werden

                    var line = string.Empty;
                    try
                    {
                        switch (logEntry.Type)
                        {
                            case LogType.Info:
                            case LogType.Warning:
                                line = GetLogText(logEntry);
                                WriteConsoleLog(line);
                                break;

                            case LogType.Error:
                                line = GetLogText(logEntry);

                                WriteConsoleLog(line);
                                WriteEventLog(line);
                                break;
                        }

                        WriteAsync(Settings.LogFile, line);
                        logEntry.IsInLogFile = true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error writing Log: " + ex.Message);
                        logEntry.IsInLogFile = false;
                    }
                }

                try
                {
                    if (m_LastCleanUp < DateTime.Now.AddMinutes(-15))
                    {
                        m_LastCleanUp = DateTime.Now;

                        var removeItemList = new List<LogData>();
                        foreach (var item in currentList.Where(x => x.IsInLogFile == true))
                            removeItemList.Add(item);

                        foreach (var item in removeItemList)
                            m_LogDataList.Remove(item);

                        GC.Collect();
                    }
                }
                catch(Exception ex)
                {
                    WriteError(new LogData
                    {
                        Source = ToString(),
                        FunctionName = "WriteToFile",
                        Ex = ex,
                    });
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error writing Log: " + ex.Message);
            }
        }

        private bool m_FileLocked { get; set; }
        private void WriteAsync(string path, string line)
        {
            if (Settings.OnlyConsoleOutput) return;
            CheckAndRenameLog();

            Task.Run(() =>
            {
                try
                {
                    var tryCnt = 0;
                    while(m_FileLocked)
                    {
                        if (tryCnt > 25)
                        {
                            Console.WriteLine($"Source: {ToString()}{Environment.NewLine}Message: WriteAync: TryCnt reached! => Abort!");
                            return;
                        }

                        Thread.Sleep(100);
                        tryCnt++;
                    }
                    m_FileLocked = true;

                    using (FileStream fs = new FileStream(path, FileMode.Append, FileAccess.Write))
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.WriteLine(line);
                    }
                    m_FileLocked = false;
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"Source: {ToString()}{Environment.NewLine}Message: Error writing Log: {ex.Message}");
                }
            });           
        }

        private void CheckAndRenameLog()
        {
            try
            {
                if (!File.Exists(Settings.LogFile)) return;

                if (Settings.MaxLogFileSize == 0) Settings.MaxLogFileSize = 10; //10MB
                var fi = new FileInfo(Settings.LogFile);
                var maxSizeInBytes = Settings.MaxLogFileSize * 1024 * 1024; //Multiple to MB
                if (fi.Length >= maxSizeInBytes)
                {
                    var dt = DateTime.Now;
                    var timestamp = $"{dt.Year}{dt.Month}{dt.Day}-{dt.Hour}{dt.Minute}";
                    var destFileName = $"{fi.Name.Replace(fi.Extension, "")}_{timestamp}.log";
                    if (File.Exists(Path.Combine(fi.Directory.FullName, destFileName)))
                        File.Delete(Path.Combine(fi.Directory.FullName, destFileName));

                    if (!File.Exists(Path.Combine(fi.Directory.FullName, destFileName)))
                        File.Move(Settings.LogFile, Path.Combine(fi.Directory.FullName, destFileName));
                    else
                        WriteWarnng(new LogData
                        {
                            FunctionName = "CheckAndRenameLog",
                            Source = ToString(),
                            Message = $"Date '{destFileName}' konnte nicht erstellt werden!",
                        });
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Source: {ToString()}{Environment.NewLine}Message: Error CheckAndRenameLog Log: {ex.Message}");
                WriteError(new LogData
                {
                    Source = ToString(),
                    FunctionName = "CheckAndRenameLog",
                    Ex = ex,
                });
            }
        }

        private void WriteEventLog(string message)
        {
            if (Settings.OnlyConsoleOutput) return;

            using (EventLog eventLog = new EventLog("TPDev.DatabaseFactory"))
            {
                eventLog.Source = "TPDev.DatabaseFactory";
                eventLog.WriteEntry(message, EventLogEntryType.Error);
            }
        }

        private void WriteConsoleLog(string message)
        {
            Console.WriteLine(message);
        }
    }
}

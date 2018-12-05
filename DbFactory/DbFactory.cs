using DbInterface;
using DbInterface.Interfaces;
using DbInterface.Models;
using DbLogger.Models;
using DbNotifyer.Models;
using MySQLLibrary.Operations;
using OracleLibrary.Operations;
using SQLiteLibrary.Operations;
using SQLLibrary.Operations;
using System;

namespace DatabaseFactory
{
    public class DbFactory
    {
        public IInsertOperations Insert { get; set; }
        public IUpdateOperations Update { get; set; }
        public IDeleteOperations Delete { get; set; }
        public ICheckOperations Check { get; set; }
        public IGetOperations Get { get; set; }
        public IExecuteOperations Execute { get; set; }


        public DbFactory(DbType type, DbConnectionData data)
        {
            DbFactorySettings.Type = type;
            switch(type)
            {
                case DbType.SQL:
                    new SQLLibrary.CONNECTION(data);
                    break;

                case DbType.SQLite:
                    new SQLiteLibrary.CONNECTION(data);
                    break;

                case DbType.MySQL:
                    new MySQLLibrary.CONNECTION(data);
                    break;

                case DbType.Oracle:
                    new OracleLibrary.CONNECTION(data);
                    break;
            }

            DbFactorySettings.Factory = this;

            Insert = GetInsertService();
            Update = GetUpdateService();
            Delete = GetDeleteService();
            Check = GetCheckService();
            Get = GetGetService();
            Execute = GetExecuteService();
        }

        public void InitLogger(string logFileName, string logPath = null, string logId = "", int debugLevel = DebugLevelConstants.Medium,
                               bool onlyConsoleOutput = false, int maxLogMBSize = 10)
        {
            if (string.IsNullOrEmpty(logPath))
                logPath = Environment.CurrentDirectory;

            if (string.IsNullOrEmpty(logId)) logId = DbFactorySettings.Type.ToString();
            SLLog.Logger = new DbLogger.DbLogger(logPath, logFileName, logId, debugLevel, onlyConsoleOutput, maxLogMBSize);
            SLLog.WriteInfo("InitLogger", "Logger successfully initialized!");
        }

        public void InitNotifyIcon(NotifyData notifyData)
        {
            SLNotify.NotifyIcon = new DbNotifyer.DbNotifyer(notifyData);
        }

        public IInsertOperations GetInsertService()
        {
            switch (DbFactorySettings.Type)
            {
                case DbType.SQL:
                    return new SQLInsert();

                case DbType.SQLite:
                    return new SQLiteInsert();

                case DbType.MySQL:
                    return new MySQLInsert();

                case DbType.Oracle:
                    return new OraInsert();
            }

            return null;
        }

        public IUpdateOperations GetUpdateService()
        {
            switch (DbFactorySettings.Type)
            {
                case DbType.SQL:
                    return new SQLUpdate();

                case DbType.SQLite:
                    return new SQLiteUpdate();

                case DbType.MySQL:
                    return new MySQLUpdate();

                case DbType.Oracle:
                    return new OraUpdate();
            }

            return null;
        }

        public IDeleteOperations GetDeleteService()
        {
            switch (DbFactorySettings.Type)
            {
                case DbType.SQL:
                    return new SQLDelete();

                case DbType.SQLite:
                    return new SQLiteDelete();

                case DbType.MySQL:
                    return new MySQLDelete();

                case DbType.Oracle:
                    return new OraDelete();
            }

            return null;
        }

        public ICheckOperations GetCheckService()
        {
            switch(DbFactorySettings.Type)
            {
                case DbType.SQL:
                    return new SQLCheck();

                case DbType.SQLite:
                    return new SQLiteCheck();

                case DbType.MySQL:
                    return new MySQLCheck();

                case DbType.Oracle:
                    return new OraCheck();
            }

            return null;
        }

        public IGetOperations GetGetService()
        {
            switch(DbFactorySettings.Type)
            {
                case DbType.SQL:
                    return new SQLGet();

                case DbType.SQLite:
                    return new SQLiteGet();

                case DbType.MySQL:
                    return new MySQLGet();

                case DbType.Oracle:
                    return new OraGet();
            }

            return null;
        }

        public IExecuteOperations GetExecuteService()
        {
            switch (DbFactorySettings.Type)
            {
                case DbType.SQL:
                    return new SQLExecute();

                case DbType.SQLite:
                    return new SQLiteExecute();

                case DbType.MySQL:
                    return new MySQLExecute();

                case DbType.Oracle:
                    return new OraExecute();
            }

            return null;
        }
    }
}

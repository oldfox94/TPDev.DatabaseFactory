using DbInterface;
using DbInterface.Interfaces;
using DbInterface.Models;
using System;

namespace DbFactory
{
    public class DbFactory
    {
        IInsertOperations Insert { get; set; }
        IUpdateOperations Update { get; set; }
        IDeleteOperations Delete { get; set; }
        IGetOperations Get { get; set; }
        IExecuteOperations Execute { get; set; }


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
            }

            DbFactorySettings.Factory = this;

            Insert = GetInsertService();
            Update = GetUpdateService();
            Delete = GetDeleteService();
            Get = GetGetService();
            Execute = GetExecuteService();
        }

        public void InitLogger(string logFileName, string logPath = null)
        {
            if (string.IsNullOrEmpty(logPath))
                logPath = Environment.CurrentDirectory;

            SLLog.Logger = new DbLogger.DbLogger(logFileName, logPath, DbFactorySettings.Type.ToString());
            SLLog.WriteInfo("InitLogger", "Logger successfully initialized!");
        }


        public IInsertOperations GetInsertService()
        {
            switch (DbFactorySettings.Type)
            {
                case DbType.SQL:
                    return null;

                case DbType.SQLite:
                    return null;

                case DbType.MySQL:
                    return null;
            }

            return null;
        }

        public IUpdateOperations GetUpdateService()
        {
            switch (DbFactorySettings.Type)
            {
                case DbType.SQL:
                    return null;

                case DbType.SQLite:
                    return null;

                case DbType.MySQL:
                    return null;
            }

            return null;
        }

        public IDeleteOperations GetDeleteService()
        {
            switch (DbFactorySettings.Type)
            {
                case DbType.SQL:
                    return null;

                case DbType.SQLite:
                    return null;

                case DbType.MySQL:
                    return null;
            }

            return null;
        }

        public IGetOperations GetGetService()
        {
            switch(DbFactorySettings.Type)
            {
                case DbType.SQL:
                    return null;

                case DbType.SQLite:
                    return null;

                case DbType.MySQL:
                    return null;
            }

            return null;
        }

        public IExecuteOperations GetExecuteService()
        {
            switch (DbFactorySettings.Type)
            {
                case DbType.SQL:
                    return null;

                case DbType.SQLite:
                    return null;

                case DbType.MySQL:
                    return null;
            }

            return null;
        }
    }
}

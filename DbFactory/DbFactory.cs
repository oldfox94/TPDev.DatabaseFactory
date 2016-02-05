using DbInterface;
using DbInterface.Interfaces;
using DbInterface.Models;
using MySQLLibrary.Operations;
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
            }

            DbFactorySettings.Factory = this;

            Insert = GetInsertService();
            Update = GetUpdateService();
            Delete = GetDeleteService();
            Check = GetCheckService();
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
                    return new SQLiteInsert();

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
                    return new SQLiteUpdate();

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
                    return new SQLiteDelete();

                case DbType.MySQL:
                    return null;
            }

            return null;
        }

        public ICheckOperations GetCheckService()
        {
            switch(DbFactorySettings.Type)
            {
                case DbType.SQL:
                    return null;

                case DbType.SQLite:
                    return new SQLiteCheck();

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
                    return new SQLiteGet();

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
                    return new SQLExecute();

                case DbType.SQLite:
                    return new SQLiteExecute();

                case DbType.MySQL:
                    return new MySQLExecute();
            }

            return null;
        }
    }
}

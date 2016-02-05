﻿using DbInterface;
using DbInterface.Interfaces;
using DbLogger.Models;
using System;

namespace SQLiteLibrary.Operations
{
    public class SQLiteCheck : ICheckOperations
    {
        SQLiteExecute m_Execute { get; set; }
        public SQLiteCheck()
        {
            m_Execute = new SQLiteExecute();
        }

        public bool ColumnExists(string tableName, string columnName)
        {
            try
            {

                return true;
            }
            catch(Exception ex)
            {
                SLLog.WriteError(new LogData
                {
                    Source = ToString(),
                    FunctionName = "ColumnExists Error!",
                    Ex = ex,
                });
                return false;
            }
        }

        public bool ColumnValueExists(string table, string column, string value)
        {
            try
            {
                var resultObj = m_Execute.ExecuteScalar(string.Format(@"SELECT {1} FROM {0} WHERE {1} = '{2}'", table, column, value));
                return resultObj != null;
            }
            catch(Exception ex)
            {
                SLLog.WriteError(new LogData
                {
                    Source = ToString(),
                    FunctionName = "ColumnValueExists Error!",
                    Ex = ex,
                });
                return false;
            }
        }

        public bool TableExists(string table)
        {
            try
            {
                var result = m_Execute.ExecuteScalar(string.Format(@"SELECT name FROM sqlite_master WHERE type='table' AND name='{0}'", table));
                if (result == null) return false;

                return result.ToString() == table;
            }
            catch(Exception ex)
            {
                SLLog.WriteError(new LogData
                {
                    Source = ToString(),
                    FunctionName = "TableExists Error!",
                    Ex = ex,
                });
                return false;
            }
        }
    }
}

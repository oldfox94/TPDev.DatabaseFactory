using DbInterface;
using DbInterface.Interfaces;
using DbInterface.Models;
using DbLogger.Models;
using System;
using System.Data;

namespace SQLLibrary.Operations
{
    public class SQLCheck : ICheckOperations
    {
        SQLExecute m_Execute { get; set; }
        public SQLCheck()
        {
            m_Execute = new SQLExecute();
        }

        public bool ColumnExists(string tableName, string columnName)
        {
            try
            {
                var result = false;

                var sql = string.Format("SELECT * FROM {0} WHERE ColumnName = '{1}'", tableName, columnName);
                var tblSchema = m_Execute.ExecuteReadTableSchema(sql);

                foreach(DataRow dr in tblSchema.Rows)
                {
                    if(dr["ColumnName"].ToString() == columnName)
                    {
                        result = true;
                        break;
                    }
                }

                return result;
            }
            catch (Exception ex)
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
            catch (Exception ex)
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
                var result = m_Execute.ExecuteScalar(string.Format(@"SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='{0}'", table));
                if (result == null) return false;

                return result.ToString() == table;
            }
            catch (Exception ex)
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

        public bool DatabaseExists(string databaseName)
        {
            var result = false;
            try
            {
                new CONNECTION(new DbConnectionData { Name = "master", ServerName = Settings.ConnectionData.ServerName, Instance = Settings.ConnectionData.Instance,
                                                      User = Settings.ConnectionData.User, Password = Settings.ConnectionData.Password }, false);

                var exResult = m_Execute.ExecuteScalar(string.Format(@"SELECT name FROM master.dbo.sysdatabases WHERE name = '{0}'", databaseName));
                if (exResult == null)
                    result = false;
                else
                    result = exResult.ToString() == databaseName;
            }
            catch(Exception ex)
            {
                SLLog.WriteError(new LogData
                {
                    Source = ToString(),
                    FunctionName = "DatabaseExists Error!",
                    Ex = ex,
                });
                result = false;
            }

            new CONNECTION(Settings.ConnectionData);
            return result;
        }
    }
}

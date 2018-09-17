using DbInterface;
using DbInterface.Interfaces;
using DbLogger.Models;
using System;
using System.Data;

namespace MySQLLibrary.Operations
{
    public class MySQLCheck : ICheckOperations
    {
        MySQLExecute m_Execute { get; set; }
        public MySQLCheck()
        {
            m_Execute = new MySQLExecute();
        }

        public bool ColumnExists(string tableName, string columnName)
        {
            try
            {
                var result = false;

                var sql = string.Format("SELECT * FROM {0} WHERE ColumnName = '{1}'", tableName, columnName);
                var tblSchema = m_Execute.ExecuteReadTableSchema(sql);

                foreach (DataRow dr in tblSchema.Rows)
                {
                    if (dr["ColumnName"].ToString() == columnName)
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
                if (Settings.ThrowExceptions) throw new Exception("ColumnExists Error!", ex);
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
                if (Settings.ThrowExceptions) throw new Exception("ColumnValueExists Error!", ex);
                return false;
            }
        }

        public bool TableExists(string table)
        {
            try
            {
                var result = m_Execute.ExecuteScalar(string.Format(@"SHOW TABLES LIKE '{0}'", table));
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
                if (Settings.ThrowExceptions) throw new Exception("TableExists Error!", ex);
                return false;
            }
        }

        public bool DatabaseExists(string databaseName)
        {
            var result = false;
            try
            {
                var exResult = m_Execute.ExecuteScalar(string.Format(@"SELECT SCHEMA_NAME FROM INFORMATION_SCHEMA.SCHEMATA WHERE SCHEMA_NAME = '{0}'", databaseName));
                if (exResult == null)
                    result = false;
                else
                    result = exResult.ToString() == databaseName;
                return result;
            }
            catch (Exception ex)
            {
                SLLog.WriteError(new LogData
                {
                    Source = ToString(),
                    FunctionName = "DatabaseExists Error!",
                    Ex = ex,
                });
                if (Settings.ThrowExceptions) throw new Exception("DatabaseExists Error!", ex);
                return false;
            }
        }
    }
}

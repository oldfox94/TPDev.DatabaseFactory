using DbInterface.Interfaces;
using System;
using System.Collections.Generic;
using DbInterface.Models;
using MySql.Data.MySqlClient;
using DbLogger.Models;
using DbInterface;

namespace MySQLLibrary.Operations
{
    public class MySQLExecute : IExecuteOperations
    {
        public int ExecuteNonQuery(string sql)
        {
            int rowsUpdated = 0;
            try
            {
                Settings.Con.Open();

                MySqlCommand cmd = new MySqlCommand(sql, Settings.Con);
                rowsUpdated = cmd.ExecuteNonQuery();

                Settings.Con.Close();
            }
            catch (Exception ex)
            {
                SLLog.WriteError(new LogData
                {
                    Source = ToString(),
                    FunctionName = "ExecuteNonQuery",
                    Ex = ex,
                });
                return -1;
            }

            return rowsUpdated;
        }

        public int ExecuteNonQuery(List<string> sqlList)
        {
            throw new NotImplementedException();
        }

        public object ExecuteScalar(string sql)
        {
            throw new NotImplementedException();
        }

        public bool RenewTbl(string tableName, List<ColumnData> columns)
        {
            throw new NotImplementedException();
        }

        public bool RenewTbl(string tableName, Dictionary<string, string> columns)
        {
            throw new NotImplementedException();
        }
    }
}

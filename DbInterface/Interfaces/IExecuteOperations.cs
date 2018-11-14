using DbInterface.Models;
using System.Collections.Generic;
using System.Data;

namespace DbInterface.Interfaces
{
    public interface IExecuteOperations
    {
        int ExecuteNonQuery(List<string> sqlList);
        int ExecuteNonQuery(string sql);

        object ExecuteScalar(string sql);

        DataTable ExecuteReadTable(string sql);
        DataTable ExecuteReadTableSchema(string sql);

        string ExecuteReadTableName(string columnName);

        bool RenewTbl(string tableName, Dictionary<string, string> columns, bool cleanUpAfterRenew = false);
        bool RenewTbl(string tableName, List<ColumnData> columns, bool cleanUpAfterRenew = false);
    }
}

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

        DataTable ExecuteReadTable(string sql, bool overwriteTableName = false);
        DataTable ExecuteReadTableSchema(string sql);

        string ExecuteReadTableName(string columnName);

        bool RenewTbl(string tableName, Dictionary<string, string> columns, List<IndizesData> indizes = null, bool cleanUpAfterRenew = false);
        bool RenewTbl(string tableName, List<ColumnData> columns, List<IndizesData> indizes = null, bool cleanUpAfterRenew = false);
    }
}

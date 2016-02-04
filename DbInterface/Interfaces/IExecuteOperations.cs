using DbInterface.Models;
using System.Collections.Generic;

namespace DbInterface.Interfaces
{
    public interface IExecuteOperations
    {
        int ExecuteNonQuery(List<string> sqlList);
        int ExecuteNonQuery(string sql);

        object ExecuteScalar(string sql);

        bool RenewTbl(string tableName, Dictionary<string, string> columns);
        bool RenewTbl(string tableName, List<ColumnData> columns);
    }
}

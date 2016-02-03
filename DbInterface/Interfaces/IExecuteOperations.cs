using DbInterface.Models;
using System.Collections.Generic;

namespace DbInterface.Interfaces
{
    public interface IExecuteOperations
    {
        bool ExecuteNonQuery(List<string> sqlList);
        bool ExecuteNonQuery(string sql);

        bool RenewTbl(string tableName, Dictionary<string, string> columns);
        bool RenewTbl(string tableName, List<ColumnData> columns);
    }
}

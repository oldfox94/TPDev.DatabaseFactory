using System.Collections.Generic;
using System.Data;

namespace DbInterface.Interfaces
{
    public interface IGetOperations
    {
        bool TableExists(string table);
        bool ColumnExists(string tableName, string columnName);
        bool ColumnValueExists(string table, string column, string value);


        DataSet GetDataSet(List<string> sqlList);

        DataTable GetTableSchema(string tableName);
        DataTable GetTable(string sql);
        DataTable GetTable(string tableName, string where = null, string orderBy = null);

        DataRow GetRow(string sql);
        DataRow GetRow(string tableName, string where = null, string orderBy = null);

        string GetValueFromColumn(string table, string column, string where);
    }
}

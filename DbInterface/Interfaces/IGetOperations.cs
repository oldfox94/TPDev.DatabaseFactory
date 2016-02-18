using System.Collections.Generic;
using System.Data;

namespace DbInterface.Interfaces
{
    public interface IGetOperations
    {
        DataSet GetDataSet(List<string> tblSqlDict, string dataSetName);

        DataTable GetTableSchema(string tableName);

        DataTable GetTable(string sql);
        DataTable GetTable(string tableName, string where = null, string orderBy = null);

        DataRow GetRow(string sql);
        DataRow GetRow(string tableName, string where = null, string orderBy = null);

        string GetValueFromColumn(string tableName, string columnName, string where);

        string GetLastSortOrder(string tableName, string sortOrderColName, string where = null);
        string GetNextSortOrder(string tableName, string sortOrderColName, string where = null);
    }
}

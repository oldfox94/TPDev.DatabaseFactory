using System.Collections.Generic;
using System.Data;

namespace DbInterface.Interfaces
{
    public interface IGetOperations
    {
        DataSet GetDataSet(List<string> tblSqlDict, string dataSetName, string additionalMessage = "");

        DataTable GetTableSchema(string tableName, string additionalMessage = "");

        DataTable GetTable(string sql, string additionalMessage = "");
        DataTable GetTable(string tableName, string where, string orderBy = null, string additionalMessage = "");

        DataRow GetRow(string sql, string additionalMessage = "");
        DataRow GetRow(string tableName, string where, string orderBy = null, string additionalMessage = "");

        string GetTableNameFromColumn(string columnName, string additionalMessage = "");
        string GetValueFromColumn(string tableName, string columnName, string where, string additionalMessage = "");

        string GetLastSortOrder(string tableName, string sortOrderColName, string where = null, string additionalMessage = "");
        string GetNextSortOrder(string tableName, string sortOrderColName, string where = null, string additionalMessage = "");
    }
}

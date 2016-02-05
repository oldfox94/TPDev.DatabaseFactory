using DbInterface.Models;
using System.Collections.Generic;
using System.Data;

namespace DbInterface.Interfaces
{
    public interface IInsertOperations
    {
        bool InsertRow(string tableName, DataRow row);
        bool InsertValue(string tableName, string columnName, string value);
        bool InsertValue(string tableName, Dictionary<string, string> data);

        bool CreateTable(string tableName, List<ColumnData> columns);
        bool CreateTable(string tableName, Dictionary<string, string> columns);
    }
}

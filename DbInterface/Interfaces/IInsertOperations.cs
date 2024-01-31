using DbInterface.Models;
using System.Collections.Generic;
using System.Data;

namespace DbInterface.Interfaces
{
    public interface IInsertOperations
    {
        bool CreateDatabase(string databaseName);

        bool CreateTable(string tableName, List<ColumnData> columns, List<IndizesData> indizes = null);
        bool CreateTable(string tableName, Dictionary<string, string> columns, List<IndizesData> indizes = null);

        bool InsertRow(string tableName, DataRow row, bool setInsertOn = true);
        bool InsertValue(string tableName, string columnName, string value, bool setInsertOn = true);
        bool InsertValue(string tableName, Dictionary<string, string> data, bool setInsertOn = true);
    }
}

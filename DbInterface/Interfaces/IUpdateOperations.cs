using System.Collections.Generic;
using System.Data;

namespace DbInterface.Interfaces
{
    public interface IUpdateOperations
    {
        bool UpdateDataSet(DataSet dataSet);

        bool UpdateTables(List<DataTable> tableList);
        bool UpdateTable(DataTable table, string tableName);

        bool UpdateOneValue(string tableName, string column, string value, string where);
    }
}

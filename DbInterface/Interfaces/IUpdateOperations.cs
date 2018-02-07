using System.Collections.Generic;
using System.Data;

namespace DbInterface.Interfaces
{
    public interface IUpdateOperations
    {
        bool UpdateDataSet(DataSet dataSet, bool setInsertOn = true, bool setModifyOn = true);

        bool UpdateTables(List<DataTable> tableList, bool setInsertOn = true, bool setModifyOn = true);
        bool UpdateTable(DataTable table, bool setInsertOn = true, bool setModifyOn = true);
        bool UpdateTable(DataTable table, string tableName, bool setInsertOn = true, bool setModifyOn = true);

        bool UpdateOneValue(string tableName, string column, string value, string where);
    }
}

using System.Collections.Generic;
using System.Data;

namespace DbInterface.Interfaces
{
    public interface IUpdateOperations
    {
        bool UpdateDataSet(DataSet dataSet, bool setInsertOn = true, bool setModifyOn = true, string additionalMessage = "");

        bool UpdateTables(List<DataTable> tableList, bool setInsertOn = true, bool setModifyOn = true, string additionalMessage = "");

        bool UpdateTable(DataTable table, bool setInsertOn = true, bool setModifyOn = true, string additionalMessage = "");
        bool UpdateTable(DataTable table, string tableName, bool setInsertOn = true, bool setModifyOn = true, string additionalMessage = "");

        bool UpdateOneValue(string tableName, string column, string value, string where, string additionalMessage = "");
    }
}

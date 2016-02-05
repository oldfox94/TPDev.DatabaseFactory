using DbInterface;
using DbInterface.Helpers;
using DbInterface.Interfaces;
using DbInterface.Models;
using DbLogger.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;

namespace SQLiteLibrary.Operations
{
    public class SQLiteUpdate : IUpdateOperations
    {
        SQLiteExecute m_Execute { get; set; }
        public SQLiteUpdate()
        {
            m_Execute = new SQLiteExecute();
        }

        public bool UpdateDataSet(DataSet dataSet)
        {
            throw new NotImplementedException();
        }

        public bool UpdateOneValue(string tableName, string column, string value, string where)
        {
            try
            {
                var whereCnd = ConvertionHelper.GetWhere(where);

                var sql = string.Format(@"UPDATE {0} SET {1} = '{2}', {3} = '{4}' WHERE {5}",
                            tableName, column, ConvertionHelper.CleanStringForSQL(value), DbCIC.ModifyOn, DateTime.Now.ToString(), whereCnd);
                var result = m_Execute.ExecuteNonQuery(sql);

                if (result == -1) return false;
                return true;
            }
            catch(Exception ex)
            {
                SLLog.WriteError(new LogData
                {
                    Source = ToString(),
                    FunctionName = "UpdateOneValue Error!",
                    Ex = ex,
                });
                return false;
            }
        }

        public bool UpdateTable(DataTable table, string tableName)
        {
            try
            {
                Settings.Con.Open();

                var adapter = new SQLiteDataAdapter(string.Format(@"SELECT * FROM {0}", tableName), Settings.Con);
                var cmd = new SQLiteCommandBuilder(adapter);
                adapter.Update(table);

                Settings.Con.Close();

                return true;
            }
            catch(Exception ex)
            {
                SLLog.WriteError(new LogData
                {
                    Source = ToString(),
                    FunctionName = "UpdateOneValue Error!",
                    Ex = ex,
                });
                return false;
            }
        }

        public bool UpdateTables(List<DataTable> tableList)
        {
            throw new NotImplementedException();
        }
    }
}

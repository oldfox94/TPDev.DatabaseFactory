using DbInterface;
using DbInterface.Helpers;
using DbInterface.Interfaces;
using DbLogger.Models;
using System;
using System.Data;

namespace MySQLLibrary.Operations
{
    public class MySQLDelete : IDeleteOperations
    {
        MySQLExecute m_Execute { get; set; }
        MySQLGet m_Get { get; set; }
        public MySQLDelete()
        {
            m_Execute = new MySQLExecute();
            m_Get = new MySQLGet();
        }

        public bool ClearDatabase(string databaseName)
        {
            try
            {
                var result = true;

                var tbl = m_Get.GetTable(string.Format(@"SELECT NAME FROM {0} WHERE type = 'table' ORDER BY NAME", ConvertionHelper.GetMasterTable(Settings.Type)), "MASTER");
                foreach (DataRow dr in tbl.Rows)
                {
                    var clearResult = ClearTable(dr["NAME"].ToString());
                    if (!clearResult) result = false;
                }

                return result;
            }
            catch (Exception ex)
            {
                SLLog.WriteError(new LogData
                {
                    Source = ToString(),
                    FunctionName = "ClearDatabase Error!",
                    Ex = ex,
                });
                return false;
            }
        }

        public bool ClearTable(string tableName)
        {
            try
            {
                var result = m_Execute.ExecuteNonQuery(string.Format(@"DELETE FROM {0}", tableName));
                if (result == -1) return false;
                return true;
            }
            catch (Exception ex)
            {
                SLLog.WriteError(new LogData
                {
                    Source = ToString(),
                    FunctionName = "ClearTable Error!",
                    Ex = ex,
                });
                return false;
            }
        }

        public bool DeleteDatabase(string databaseName)
        {
            throw new NotImplementedException();
        }

        public bool DeleteRows(string tableName, string where)
        {
            try
            {
                var whereCnd = ConvertionHelper.GetWhere(where);

                var result = m_Execute.ExecuteNonQuery(string.Format(@"DELETE FROM {0} {1}", tableName, whereCnd));
                if (result == -1) return false;
                return true;
            }
            catch (Exception ex)
            {
                SLLog.WriteError(new LogData
                {
                    Source = ToString(),
                    FunctionName = "DeleteRows Error!",
                    Ex = ex,
                });
                return false;
            }
        }

        public bool DeleteTable(string tableName)
        {
            throw new NotImplementedException();
        }
    }
}

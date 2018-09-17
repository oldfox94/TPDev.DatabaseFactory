using DbInterface;
using DbInterface.Helpers;
using DbInterface.Interfaces;
using DbLogger.Models;
using System;
using System.Data;

namespace OracleLibrary.Operations
{
    public class OraDelete : IDeleteOperations
    {
        OraExecute m_Execute { get; set; }
        OraGet m_Get { get; set; }
        public OraDelete()
        {
            m_Execute = new OraExecute();
            m_Get = new OraGet();
        }

        public bool ClearTable(string tableName)
        {
            try
            {
                var result = m_Execute.ExecuteNonQuery(string.Format(@"DELETE FROM {0}", tableName));
                if (result == -2) return false;
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
                if (Settings.ThrowExceptions) throw new Exception("ClearTable Error!", ex);
                return false;
            }
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
                if (Settings.ThrowExceptions) throw new Exception("ClearDatabase Error!", ex);
                return false;
            }
        }

        public bool DeleteRows(string tableName, string where)
        {
            try
            {
                var whereCnd = ConvertionHelper.GetWhere(where);

                var result = m_Execute.ExecuteNonQuery(string.Format(@"DELETE FROM {0} {1}", tableName, whereCnd));
                if (result == -2) return false;
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
                if (Settings.ThrowExceptions) throw new Exception("DeleteRows Error!", ex);
                return false;
            }
        }

        public bool DeleteTable(string tableName)
        {
            try
            {
                var result = m_Execute.ExecuteNonQuery(string.Format(@"DROP TABLE {0}", tableName));
                if (result == -2) return false;
                return true;
            }
            catch (Exception ex)
            {
                SLLog.WriteError(new LogData
                {
                    Source = ToString(),
                    FunctionName = "DeleteTable Error!",
                    Ex = ex,
                });
                if (Settings.ThrowExceptions) throw new Exception("DeleteTable Error!", ex);
                return false;
            }
        }

        public bool DeleteDatabase(string databaseName)
        {
            try
            {
                var result = m_Execute.ExecuteNonQuery(string.Format(@"DROP DATABASE {0}", databaseName));
                if (result == -2) return false;
                return true;
            }
            catch (Exception ex)
            {
                SLLog.WriteError(new LogData
                {
                    Source = ToString(),
                    FunctionName = "DeleteDatabase Error!",
                    Ex = ex,
                });
                if (Settings.ThrowExceptions) throw new Exception("DeleteDatabase Error!", ex);
                return false;
            }
        }
    }
}

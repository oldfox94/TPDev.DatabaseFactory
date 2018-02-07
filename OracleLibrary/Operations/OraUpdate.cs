using DbInterface;
using DbInterface.Helpers;
using DbInterface.Interfaces;
using DbInterface.Models;
using DbLogger.Models;
using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace OracleLibrary.Operations
{
    public class OraUpdate : IUpdateOperations
    {
        OraExecute m_Execute { get; set; }
        public OraUpdate()
        {
            m_Execute = new OraExecute();
        }

        public bool UpdateDataSet(DataSet dataSet, bool setInsertOn = true, bool setModifyOn = true)
        {
            try
            {
                var result = false;
                foreach (DataTable tbl in dataSet.Tables)
                {
                    result = UpdateTable(tbl, setInsertOn, setModifyOn);
                    if (!result) break;
                }

                return result;
            }
            catch (Exception ex)
            {
                SLLog.WriteError(new LogData
                {
                    Source = ToString(),
                    FunctionName = "UpdateDataSet Error!",
                    Ex = ex,
                });
                return false;
            }
        }

        public bool UpdateTables(List<DataTable> tableList, bool setInsertOn = true, bool setModifyOn = true)
        {
            try
            {
                var result = false;
                foreach (DataTable tbl in tableList)
                {
                    result = UpdateTable(tbl, setInsertOn, setModifyOn);
                }

                return result;
            }
            catch (Exception ex)
            {
                SLLog.WriteError(new LogData
                {
                    Source = ToString(),
                    FunctionName = "UpdateTables Error!",
                    Ex = ex,
                });
                return false;
            }
        }

        public bool UpdateTable(DataTable table, bool setInsertOn = true, bool setModifyOn = true)
        {
            try
            {
                var tableName = table.TableName;
                return UpdateTable(table, tableName, setInsertOn, setModifyOn);
            }
            catch (Exception ex)
            {
                SLLog.WriteError(new LogData
                {
                    Source = ToString(),
                    FunctionName = "UpdateTable Error!",
                    Ex = ex,
                });
                return false;
            }
        }

        public bool UpdateTable(DataTable table, string tableName, bool setInsertOn = true, bool setModifyOn = true)
        {
            try
            {
                TableHelper.SetDefaultColumnValues(table, setInsertOn, setModifyOn);

                var con = CONNECTION.OpenCon();

                var adapter = new OracleDataAdapter(string.Format(@"SELECT * FROM {0}", tableName), con);
                var cmd = new OracleCommandBuilder(adapter);
                adapter.Update(table);

                cmd.Dispose();
                adapter.Dispose();
                CONNECTION.CloseCon(con);

                return true;
            }
            catch (DBConcurrencyException cex)
            {
                SLLog.WriteError(new LogData
                {
                    Source = ToString(),
                    FunctionName = "UpdateTable DBConcurrencyError!",
                    Ex = cex,
                });
                return false;
            }
            catch (Exception ex)
            {
                SLLog.WriteError(new LogData
                {
                    Source = ToString(),
                    FunctionName = "UpdateTable Error!",
                    Ex = ex,
                });
                return false;
            }
        }

        public bool UpdateOneValue(string tableName, string column, string value, string where)
        {
            try
            {
                var whereCnd = ConvertionHelper.GetWhere(where);

                var sql = string.Format(@"UPDATE {0} SET {1} = '{2}', {3} = '{4}' {5}",
                            tableName, column, ConvertionHelper.CleanStringForSQL(value), DbCIC.ModifyOn, DateTime.Now.ToString(), whereCnd);
                var result = m_Execute.ExecuteNonQuery(sql);

                if (result == -2) return false;
                return true;
            }
            catch (Exception ex)
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
    }
}

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
            try
            {
                var result = false;
                foreach(DataTable tbl in dataSet.Tables)
                {
                    result = UpdateTable(tbl);
                    if (!result) break;
                }

                return result;
            }
            catch(Exception ex)
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

        public bool UpdateOneValue(string tableName, string column, string value, string where)
        {
            try
            {
                var whereCnd = ConvertionHelper.GetWhere(where);

                var sql = string.Format(@"UPDATE {0} SET {1} = '{2}', {3} = '{4}' {5}",
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

        public bool UpdateTable(DataTable table)
        {
            try
            {
                var tableName = table.TableName;
                return UpdateTable(table, tableName);
            }
            catch(Exception ex)
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

        public bool UpdateTable(DataTable table, string tableName)
        {
            try
            {
                TableHelper.SetDefaultColumnValues(table);

                var con = CONNECTION.OpenCon();

                var adapter = new SQLiteDataAdapter(string.Format(@"SELECT * FROM {0}", tableName), con);
                var cmd = new SQLiteCommandBuilder(adapter);
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
            catch(Exception ex)
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

        public bool UpdateTables(List<DataTable> tableList)
        {
            try
            {
                var result = false;
                foreach(DataTable tbl in tableList)
                {
                    result = UpdateTable(tbl);
                }

                return result;
            }
            catch(Exception ex)
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
    }
}

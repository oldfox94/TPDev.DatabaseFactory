using DbInterface;
using DbInterface.Helpers;
using DbInterface.Interfaces;
using DbInterface.Models;
using DbLogger.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace MySQLLibrary.Operations
{
    public class MySQLUpdate : IUpdateOperations
    {
        MySQLExecute m_Execute { get; set; }
        public MySQLUpdate()
        {
            m_Execute = new MySQLExecute();
        }

        public bool UpdateDataSet(DataSet dataSet, bool setInsertOn = true, bool setModifyOn = true, string additionalMessage = "")
        {
            try
            {
                var result = false;
                foreach (DataTable tbl in dataSet.Tables)
                {
                    result = UpdateTable(tbl, setInsertOn, setModifyOn, additionalMessage);
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
                    AdditionalMessage = additionalMessage,
                    Ex = ex,
                });
                if (Settings.ThrowExceptions) throw new Exception("UpdateDataSet Error!", ex);
                return false;
            }
        }

        public bool UpdateOneValue(string tableName, string column, string value, string where, string additionalMessage = "")
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
                    AdditionalMessage = additionalMessage,
                    Ex = ex,
                });
                if (Settings.ThrowExceptions) throw new Exception("UpdateOneValue Error!", ex);
                return false;
            }
        }

        public bool UpdateTable(DataTable table, bool setInsertOn = true, bool setModifyOn = true, string additionalMessage = "")
        {
            Exception exc;
            return UpdateTable(table, out exc, setInsertOn, setModifyOn, additionalMessage);
        }
        public bool UpdateTable(DataTable table, out Exception exc, bool setInsertOn = true, bool setModifyOn = true, string additionalMessage = "")
        {
            exc = null;
            try
            {
                var tableName = table.TableName;
                return UpdateTable(table, tableName, setInsertOn, setModifyOn, additionalMessage);
            }
            catch (Exception ex)
            {
                exc = ex;
                SLLog.WriteError(new LogData
                {
                    Source = ToString(),
                    FunctionName = "UpdateTable Error!",
                    AdditionalMessage = additionalMessage,
                    Ex = ex,
                });
                if (Settings.ThrowExceptions) throw new Exception("UpdateTable Error!", ex);
                return false;
            }
        }

        public bool UpdateTable(DataTable table, string tableName, bool setInsertOn = true, bool setModifyOn = true, string additionalMessage = "")
        {
            Exception exc;
            return UpdateTable(table, tableName, out exc, setInsertOn, setModifyOn, additionalMessage);
        }
        public bool UpdateTable(DataTable table, string tableName, out Exception exc, bool setInsertOn = true, bool setModfyOn = true, string additionalMessage = "")
        {
            exc = null;
            try
            {
                TableHelper.SetDefaultColumnValues(table, setInsertOn, setModfyOn);

                var con = CONNECTION.OpenCon();

                var adapter = new MySqlDataAdapter(string.Format(@"SELECT * FROM {0} WHERE 1=0", tableName), con);
                var cmd = new MySqlCommandBuilder(adapter);

                adapter.Update(table);

                cmd.Dispose();
                adapter.Dispose();
                CONNECTION.CloseCon(con);

                return true;
            }
            catch (DBConcurrencyException cex)
            {
                exc = cex;
                SLLog.WriteError(new LogData
                {
                    Source = ToString(),
                    FunctionName = "UpdateTable DBConcurrencyError!",
                    AdditionalMessage = $"Table: {tableName}{Environment.NewLine}AdditionalMessage: {additionalMessage}",
                    Ex = cex,
                });
                if (Settings.ThrowExceptions) throw new Exception("UpdateTable Error!", cex);
                return false;
            }
            catch (Exception ex)
            {
                exc = ex;
                SLLog.WriteError(new LogData
                {
                    Source = ToString(),
                    FunctionName = "UpdateTable Error!",
                    AdditionalMessage = $"Table: {tableName}{Environment.NewLine}AdditionalMessage: {additionalMessage}",
                    Ex = ex,
                });
                if (Settings.ThrowExceptions) throw new Exception("UpdateTable Error!", ex);
                return false;
            }
        }

        public bool UpdateTables(List<DataTable> tableList, bool setInsertOn = true, bool setModifyOn = true, string additionalMessage = "")
        {
            Exception exc;
            return UpdateTables(tableList, out exc, setInsertOn, setModifyOn, additionalMessage);
        }
        public bool UpdateTables(List<DataTable> tableList, out Exception exc, bool setInsertOn = true, bool setModifyOn = true, string additionalMessage = "")
        {
            exc = null;
            try
            {
                var result = false;
                foreach (DataTable tbl in tableList)
                {
                    result = UpdateTable(tbl, setInsertOn, setModifyOn, additionalMessage);
                }

                return result;
            }
            catch (Exception ex)
            {
                exc = ex;
                SLLog.WriteError(new LogData
                {
                    Source = ToString(),
                    FunctionName = "UpdateTables Error!",
                    AdditionalMessage = additionalMessage,
                    Ex = ex,
                });
                if (Settings.ThrowExceptions) throw new Exception("UpdateTables Error!", ex);
                return false;
            }
        }
    }
}

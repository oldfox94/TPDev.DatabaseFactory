using DbInterface;
using DbInterface.Helpers;
using DbInterface.Interfaces;
using DbInterface.Models;
using DbLogger.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace SQLLibrary.Operations
{
    public class SQLUpdate : IUpdateOperations
    {
        SQLExecute m_Execute { get; set; }
        public SQLUpdate()
        {
            m_Execute = new SQLExecute();
        }

        public bool UpdateDataSet(DataSet dataSet, bool setInsertOn = true, bool setModifyOn = true, string additionalMessage = "")
        {
            try
            {
                var con = CONNECTION.OpenCon();
                using (SqlTransaction tx = con.BeginTransaction(IsolationLevel.Serializable))
                {
                    for (int i = 0; i < dataSet.Tables.Count; i++)
                    {
                        TableHelper.SetDefaultColumnValues(dataSet.Tables[i], setInsertOn, setModifyOn);

                        var query = String.Format(CultureInfo.InvariantCulture, "SELECT * FROM {0} WHERE 1=0", dataSet.Tables[i].TableName);
                        var da = new SqlDataAdapter(query, con) { SelectCommand = { Transaction = tx } };
#pragma warning disable 168
                        var cb = new SqlCommandBuilder(da);
                        da.UpdateBatchSize = dataSet.Tables[i].Rows.Count > 50 ? 50 : dataSet.Tables[i].Rows.Count;
#pragma warning restore 168
                        Console.WriteLine($"Update in one Transaction => '{dataSet.Tables[i].TableName}'");
                        da.Update(dataSet, dataSet.Tables[i].TableName);

                        cb.Dispose();
                        da.Dispose();
                    }

                    tx.Commit();
                }

                CONNECTION.CloseCon(con);

                return true;
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
            var currentSql = string.Empty;
            try
            {
                var whereCnd = ConvertionHelper.GetWhere(where);

                var sql = currentSql = string.Format(@"UPDATE {0} SET {1} = '{2}', {3} = '{4}' {5}",
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
                    AdditionalMessage = $"SQL: {currentSql}{Environment.NewLine}AdditionalMessage: {additionalMessage}",
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
            catch (DBConcurrencyException cex)
            {
                SLLog.WriteError(new LogData
                {
                    Source = ToString(),
                    FunctionName = "UpdateTable DBConcurrencyError!",
                    AdditionalMessage = additionalMessage,
                    Ex = cex.InnerException != null ? cex.InnerException : cex,
                });
                if (Settings.ThrowExceptions) throw new DBConcurrencyException("UpdateTable Error!", cex);
                return false;
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
        public bool UpdateTable(DataTable table, string tableName, out Exception exc, bool setInsertOn = true, bool setModifyOn = true, string additionalMessage = "")
        {
            exc = null;
            try
            {
                TableHelper.SetDefaultColumnValues(table, setInsertOn, setModifyOn);

                var con = CONNECTION.OpenCon();

                var query = string.Format(CultureInfo.InvariantCulture, "SELECT * FROM {0} WHERE 1=0", tableName);
                var command = new SqlCommand(query, con);
                var da = new SqlDataAdapter(command);
                var builder = new SqlCommandBuilder(da);

                var cb = new SqlCommandBuilder(da);

                Console.WriteLine($"Update Table => '{tableName}'");
                da.Update(table);

                command.Dispose();
                cb.Dispose();
                da.Dispose();
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
                    Ex = cex.InnerException != null ? cex.InnerException : cex,
                });
                if (Settings.ThrowExceptions) throw new DBConcurrencyException("UpdateTable Error!", cex);
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
                    if (!result) return result;
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
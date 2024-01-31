using DbInterface;
using DbInterface.Helpers;
using DbInterface.Interfaces;
using DbInterface.Models;
using DbLogger.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.Threading;

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
            var stpWatch = new Stopwatch();
            stpWatch.Start();

            var result = false;
            var con = CONNECTION.OpenCon();
            try
            {
                using (SqlTransaction tx = con.BeginTransaction(IsolationLevel.Serializable))
                {
                    for (int i = 0; i < dataSet.Tables.Count; i++)
                    {
                        TableHelper.SetDefaultColumnValues(dataSet.Tables[i], setInsertOn, setModifyOn);

                        var query = string.Format(CultureInfo.InvariantCulture, "SELECT * FROM {0} WHERE 1=0", dataSet.Tables[i].TableName);
                        using (var da = new SqlDataAdapter(query, con) { SelectCommand = { Transaction = tx } })
                        {
                            if (Settings.Timeout > 0)
                                da.SelectCommand.CommandTimeout = Settings.Timeout;

                            using (var cb = new SqlCommandBuilder(da))
                            {
                                #pragma warning disable 168
                                da.UpdateBatchSize = dataSet.Tables[i].Rows.Count > 50 ? 50 : dataSet.Tables[i].Rows.Count;
                                #pragma warning restore 168

                                Console.WriteLine($"Update in one Transaction => '{dataSet.Tables[i].TableName}'");
                                da.Update(dataSet, dataSet.Tables[i].TableName);

                            }
                        }
                    }

                    tx.Commit();
                }

                stpWatch.Stop();
                SLLog.WriteInfo("UpdateDataSet", $"Update DataSet successfully -> Elapsed time: {stpWatch.Elapsed}", debugLevel: DebugLevelConstants.High);

                result = true;
            }
            catch (Exception ex)
            {
                stpWatch.Stop();

                SLLog.WriteError(new LogData
                {
                    Source = ToString(),
                    FunctionName = "UpdateDataSet Error!",
                    AdditionalMessage = $"AdditionalMessage: {additionalMessage} -> Elapsed time: {stpWatch.Elapsed}",
                    Ex = ex,
                });
                if (Settings.ThrowExceptions) throw new Exception("UpdateDataSet Error!", ex);
                result = false;
            }
            finally
            {
                CONNECTION.CloseCon(con);
            }
            return result;
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
            var stpWatch = new Stopwatch();
            stpWatch.Start();

            exc = null;
            var result = false;
            var con = CONNECTION.OpenCon();
            try
            {
                TableHelper.SetDefaultColumnValues(table, setInsertOn, setModifyOn);

                var query = string.Format(CultureInfo.InvariantCulture, "SELECT * FROM {0} WHERE 1=0", tableName);
                using (var da = new SqlDataAdapter(query, con))
                {
                    if (Settings.Timeout > 0)
                        da.SelectCommand.CommandTimeout = Settings.Timeout;

                    using (var cb = new SqlCommandBuilder(da))
                    {
                        Console.WriteLine($"Update Table => '{tableName}'");
                        da.Update(table);
                    }
                }

                stpWatch.Stop();
                SLLog.WriteInfo("UpdateTable", $"Update Table '{tableName}' successfully -> Elapsed time: {stpWatch.Elapsed}", debugLevel: DebugLevelConstants.High);

                result = true;
            }
            catch (DBConcurrencyException cex)
            {
                stpWatch.Stop();

                exc = cex;
                SLLog.WriteError(new LogData
                {
                    Source = ToString(),
                    FunctionName = "UpdateTable DBConcurrencyError!",
                    AdditionalMessage = $"Table: {tableName}{Environment.NewLine}AdditionalMessage: {additionalMessage} -> Elapsed time: {stpWatch.Elapsed} in Thread {Thread.CurrentThread.Name}",
                    Ex = cex.InnerException != null ? cex.InnerException : cex,
                });
                if (Settings.ThrowExceptions) throw new DBConcurrencyException("UpdateTable Error!", cex);
                result = false;
            }
            catch (Exception ex)
            {
                stpWatch.Stop();

                exc = ex;
                SLLog.WriteError(new LogData
                {
                    Source = ToString(),
                    FunctionName = "UpdateTable Error!",
                    AdditionalMessage = $"Table: {tableName}{Environment.NewLine}AdditionalMessage: {additionalMessage} -> Elapsed time: {stpWatch.Elapsed} in Thread {Thread.CurrentThread.Name}",
                    Ex = ex,
                });
                if (Settings.ThrowExceptions) throw new Exception("UpdateTable Error!", ex);
                result = false;
            }
            finally
            {
                CONNECTION.CloseCon(con);
            }
            return result;
        }

        public bool UpdateTables(List<DataTable> tableList, bool setInsertOn = true, bool setModifyOn = true, string additionalMessage = "")
        {
            Exception exc;
            return UpdateTables(tableList, out exc, setInsertOn, setModifyOn, additionalMessage);
        }
        public bool UpdateTables(List<DataTable> tableList, out Exception exc, bool setInsertOn = true, bool setModifyOn = true, string additionalMessage = "")
        {
            var stpWatch = new Stopwatch();
            stpWatch.Start();

            exc = null;
            var result = false;
            var con = CONNECTION.OpenCon();
            try
            {
                foreach (DataTable tbl in tableList)
                {
                    var tableName = tbl.TableName;
                    SLLog.WriteInfo("UpdateTables", $"START -> Update Table '{tableName}' successfully -> Elapsed time: {stpWatch.Elapsed}", debugLevel: DebugLevelConstants.High);

                    TableHelper.SetDefaultColumnValues(tbl, setInsertOn, setModifyOn);

                    var query = string.Format(CultureInfo.InvariantCulture, "SELECT * FROM {0} WHERE 1=0", tableName);
                    using (var da = new SqlDataAdapter(query, con))
                    {
                        using (var cb = new SqlCommandBuilder(da))
                        {
                            if (Settings.Timeout > 0)
                                da.SelectCommand.CommandTimeout = Settings.Timeout;

                            Console.WriteLine($"Update Table => '{tableName}'");
                            da.Update(tbl);
                        }
                    }

                    SLLog.WriteInfo("UpdateTables", $"END -> Update Table '{tableName}' successfully -> Elapsed time: {stpWatch.Elapsed}", debugLevel: DebugLevelConstants.High);
                }

                stpWatch.Stop();
                result = true;
            }
            catch (Exception ex)
            {
                exc = ex;
                SLLog.WriteError(new LogData
                {
                    Source = ToString(),
                    FunctionName = "UpdateTables Error!",
                    AdditionalMessage = $"TableCnt: {tableList} -> AdditionalMessage: {additionalMessage} -> Elapsed time: {stpWatch.Elapsed} in Thread {Thread.CurrentThread.Name}",
                    Ex = ex,
                });
                if (Settings.ThrowExceptions) throw new Exception("UpdateTables Error!", ex);
                result = false;
            }
            finally
            {
                CONNECTION.CloseCon(con);
            }
            return result;
        }
    }
}
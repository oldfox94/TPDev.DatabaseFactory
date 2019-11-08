﻿using DbInterface;
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

        public bool UpdateDataSet(DataSet dataSet, bool setInsertOn = true, bool setModifyOn = true, string additionalMessage = "")
        {
            try
            {
                using (var con = new SQLiteConnection(Settings.ConnectionString))
                {
                    foreach (DataTable tbl in dataSet.Tables)
                    {
                        using (var adapter = new SQLiteDataAdapter(string.Format(@"SELECT * FROM {0} WHERE 1=0", tbl.TableName), con))
                        {
                            using (var cmd = new SQLiteCommandBuilder(adapter))
                            {
                                adapter.Update(tbl);
                                cmd.Dispose();
                            }
                            adapter.Dispose();
                        }
                    }
                }
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
            try
            {
                TableHelper.SetDefaultColumnValues(table, setInsertOn, setModifyOn);

                using (var con = new SQLiteConnection(Settings.ConnectionString))
                {
                    using (var adapter = new SQLiteDataAdapter(string.Format(@"SELECT * FROM {0} WHERE 1=0", tableName), con))
                    {
                        using (var cmd = new SQLiteCommandBuilder(adapter))
                        {
                            adapter.Update(table);
                            cmd.Dispose();
                        }
                        adapter.Dispose();
                    }
                }
                return true;
            }
            catch (DBConcurrencyException cex)
            {
                SLLog.WriteError(new LogData
                {
                    Source = ToString(),
                    FunctionName = "UpdateTable DBConcurrencyError!",
                    AdditionalMessage = $"Table: {tableName}{Environment.NewLine}AdditionalMessage: {additionalMessage}",
                    Ex = cex,
                });
                if (Settings.ThrowExceptions) throw new DBConcurrencyException("UpdateTable Error!", cex);
                return false;
            }
            catch (Exception ex)
            {
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

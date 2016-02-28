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

        public bool UpdateDataSet(DataSet dataSet)
        {
            try
            {
                var result = false;
                foreach (DataTable tbl in dataSet.Tables)
                {
                    result = UpdateTable(tbl);
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

        public bool UpdateTable(DataTable table)
        {
            try
            {
                var tableName = table.TableName;
                return UpdateTable(table, tableName);
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

        public bool UpdateTable(DataTable table, string tableName)
        {
            try
            {
                TableHelper.SetDefaultColumnValues(table);

                Settings.Con.Open();

                var adapter = new MySqlDataAdapter(string.Format(@"SELECT * FROM {0}", tableName), Settings.Con);
                var cmd = new MySqlCommandBuilder(adapter);

                adapter.Update(table);

                Settings.Con.Close();

                return true;
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

        public bool UpdateTables(List<DataTable> tableList)
        {
            try
            {
                var result = false;
                foreach (DataTable tbl in tableList)
                {
                    result = UpdateTable(tbl);
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
    }
}

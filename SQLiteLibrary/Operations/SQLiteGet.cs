using DbInterface;
using DbInterface.Helpers;
using DbInterface.Interfaces;
using DbLogger.Models;
using System;
using System.Collections.Generic;
using System.Data;

namespace SQLiteLibrary.Operations
{
    public class SQLiteGet : IGetOperations
    {
        SQLiteExecute m_Execute { get; set; }
        public SQLiteGet()
        {
            m_Execute = new SQLiteExecute();
        }

        public DataSet GetDataSet(List<string> tblSqlDict, string dataSetName, string additionalMessage = "")
        {
            var ds = new DataSet(dataSetName);
            try
            {
                foreach(var item in tblSqlDict)
                {
                    var tbl = GetTable(item, additionalMessage);
                    ds.Tables.Add(tbl);   
                }
            }
            catch(Exception ex)
            {
                SLLog.WriteError(new LogData
                {
                    Source = ToString(),
                    FunctionName = "GetDataSet Error!",
                    AdditionalMessage = $"AdditionalMessage: {additionalMessage}",
                    Ex = ex,
                });
                if (Settings.ThrowExceptions) throw new Exception("GetDataSet Error!", ex);
            }

            return ds;
        }

        public DataRow GetRow(string sql, string additionalMessage = "")
        {
            try
            {
                var tbl = GetTable(sql, additionalMessage);
                if (tbl.Rows.Count <= 0) return null;
                return tbl.Rows[0];
            }
            catch(Exception ex)
            {
                SLLog.WriteError(new LogData
                {
                    Source = ToString(),
                    FunctionName = "GetRow Error!",
                    AdditionalMessage = $"AdditionalMessage: {additionalMessage}",
                    Ex = ex,
                });
                if (Settings.ThrowExceptions) throw new Exception("GetRow Error!", ex);
                return null;
            }
        }

        public DataRow GetRow(string tableName, string where, string orderBy = null, string additionalMessage = "")
        {
            try
            {
                var whereCond = ConvertionHelper.GetWhere(where);
                var orderCnd = ConvertionHelper.GetOrderBy(orderBy);

                var sql = string.Format(@"SELECT * FROM {0} {1} {2}", tableName, whereCond, orderCnd);
                return GetRow(sql, additionalMessage);
            }
            catch (Exception ex)
            {
                SLLog.WriteError(new LogData
                {
                    Source = ToString(),
                    FunctionName = "GetRow Error!",
                    AdditionalMessage = $"AdditionalMessage: {additionalMessage}",
                    Ex = ex,
                });
                if (Settings.ThrowExceptions) throw new Exception("GetRow Error!", ex);
                return null;
            }
        }

        public DataTable GetTable(string sql, string additionalMessage = "")
        {
            var dt = new DataTable();
            try
            {
                dt = m_Execute.ExecuteReadTable(sql);
                SLLog.WriteInfo("GetTable", "Getting Table successfully!", true);
            }
            catch(Exception ex)
            {
                SLLog.WriteError(new LogData
                {
                    Source = ToString(),
                    FunctionName = "GetTable Error!",
                    AdditionalMessage = $"AdditionalMessage: {additionalMessage}",
                    Ex = ex,
                });
                if (Settings.ThrowExceptions) throw new Exception("GetTable Error!", ex);
            }

            return dt;
        }

        public DataTable GetTable(string tableName, string where, string orderBy = null, string additionalMessage = "")
        {
            var dt = new DataTable(tableName);
            try
            {
                var whereCond = ConvertionHelper.GetWhere(where);
                var orderCond = ConvertionHelper.GetOrderBy(orderBy);

                var sql = string.Format(@"SELECT * FROM {0} {1} {2}", tableName, whereCond, orderCond);
                dt = GetTable(sql, additionalMessage);
            }
            catch(Exception ex)
            {
                SLLog.WriteError(new LogData
                {
                    Source = ToString(),
                    FunctionName = "GetTable Error!",
                    AdditionalMessage = $"AdditionalMessage: {additionalMessage}",
                    Ex = ex,
                });
                if (Settings.ThrowExceptions) throw new Exception("GetTable Error!", ex);
            }

            return dt;
        }

        public DataTable GetTableSchema(string tableName, string additionalMessage = "")
        {
            DataTable schemaTbl = new DataTable();
            try
            {
                var sql = string.Format("SELECT * FROM {0}", tableName);
                schemaTbl = m_Execute.ExecuteReadTableSchema(sql);

                SLLog.WriteInfo("GetTableSchema", "Getting Schema Table successfully!", true);
            }
            catch(Exception ex)
            {
                SLLog.WriteError(new LogData
                {
                    Source = ToString(),
                    FunctionName = "GetTableSchema Error!",
                    AdditionalMessage = $"AdditionalMessage: {additionalMessage}",
                    Ex = ex,
                });
                if (Settings.ThrowExceptions) throw new Exception("GetTableSchema Error!", ex);
            }

            return schemaTbl;
        }

        public string GetValueFromColumn(string tableName, string columnName, string where, string additionalMessage = "")
        {
            var resultStr = string.Empty;
            try
            {
                var whereCnd = ConvertionHelper.GetWhere(where);
                var sql = string.Format(@"SELECT {1} FROM {0} {2}", tableName, columnName, whereCnd);

                var tbl = GetTable(sql, additionalMessage);
                if (tbl.Rows.Count <= 0) return resultStr;

                resultStr = tbl.Rows[0][columnName].ToString();
            }
            catch(Exception ex)
            {
                SLLog.WriteError(new LogData
                {
                    Source = ToString(),
                    FunctionName = "GetValueFromColumn Error!",
                    AdditionalMessage = $"AdditionalMessage: {additionalMessage}",
                    Ex = ex,
                });
                if (Settings.ThrowExceptions) throw new Exception("GetValueFromColumn Error!", ex);
            }

            return resultStr;
        }

        public string GetTableNameFromColumn(string columnName, string additionalMessage = "")
        {
            try
            {
                return m_Execute.ExecuteReadTableName(columnName);
            }
            catch (Exception ex)
            {
                SLLog.WriteError(new LogData
                {
                    Source = ToString(),
                    FunctionName = "GetTableNameFromColumn Error!",
                    AdditionalMessage = $"AdditionalMessage: {additionalMessage}",
                    Ex = ex,
                });
                if (Settings.ThrowExceptions) throw new Exception("GetTableNameFromColumn Error!", ex);
                return string.Empty;
            }
        }

        public string GetLastSortOrder(string tableName, string sortOrderColName, string where = null, string additionalMessage = "")
        {
            var result = "0";
            try
            {
                var whereCnd = ConvertionHelper.GetWhere(where);

                var sql = string.Format(@"SELECT {0} FROM {1} {2} ORDER BY CAST({0} AS INTEGER) DESC", sortOrderColName, tableName, whereCnd);
                var tbl = GetTable(sql, additionalMessage);

                if (tbl.Rows.Count <= 0) return result;
                result = string.IsNullOrEmpty(tbl.Rows[0][sortOrderColName].ToString()) ? "0" : tbl.Rows[0][sortOrderColName].ToString();

                SLLog.WriteInfo("GetLastSortOrder", "Getting last sort order successfully! => " + result, true);
            }
            catch (Exception ex)
            {
                SLLog.WriteError(new LogData
                {
                    Source = ToString(),
                    FunctionName = "GetLastSortOrder Error!",
                    AdditionalMessage = $"AdditionalMessage: {additionalMessage}",
                    Ex = ex,
                });
                if (Settings.ThrowExceptions) throw new Exception("GetLastSortOrder Error!", ex);
            }

            return result;
        }

        public string GetNextSortOrder(string tableName, string sortOrderColName, string where = null, string additionalMessage = "")
        {
            var lstSortOrder = string.Empty;
            try
            {
                var result = GetLastSortOrder(tableName, sortOrderColName, where, additionalMessage);
                lstSortOrder = Convert.ToString(Convert.ToInt32(result) + 1);

                SLLog.WriteInfo("GetNextSortOrder", "Getting next sort order successfully! => " + lstSortOrder, true);
            }
            catch(Exception ex)
            {
                SLLog.WriteError(new LogData
                {
                    Source = ToString(),
                    FunctionName = "GetNextSortOrder Error!",
                    AdditionalMessage = $"AdditionalMessage: {additionalMessage}",
                    Ex = ex,
                });
                if (Settings.ThrowExceptions) throw new Exception("GetNextSortOrder Error!", ex);
            }

            return lstSortOrder;
        }
    }
}

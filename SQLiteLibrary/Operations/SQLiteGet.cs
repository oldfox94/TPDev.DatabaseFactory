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

        public DataSet GetDataSet(List<string> tblSqlDict, string dataSetName)
        {
            var ds = new DataSet(dataSetName);
            try
            {
                foreach(var item in tblSqlDict)
                {
                    var tbl = GetTable(item);
                    ds.Tables.Add(tbl);   
                }
            }
            catch(Exception ex)
            {
                SLLog.WriteError(new LogData
                {
                    Source = ToString(),
                    FunctionName = "GetDataSet Error!",
                    Ex = ex,
                });
            }

            return ds;
        }

        public DataRow GetRow(string sql)
        {
            try
            {
                var tbl = GetTable(sql);
                if (tbl.Rows.Count <= 0) return null;
                return tbl.Rows[0];
            }
            catch(Exception ex)
            {
                SLLog.WriteError(new LogData
                {
                    Source = ToString(),
                    FunctionName = "GetRow Error!",
                    Ex = ex,
                });
                return null;
            }
        }

        public DataRow GetRow(string tableName, string where = null, string orderBy = null)
        {
            try
            {
                var whereCond = ConvertionHelper.GetWhere(where);
                var orderCnd = ConvertionHelper.GetOrderBy(orderBy);

                var sql = string.Format(@"SELECT * FROM {0} {1} {2}", tableName, whereCond, orderCnd);
                return GetRow(sql);
            }
            catch (Exception ex)
            {
                SLLog.WriteError(new LogData
                {
                    Source = ToString(),
                    FunctionName = "GetRow Error!",
                    Ex = ex,
                });
                return null;
            }
        }

        public DataTable GetTable(string sql)
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
                    Ex = ex,
                });
            }

            return dt;
        }

        public DataTable GetTable(string tableName, string where = null, string orderBy = null)
        {
            var dt = new DataTable(tableName);
            try
            {
                var whereCond = ConvertionHelper.GetWhere(where);
                var orderCond = ConvertionHelper.GetOrderBy(orderBy);

                var sql = string.Format(@"SELECT * FROM {0} {1} {2}", tableName, whereCond, orderCond);
                dt = GetTable(sql);
            }
            catch(Exception ex)
            {
                SLLog.WriteError(new LogData
                {
                    Source = ToString(),
                    FunctionName = "GetTable Error!",
                    Ex = ex,
                });
            }

            return dt;
        }

        public DataTable GetTableSchema(string tableName)
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
                    Ex = ex,
                });
            }

            return schemaTbl;
        }

        public string GetValueFromColumn(string tableName, string columnName, string where)
        {
            var resultStr = string.Empty;
            try
            {
                var whereCnd = ConvertionHelper.GetWhere(where);

                var sql = string.Format(@"SELECT {1} FROM {0} {2}", tableName, columnName, whereCnd);

                var tbl = GetTable(sql);
                if (tbl.Rows.Count <= 0) return resultStr;

                resultStr = tbl.Rows[0][columnName].ToString();
            }
            catch(Exception ex)
            {
                SLLog.WriteError(new LogData
                {
                    Source = ToString(),
                    FunctionName = "GetValueFromColumn Error!",
                    Ex = ex,
                });
            }

            return resultStr;
        }

        public string GetTableNameFromColumn(string columnName)
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
                    Ex = ex,
                });
                return string.Empty;
            }
        }

        public string GetLastSortOrder(string tableName, string sortOrderColName, string where = null)
        {
            var result = "0";
            try
            {
                var whereCnd = ConvertionHelper.GetWhere(where);

                var sql = string.Format(@"SELECT {0} FROM {1} {2} ORDER BY CAST({0} AS INTEGER) DESC", sortOrderColName, tableName, whereCnd);
                var tbl = GetTable(sql);

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
                    Ex = ex,
                });
            }

            return result;
        }

        public string GetNextSortOrder(string tableName, string sortOrderColName, string where = null)
        {
            var lstSortOrder = string.Empty;
            try
            {
                var result = GetLastSortOrder(tableName, sortOrderColName, where);
                lstSortOrder = Convert.ToString(Convert.ToInt32(result) + 1);

                SLLog.WriteInfo("GetNextSortOrder", "Getting next sort order successfully! => " + lstSortOrder, true);
            }
            catch(Exception ex)
            {
                SLLog.WriteError(new LogData
                {
                    Source = ToString(),
                    FunctionName = "GetNextSortOrder Error!",
                    Ex = ex,
                });
            }

            return lstSortOrder;
        }
    }
}

﻿using DbInterface;
using DbInterface.Helpers;
using DbInterface.Interfaces;
using DbLogger.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;

namespace SQLiteLibrary.Operations
{
    public class SQLiteGet : IGetOperations
    {
        public SQLiteGet()
        {

        }

        public DataSet GetDataSet(Dictionary<string, string> tblSqlDict, string dataSetName)
        {
            var ds = new DataSet(dataSetName);
            try
            {
                foreach(var item in tblSqlDict)
                {
                    var tbl = GetTable(item.Value, item.Key);
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
                var tbl = GetTable(sql, "tbl");
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

        public DataTable GetTable(string sql, string tableName)
        {
            var dt = new DataTable(tableName);
            try
            {
                Settings.Con.Open();

                var cmd = new SQLiteCommand(sql, Settings.Con);
                var reader = cmd.ExecuteReader();
                dt.Load(reader);

                reader.Close();
                Settings.Con.Close();
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
                var orderCnd = ConvertionHelper.GetOrderBy(orderBy);

                var sql = string.Format(@"SELECT * FROM {0} {1} {2}", tableName, where, orderBy);
                dt = GetTable(sql, tableName);
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
                Settings.Con.Open();

                var cmd = new SQLiteCommand("SELECT * FROM {0}", Settings.Con);

                var reader = cmd.ExecuteReader();
                schemaTbl = reader.GetSchemaTable();

                reader.Close();
                Settings.Con.Close();
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

                var tbl = GetTable(sql, tableName);
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

        public string GetLastSortOrder(string tableName, string sortOrderColName, string where = null)
        {
            var result = "0";
            try
            {
                var whereCnd = ConvertionHelper.GetWhere(where);

                var sql = string.Format(@"SELECT {0} FROM {1} {2} ORDER BY {0} DESC", sortOrderColName, tableName, whereCnd);
                var tbl = GetTable(sql, tableName);

                if (tbl.Rows.Count <= 0) return result;
                result = string.IsNullOrEmpty(tbl.Rows[0][sortOrderColName].ToString()) ? "0" : tbl.Rows[0][sortOrderColName].ToString();
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
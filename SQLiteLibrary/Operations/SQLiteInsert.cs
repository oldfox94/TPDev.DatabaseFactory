using DbInterface.Interfaces;
using System;
using System.Collections.Generic;
using DbInterface.Models;
using System.Data;
using DbLogger.Models;
using DbInterface;
using DbInterface.Helpers;

namespace SQLiteLibrary.Operations
{
    public class SQLiteInsert : IInsertOperations
    {
        SQLiteExecute m_Execute { get; set; }
        public SQLiteInsert()
        {
            m_Execute = new SQLiteExecute();
        }

        public bool CreateTable(string tableName, Dictionary<string, string> columns)
        {
            try
            {
                var colList = new List<ColumnData>();
                foreach(var col in columns)
                {
                    colList.Add(new ColumnData
                    {
                        Name = col.Key,
                        Type = col.Value
                    });
                }

                return CreateTable(tableName, colList);
            }
            catch(Exception ex)
            {
                SLLog.WriteError(new LogData
                {
                    Source = ToString(),
                    FunctionName = "CreateTable Error!",
                    Ex = ex,
                });
                return false;
            }
        }

        public bool CreateTable(string tableName, List<ColumnData> columns)
        {
            try
            {
                ColumnHelper.SetDefaultColumns(columns);

                var sql = ScriptHelper.GetCreateTableSql(tableName, columns);
                var result = m_Execute.ExecuteNonQuery(sql);

                if (result == -1) return false;
                return true;
            }
            catch (Exception ex)
            {
                SLLog.WriteError(new LogData
                {
                    Source = ToString(),
                    FunctionName = "CreateTable Error!",
                    Ex = ex,
                });
                return false;
            }
        }

        public bool InsertRow(string tableName, DataRow row)
        {
            try
            {
                var colRowDict = new Dictionary<string, string>();
                foreach(DataColumn dc in row.Table.Columns)
                {
                    colRowDict.Add(dc.ColumnName, row[dc.ColumnName].ToString());
                }

                return InsertValue(tableName, colRowDict);
            }
            catch(Exception ex)
            {
                SLLog.WriteError(new LogData
                {
                    Source = ToString(),
                    FunctionName = "InsertRow Error!",
                    Ex = ex,
                });
                return false;
            }
        }

        public bool InsertValue(string tableName, string columnName, string value)
        {
            try
            {
                var colRowDict = new Dictionary<string, string>();
                colRowDict.Add(columnName, value);
                return InsertValue(tableName, colRowDict);
            }
            catch(Exception ex)
            {
                SLLog.WriteError(new LogData
                {
                    Source = ToString(),
                    FunctionName = "InsertValue Error!",
                    Ex = ex,
                });
                return false;
            }
        }

        public bool InsertValue(string tableName, Dictionary<string, string> data)
        {
            try
            {
                var sql = ScriptHelper.GetInsertSqlScript(tableName, data);
                var result = m_Execute.ExecuteNonQuery(sql);

                if (result == -1) return false;
                return true;
            }
            catch(Exception ex)
            {
                SLLog.WriteError(new LogData
                {
                    Source = ToString(),
                    FunctionName = "InsertValue Error!",
                    Ex = ex,
                });
                return false;
            }
        }
    }
}

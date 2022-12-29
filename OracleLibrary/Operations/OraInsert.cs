using DbInterface;
using DbInterface.Helpers;
using DbInterface.Interfaces;
using DbInterface.Models;
using DbLogger.Models;
using System;
using System.Collections.Generic;
using System.Data;

namespace OracleLibrary.Operations
{
    public class OraInsert : IInsertOperations
    {
        OraExecute m_Execute { get; set; }
        public OraInsert()
        {
            m_Execute = new OraExecute();
        }

        public bool InsertRow(string tableName, DataRow row, bool setInsertOn = true)
        {
            try
            {
                var colRowDict = new Dictionary<string, string>();
                foreach (DataColumn dc in row.Table.Columns)
                {
                    colRowDict.Add(dc.ColumnName, row[dc.ColumnName].ToString());
                }

                return InsertValue(tableName, colRowDict, setInsertOn);
            }
            catch (Exception ex)
            {
                SLLog.WriteError(new LogData
                {
                    Source = ToString(),
                    FunctionName = "InsertRow Error!",
                    Ex = ex,
                });
                if (Settings.ThrowExceptions) throw new Exception("InsertRow Error!", ex);
                return false;
            }
        }

        public bool InsertValue(string tableName, string columnName, string value, bool setInsertOn = true)
        {
            try
            {
                var colRowDict = new Dictionary<string, string>();
                colRowDict.Add(columnName, value);
                return InsertValue(tableName, colRowDict, setInsertOn);
            }
            catch (Exception ex)
            {
                SLLog.WriteError(new LogData
                {
                    Source = ToString(),
                    FunctionName = "InsertValue Error!",
                    Ex = ex,
                });
                if (Settings.ThrowExceptions) throw new Exception("InsertValue Error!", ex);
                return false;
            }
        }

        public bool InsertValue(string tableName, Dictionary<string, string> data, bool setInsertOn = true)
        {
            try
            {
                var sql = ScriptHelper.GetInsertSqlScript(tableName, data, setInsertOn);
                var result = m_Execute.ExecuteNonQuery(sql);

                if (result == -2) return false;
                return true;
            }
            catch (Exception ex)
            {
                SLLog.WriteError(new LogData
                {
                    Source = ToString(),
                    FunctionName = "InsertValue Error!",
                    Ex = ex,
                });
                if (Settings.ThrowExceptions) throw new Exception("InsertValue Error!", ex);
                return false;
            }
        }

        public bool CreateTable(string tableName, List<ColumnData> columns)
        {
            try
            {
                ColumnHelper.SetDefaultColumns(columns, DbInterface.Models.DbType.Oracle);

                var sql = ScriptHelper.GetCreateTableSql(tableName, columns);
                var result = m_Execute.ExecuteNonQuery(sql);

                if (result == -2) return false;
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
                if (Settings.ThrowExceptions) throw new Exception("CreateTable Error!", ex);
                return false;
            }
        }

        public bool CreateTable(string tableName, Dictionary<string, string> columns)
        {
            try
            {
                var colList = new List<ColumnData>();
                foreach (var col in columns)
                {
                    colList.Add(new ColumnData
                    {
                        Name = col.Key,
                        Type = col.Value
                    });
                }

                return CreateTable(tableName, colList);
            }
            catch (Exception ex)
            {
                SLLog.WriteError(new LogData
                {
                    Source = ToString(),
                    FunctionName = "CreateTable Error!",
                    Ex = ex,
                });
                if (Settings.ThrowExceptions) throw new Exception("CreateTable Error!", ex);
                return false;
            }
        }

        public bool CreateDatabase(string databaseName)
        {
            try
            {
                return false;
            }
            catch (Exception ex)
            {
                SLLog.WriteError(new LogData
                {
                    Source = ToString(),
                    FunctionName = "CreateDatabase Error!",
                    Ex = ex,
                });
                if (Settings.ThrowExceptions) throw new Exception("CreateDatabase Error!", ex);
                return false;
            }
        }
    }
}

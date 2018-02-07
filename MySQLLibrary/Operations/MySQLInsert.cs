using DbInterface;
using DbInterface.Helpers;
using DbInterface.Interfaces;
using DbInterface.Models;
using DbLogger.Models;
using System;
using System.Collections.Generic;
using System.Data;

namespace MySQLLibrary.Operations
{
    public class MySQLInsert : IInsertOperations
    {
        MySQLExecute m_Execute { get; set; }
        public MySQLInsert()
        {
            m_Execute = new MySQLExecute();
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
                return false;
            }
        }

        public bool CreateTable(string tableName, List<ColumnData> columns)
        {
            try
            {
                ColumnHelper.SetDefaultColumns(columns);

                var sql = ScriptHelper.GetMySQLCreateTableSql(tableName, columns);
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
                return false;
            }
        }

        public bool CreateDatabase(string databaseName)
        {
            var result = false;
            try
            {
                var cmdResult = m_Execute.ExecuteNonQuery(string.Format(@"CREATE DATABASE {0};", databaseName));
                result = cmdResult != -2;
            }
            catch(Exception ex)
            {
                SLLog.WriteError(new LogData
                {
                    Source = ToString(),
                    FunctionName = "CreateDatabase Error!",
                    Ex = ex,
                });
                result = false;
            }

            //new CONNECTION(Settings.ConnectionData);
            return result;
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
                return false;
            }
        }
    }
}

using DbInterface.Interfaces;
using System;
using System.Collections.Generic;
using DbInterface.Models;
using System.Data.SQLite;
using DbInterface;
using DbLogger.Models;
using DbInterface.Helpers;
using System.Data;

namespace SQLiteLibrary.Operations
{
    public class SQLiteExecute : IExecuteOperations
    {
        SQLiteGet m_Get { get; set; }
        public SQLiteExecute()
        {
            m_Get = new SQLiteGet();
        }

        public int ExecuteNonQuery(string sql)
        {
            int rowsUpdated = 0;
            try
            {
                Settings.Con.Open();

                SQLiteCommand cmd = new SQLiteCommand(sql, Settings.Con);
                rowsUpdated = cmd.ExecuteNonQuery();

                Settings.Con.Close();
            }
            catch(Exception ex)
            {
                SLLog.WriteError(new LogData
                {
                    Source = ToString(),
                    FunctionName = "ExecuteNonQuery Error!",
                    Ex = ex,
                });
                return -1;
            }

            return rowsUpdated;
        }

        public int ExecuteNonQuery(List<string> sqlList)
        {
            int rowsUpdated = 0;
            try
            {
                Settings.Con.Open();

                foreach(var sql in sqlList)
                {
                    var cmd = new SQLiteCommand(sql, Settings.Con);
                    rowsUpdated += cmd.ExecuteNonQuery();
                }

                Settings.Con.Close();
            }
            catch(Exception ex)
            {
                SLLog.WriteError(new LogData
                {
                    Source = ToString(),
                    FunctionName = "ExecuteNonQuery Error!",
                    Ex = ex,
                });
                return -1;
            }

            return rowsUpdated;
        }

        public object ExecuteScalar(string sql)
        {
            object value = null;
            try
            {
                Settings.Con.Open();

                var cmd = new SQLiteCommand(sql, Settings.Con);
                value = cmd.ExecuteScalar();

                Settings.Con.Close();
            }
            catch(Exception ex)
            {
                SLLog.WriteError(new LogData
                {
                    Source = ToString(),
                    FunctionName = "ExecuteNonQuery Error!",
                    Ex = ex,
                });
                return null;
            }

            return value;
        }

        public bool RenewTbl(string tableName, List<ColumnData> columns)
        {
            try
            {
                var result = true;

                var scriptList = new List<string>();
                var oldColumns = new List<ColumnData>();
                var timeStamp = DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second + "_" +
                                DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString();

                //Standart Felder erstellen
                ColumnHelper.SetDefaultColumns(columns);

                var oldTblSchema = m_Get.GetTableSchema(tableName);
                foreach (DataRow row in oldTblSchema.Rows)
                {
                    var oldCol = columns.Find(i => i.Name == row["ColumnName"].ToString());
                    if (oldCol == null) continue;
                    oldCol.existsInDB = true;

                    oldColumns.Add(oldCol);
                }

                scriptList.Add(string.Format("ALTER TABLE {0} RENAME TO {0}_OLD{1}", tableName, timeStamp));

                scriptList.Add(ScriptHelper.GetCreateTableSql(tableName, columns));

                var insertSQL = string.Format("INSERT INTO {0} ({1}) ", tableName, ColumnHelper.GetColumnString(columns, true));
                insertSQL += string.Format("SELECT {2} FROM {0}_OLD{1}", tableName, timeStamp, ColumnHelper.GetColumnString(columns));
                scriptList.Add(insertSQL);


                var exResult = ExecuteNonQuery(scriptList);
                if (exResult == -1)
                    result = false;

                return result;
            }
            catch(Exception ex)
            {
                SLLog.WriteError(new LogData
                {
                    Source = ToString(),
                    FunctionName = "RenewTbl Error!",
                    Ex = ex,
                });
                return false;
            }
        }

        public bool RenewTbl(string tableName, Dictionary<string, string> columns)
        {
            try
            {
                var colList = new List<ColumnData>();

                foreach (var col in columns)
                {
                    var colData = new ColumnData
                    {
                        Name = col.Key,
                        Type = col.Value,
                    };
                    if (col.Value == DbDEF.TxtNotNull)
                        colData.DefaultValue = "default";

                    colList.Add(colData);
                }

                return RenewTbl(tableName, colList);
            }
            catch(Exception ex)
            {
                SLLog.WriteError(new LogData
                {
                    Source = ToString(),
                    FunctionName = "RenewTbl Error!",
                    Ex = ex,
                });
                return false;
            }
        }
    }
}

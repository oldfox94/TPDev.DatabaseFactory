using DbInterface.Models;
using System.Collections.Generic;

namespace DbInterface.Helpers
{
    public static class ScriptHelper
    {
        public static string GetCreateTableSql(string tableName, List<ColumnData> columns)
        {
            var sql = string.Format(@"CREATE TABLE [{0}] (", tableName);

            int count = 0;
            foreach (var col in columns)
            {
                count++;

                sql += string.Format(@"[{0}] {1}", col.Name, col.Type);
                if (!string.IsNullOrEmpty(col.DefaultValue))
                {
                    sql += string.Format(@" DEFAULT '{0}'", col.DefaultValue);
                }

                if (count != columns.Count)
                    sql += ", ";
                else
                    sql += ")";
            }

            return sql;
        }

        public static string GetSQLiteCreateTableSql(string tableName, List<ColumnData> columns)
        {
            var sql = string.Format(@"CREATE TABLE [{0}] (", tableName);

            int colCount = 0;
            var fkList = new List<FkData>();
            foreach (var col in columns)
            {
                colCount++;

                if (col.FkList != null)
                {
                    foreach(var fk in col.FkList)
                    {
                        fk.SourceColumn = col.Name;
                        fkList.Add(fk);
                    }
                }

                sql += string.Format(@"[{0}] {1}", col.Name, col.Type);
                if (!string.IsNullOrEmpty(col.DefaultValue))
                {
                    sql += string.Format(@" DEFAULT '{0}'", col.DefaultValue);
                }

                if (colCount != columns.Count) sql += ", ";
            }

            int fkCount = 0;
            foreach(var fk in fkList)
            {
                fkCount++;
                sql += string.Format(", FOREIGN KEY ({0}) REFERENCES {1}({2})", fk.SourceColumn, fk.RefTable, fk.RefColumn);
            }

            sql += ")";
            return sql;
        }

        public static string GetMySQLCreateTableSql(string tableName, List<ColumnData> columns)
        {
            var sql = string.Format(@"CREATE TABLE {0} (", tableName);

            int count = 0;
            foreach(var col in columns)
            {
                count++;

                sql += string.Format(@"{0} {1}", col.Name, col.Type);
                if(!string.IsNullOrEmpty(col.DefaultValue))
                {
                    sql += string.Format(@" DEFAULT '{0}'", col.DefaultValue);
                }

                if (count != columns.Count)
                    sql += ", ";
                else
                    sql += ")";
            }

            return sql;
        }

        public static string GetInsertSqlScript(string tableName, Dictionary<string, string> data, bool setInsertOn = true)
        {
            var sql = string.Empty;
            string columns = "";
            string values = "";

            ColumnHelper.SetDefaultColumnValues(data, setInsertOn);

            foreach(KeyValuePair<string, string> val in data)
            {
                columns += string.Format(@" {0},", val.Key.ToString());
                values += string.Format(@" '{0}',", ConvertionHelper.CleanStringForSQL(val.Value));
            }
            columns = columns.Substring(0, columns.Length -1);
            values = values.Substring(0, values.Length -1);

            sql = string.Format(@"INSERT INTO {0}({1}) values({2})", tableName, columns, values);
            return sql;
        }
    }
}

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

                sql += string.Format(@"[{0}] ", col.Name);
                foreach (var colSettings in columns)
                {
                    if (colSettings.Name == col.Name)
                    {
                        sql += colSettings.Type;
                        if (!string.IsNullOrEmpty(colSettings.DefaultValue))
                        {
                            sql += string.Format(@" DEFAULT '{0}'", colSettings.DefaultValue);
                        }
                        break;
                    }
                }

                if (count != columns.Count)
                    sql += ", ";
                else
                    sql += ")";
            }

            return sql;
        }

        public static string GetMySQLCreateTableSql(string tableName, List<ColumnData> columns)
        {
            var sql = string.Format(@"CREATE TABLE {0} (", tableName);

            int count = 0;
            foreach(var col in columns)
            {
                count++;

                sql += string.Format(@"{0} ", col.Name);
                foreach(var colSettings in columns)
                {
                    if(colSettings.Name == col.Name)
                    {
                        sql += colSettings.Type;
                        if(!string.IsNullOrEmpty(colSettings.DefaultValue))
                        {
                            sql += string.Format(@" DEFAULT '{0}'", colSettings.DefaultValue);
                        }
                        break;
                    }
                }

                if (count != columns.Count)
                    sql += ", ";
                else
                    sql += ")";
            }

            return sql;
        }

        public static string GetInsertSqlScript(string tableName, Dictionary<string, string> data)
        {
            var sql = string.Empty;
            string columns = "";
            string values = "";

            ColumnHelper.SetDefaultColumnValues(data);

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

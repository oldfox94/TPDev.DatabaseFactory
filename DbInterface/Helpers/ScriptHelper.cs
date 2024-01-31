using DbInterface.Models;
using System;
using System.Collections.Generic;

namespace DbInterface.Helpers
{
    public static class ScriptHelper
    {
        public static string GetCreateTableSql(string tableName, List<ColumnData> columns)
        {
            var sql = string.Format(@"CREATE TABLE [{0}] (", tableName);

            int count = 0;
            var fkList = new List<FkData>();
            foreach (var col in columns)
            {
                count++;

                if (col.FkList != null)
                {
                    foreach (var fk in col.FkList)
                    {
                        fk.SourceColumn = col.Name;
                        fkList.Add(fk);
                    }
                }

                sql += string.Format(@"[{0}] {1}", col.Name, col.Type);
                if (!string.IsNullOrEmpty(col.DefaultValue))
                    sql += string.Format(@" DEFAULT '{0}'", col.DefaultValue);

                if (count != columns.Count)
                    sql += ", ";
            }
            sql += $");{Environment.NewLine}";

            //Create FOREIGN KEY's
            foreach (var fk in fkList)
            {
                sql += $@"ALTER TABLE {tableName} ADD CONSTRAINT FK_{fk.SourceColumn}_{fk.RefColumn} 
                            FOREIGN KEY ({fk.SourceColumn}) REFERENCES {fk.RefTable}({fk.RefColumn});{Environment.NewLine}";
            }

            return sql;
        }

        public static string GetSQLIndizesScript(string tableName, List<IndizesData> indizes)
        {
            if (indizes == null)
                return string.Empty;

            var sql = "";
            foreach (var idz in indizes)
            {
                var idzColString = "";
                foreach (var idzCol in idz.Columns)
                    idzColString += $"{idzCol}, ";

                if (!string.IsNullOrEmpty(idzColString.Trim()))
                    idzColString = idzColString.Trim().Remove(idzColString.Trim().Length - 1, 1);

                if (string.IsNullOrEmpty(idzColString.Trim()))
                    continue;

                sql += $@"CREATE INDEX I_{idz.Columns[0]} ON {tableName}({idzColString});{Environment.NewLine}";
            }
            return sql;
        }

        public static string GetSQLLiteCreateTableSql(string tableName, List<ColumnData> columns)
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
                    sql += string.Format(@" DEFAULT '{0}'", col.DefaultValue);

                if (colCount != columns.Count)
                    sql += ", ";
            }
            sql += $");{Environment.NewLine}";


            //Create FOREIGN KEY's
            foreach (var fk in fkList)
            {
                sql += $@"ALTER TABLE {tableName} ADD CONSTRAINT FK_{fk.SourceColumn}_{fk.RefColumn} 
                            FOREIGN KEY ({fk.SourceColumn}) REFERENCES {fk.RefTable}({fk.RefColumn});{Environment.NewLine}";
            }

            return sql;
        }

        public static string GetSQLLiteIndizesScript(string tableName, List<IndizesData> indizes)
        {
            var sql = "";
            foreach (var idz in indizes)
            {
                var idzColString = "";
                foreach (var idzCol in idz.Columns)
                    idzColString += $"{idzCol}, ";

                if (!string.IsNullOrEmpty(idzColString.Trim()))
                    idzColString = idzColString.Remove(idzColString.Length - 1, 1);

                if (string.IsNullOrEmpty(idzColString.Trim()))
                    continue;

                sql += $@"CREATE INDEX I_{idz.Columns[0]} ON {tableName}({idzColString});{Environment.NewLine}";
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
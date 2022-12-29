﻿using DbInterface.Models;
using System;
using System.Collections.Generic;

namespace DbInterface.Helpers
{
    public static class ConvertionHelper
    {
        public static string GetWhere(string where)
        {
            var whereCond = string.Empty;
            if (!string.IsNullOrEmpty(where))
            {
                if (where.ToUpper().Contains("WHERE"))
                {
                    whereCond = where;
                }
                else
                {
                    whereCond = "WHERE " + where;
                }
            }

            return whereCond;
        }

        public static string GetOrderBy(string orderBy)
        {
            var orderCnd = string.Empty;
            if (!string.IsNullOrEmpty(orderBy))
            {
                if (orderBy.ToUpper().Contains("ORDER BY"))
                {
                    orderCnd = orderBy;
                }
                else
                {
                    orderCnd = "ORDER BY" + orderBy;
                }
            }

            return orderCnd;
        }

        public static string GetMasterTable(DbType type)
        {
            switch(type)
            {
                case DbType.SQL:
                    return string.Empty;

                case DbType.SQLite:
                    return "SQLITE_MASTER";

                case DbType.MySQL:
                    return string.Empty;
            }

            return string.Empty;
        }

        public static string CleanStringForSQL(string strIn)
        {
            if (string.IsNullOrEmpty(strIn)) return string.Empty;
            return strIn.Replace("'", "''");
        }

        //Get predefined operation strings for SQL
        public static string WHERE(string ColName, string Value)
        {
            return ColName + " = '" + Value + "'";
        }

        public static string WHERE(string ColName, string whereOperator, string Value)
        {
            return ColName + " " + whereOperator + " '" + Value + "'";
        }

        public static string WHERE(string ColName, string whereOperator, DateTime Value)
        {
            return WHERE(DbType.SQL, ColName, whereOperator, Value);
        }
        public static string WHERE(DbType dbType, string ColName, string whereOperator, DateTime Value)
        {
            string date = Value.ToString("yyyy-MM-dd");

            var where = string.Empty;
            switch(dbType)
            {
                case DbType.SQLite:
                    where = "datetime('" + date + "') " + whereOperator;
                    where += string.Format(" datetime(substr({0}, 7, 4) || '-' || substr({0}, 4, 2) || '-' || substr({0}, 1, 2))", ColName);
                    break;

                case DbType.SQL:
                    date = Value.ToString();

                    where = "CAST('" + date + "' AS DATETIME) " + whereOperator;
                    where += string.Format(" CAST({0} AS DATETIME)", ColName);
                    break;

                default:
                    where = "CAST('" + date + "' AS DATETIME) " + whereOperator;
                    where += string.Format(" CAST(substring({0}, 7, 4) + '-' + substring({0}, 4, 2) + '-' + substring({0}, 1, 2) AS DATETIME)", ColName);
                    break;
            }
            return where;
        }

        public static string WHERE(string ColName, DateTime minValue, DateTime maxValue)
        {
            return WHERE(DbType.SQL, ColName, minValue, maxValue);
        }
        public static string WHERE(DbType dbType, string ColName, DateTime minValue, DateTime maxValue)
        {
            string minDate = minValue.ToString("yyyy-MM-dd");
            string maxDate = maxValue.ToString("yyyy-MM-dd");

            string colDate = string.Empty;
            var where = string.Empty;
            switch (dbType)
            {
                case DbType.SQLite:
                    colDate = string.Format(" datetime(substr({0}, 7, 4) || '-' || substr({0}, 4, 2) || '-' || substr({0}, 1, 2))", ColName);

                    where = string.Format("datetime('{0}') AND datetime('{1}')", minDate, maxDate);
                    where = string.Format(@"{0} BETWEEN {1}", colDate, where);
                    break;

                case DbType.SQL:
                    minDate = minValue.ToString();
                    maxDate = maxValue.ToString();
                    colDate = string.Format(" CAST({0} AS DATETIME)", ColName);

                    where = string.Format("CAST('{0}' AS DATETIME) AND CAST('{1}' AS DATETIME)", minDate, maxDate);
                    where = string.Format(@"{0} BETWEEN {1}", colDate, where);
                    break;

                default:
                    colDate = string.Format(" CAST(substring({0}, 7, 4) + '-' + substring({0}, 4, 2) + '-' + substring({0}, 1, 2) AS DATETIME)", ColName);

                    where = string.Format("CAST('{0}' AS DATETIME) AND CAST('{1}' AS DATETIME)", minDate, maxDate);
                    where = string.Format(@"{0} BETWEEN {1}", colDate, where);
                    break;
            }

            return where;
        }

        public static string IsInList(string ColName, List<string> valueList)
        {
            var result = string.Empty;
            foreach(var item in valueList)
            {
                result += string.Format(@"'{0}',", item);
            }

            if(valueList.Count > 0)
            {
                result = result.Remove(result.Length - 1, 1);
                result = string.Format(@" {0} IN({1}) ", ColName, result);
            }

            return result;
        }

        public static string ORDERBY(string colName, string orderBy)
        {
            return colName + " " + orderBy;
        }

        public static string ORDERBYDATETIME(DbType dbType, string colName, string orderBy)
        {
            switch (dbType)
            {
                case DbType.SQLite:
                    return string.Format(" substr({0}, 7, 4) || substr({0}, 4, 2) || substr({0}, 1, 2) || substr({0}, 12, 2) || substr({0}, 15, 2) || substr({0}, 18, 2) {1}", colName, orderBy);

                case DbType.SQL:
                    return $" CAST({colName} AS DATETIME) {orderBy}";

                default:
                    return string.Format(" substring({0}, 7, 4) + substring({0}, 4, 2) + substring({0}, 1, 2) + substring({0}, 12, 2) + substring({0}, 15, 2) + substring({0}, 18, 2) {1}", colName, orderBy);
            }
        }


        public static string GetSelectWithLimit(DbType dbType, int limit, string innerQuery)
        {
            switch(dbType)
            {
                case DbType.SQLite:
                    return string.Format(@"SELECT {1} LIMIT {0}", limit, innerQuery);

                case DbType.SQL:
                    return string.Format(@"SELECT TOP {0} {1}", limit, innerQuery);

                default:
                    return string.Format(@"SELECT TOP {0} {1}", limit, innerQuery);
            }
        }

        public static string GetDateFromString(DbType dbType, string columnName)
        {
            switch (dbType)
            {
                case DbType.SQLite:
                    return string.Format(
                        @" strftime('%d.%m.%Y %H:%M:%S',substr({0}, 7, 4) || '-' || substr({0}, 4, 2) || '-' || substr({0}, 1, 2))",
                        columnName);

                case DbType.SQL:
                    return string.Format(
                        @" CONVERT(DATE, substring({0}, 7, 4) + '-' + substring({0}, 4, 2) + '-' + substring({0}, 1, 2))",
                        columnName);

                default:
                    return string.Format(
                        @" CAST(substring({0}, 7, 4) + '-' + substring({0}, 4, 2) + '-' + substring({0}, 1, 2))",
                        columnName);
            }
        }
        public static string GetDateTimeFromString(DbType dbType, string columnName)
        {
            switch(dbType)
            {
                case DbType.SQLite:
                    return string.Format(
                        @" strftime('%d.%m.%Y %H:%M:%S',substr({0}, 7, 4) || '-' || substr({0}, 4, 2) || '-' || substr({0}, 1, 2) || ' ' ||
								                        substr({0}, 12, 2) || ':' || substr({0}, 15, 2) || ':' || substr({0}, 18, 2))",
                        columnName);

                case DbType.SQL:
                    return string.Format(
                        @" CONVERT(DATETIME, {0})",
                        columnName);

                default:
                    return string.Format(
                        @" CAST(substring({0}, 7, 4) + '-' + substring({0}, 4, 2) + '-' + substring({0}, 1, 2) + ' ' +
								substring({0}, 12, 2) + ':' + substring({0}, 15, 2) + ':' + substring({0}, 18, 2))",
                        columnName);
            }
        }

        public static string GetTimeSum(DbType dbType, string columnName)
        {
            switch(dbType)
            {
                case DbType.SQLite:
                    return string.Format(@" strftime('%H:%M:%S', SUM(strftime('%s', {0})) ,'unixepoch')", columnName);

                case DbType.SQL:
                    return string.Format(@" CONVERT(varchar, SUM(DATEDIFF(MINUTE, '0:00:00', {0})))", columnName);

                default:
                    return string.Format(@" CONVERT(varchar, SUM(CONVERT(TIME({0})), 108)", columnName);
            }
        }
    }
}

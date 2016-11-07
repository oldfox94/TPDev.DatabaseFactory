using DbInterface.Models;
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
            string date = Value.ToString("yyyy-MM-dd");
            var where = "datetime('" + date + "') " + whereOperator;
            where += string.Format(" datetime(substr({0}, 7, 4) || '-' || substr({0}, 4, 2) || '-' || substr({0}, 1, 2))", ColName);
            return where;
        }

        public static string WHERE(string ColName, DateTime minValue, DateTime maxValue)
        {
            string minDate = minValue.ToString("yyyy-MM-dd");
            string maxDate = maxValue.ToString("yyyy-MM-dd");
            string colDate = string.Format(" datetime(substr({0}, 7, 4) || '-' || substr({0}, 4, 2) || '-' || substr({0}, 1, 2))", ColName);

            var where = string.Format("datetime('{0}') AND datetime('{1}')", minDate, maxDate);
            where = string.Format(@"{0} BETWEEN {1}", colDate, where);
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
    }
}

using DbInterface.Models;
using System;
using System.Collections.Generic;

namespace DbInterface.Helpers
{
    public static class ColumnHelper
    {
        public static List<ColumnData> SetDefaultColumns(List<ColumnData> columns)
        {
            if (columns.Find(i => i.Name == DbCIC.DsStatus) == null)
                columns.Add(new ColumnData { Name = DbCIC.DsStatus, Type = DbDEF.VarchrNull(3), DefaultValue = "1" });

            if (columns.Find(i => i.Name == DbCIC.InsertOn) == null)
                columns.Add(new ColumnData { Name = DbCIC.InsertOn, Type = DbDEF.VarchrNull(20), DefaultValue = "01.01.1900" });

            if (columns.Find(i => i.Name == DbCIC.ModifyOn) == null)
                columns.Add(new ColumnData { Name = DbCIC.ModifyOn, Type = DbDEF.VarchrNull(20) });

            return columns;
        }

        public static Dictionary<string, string> SetDefaultColumnValues(Dictionary<string, string> data)
        {
            if (!data.ContainsKey(DbCIC.DsStatus))
                data.Add(DbCIC.DsStatus, "1");

            if (!data.ContainsKey(DbCIC.InsertOn))
                data.Add(DbCIC.InsertOn, DateTime.Now.ToString());
            else
                data[DbCIC.InsertOn] = DateTime.Now.ToString();

            return data;
        }

        public static string GetColumnString(List<ColumnData> colList, bool ignoreExists = false)
        {
            var colRetString = string.Empty;

            foreach (var col in colList)
            {
                if (col.existsInDB || ignoreExists)
                    colRetString += col.Name;
                else
                    colRetString += string.Format("'{0}' AS {1}", col.DefaultValue, col.Name);

                colRetString += ", ";
            }

            colRetString = colRetString.Remove(colRetString.Length - 2, 2);

            return colRetString;
        }
    }
}

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
                columns.Add(new ColumnData { Name = DbCIC.DsStatus, Type = DbDEF.TxtNull, DefaultValue = "1" });

            if (columns.Find(i => i.Name == DbCIC.InsertOn) == null)
                columns.Add(new ColumnData { Name = DbCIC.InsertOn, Type = DbDEF.TxtNull, DefaultValue = "01.01.1900" });

            if (columns.Find(i => i.Name == DbCIC.ModifyOn) == null)
                columns.Add(new ColumnData { Name = DbCIC.ModifyOn, Type = DbDEF.TxtNull });

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
    }
}

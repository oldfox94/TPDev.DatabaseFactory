using DbInterface.Models;
using System;
using System.Data;

namespace DbInterface.Helpers
{
    public class TableHelper
    {
        public static void SetDefaultColumnValues(DataTable tbl)
        {
            foreach(DataRow dr in tbl.Rows)
            {
                if (dr.RowState == DataRowState.Unchanged) continue;

                if (dr.RowState == DataRowState.Added)
                {
                    dr[DbCIC.InsertOn] = DateTime.Now.ToString();
                    dr[DbCIC.DsStatus] = "1";
                }

                if(dr.RowState == DataRowState.Modified || dr.RowState == DataRowState.Detached)
                {
                    dr[DbCIC.ModifyOn] = DateTime.Now.ToString();
                    if(string.IsNullOrEmpty(dr[DbCIC.DsStatus].ToString())) dr[DbCIC.DsStatus] = "1";
                }
            }
        }
    }
}

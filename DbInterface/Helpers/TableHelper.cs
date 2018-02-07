using DbInterface.Models;
using System;
using System.Data;

namespace DbInterface.Helpers
{
    public class TableHelper
    {
        public static void SetDefaultColumnValues(DataTable tbl, bool setInsertOn = true, bool setModifyOn = true)
        {
            foreach(DataRow dr in tbl.Rows)
            {
                if (dr.RowState == DataRowState.Unchanged) continue;

                if (dr.RowState == DataRowState.Added)
                {
                    if(setInsertOn) dr[DbCIC.InsertOn] = DateTime.Now.ToString();
                    if(string.IsNullOrEmpty(dr[DbCIC.DsStatus].ToString())) dr[DbCIC.DsStatus] = "1";
                }

                if(dr.RowState == DataRowState.Modified || dr.RowState == DataRowState.Detached)
                {
                    if(setModifyOn) dr[DbCIC.ModifyOn] = DateTime.Now.ToString();
                    if(string.IsNullOrEmpty(dr[DbCIC.DsStatus].ToString())) dr[DbCIC.DsStatus] = "1";
                }
            }
        }
    }
}

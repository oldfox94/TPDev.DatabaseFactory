using DbInterface.Models;
using Oracle.DataAccess.Client;
using System;

namespace OracleLibrary
{
    public class CONNECTION
    {
        public CONNECTION(DbConnectionData conData)
        {
            //Data Source=OraDb;User Id=scott;Password=tiger; lt. Oracle Doku
            if (string.IsNullOrEmpty(conData.Instance))
            {
                Settings.ConnectionString = string.Format(@"Data Source={0};Initial Catalog={1};User Id={2};Password = {3};",
                                                conData.ServerName, conData.Name, conData.User, conData.Password);
            }
            else
            {
                Settings.ConnectionString = string.Format(@"Data Source={1}\{0};Initial Catalog={2};User Id={3};Password = {4};",
                                                conData.Instance, conData.ServerName, conData.Name, conData.User, conData.Password);
            }
        }

        public static OracleConnection OpenCon()
        {
            var con = new OracleConnection(Settings.ConnectionString);
            con.Open();
            return con;
        }

        public static void CloseCon(OracleConnection con)
        {
            con.Close();
            con.Dispose();

            GC.Collect();
        }
    }
}

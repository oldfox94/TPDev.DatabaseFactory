using DbInterface.Models;
using System;
using System.Data.SqlClient;

namespace SQLLibrary
{
    public class CONNECTION
    {
        public CONNECTION(DbConnectionData conData, bool overwriteConData = true)
        {
            if(overwriteConData)
                Settings.ConnectionData = new DbConnectionData { Instance = conData.Instance, Name = conData.Name, Password = conData.Password, Path = conData.Path,
                                                                 Port = conData.Port, ServerName = conData.ServerName, User = conData.User };

            if(string.IsNullOrEmpty(conData.Instance))
            {
                Settings.ConnectionString = string.Format(@"Data Source={0};Initial Catalog={1};User Id={2};Password = {3};",
                                                conData.ServerName, conData.Name, conData.User, conData.Password);
            }
            else
            {
                Settings.ConnectionString = string.Format(@"Data Source={1}\{0};Initial Catalog={2};User Id={3};Password = {4};", 
                                                conData.Instance, conData.ServerName);
            }

            //Set Addentional Settings
            Settings.ThrowExceptions = conData.ThrowExceptions;
        }

        public static SqlConnection OpenCon()
        {
            var con = new SqlConnection(Settings.ConnectionString);
            con.Open();
            return con;
        }

        public static void CloseCon(SqlConnection con)
        {
            con.Close();
            con.Dispose();

            GC.Collect();
        }
    }
}

using DbInterface.Models;
using System.Data.SqlClient;

namespace SQLLibrary
{
    public class CONNECTION
    {
        public CONNECTION(DbConnectionData conData)
        {
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

            Settings.Con = new SqlConnection(Settings.ConnectionString);
        }
    }
}

using DbInterface.Models;
using MySql.Data.MySqlClient;

namespace MySQLLibrary
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
                                                conData.Instance, conData.ServerName, conData.Name, conData.User, conData.Password);
            }

            Settings.Con = new MySqlConnection(Settings.ConnectionString);
        }
    }
}

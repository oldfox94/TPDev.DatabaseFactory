using DbInterface.Models;
using MySql.Data.MySqlClient;

namespace MySQLLibrary
{
    public class Settings
    {
        public static DbType Type = DbType.MySQL;

        public static string ConnectionString { get; set; }
        public static MySqlConnection Con { get; set; }
    }
}

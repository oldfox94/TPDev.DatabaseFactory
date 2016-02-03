using MySql.Data.MySqlClient;

namespace MySQLLibrary
{
    public class Settings
    {
        public static string ConnectionString { get; set; }
        public static MySqlConnection Con { get; set; }
    }
}

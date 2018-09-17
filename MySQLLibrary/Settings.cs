using DbInterface.Models;

namespace MySQLLibrary
{
    public class Settings
    {
        public static DbType Type = DbType.MySQL;
        public static DbConnectionData ConnectionData { get; set; }
        public static string ConnectionString { get; set; }

        //Addentional Settings
        public static bool ThrowExceptions { get; set; }
    }
}

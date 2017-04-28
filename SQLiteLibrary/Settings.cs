using DbInterface.Models;

namespace SQLiteLibrary
{
    public class Settings
    {
        public static DbType Type = DbType.SQLite;

        public static string ConnectionString { get; set; }

        //Addentional Settings
        public static bool ThrowExceptions { get; set; }
    }
}

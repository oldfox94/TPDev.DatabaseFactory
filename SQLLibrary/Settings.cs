using DbInterface.Models;

namespace SQLLibrary
{
    public class Settings
    {
        public static DbType Type = DbType.SQL;
        public static string ConnectionString { get; set; }
    }
}

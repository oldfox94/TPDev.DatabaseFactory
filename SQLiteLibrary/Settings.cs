using DbInterface.Models;
using System.Data.SQLite;

namespace SQLiteLibrary
{
    public class Settings
    {
        public static DbType Type = DbType.SQLite;

        public static string ConnectionString { get; set; }
        public static SQLiteConnection Con { get; set; }
    }
}

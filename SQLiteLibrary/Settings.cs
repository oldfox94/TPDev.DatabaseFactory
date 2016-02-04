using System.Data.SQLite;

namespace SQLiteLibrary
{
    public class Settings
    {
        public static string ConnectionString { get; set; }
        public static SQLiteConnection Con { get; set; }
    }
}

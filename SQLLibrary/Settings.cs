using System.Data.SqlClient;

namespace SQLLibrary
{
    public class Settings
    {
        public static string ConnectionString { get; set; }
        public static SqlConnection Con { get; set; }
    }
}

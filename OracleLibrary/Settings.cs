using DbInterface.Models;

namespace OracleLibrary
{
    public class Settings
    {
        public static DbType Type = DbType.Oracle;

        public static string ConnectionString { get; set; }

        //Addentional Settings
        public static bool ThrowExceptions { get; set; }
    }
}

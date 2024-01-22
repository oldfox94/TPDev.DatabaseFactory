using DbInterface.Models;

namespace DatabaseFactory
{
    public class DbFactorySettings
    {
        public static DbFactory Factory { get; set; }
        public static DbType Type { get; set; }
    }
}
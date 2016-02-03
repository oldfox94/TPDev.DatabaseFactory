using DbInterface.Models;

namespace DbFactory
{
    public class DbFactorySettings
    {
        public static DbFactory Factory { get; set; }
        public static DbType Type { get; set; }
    }
}

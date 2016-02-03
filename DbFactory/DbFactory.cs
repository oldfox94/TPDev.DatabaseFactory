using DbInterface.Models;

namespace DbFactory
{
    public class DbFactory
    {
        public DbFactory(DbType type, DbConnectionData data)
        {
            DbFactorySettings.Type = type;
            switch(type)
            {
                case DbType.SQL:
                    break;

                case DbType.SQLite:
                    break;

                case DbType.MySQL:
                    break;
            }

            DbFactorySettings.Factory = this;

        }
    }
}

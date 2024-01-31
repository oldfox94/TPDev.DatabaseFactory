namespace DbInterface.Models
{
    public enum DbType
    {
        SQL,
        SQLite,
        MySQL,
        Oracle,
    }

    public static class DbORDER
    {
        public static string Ascending = "ASC";
        public static string Descending = "DESC";
    }

    public static class DbOPERATOR
    {
        public const string Smaller = "<";
        public static string Greater = ">";
        public static string Is = "=";
        public static string IsNot = "!=";
    }

    public static class DbDEF
    {
        public const string UniqueidentifierNull = "UNIQUEIDENTIFIER  NULL";
        public const string UniqueidentifierNotNull = "UNIQUEIDENTIFIER  NOT NULL";
        public const string UniqueidentifierNotNullPk = "UNIQUEIDENTIFIER  NOT NULL PRIMARY KEY";

        public const string TxtNull = "TEXT  NULL";
        public const string TxtNotNull = "TEXT  NOT NULL";
        public const string TxtNotNullPk = "TEXT  NOT NULL PRIMARY KEY";
        public const string TxtUniNotNull = "TEXT  UNIQUE NOT NULL";
        public const string TxtUniNotNullPk = "TEXT  UNIQUE NOT NULL PRIMARY KEY";

        public const string DateTimeNull = "DATETIME  NULL";
        public const string DateTimeNotNull = "DATETIME  NOT NULL";

        public const string VarchrNullMax = "VARCHAR(MAX)  NULL";
        public const string VarchrNotNullMax = "VARCHAR(MAX)  NOT NULL";
        public const string VarchrNotNullPkMax = "VARCHAR(MAX)  NOT NULL PRIMARY KEY";
        public const string VarchrUniNotNullMax = "VARCHAR(MAX)  UNIQUE NOT NULL";
        public const string VarchrUniNotNullPkMax = "VARCHAR(MAX)  UNIQUE NOT NULL PRIMARY KEY";

        public static string VarchrNull(int digits = 1){ return string.Format("VARCHAR({0})  NULL", digits); }
        public static string VarchrNotNull(int digits = 1) { return string.Format("VARCHAR({0})  NOT NULL", digits); }
        public static string VarchrNotNullPk(int digits = 1) { return string.Format("VARCHAR({0})  NOT NULL PRIMARY KEY", digits); }
        public static string VarchrUniNotNUll(int digits = 1) { return string.Format("VARCHAR({0})  UNIQUE NOT NULL", digits); }
        public static string VarchrUniNotNullPk(int digits = 1) { return string.Format("VARCHAR({0})  UNIQUE NOT NULL PRIMARY KEY", digits); }

        public const string IntNull = "INTEGER  NULL";
        public const string IntNotNull = "INTEGER  NOT NULL";

        public const string BigIntNull = "BIGINT  NULL";
        public const string BigIntNotNull = "BIGINT  NOT NULL";

        public const string BoolNull = "BOOLEAN  NULL";
        public const string BoolNotNull = "BOOLEAN  NOT NULL";
    }
}
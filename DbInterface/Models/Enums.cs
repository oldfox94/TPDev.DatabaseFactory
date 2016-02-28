namespace DbInterface.Models
{
    public enum DbType
    {
        SQL,
        SQLite,
        MySQL,
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
        public const string TxtNull = "TEXT  NULL";
        public const string TxtNotNull = "TEXT  NOT NULL";
        public const string TxtNotNullPk = "TEXT  NOT NULL PRIMARY KEY";
        public const string TxtUniNotNullPk = "TEXT  UNIQUE NOT NULL PRIMARY KEY";

        public static string VarchrNull(int digits = 1){ return string.Format("VARCHAR({0})  NULL", digits); }
        public static string VarchrNotNull(int digits = 1) { return string.Format("VARCHAR({0})  NOT NULL", digits); }
        public static string VarchrNotNullPk(int digits = 1) { return string.Format("VARCHAR({0})  NOT NULL PRIMARY KEY", digits); }
        public static string VarchrUniNotNullPk(int digits = 1) { return string.Format("VARCHAR({0})  UNIQUE NOT NULL PRIMARY KEY", digits); }

        public const string IntNull = "INTEGER  NULL";
        public const string IntNotNull = "INTEGER  NOT NULL";

        public const string BoolNull = "BOOLEAN  NULL";
        public const string BoolNotNull = "BOOLEAN  NOT NULL";
    }
}

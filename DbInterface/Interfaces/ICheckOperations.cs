namespace DbInterface.Interfaces
{
    public interface ICheckOperations
    {
        bool DatabaseExists(string databaseName);
        bool TableExists(string table);
        bool ColumnExists(string tableName, string columnName);
        bool ColumnValueExists(string table, string column, string value);
    }
}

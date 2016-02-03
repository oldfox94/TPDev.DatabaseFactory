namespace DbInterface.Interfaces
{
    public interface IDeleteOperations
    {
        bool ClearTable(string tableName);
        bool ClearDatabase(string databaseName);

        bool DeleteRows(string tableName, string where);
        bool DeleteTable(string tableName);
        bool DeleteDatabase(string databaseName);
    }
}

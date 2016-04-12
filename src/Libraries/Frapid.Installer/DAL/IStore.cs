namespace Frapid.Installer.DAL
{
    public interface IStore
    {
        string ProviderName { get; }
        void CreateDb(string tenant);
        bool HasDb(string dbName);
        bool HasSchema(string database, string schema);
        void RunSql(string database, string fromFile);
    }
}
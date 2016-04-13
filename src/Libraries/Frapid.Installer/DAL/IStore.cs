namespace Frapid.Installer.DAL
{
    public interface IStore
    {
        string ProviderName { get; }
        void CreateDb(string tenant);
        bool HasDb(string tenant, string database);
        bool HasSchema(string tenant, string database, string schema);
        void RunSql(string tenant, string database, string fromFile);
    }
}
namespace Frapid.Configuration.DbServer
{
    public interface IDbServer
    {
        string ProviderName { get; }
        string GetSuperUserConnectionString(string database = "");
        string GetConnectionString(string database = "", string userId = "", string password = "");
        string GetReportUserConnectionString(string database = "");
        string GetMetaConnectionString();
        string GetConnectionString(string host, string database, string username, string password, int port, bool enablePooling = true, int minPoolSize = 0, int maxPoolSize = 100);
        string GetProcedureCommand(string procedureName, string[] parameters);
        string DefaultSchemaQualify(string input);
        string AddLimit(string limit);
        string AddOffset(string offset);
    }
}
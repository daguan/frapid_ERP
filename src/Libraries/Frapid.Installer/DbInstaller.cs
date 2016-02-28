using System.Globalization;
using Frapid.Configuration;
using Frapid.DataAccess;

namespace Frapid.Installer
{
    public sealed class DbInstaller
    {
        public DbInstaller(string domain)
        {
            this.Catalog = domain;
        }

        public string Catalog { get; }

        public bool Install()
        {
            var inspector = new DbInspector(this.Catalog);
            bool hasDb = inspector.HasDb();
            bool canInstall = inspector.IsWellKnownDb();

            if (!hasDb && canInstall)
            {
                this.CreateDb();
                return true;
            }

            return false;
        }

        private void CreateDb()
        {
            string sql = "CREATE DATABASE {0} WITH ENCODING='UTF8' TEMPLATE=template0 LC_COLLATE='C' LC_CTYPE='C';";
            sql = string.Format(CultureInfo.InvariantCulture, sql,
                Sanitizer.SanitizeIdentifierName(this.Catalog.ToLower()));

            string catalog = Factory.MetaDatabase;
            string connectionString = ConnectionString.GetSuperUserConnectionString(catalog);
            Factory.Execute(connectionString, sql);
        }
    }
}
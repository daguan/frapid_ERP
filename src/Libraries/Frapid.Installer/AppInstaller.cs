using System.Data;
using System.Globalization;
using System.IO;
using System.Web.Hosting;
using Frapid.Configuration;
using Frapid.DataAccess;
using Frapid.Installer.Models;
using Microsoft.VisualBasic.FileIO;
using Npgsql;

namespace Frapid.Installer
{
    public class AppInstaller
    {
        public AppInstaller(string catalog, Installable installable)
        {
            this.Catalog = catalog;
            this.Installable = installable;
        }

        public Installable Installable { get; }
        protected string Catalog { get; set; }

        public bool HasSchema()
        {
            const string sql = "SELECT COUNT(*) FROM pg_catalog.pg_namespace WHERE nspname=@0;";
            return Factory.Scalar<int>(this.Catalog, sql, this.Installable.DbSchema).Equals(1);
        }

        public void Install()
        {
            foreach (var dependency in this.Installable.Dependencies)
            {
                new AppInstaller(this.Catalog, dependency).Install();
            }

            this.CreateSchema();
            this.CreateMy();
            this.CreateOverride();
        }

        protected void CreateMy()
        {
            if (string.IsNullOrWhiteSpace(this.Installable.My))
            {
                return;
            }

            string catalog = this.Catalog;
            if (this.Installable.IsMeta)
            {
                catalog = Factory.MetaDatabase;
            }

            string db = this.Installable.My;
            string path = HostingEnvironment.MapPath(db);
            this.RunSql(catalog, path);
        }

        protected void CreateSchema()
        {
            if (string.IsNullOrWhiteSpace(this.Installable.DbSchema))
            {
                return;
            }

            if (this.HasSchema())
            {
                return;
            }

            string catalog = this.Catalog;
            if (this.Installable.IsMeta)
            {
                catalog = Factory.MetaDatabase;
            }

            string db = this.Installable.BlankDbPath;

            if (this.Installable.InstallSample && !string.IsNullOrWhiteSpace(this.Installable.SampleDbPath))
            {
                db = this.Installable.SampleDbPath;
            }

            string path = HostingEnvironment.MapPath(db);
            this.RunSql(catalog, path);
        }

        private void RunSql(string catalog, string fromFile)
        {
            if (string.IsNullOrWhiteSpace(fromFile))
            {
                return;
            }

            string sql = File.ReadAllText(fromFile);
            string connectionString = ConnectionString.GetSuperUserConnectionString(catalog);

            using (var connection = new NpgsqlConnection(connectionString))
            {
                using (var command = new NpgsqlCommand())
                {
                    command.CommandText = sql;
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;

                    connection.Open();
                    command.ExecuteScalar();
                }
            }
        }


        protected void CreateOverride()
        {
            if (string.IsNullOrWhiteSpace(this.Installable.OverrideTemplatePath) ||
                string.IsNullOrWhiteSpace(this.Installable.OverrideDestination))
            {
                return;
            }

            string source = HostingEnvironment.MapPath(this.Installable.OverrideTemplatePath);
            string destination = string.Format(CultureInfo.InvariantCulture, this.Installable.OverrideDestination, this.Catalog);
            destination = HostingEnvironment.MapPath(destination);


            if (string.IsNullOrWhiteSpace(source) || string.IsNullOrWhiteSpace(destination))
            {
                return;
            }

            if (!Directory.Exists(source))
            {
                return;
            }

            FileSystem.CopyDirectory(source, destination, true);
        }
    }
}
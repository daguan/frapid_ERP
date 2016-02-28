using System.Data;
using System.Globalization;
using System.IO;
using System.Web.Hosting;
using Frapid.Configuration;
using Frapid.DataAccess;
using Frapid.Installer.Models;
using Microsoft.VisualBasic.FileIO;
using Npgsql;
using Serilog;

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

            using (var db = DbProvider.Get(ConnectionString.GetSuperUserConnectionString(this.Catalog)).GetDatabase())
            {
                return db.ExecuteScalar<int>(sql, this.Installable.DbSchema).Equals(1);
            }
        }

        public void Install()
        {
            foreach (var dependency in this.Installable.Dependencies)
            {
                Log.Verbose($"Installing module {dependency.ApplicationName} because the module {this.Installable.ApplicationName} depends on it.");
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
                Log.Verbose($"Skipped {this.Installable.ApplicationName} schema creation because it already exists.");
                return;
            }

            Log.Verbose($"Creating schema {this.Installable.DbSchema}");

            string catalog = this.Catalog;
            if (this.Installable.IsMeta)
            {
                Log.Verbose($"Creating database of {this.Installable.ApplicationName} under meta catalog {Factory.MetaDatabase}.");
                catalog = Factory.MetaDatabase;
            }

            string db = this.Installable.BlankDbPath;
            string path = HostingEnvironment.MapPath(db);
            this.RunSql(catalog, path);

            if (this.Installable.InstallSample && !string.IsNullOrWhiteSpace(this.Installable.SampleDbPath))
            {
                Log.Verbose($"Creating sample data of {this.Installable.ApplicationName}.");
                db = this.Installable.SampleDbPath;
                path = HostingEnvironment.MapPath(db);
                this.RunSql(catalog, path);
            }
        }

        private void RunSql(string catalog, string fromFile)
        {
            if (string.IsNullOrWhiteSpace(fromFile) || File.Exists(fromFile).Equals(false))
            {
                return;
            }

            string sql = File.ReadAllText(fromFile);

            //PetaPoco/NPoco Escape
            //ORM: Remove this behavior if you change the ORM.
            sql = sql.Replace("@", "@@");

            Log.Verbose($"Running SQL {sql}");

            string connectionString = ConnectionString.GetSuperUserConnectionString(catalog);
            Factory.Execute(connectionString, sql);
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

            Log.Verbose($"Creating overide. Source: {source}, desitation: {destination}.");
            FileSystem.CopyDirectory(source, destination, true);
        }
    }
}
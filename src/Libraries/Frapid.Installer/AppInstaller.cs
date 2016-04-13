using System.Globalization;
using System.IO;
using System.Web.Hosting;
using Frapid.DataAccess;
using Frapid.Installer.DAL;
using Frapid.Installer.Models;
using Microsoft.VisualBasic.FileIO;
using Serilog;

namespace Frapid.Installer
{
    public class AppInstaller
    {
        public AppInstaller(string tenant, string database, Installable installable)
        {
            this.Tenant = tenant;
            this.Database = database;
            this.Installable = installable;
        }

        public Installable Installable { get; }
        protected string Tenant { get; set; }
        protected string Database { get; set; }

        public bool HasSchema(string database)
        {
            return Store.HasSchema(this.Tenant, database, this.Installable.DbSchema);
        }

        public void Install()
        {
            foreach (var dependency in this.Installable.Dependencies)
            {
                Log.Verbose(
                    $"Installing module {dependency.ApplicationName} because the module {this.Installable.ApplicationName} depends on it.");
                new AppInstaller(this.Tenant, this.Database, dependency).Install();
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

            string database = this.Database;
            if (this.Installable.IsMeta)
            {
                database = Factory.GetMetaDatabase(database);
            }

            string db = this.Installable.My;
            string path = HostingEnvironment.MapPath(db);
            this.RunSql(database, database, path);
        }

        protected void CreateSchema()
        {
            string database = this.Database;

            if (this.Installable.IsMeta)
            {
                Log.Verbose(
                    $"Creating database of {this.Installable.ApplicationName} under meta database {Factory.GetMetaDatabase(this.Database)}.");
                database = Factory.GetMetaDatabase(this.Database);
            }

            if (string.IsNullOrWhiteSpace(this.Installable.DbSchema))
            {
                return;
            }

            if (this.HasSchema(database))
            {
                Log.Verbose($"Skipped {this.Installable.ApplicationName} schema creation because it already exists.");
                return;
            }

            Log.Verbose($"Creating schema {this.Installable.DbSchema}");


            string db = this.Installable.BlankDbPath;
            string path = HostingEnvironment.MapPath(db);
            this.RunSql(database, database, path);

            if (this.Installable.InstallSample && !string.IsNullOrWhiteSpace(this.Installable.SampleDbPath))
            {
                Log.Verbose($"Creating sample data of {this.Installable.ApplicationName}.");
                db = this.Installable.SampleDbPath;
                path = HostingEnvironment.MapPath(db);
                this.RunSql(database, database, path);
            }
        }

        private void RunSql(string tenant, string database, string fromFile)
        {
            Store.RunSql(tenant, database, fromFile);
        }


        protected void CreateOverride()
        {
            if (string.IsNullOrWhiteSpace(this.Installable.OverrideTemplatePath) ||
                string.IsNullOrWhiteSpace(this.Installable.OverrideDestination))
            {
                return;
            }

            string source = HostingEnvironment.MapPath(this.Installable.OverrideTemplatePath);
            string destination = string.Format(CultureInfo.InvariantCulture, this.Installable.OverrideDestination,
                this.Database);
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
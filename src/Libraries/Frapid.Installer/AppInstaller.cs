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
        public AppInstaller(string database, Installable installable)
        {
            this.Database = database;
            this.Installable = installable;
        }

        public Installable Installable { get; }
        protected string Database { get; set; }

        public bool HasSchema(string database)
        {
            return Store.HasSchema(database, this.Installable.DbSchema);
        }

        public void Install()
        {
            foreach (var dependency in this.Installable.Dependencies)
            {
                Log.Verbose(
                    $"Installing module {dependency.ApplicationName} because the module {this.Installable.ApplicationName} depends on it.");
                new AppInstaller(this.Database, dependency).Install();
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
                database = Factory.MetaDatabase;
            }

            string db = this.Installable.My;
            string path = HostingEnvironment.MapPath(db);
            this.RunSql(database, path);
        }

        protected void CreateSchema()
        {
            string database = this.Database;
            if (this.Installable.IsMeta)
            {
                Log.Verbose(
                    $"Creating database of {this.Installable.ApplicationName} under meta database {Factory.MetaDatabase}.");
                database = Factory.MetaDatabase;
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
            this.RunSql(database, path);

            if (this.Installable.InstallSample && !string.IsNullOrWhiteSpace(this.Installable.SampleDbPath))
            {
                Log.Verbose($"Creating sample data of {this.Installable.ApplicationName}.");
                db = this.Installable.SampleDbPath;
                path = HostingEnvironment.MapPath(db);
                this.RunSql(database, path);
            }
        }

        private void RunSql(string database, string fromFile)
        {
            Store.RunSql(database, fromFile);
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
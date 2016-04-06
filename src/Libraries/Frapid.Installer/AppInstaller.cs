using System.Globalization;
using System.IO;
using System.Web.Hosting;
using Frapid.Configuration;
using Frapid.DataAccess;
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

        public bool HasSchema()
        {
            const string sql = "SELECT COUNT(*) FROM pg_catalog.pg_namespace WHERE nspname=@0;";

            using (var db = DbProvider.Get(ConnectionString.GetSuperUserConnectionString(this.Database)).GetDatabase())
            {
                return db.ExecuteScalar<int>(sql, this.Installable.DbSchema).Equals(1);
            }
        }

        public void Install()
        {
            foreach (var dependency in this.Installable.Dependencies)
            {
                Log.Verbose($"Installing module {dependency.ApplicationName} because the module {this.Installable.ApplicationName} depends on it.");
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

            string database = this.Database;
            if (this.Installable.IsMeta)
            {
                Log.Verbose(
                    $"Creating database of {this.Installable.ApplicationName} under meta database {Factory.MetaDatabase}.");
                database = Factory.MetaDatabase;
            }

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
            if (string.IsNullOrWhiteSpace(fromFile) || File.Exists(fromFile).Equals(false))
            {
                return;
            }

            string sql = File.ReadAllText(fromFile);

            //PetaPoco/NPoco Escape
            //ORM: Remove this behavior if you change the ORM.
            sql = sql.Replace("@", "@@");

            Log.Verbose($"Running SQL {sql}");

            string connectionString = ConnectionString.GetSuperUserConnectionString(database);
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
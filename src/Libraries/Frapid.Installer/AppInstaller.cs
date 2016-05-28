using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using Frapid.Configuration;
using Frapid.DataAccess;
using Frapid.Framework;
using Frapid.Installer.DAL;
using Frapid.Installer.Helpers;
using Frapid.Installer.Models;

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

        public async Task<bool> HasSchemaAsync(string database)
        {
            return await Store.HasSchemaAsync(this.Tenant, database, this.Installable.DbSchema);
        }

        public async Task InstallAsync()
        {
            foreach (var dependency in this.Installable.Dependencies)
            {
                InstallerLog.Verbose(
                    $"Installing module {dependency.ApplicationName} because the module {this.Installable.ApplicationName} depends on it.");
                await new AppInstaller(this.Tenant, this.Database, dependency).InstallAsync();
            }

            await this.CreateSchemaAsync();
            await this.CreateMyAsync();
            this.CreateOverride();
        }

        protected async Task CreateMyAsync()
        {
            if (string.IsNullOrWhiteSpace(this.Installable.My))
            {
                return;
            }

            var database = this.Database;
            if (this.Installable.IsMeta)
            {
                database = Factory.GetMetaDatabase(database);
            }

            var db = this.Installable.My;
            var path = PathMapper.MapPath(db);
            await this.RunSqlAsync(database, database, path);
        }

        protected async Task CreateSchemaAsync()
        {
            var database = this.Database;

            if (this.Installable.IsMeta)
            {
                InstallerLog.Verbose(
                    $"Creating database of {this.Installable.ApplicationName} under meta database {Factory.GetMetaDatabase(this.Database)}.");
                database = Factory.GetMetaDatabase(this.Database);
            }

            if (string.IsNullOrWhiteSpace(this.Installable.DbSchema))
            {
                return;
            }

            if (await this.HasSchemaAsync(database))
            {
                InstallerLog.Verbose(
                    $"Skipped {this.Installable.ApplicationName} schema creation because it already exists.");
                return;
            }

            InstallerLog.Verbose($"Creating schema {this.Installable.DbSchema}");


            var db = this.Installable.BlankDbPath;
            var path = PathMapper.MapPath(db);
            await this.RunSqlAsync(this.Tenant, database, path);

            if (this.Installable.InstallSample && !string.IsNullOrWhiteSpace(this.Installable.SampleDbPath))
            {
                InstallerLog.Verbose($"Creating sample data of {this.Installable.ApplicationName}.");
                db = this.Installable.SampleDbPath;
                path = PathMapper.MapPath(db);
                await this.RunSqlAsync(database, database, path);
            }
        }

        private async Task RunSqlAsync(string tenant, string database, string fromFile)
        {
            await Store.RunSqlAsync(tenant, database, fromFile);
        }


        protected void CreateOverride()
        {
            if (string.IsNullOrWhiteSpace(this.Installable.OverrideTemplatePath) ||
                string.IsNullOrWhiteSpace(this.Installable.OverrideDestination))
            {
                return;
            }

            var source = PathMapper.MapPath(this.Installable.OverrideTemplatePath);
            var destination = string.Format(CultureInfo.InvariantCulture, this.Installable.OverrideDestination,
                this.Database);
            destination = PathMapper.MapPath(destination);


            if (string.IsNullOrWhiteSpace(source) || string.IsNullOrWhiteSpace(destination))
            {
                return;
            }

            if (!Directory.Exists(source))
            {
                return;
            }

            InstallerLog.Verbose($"Creating overide. Source: {source}, desitation: {destination}.");
            FileHelper.CopyDirectory(source, destination);
        }
    }
}
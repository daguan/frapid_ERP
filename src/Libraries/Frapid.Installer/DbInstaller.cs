using System.Threading.Tasks;
using Frapid.Configuration.Db;
using Frapid.Installer.DAL;
using Frapid.Installer.Helpers;

namespace Frapid.Installer
{
    public sealed class DbInstaller
    {
        public DbInstaller(string domain)
        {
            this.Tenant = domain;
        }

        public string Tenant { get; }

        public async Task<bool> InstallAsync()
        {
            string meta = DbProvider.GetMetaDatabase(this.Tenant);
            var inspector = new DbInspector(this.Tenant, meta);
            bool hasDb = await inspector.HasDbAsync().ConfigureAwait(false);
            bool canInstall = inspector.IsWellKnownDb();

            if(hasDb)
            {
                InstallerLog.Verbose($"No need to create database \"{this.Tenant}\" because it already exists.");
            }

            if(!canInstall)
            {
                InstallerLog.Verbose($"Cannot create a database under the name \"{this.Tenant}\" because the name is not a well-known tenant name.");
            }

            if(!hasDb && canInstall)
            {
                InstallerLog.Information($"Creating database \"{this.Tenant}\".");
                await this.CreateDbAsync().ConfigureAwait(false);
                return true;
            }

            return false;
        }

        private async Task CreateDbAsync()
        {
            await Store.CreateDbAsync(this.Tenant).ConfigureAwait(false);
        }
    }
}
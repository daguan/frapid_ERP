using Frapid.Installer.DAL;
using Serilog;

namespace Frapid.Installer
{
    public sealed class DbInstaller
    {
        public DbInstaller(string domain)
        {
            this.Tenant = domain;
        }

        public string Tenant { get; }

        public bool Install()
        {
            var inspector = new DbInspector(this.Tenant);
            bool hasDb = inspector.HasDb();
            bool canInstall = inspector.IsWellKnownDb();

            if (hasDb)
            {
                Log.Verbose($"No need to create database \"{this.Tenant}\" because it already exists.");
            }

            if (!canInstall)
            {
                Log.Verbose(
                    $"Cannot create a database under the name \"{this.Tenant}\" because the name is not a well-known tenant name.");
            }

            if (!hasDb && canInstall)
            {
                Log.Information($"Creating database \"{this.Tenant}\".");
                this.CreateDb();
                return true;
            }

            return false;
        }

        private void CreateDb()
        {
            Store.CreateDb(this.Tenant);
        }
    }
}
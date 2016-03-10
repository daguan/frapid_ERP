using System.IO;
using System.IO.Compression;
using System.Web.Hosting;
using Microsoft.VisualBasic.FileIO;

namespace Frapid.Backups
{
    public sealed class Resources
    {
        public Resources(string tenant, string backupPath, string fileName)
        {
            this.Tenant = tenant;
            this.BackupPath = backupPath;
            this.FileName = fileName;
        }

        public string Tenant { get; set; }
        public string BackupPath { get; set; }
        public string FileName { get; set; }
        public string BackupDirectory { get; set; }

        public void AddTenantDataToBackup()
        {
            string source = HostingEnvironment.MapPath($"/Tenants/{this.Tenant}");
            string destination = this.BackupPath;

            if (source != null)
            {
                FileSystem.CopyDirectory(source, destination);
            }
        }

        public void Compress()
        {
            ZipFile.CreateFromDirectory(this.BackupDirectory, this.BackupDirectory + ".zip");
        }

        public void Clean()
        {
            Directory.Delete(this.BackupDirectory, true);
        }
    }
}
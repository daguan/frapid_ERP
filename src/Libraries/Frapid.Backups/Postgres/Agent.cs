using System;
using System.IO;
using System.Web.Hosting;

namespace Frapid.Backups.Postgres
{
    public class Agent : IDbAgent
    {
        public event Progressing Progress;
        public event Complete Complete;
        public event Fail Fail;
        public string FileName { get; set; }
        public string Tenant { get; set; }
        public DbServer Server { get; set; }
        public string BackupFileLocation { get; set; }

        public bool Backup(Action<string> successCallback, Action<string> failCallback)
        {
            if (string.IsNullOrWhiteSpace(this.BackupFileLocation))
            {
                string message = "Cannot find a suitable directory to create a PostgreSQL DB Backup.";
                this.OnOnBackupFail(new ProgressInfo(message));
                failCallback(message);
                return false;

            }

            string backupDirectory = Path.Combine(this.BackupFileLocation, this.FileName);
            string path = Path.Combine(backupDirectory, "db.backup");

            Directory.CreateDirectory(backupDirectory);


            var process = new Process(this.Server, path, this.Tenant);

            process.Progress += delegate(ProgressInfo info)
            {
                var progress = this.Progress;
                progress?.Invoke(new ProgressInfo(info.Message));
            };

            process.BackupComplete += delegate (object sender, EventArgs args)
            {
                this.OnOnBackupComplete(sender, args);
                successCallback(this.FileName);
            };

            bool result = process.Execute();

            if (!result)
            {
                string message = "Could not create backup.";
                this.OnOnBackupFail(new ProgressInfo(message));
                failCallback(message);
                return false;
            }

            return true;
        }

        private void OnOnBackupComplete(object sender, EventArgs e)
        {
            var complete = this.Complete;
            complete?.Invoke(sender, e);
        }

        private void OnOnBackupFail(ProgressInfo progressinfo)
        {
            var fail = this.Fail;
            fail?.Invoke(progressinfo);
        }
    }
}
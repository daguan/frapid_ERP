using System;
using System.IO;
using Frapid.DataAccess;

namespace Frapid.Backups.SqlServer
{
    public class Agent : IDbAgent
    {
        public string Tenant { get; set; }
        public event Progressing Progress;
        public event Complete Complete;
        public event Fail Fail;
        public string FileName { get; set; }
        public DbServer Server { get; set; }
        public string BackupFileLocation { get; set; }

        public bool Backup(Action<string> successCallback, Action<string> failCallback)
        {
            string destination = Path.Combine(this.BackupFileLocation, this.FileName);

            string sql = $"BACKUP DATABASE {this.Tenant} TO DISK='@0';";

            try
            {
                Factory.NonQuery(this.Tenant, sql, destination);
            }
            catch (Exception ex)
            {
                string message = "Could not create backup." + ex.Message;
                this.OnOnBackupFail(new ProgressInfo(message));
                failCallback(message);

                return false;
            }

            this.OnOnBackupComplete(this, new EventArgs());
            successCallback(this.BackupFileLocation);
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
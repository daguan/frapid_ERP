using System;

namespace Frapid.Backups
{
    public interface IDbAgent
    {
        event Progressing Progress;
        event Complete Complete;
        event Fail Fail;
        string FileName { get; set; }
        DbServer Server { get; set; }
        string BackupFileLocation { get; set; }
        bool Backup(Action<string> success, Action<string> fail);
    }
}
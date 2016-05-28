using System;
using System.Data;

namespace Frapid.NPoco
{
    public class Transaction : ITransaction
    {
        IDatabase _db;

        public Transaction(IDatabase db, IsolationLevel isolationLevel)
        {
            this._db = db;
            this._db.BeginTransaction(isolationLevel);
        }

        public virtual void Complete()
        {
            this._db.CompleteTransaction();
            this._db = null;
        }

        public void Dispose()
        {
            if (this._db != null)
            {
                this._db.AbortTransaction();
            }
        }
    }

    public interface ITransaction : IDisposable
    {
        void Complete();
    }
}
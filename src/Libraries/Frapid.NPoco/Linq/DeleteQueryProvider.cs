using System;
using System.Linq.Expressions;
using Frapid.NPoco.Expressions;
#if NET45
using System.Threading.Tasks;
#endif

namespace Frapid.NPoco.Linq
{
    public interface IDeleteQueryProvider<T>
    {
        IDeleteQueryProvider<T> Where(Expression<Func<T, bool>> whereExpression);
        int Execute();
#if NET45
        Task<int> ExecuteAsync();
#endif
    }

    public class DeleteQueryProvider<T> : IDeleteQueryProvider<T>
    {
        private readonly IDatabase _database;
        private SqlExpression<T> _sqlExpression;

        public DeleteQueryProvider(IDatabase database)
        {
            this._database = database;
            this._sqlExpression = database.DatabaseType.ExpressionVisitor<T>(database, database.PocoDataFactory.ForType(typeof(T)), false);
        }

        public IDeleteQueryProvider<T> Where(Expression<Func<T, bool>> whereExpression)
        {
            this._sqlExpression = this._sqlExpression.Where(whereExpression);
            return this;
        }

        public int Execute()
        {
            return this._database.Execute(this._sqlExpression.Context.ToDeleteStatement(), this._sqlExpression.Context.Params);
        }

#if NET45
        public Task<int> ExecuteAsync()
        {
            return _database.ExecuteAsync(_sqlExpression.Context.ToDeleteStatement(), _sqlExpression.Context.Params);
        }
#endif
    }
}
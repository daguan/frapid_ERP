using System;
using System.Linq.Expressions;
using Frapid.NPoco.Expressions;
#if NET45
using System.Threading.Tasks;
#endif

namespace Frapid.NPoco.Linq
{
    public interface IUpdateQueryProvider<T>
    {
        IUpdateQueryProvider<T> Where(Expression<Func<T, bool>> whereExpression);
        IUpdateQueryProvider<T> ExcludeDefaults();
        IUpdateQueryProvider<T> OnlyFields(Expression<Func<T, object>> onlyFields);
        int Execute(T obj);
#if NET45
        Task<int> ExecuteAsync(T obj);
#endif
    }

    public class UpdateQueryProvider<T> : IUpdateQueryProvider<T>
    {
        private readonly IDatabase _database;
        private SqlExpression<T> _sqlExpression;
        private bool _excludeDefaults;
        private bool _onlyFields;

        public UpdateQueryProvider(IDatabase database)
        {
            this._database = database;
            this._sqlExpression = database.DatabaseType.ExpressionVisitor<T>(database, database.PocoDataFactory.ForType(typeof(T)), false);
        }

        public IUpdateQueryProvider<T> Where(Expression<Func<T, bool>> whereExpression)
        {
            this._sqlExpression = this._sqlExpression.Where(whereExpression);
            return this;
        }

        public IUpdateQueryProvider<T> ExcludeDefaults()
        {
            this._excludeDefaults = true;
            return this;
        }

        public IUpdateQueryProvider<T> OnlyFields(Expression<Func<T, object>> onlyFields)
        {
            this._sqlExpression = this._sqlExpression.Update(onlyFields);
            this._onlyFields = true;
            return this;
        }

        public int Execute(T obj)
        {
            string updateStatement = this._sqlExpression.Context.ToUpdateStatement(obj, this._excludeDefaults, this._onlyFields);
            return this._database.Execute(updateStatement, this._sqlExpression.Context.Params);
        }

#if NET45
        public Task<int> ExecuteAsync(T obj)
        {
            var updateStatement = _sqlExpression.Context.ToUpdateStatement(obj, _excludeDefaults, _onlyFields);
            return _database.ExecuteAsync(updateStatement, _sqlExpression.Context.Params);
        }
#endif
    }
}
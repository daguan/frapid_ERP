using System;
using System.Linq.Expressions;
using Frapid.NPoco.Expressions;

namespace Frapid.NPoco.Linq
{
    public class QueryBuilder<T>
    {
        public QueryBuilderData<T> Data { get; private set; }

        public QueryBuilder()
        {
            this.Data = new QueryBuilderData<T>();
        }

        public virtual QueryBuilder<T> Limit(int rows)
        {
            this.Data.Rows = rows;
            return this;
        }

        public virtual QueryBuilder<T> Limit(int skip, int rows)
        {
            this.Data.Rows = rows;
            this.Data.Skip = skip;
            return this;
        }

        public virtual QueryBuilder<T> Where(Expression<Func<T, bool>> whereExpression)
        {
            this.Data.WhereExpression = this.Data.WhereExpression == null ? PredicateBuilder.Create(whereExpression) : this.Data.WhereExpression.And(whereExpression);
            return this;
        }

        public virtual QueryBuilder<T> OrderBy(Expression<Func<T, object>> orderByExpression)
        {
            this.Data.OrderByExpression = orderByExpression;
            return this;
        }

        public virtual QueryBuilder<T> OrderByDescending(Expression<Func<T, object>> orderByDescendingExpression)
        {
            this.Data.OrderByDescendingExpression = orderByDescendingExpression;
            return this;
        }

        public virtual QueryBuilder<T> ThenBy(Expression<Func<T, object>> thenByExpression)
        {
            this.Data.ThenByExpression.Add(thenByExpression);
            return this;
        }

        public virtual QueryBuilder<T> ThenByDescending(Expression<Func<T, object>> thenByDescendingExpression)
        {
            this.Data.ThenByDescendingExpression.Add(thenByDescendingExpression);
            return this;
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Frapid.NPoco.Expressions;

namespace Frapid.NPoco.Linq
{
    public interface IQueryResultProvider<T>
    {
        T FirstOrDefault();
        T FirstOrDefault(Expression<Func<T, bool>> whereExpression);
        T First();
        T First(Expression<Func<T, bool>> whereExpression);
        T SingleOrDefault();
        T SingleOrDefault(Expression<Func<T, bool>> whereExpression);
        T Single();
        T Single(Expression<Func<T, bool>> whereExpression);
        int Count();
        int Count(Expression<Func<T, bool>> whereExpression);
        bool Any();
        bool Any(Expression<Func<T, bool>> whereExpression);
        List<T> ToList();
        T[] ToArray();
        IEnumerable<T> ToEnumerable();
#if !NET35
        List<dynamic> ToDynamicList();
        IEnumerable<dynamic> ToDynamicEnumerable();
#endif
        Page<T> ToPage(int page, int pageSize);
        List<T2> ProjectTo<T2>(Expression<Func<T, T2>> projectionExpression);
        List<T2> Distinct<T2>(Expression<Func<T, T2>> projectionExpression);
        List<T> Distinct();
#if !NET35 && !NET40
        System.Threading.Tasks.Task<List<T>> ToListAsync();
        System.Threading.Tasks.Task<T[]> ToArrayAsync();
        System.Threading.Tasks.Task<IEnumerable<T>> ToEnumerableAsync();
        System.Threading.Tasks.Task<T> FirstOrDefaultAsync();
        System.Threading.Tasks.Task<T> FirstAsync();
        System.Threading.Tasks.Task<T> SingleOrDefaultAsync();
        System.Threading.Tasks.Task<T> SingleAsync();
        System.Threading.Tasks.Task<int> CountAsync();
        System.Threading.Tasks.Task<bool> AnyAsync();
        System.Threading.Tasks.Task<Page<T>> ToPageAsync(int page, int pageSize);
        System.Threading.Tasks.Task<List<T2>> ProjectToAsync<T2>(Expression<Func<T, T2>> projectionExpression);
#endif
    }

    public interface IQueryProvider<T> : IQueryResultProvider<T>
    {
        IQueryProvider<T> Where(Expression<Func<T, bool>> whereExpression);
        IQueryProvider<T> Where(string sql, params object[] args);
        IQueryProvider<T> Where(Sql sql);
        IQueryProvider<T> Where(Func<QueryContext<T>, Sql> queryBuilder);
        IQueryProvider<T> OrderBy(Expression<Func<T, object>> column);
        IQueryProvider<T> OrderByDescending(Expression<Func<T, object>> column);
        IQueryProvider<T> ThenBy(Expression<Func<T, object>> column);
        IQueryProvider<T> ThenByDescending(Expression<Func<T, object>> column);
        IQueryProvider<T> Limit(int rows);
        IQueryProvider<T> Limit(int skip, int rows);
        IQueryProvider<T> From(QueryBuilder<T> builder);
    }

    public interface IQueryProviderWithIncludes<T> : IQueryProvider<T>
    {
        IQueryProvider<T> IncludeMany(Expression<Func<T, IList>> expression, JoinType joinType = JoinType.Left);
        IQueryProviderWithIncludes<T> Include<T2>(Expression<Func<T, T2>> expression, JoinType joinType = JoinType.Left) where T2 : class;
        IQueryProviderWithIncludes<T> Include<T2>(Expression<Func<T, T2>> expression, string tableAlias, JoinType joinType = JoinType.Left) where T2 : class;
        IQueryProviderWithIncludes<T> UsingAlias(string empty);
    }

    public class QueryProvider<T> : IQueryProviderWithIncludes<T>, ISimpleQueryProviderExpression<T>
    {
        private readonly Database _database;
        private SqlExpression<T> _sqlExpression;
        private Dictionary<string, JoinData> _joinSqlExpressions = new Dictionary<string, JoinData>();
        private readonly ComplexSqlBuilder<T> _buildComplexSql;
        private Expression<Func<T, IList>> _listExpression = null;
        private PocoData _pocoData;

        public QueryProvider(Database database, Expression<Func<T, bool>> whereExpression)
        {
            this._database = database;
            this._pocoData = database.PocoDataFactory.ForType(typeof (T));
            this._sqlExpression = database.DatabaseType.ExpressionVisitor<T>(database, this._pocoData, true);
            this._buildComplexSql = new ComplexSqlBuilder<T>(database, this._pocoData, this._sqlExpression, this._joinSqlExpressions);
            this._sqlExpression = this._sqlExpression.Where(whereExpression);
        }

        public QueryProvider(Database database)
            : this(database, null)
        {
        }

        SqlExpression<T> ISimpleQueryProviderExpression<T>.AtlasSqlExpression => this._sqlExpression;

        public IQueryProvider<T> IncludeMany(Expression<Func<T, IList>> expression, JoinType joinType = JoinType.Left)
        {
            this._listExpression = expression;
            return this.QueryProviderWithIncludes(expression, null, joinType);
        }

        public IQueryProviderWithIncludes<T> Include<T2>(Expression<Func<T, T2>> expression, JoinType joinType = JoinType.Left) where T2 : class
        {
            return this.QueryProviderWithIncludes(expression, null, joinType);
        }

        public IQueryProviderWithIncludes<T> Include<T2>(Expression<Func<T, T2>> expression, string tableAlias, JoinType joinType = JoinType.Left) where T2 : class
        {
            return this.QueryProviderWithIncludes(expression, tableAlias, joinType);
        }

        public IQueryProviderWithIncludes<T> UsingAlias(string tableAlias)
        {
            if (!string.IsNullOrEmpty(tableAlias))
                this._pocoData.TableInfo.AutoAlias = tableAlias;
            return this;
        }

        private IQueryProviderWithIncludes<T> QueryProviderWithIncludes(Expression expression, string tableAlias, JoinType joinType)
        {
            Dictionary<string, JoinData> joinExpressions = this._buildComplexSql.GetJoinExpressions(expression, tableAlias, joinType);
            foreach (KeyValuePair<string, JoinData> joinExpression in joinExpressions)
            {
                this._joinSqlExpressions[joinExpression.Key] = joinExpression.Value;
            }

            return this;
        }

        public IQueryProvider<T> From(QueryBuilder<T> builder)
        {
            if (!builder.Data.Skip.HasValue && builder.Data.Rows.HasValue)
            {
                this.Limit(builder.Data.Rows.Value);
            }

            if (builder.Data.Skip.HasValue && builder.Data.Rows.HasValue)
            {
                this.Limit(builder.Data.Skip.Value, builder.Data.Rows.Value);
            }

            if (builder.Data.WhereExpression != null)
            {
                this.Where(builder.Data.WhereExpression);
            }

            if (builder.Data.OrderByExpression != null)
            {
                this.OrderBy(builder.Data.OrderByExpression);
            }

            if (builder.Data.OrderByDescendingExpression != null)
            {
                this.OrderByDescending(builder.Data.OrderByDescendingExpression);
            }

            if (builder.Data.ThenByExpression.Any())
            {
                foreach (Expression<Func<T, object>> expression in builder.Data.ThenByExpression)
                {
                    this.ThenBy(expression);
                }
            }

            if (builder.Data.ThenByDescendingExpression.Any())
            {
                foreach (Expression<Func<T, object>> expression in builder.Data.ThenByDescendingExpression)
                {
                    this.ThenByDescending(expression);
                }
            }
            
            return this;
        }

        private void AddWhere(Expression<Func<T, bool>> whereExpression)
        {
            if (whereExpression != null)
                this._sqlExpression = this._sqlExpression.Where(whereExpression);
        }

        public T FirstOrDefault()
        {
            return this.FirstOrDefault(null);
        }

        public T FirstOrDefault(Expression<Func<T, bool>> whereExpression)
        {
            this.AddWhere(whereExpression);
            return this.ToEnumerable().FirstOrDefault();
        }

        public T First()
        {
            return this.First(null);
        }

        public T First(Expression<Func<T, bool>> whereExpression)
        {
            this.AddWhere(whereExpression);
            return this.ToEnumerable().First();
        }

        public T SingleOrDefault()
        {
            return this.SingleOrDefault(null);
        }

        public T SingleOrDefault(Expression<Func<T, bool>> whereExpression)
        {
            this.AddWhere(whereExpression);
            return this.ToEnumerable().SingleOrDefault();
        }

        public T Single()
        {
            return this.Single(null);
        }

        public T Single(Expression<Func<T, bool>> whereExpression)
        {
            this.AddWhere(whereExpression);
            return this.ToEnumerable().Single();
        }

        public int Count()
        {
            return this.Count(null);
        }

        public int Count(Expression<Func<T, bool>> whereExpression)
        {
            if (whereExpression != null)
                this._sqlExpression = this._sqlExpression.Where(whereExpression);

            Sql sql = this._buildComplexSql.BuildJoin(this._database, this._sqlExpression, this._joinSqlExpressions.Values.ToList(), null, true, false);

            return this._database.ExecuteScalar<int>(sql);
        }

        public bool Any()
        {
            return this.Count() > 0;
        }

        public bool Any(Expression<Func<T, bool>> whereExpression)
        {
            return this.Count(whereExpression) > 0;
        }

        public Page<T> ToPage(int page, int pageSize)
        {
            return this.ToPage(page, pageSize, (paged, action) =>
            {
                List<T> list = this.ToList();
                action(paged, list);
                return paged;
            });
        }

        private TRet ToPage<TRet>(int page, int pageSize, Func<Page<T>, Action<Page<T>, List<T>>, TRet> executeFunc)
        {
            int offset = (page - 1) * pageSize;

            // Save the one-time command time out and use it for both queries
            int saveTimeout = this._database.OneTimeCommandTimeout;

            // Setup the paged result
            Page<T> result = new Page<T>();
            result.CurrentPage = page;
            result.ItemsPerPage = pageSize;
            result.TotalItems = this.Count();
            result.TotalPages = result.TotalItems / pageSize;
            if ((result.TotalItems % pageSize) != 0)
                result.TotalPages++;

            this._database.OneTimeCommandTimeout = saveTimeout;

            this._sqlExpression = this._sqlExpression.Limit(offset, pageSize);

            return executeFunc(result, (paged, list) =>
            {
                paged.Items = list;
            });
        }

        public List<T2> ProjectTo<T2>(Expression<Func<T, T2>> projectionExpression)
        {
            Sql sql = this._buildComplexSql.GetSqlForProjection(projectionExpression, false);
            return this.ExecuteQuery(sql).Select(projectionExpression.Compile()).ToList();
        }

        public List<T2> Distinct<T2>(Expression<Func<T, T2>> projectionExpression)
        {
            Sql sql = this._buildComplexSql.GetSqlForProjection(projectionExpression, true);
            return this.ExecuteQuery(sql).Select(projectionExpression.Compile()).ToList();
        }

        public List<T> Distinct()
        {
            return this.ExecuteQuery(new Sql(this._sqlExpression.Context.ToSelectStatement(true, true), this._sqlExpression.Context.Params)).ToList();
        }

        public T[] ToArray()
        {
            return this.ToEnumerable().ToArray();
        }

        public List<T> ToList()
        {
            return this.ToEnumerable().ToList();
        }

        public IEnumerable<T> ToEnumerable()
        {
            Sql sql = this.BuildSql();
            return this.ExecuteQuery(sql);
        }

#if !NET35
        public List<dynamic> ToDynamicList()
        {
            return this.ToDynamicEnumerable().ToList();
        }
        
        public IEnumerable<dynamic> ToDynamicEnumerable()
        {
            Sql sql = this.BuildSql();
            return this._database.QueryImp<dynamic>(null, null, null, sql);
        }
#endif
   
        private IEnumerable<T> ExecuteQuery(Sql sql)
        {
            return this._database.QueryImp(default(T), this._listExpression, null, sql);
        }
        
        private Sql BuildSql()
        {
            Sql sql;
            if (this._joinSqlExpressions.Any())
                sql = this._buildComplexSql.BuildJoin(this._database, this._sqlExpression, this._joinSqlExpressions.Values.ToList(), null, false, false);
            else
                sql = new Sql(true, this._sqlExpression.Context.ToSelectStatement(), this._sqlExpression.Context.Params);
            return sql;
        }

#if !NET35 && !NET40
        public async System.Threading.Tasks.Task<List<T>> ToListAsync()
        {
            return (await this.ToEnumerableAsync().ConfigureAwait(false)).ToList();
        }

        public async System.Threading.Tasks.Task<T[]> ToArrayAsync()
        {
            return (await this.ToEnumerableAsync().ConfigureAwait(false)).ToArray();
        }

        public System.Threading.Tasks.Task<IEnumerable<T>> ToEnumerableAsync()
        {
            return this._database.QueryAsync<T>(this.BuildSql());
        }

        public async System.Threading.Tasks.Task<T> FirstOrDefaultAsync()
        {
            this.AddWhere(null);
            return (await this.ToEnumerableAsync().ConfigureAwait(false)).FirstOrDefault();
        }

        public async System.Threading.Tasks.Task<T> FirstAsync()
        {
            this.AddWhere(null);
            return (await this.ToEnumerableAsync().ConfigureAwait(false)).First();
        }

        public async System.Threading.Tasks.Task<T> SingleOrDefaultAsync()
        {
            this.AddWhere(null);
            return (await this.ToEnumerableAsync().ConfigureAwait(false)).SingleOrDefault();
        }

        public async System.Threading.Tasks.Task<T> SingleAsync()
        {
            this.AddWhere(null);
            return (await this.ToEnumerableAsync().ConfigureAwait(false)).Single();
        }

        public async System.Threading.Tasks.Task<int> CountAsync()
        {
            Sql sql = this._buildComplexSql.BuildJoin(this._database, this._sqlExpression, this._joinSqlExpressions.Values.ToList(), null, true, false);
            return await this._database.ExecuteScalarAsync<int>(sql).ConfigureAwait(false);
        }

        public async System.Threading.Tasks.Task<bool> AnyAsync()
        {
            return (await this.CountAsync().ConfigureAwait(false)) > 0;
        }

        public System.Threading.Tasks.Task<Page<T>> ToPageAsync(int page, int pageSize)
        {
            return this.ToPage(page, pageSize, async (paged, action) =>
            {
                List<T> list = await this.ToListAsync().ConfigureAwait(false);
                action(paged, list);
                return paged;
            });
        }

        public async System.Threading.Tasks.Task<List<T2>> ProjectToAsync<T2>(Expression<Func<T, T2>> projectionExpression)
        {
            Sql sql = this._buildComplexSql.GetSqlForProjection(projectionExpression, false);
            return (await this._database.QueryAsync<T>(sql).ConfigureAwait(false)).Select(projectionExpression.Compile()).ToList();
        }
#endif

        public IQueryProvider<T> Where(Expression<Func<T, bool>> whereExpression)
        {
            this._sqlExpression = this._sqlExpression.Where(whereExpression);
            return this;
        }

        public IQueryProvider<T> Where(string sql, params object[] args)
        {
            this._sqlExpression = this._sqlExpression.Where(sql, args);
            return this;
        }

        public IQueryProvider<T> Where(Sql sql)
        {
            this._sqlExpression = this._sqlExpression.Where(sql.SQL, sql.Arguments);
            return this;
        }

        public IQueryProvider<T> Where(Func<QueryContext<T>, Sql> queryBuilder)
        {
            Sql sql = queryBuilder(new QueryContext<T>(this._database, this._pocoData, this._joinSqlExpressions));
            return this.Where(sql);
        }

        public IQueryProvider<T> Limit(int rows)
        {
            this.ThrowIfOneToMany();
            this._sqlExpression = this._sqlExpression.Limit(rows);
            return this;
        }

        public IQueryProvider<T> Limit(int skip, int rows)
        {
            this.ThrowIfOneToMany();
            this._sqlExpression = this._sqlExpression.Limit(skip, rows);
            return this;
        }

        private void ThrowIfOneToMany()
        {
            if (this._listExpression != null)
            {
                throw new NotImplementedException("One to many queries with paging is not implemented");
            }
        }

        public IQueryProvider<T> OrderBy(Expression<Func<T, object>> column)
        {
            this._sqlExpression = this._sqlExpression.OrderBy(column);
            return this;
        }

        public IQueryProvider<T> OrderByDescending(Expression<Func<T, object>> column)
        {
            this._sqlExpression = this._sqlExpression.OrderByDescending(column);
            return this;
        }

        public IQueryProvider<T> ThenBy(Expression<Func<T, object>> column)
        {
            this._sqlExpression = this._sqlExpression.ThenBy(column);
            return this;
        }

        public IQueryProvider<T> ThenByDescending(Expression<Func<T, object>> column)
        {
            this._sqlExpression = this._sqlExpression.ThenByDescending(column);
            return this;
        }
    }
}

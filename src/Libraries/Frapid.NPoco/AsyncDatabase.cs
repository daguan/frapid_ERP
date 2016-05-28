#if !NET35 && !NET40
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Frapid.NPoco.Expressions;

namespace Frapid.NPoco
{
    public partial class Database
    {
        /// <summary>
        /// Performs an SQL Insert
        /// </summary>
        /// <param name="tableName">The name of the table to insert into</param>
        /// <param name="primaryKeyName">The name of the primary key column of the table</param>
        /// <param name="poco">The POCO object that specifies the column values to be inserted</param>
        /// <returns>The auto allocated primary key of the new record</returns>
        public Task<object> InsertAsync(string tableName, string primaryKeyName, object poco)
        {
            return this.InsertAsync(tableName, primaryKeyName, true, poco);
        }


        /// <summary>
        /// Performs an SQL Insert
        /// </summary>
        /// <param name="poco">The POCO object that specifies the column values to be inserted</param>
        /// <returns>The auto allocated primary key of the new record, or null for non-auto-increment tables</returns>
        /// <remarks>The name of the table, it's primary key and whether it's an auto-allocated primary key are retrieved
        /// from the POCO's attributes</remarks>
        public Task<object> InsertAsync<T>(T poco)
        {
            TableInfo tableInfo = this.PocoDataFactory.TableInfoForType(poco.GetType());
            return this.InsertAsync(tableInfo.TableName, tableInfo.PrimaryKey, tableInfo.AutoIncrement, poco);
        }

        /// <summary>
        /// Performs an SQL Insert
        /// </summary>
        /// <param name="tableName">The name of the table to insert into</param>
        /// <param name="primaryKeyName">The name of the primary key column of the table</param>
        /// <param name="autoIncrement">True if the primary key is automatically allocated by the DB</param>
        /// <param name="poco">The POCO object that specifies the column values to be inserted</param>
        /// <returns>The auto allocated primary key of the new record, or null for non-auto-increment tables</returns>
        /// <remarks>Inserts a poco into a table.  If the poco has a property with the same name 
        /// as the primary key the id of the new record is assigned to it.  Either way,
        /// the new id is returned.</remarks>
        public virtual Task<object> InsertAsync<T>(string tableName, string primaryKeyName, bool autoIncrement, T poco)
        {
            PocoData pd = this.PocoDataFactory.ForObject(poco, primaryKeyName, autoIncrement);
            return this.InsertAsyncImp(pd, tableName, primaryKeyName, autoIncrement, poco);
        }

        private async Task<object> InsertAsyncImp<T>(PocoData pocoData, string tableName, string primaryKeyName, bool autoIncrement, T poco)
        {
            if (!this.OnInsertingInternal(new InsertContext(poco, tableName, autoIncrement, primaryKeyName)))
                return 0;

            try
            {
                this.OpenSharedConnectionInternal();

                InsertStatements.PreparedInsertSql preparedInsert = InsertStatements.PrepareInsertSql(this, pocoData, tableName, primaryKeyName, autoIncrement, poco);

                using (DbCommand cmd = this.CreateCommand(this._sharedConnection, preparedInsert.Sql, preparedInsert.Rawvalues.ToArray()))
                {
                    // Assign the Version column
                    InsertStatements.AssignVersion(poco, preparedInsert);

                    object id;
                    if (!autoIncrement)
                    {
                        await this._dbType.ExecuteNonQueryAsync(this, cmd).ConfigureAwait(false);
                        id = InsertStatements.AssignNonIncrementPrimaryKey(primaryKeyName, poco, preparedInsert);
                    }
                    else
                    {
                        id = await this._dbType.ExecuteInsertAsync(this, cmd, primaryKeyName, preparedInsert.PocoData.TableInfo.UseOutputClause, poco, preparedInsert.Rawvalues.ToArray()).ConfigureAwait(false);
                        InsertStatements.AssignPrimaryKey(primaryKeyName, poco, id, preparedInsert);
                    }

                    return id;
                }
            }
            catch (Exception x)
            {
                this.OnExceptionInternal(x);
                throw;
            }
            finally
            {
                this.CloseSharedConnectionInternal();
            }
        }

        public Task<int> UpdateAsync<T>(T poco, Expression<Func<T, object>> fields)
        {
            SqlExpression<T> expression = this.DatabaseType.ExpressionVisitor<T>(this, this.PocoDataFactory.ForType(typeof(T)));
            expression = expression.Select(fields);
            IEnumerable<string> columnNames = ((ISqlExpression)expression).SelectMembers.Select(x => x.PocoColumn.ColumnName);
            IEnumerable<string> otherNames = ((ISqlExpression)expression).GeneralMembers.Select(x => x.PocoColumn.ColumnName);
            return this.UpdateAsync(poco, columnNames.Union(otherNames));
        }

        public Task<int> UpdateAsync(object poco)
        {
            return this.UpdateAsync(poco, null, null);
        }

        public Task<int> UpdateAsync(object poco, IEnumerable<string> columns)
        {
            return this.UpdateAsync(poco, null, columns);
        }
        
        public Task<int> UpdateAsync(object poco, object primaryKeyValue, IEnumerable<string> columns)
        {
            TableInfo tableInfo = this.PocoDataFactory.TableInfoForType(poco.GetType());
            return this.UpdateAsync(tableInfo.TableName, tableInfo.PrimaryKey, poco, primaryKeyValue, columns);
        }

        public virtual Task<int> UpdateAsync(string tableName, string primaryKeyName, object poco, object primaryKeyValue, IEnumerable<string> columns)
        {
            return this.UpdateImp (tableName, primaryKeyName, poco, primaryKeyValue, columns,
                async (sql, args, next) => next(await this.ExecuteAsync(sql, args).ConfigureAwait(false)), TaskAsyncHelper.FromResult(0));
        }

        public Task<int> DeleteAsync(object poco)
        {
            TableInfo tableInfo = this.PocoDataFactory.TableInfoForType(poco.GetType());
            return this.DeleteAsync(tableInfo.TableName, tableInfo.PrimaryKey, poco);
        }

        public Task<int> DeleteAsync(string tableName, string primaryKeyName, object poco)
        {
            return this.DeleteAsync(tableName, primaryKeyName, poco, null);
        }

        public virtual Task<int> DeleteAsync(string tableName, string primaryKeyName, object poco, object primaryKeyValue)
        {
            return this.DeleteImp(tableName, primaryKeyName, poco, primaryKeyValue, this.ExecuteAsync, TaskAsyncHelper.FromResult(0));
        }

        public Task<Page<T>> PageAsync<T>(long page, long itemsPerPage, Sql sql)
        {
            return this.PageAsync<T>(page, itemsPerPage, sql.SQL, sql.Arguments);
        }

        public Task<Page<T>> PageAsync<T>(long page, long itemsPerPage, string sql, params object[] args)
        {
            return this.PageImp<T, Task<Page<T>>>(page, itemsPerPage, sql, args,
                async (paged, thesql) =>
                {
                    paged.Items = (await this.QueryAsync<T>(thesql).ConfigureAwait(false)).ToList();
                    return paged;
                });
        }

        public Task<List<T>> FetchAsync<T>(long page, long itemsPerPage, string sql, params object[] args)
        {
            return this.SkipTakeAsync<T>((page - 1) * itemsPerPage, itemsPerPage, sql, args);
        }

        public Task<List<T>> FetchAsync<T>(long page, long itemsPerPage, Sql sql)
        {
            return this.SkipTakeAsync<T>((page - 1) * itemsPerPage, itemsPerPage, sql.SQL, sql.Arguments);
        }

        public Task<List<T>> SkipTakeAsync<T>(long skip, long take, string sql, params object[] args)
        {
            string sqlCount, sqlPage;
            this.BuildPageQueries<T>(skip, take, sql, ref args, out sqlCount, out sqlPage);
            return this.FetchAsync<T>(sqlPage, args);
        }

        public Task<List<T>> SkipTakeAsync<T>(long skip, long take, Sql sql)
        {
            return this.SkipTakeAsync<T>(skip, take, sql.SQL, sql.Arguments);
        }

        public async Task<List<T>> FetchAsync<T>(string sql, params object[] args)
        {
            return (await this.QueryAsync<T>(sql, args).ConfigureAwait(false)).ToList();
        }

        public async Task<List<T>> FetchAsync<T>(Sql sql)
        {
            return (await this.QueryAsync<T>(sql).ConfigureAwait(false)).ToList();
        }

        public Task<IEnumerable<T>> QueryAsync<T>(string sql, params object[] args)
        {
            return this.QueryAsync<T>(new Sql(sql, args));
        }

        public Task<IEnumerable<T>> QueryAsync<T>(Sql sql)
        {
            return this.QueryAsync(default(T), null, null, sql);
        }

        internal async Task<IEnumerable<T>> QueryAsync<T>(T instance, Expression<Func<T, IList>> listExpression, Func<T, object[]> idFunc, Sql Sql)
        {
            string sql = Sql.SQL;
            object[] args = Sql.Arguments;

            if (this.EnableAutoSelect) sql = AutoSelectHelper.AddSelectClause(this, typeof(T), sql);

            try
            {
                this.OpenSharedConnectionInternal();
                using (DbCommand cmd = this.CreateCommand(this._sharedConnection, sql, args))
                {
                    DbDataReader r;
                    try
                    {
                        r = await this.ExecuteReaderHelperAsync(cmd).ConfigureAwait(false);
                    }
                    catch (Exception x)
                    {
                        this.OnExceptionInternal(x);
                        throw;
                    }

                    return listExpression != null ? this.ReadOneToMany(instance, r, listExpression, idFunc) : this.Read<T>(typeof(T), instance, r);
                }
            }
            catch
            {
                this.CloseSharedConnectionInternal();
                throw;
            }
        }

        public async Task<T> SingleByIdAsync<T>(object primaryKey)
        {
            Sql sql = this.GenerateSingleByIdSql<T>(primaryKey);
            return (await this.QueryAsync<T>(sql).ConfigureAwait(false)).Single();
        }

        public async Task<T> SingleOrDefaultByIdAsync<T>(object primaryKey)
        {
            Sql sql = this.GenerateSingleByIdSql<T>(primaryKey);
            return (await this.QueryAsync<T>(sql).ConfigureAwait(false)).SingleOrDefault();
        }

        public System.Threading.Tasks.Task<int> ExecuteAsync(string sql, params object[] args)
        {
            return this.ExecuteAsync(new Sql(sql, args));
        }

        public async Task<int> ExecuteAsync(Sql Sql)
        {
            string sql = Sql.SQL;
            object[] args = Sql.Arguments;

            try
            {
                this.OpenSharedConnectionInternal();
                using (DbCommand cmd = this.CreateCommand(this._sharedConnection, sql, args))
                {
                    int result = await this.ExecuteNonQueryHelperAsync(cmd).ConfigureAwait(false);
                    return result;
                }
            }
            catch (Exception x)
            {
                this.OnExceptionInternal(x);
                throw;
            }
            finally
            {
                this.CloseSharedConnectionInternal();
            }
        }

        public Task<T> ExecuteScalarAsync<T>(string sql, object[] args)
        {
            return this.ExecuteScalarAsync<T>(new Sql(sql, args));
        }

        public async Task<T> ExecuteScalarAsync<T>(Sql Sql)
        {
            string sql = Sql.SQL;
            object[] args = Sql.Arguments;

            try
            {
                this.OpenSharedConnectionInternal();
                using (DbCommand cmd = this.CreateCommand(this._sharedConnection, sql, args))
                {
                    object val = await this.ExecuteScalarHelperAsync(cmd).ConfigureAwait(false);

                    if (val == null || val == DBNull.Value)
                        return await TaskAsyncHelper.FromResult(default(T)).ConfigureAwait(false);

                    Type t = typeof(T);
                    Type u = Nullable.GetUnderlyingType(t);

                    return (T)Convert.ChangeType(val, u ?? t);
                }
            }
            catch (Exception x)
            {
                this.OnExceptionInternal(x);
                throw;
            }
            finally
            {
                this.CloseSharedConnectionInternal();
            }
        }

        internal async Task<int> ExecuteNonQueryHelperAsync(DbCommand cmd)
        {
            this.DoPreExecute(cmd);
            int result = await this._dbType.ExecuteNonQueryAsync(this, cmd).ConfigureAwait(false);
            this.OnExecutedCommandInternal(cmd);
            return result;
        }

        internal async Task<object> ExecuteScalarHelperAsync(DbCommand cmd)
        {
            this.DoPreExecute(cmd);
            object result = await this._dbType.ExecuteScalarAsync(this, cmd).ConfigureAwait(false);
            this.OnExecutedCommandInternal(cmd);
            return result;
        }

        internal async Task<DbDataReader> ExecuteReaderHelperAsync(DbCommand cmd)
        {
            this.DoPreExecute(cmd);
            DbDataReader reader = await this._dbType.ExecuteReaderAsync(this, cmd).ConfigureAwait(false);
            this.OnExecutedCommandInternal(cmd);
            return reader;
        }
    }

    public class TaskAsyncHelper
    {
        public static Task<T> FromResult<T>(T value)
        {
            TaskCompletionSource<T> tcs = new TaskCompletionSource<T>();
            tcs.SetResult(value);
            return tcs.Task;
        }
    }
}

#endif

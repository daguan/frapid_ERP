using System.Collections.Generic;
using System.Threading.Tasks;
using Frapid.Configuration.Db;
using Frapid.NPoco;

namespace Frapid.DataAccess
{
    public static class Factory
    {
        public static string GetProviderName(string tenant)
        {
            return DbProvider.GetProviderName(tenant);
        }

        public static string GetMetaDatabase(string tenant)
        {
            return DbProvider.GetMetaDatabase(tenant);
        }

        public static async Task<IEnumerable<T>> GetAsync<T>(string database, string sql, params object[] args)
        {
            using(var db = DbProvider.GetDatabase(database))
            {
                return await db.FetchAsync<T>(sql, args);
            }
        }

        public static async Task<IEnumerable<T>> GetAsync<T>(string database, string sql)
        {
            using(var db = DbProvider.GetDatabase(database))
            {
                return await db.FetchAsync<T>(sql);
            }
        }

        public static async Task<IEnumerable<T>> GetAsync<T>(string database, Sql sql)
        {
            using(var db = DbProvider.GetDatabase(database))
            {
                return await db.FetchAsync<T>(sql);
            }
        }

        public static async Task<object> InsertAsync(string database, object poco, string tableName = "", string primaryKeyName = "", bool autoIncrement = true)
        {
            using(var db = DbProvider.GetDatabase(database))
            {
                if(!string.IsNullOrWhiteSpace(tableName) &&
                   !string.IsNullOrWhiteSpace(primaryKeyName))
                {
                    return await db.InsertAsync(tableName, primaryKeyName, autoIncrement, poco);
                }

                return await db.InsertAsync(poco);
            }
        }

        public static async Task<object> UpdateAsync(string database, object poco, object primaryKeyValue, string tableName = "", string primaryKeyName = "")
        {
            using(var db = DbProvider.GetDatabase(database))
            {
                if(!string.IsNullOrWhiteSpace(tableName) &&
                   !string.IsNullOrWhiteSpace(primaryKeyName))
                {
                    return await db.UpdateAsync(tableName, primaryKeyName, poco, primaryKeyValue, null);
                }

                return await db.UpdateAsync(poco, primaryKeyValue, null);
            }
        }


        public static async Task<T> ScalarAsync<T>(string database, string sql, params object[] args)
        {
            using(var db = DbProvider.GetDatabase(database))
            {
                return await db.ExecuteScalarAsync<T>(sql, args);
            }
        }

        public static async Task<T> ScalarAsync<T>(string database, Sql sql)
        {
            using(var db = DbProvider.GetDatabase(database))
            {
                return await db.ExecuteScalarAsync<T>(sql);
            }
        }

        public static async Task NonQueryAsync(string database, string sql, params object[] args)
        {
            using(var db = DbProvider.GetDatabase(database))
            {
                await db.ExecuteAsync(sql, args);
            }
        }

        public static async Task ExecuteAsync(string connectionString, string tenant, string sql, params object[] args)
        {
            using(var db = DbProvider.GetDatabase(tenant, connectionString))
            {
                await db.ExecuteAsync(sql, args);
            }
        }

        public static async Task<T> ExecuteScalarAsync<T>(string connectionString, string tenant, Sql sql)
        {
            using(var db = DbProvider.GetDatabase(tenant, connectionString))
            {
                return await db.ExecuteScalarAsync<T>(sql);
            }
        }
    }
}
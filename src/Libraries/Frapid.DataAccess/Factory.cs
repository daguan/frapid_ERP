using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Frapid.Configuration;
using NPoco;

namespace Frapid.DataAccess
{
    public static class Factory
    {
        public const string ProviderName = "Npgsql";
        public static readonly string MetaDatabase = ConfigurationManager.GetConfigurationValue("DbServerConfigFileLocation", "MetaDatabase");

        public static T Single<T>(string catalog, string sql, params object[] args)
        {
            using (Database db = Provider.Get(ConnectionString.GetConnectionString(catalog)).GetDatabase())
            {
                return db.Single<T>(sql, args);
            }
        }

        public static async Task<T> SingleAsync<T>(string catalog, string sql, params object[] args)
        {
            using (Database db = Provider.Get(ConnectionString.GetConnectionString(catalog)).GetDatabase())
            {
                var t = await db.QueryAsync<T>(sql, args);
                return t.FirstOrDefault();
            }
        }

        public static IEnumerable<T> Get<T>(string catalog, string sql, params object[] args)
        {
            using (Database db = Provider.Get(ConnectionString.GetConnectionString(catalog)).GetDatabase())
            {
                return db.Query<T>(sql, args);
            }
        }

        public static async Task<IEnumerable<T>> GetAsync<T>(string catalog, string sql, params object[] args)
        {
            using (Database db = Provider.Get(ConnectionString.GetConnectionString(catalog)).GetDatabase())
            {
                return await db.QueryAsync<T>(sql, args);
            }
        }

        public static IEnumerable<T> Get<T>(string catalog, string sql)
        {
            using (Database db = Provider.Get(ConnectionString.GetConnectionString(catalog)).GetDatabase())
            {
                return db.Query<T>(sql);
            }
        }

        public static async Task<IEnumerable<T>> GetAsync<T>(string catalog, string sql)
        {
            using (Database db = Provider.Get(ConnectionString.GetConnectionString(catalog)).GetDatabase())
            {
                return await db.QueryAsync<T>(sql);
            }
        }

        public static IEnumerable<T> Get<T>(string catalog, Sql sql)
        {
            using (Database db = Provider.Get(ConnectionString.GetConnectionString(catalog)).GetDatabase())
            {
                return db.Query<T>(sql);
            }
        }

        public static async Task<IEnumerable<T>> GetAsync<T>(string catalog, Sql sql)
        {
            using (Database db = Provider.Get(ConnectionString.GetConnectionString(catalog)).GetDatabase())
            {
                return await db.QueryAsync<T>(sql);
            }
        }

        public static object Insert(string catalog, object poco, string tableName = "", string primaryKeyName = "")
        {
            using (Database db = Provider.Get(ConnectionString.GetConnectionString(catalog)).GetDatabase())
            {
                if (!string.IsNullOrWhiteSpace(tableName) && !string.IsNullOrWhiteSpace(primaryKeyName))
                {
                    return db.Insert(tableName, primaryKeyName, poco);
                }

                return db.Insert(poco);
            }
        }

        public static async Task<object> InsertAsync(string catalog, object poco, string tableName = "",
            string primaryKeyName = "")
        {
            using (Database db = Provider.Get(ConnectionString.GetConnectionString(catalog)).GetDatabase())
            {
                if (!string.IsNullOrWhiteSpace(tableName) && !string.IsNullOrWhiteSpace(primaryKeyName))
                {
                    return await db.InsertAsync(tableName, primaryKeyName, poco);
                }

                return await db.InsertAsync(poco);
            }
        }

        public static object Update(string catalog, object poco, object primaryKeyValue, string tableName = "",
            string primaryKeyName = "")
        {
            using (Database db = Provider.Get(ConnectionString.GetConnectionString(catalog)).GetDatabase())
            {
                if (!string.IsNullOrWhiteSpace(tableName) && !string.IsNullOrWhiteSpace(primaryKeyName))
                {
                    return db.Update(tableName, primaryKeyName, poco, primaryKeyValue);
                }

                return db.Update(poco, primaryKeyValue);
            }
        }

        public static async Task<int> UpdateAsync(string catalog, object poco, object primaryKeyValue,
            IEnumerable<string> columns, string tableName = "", string primaryKeyName = "")
        {
            using (Database db = Provider.Get(ConnectionString.GetConnectionString(catalog)).GetDatabase())
            {
                if (!string.IsNullOrWhiteSpace(tableName) && !string.IsNullOrWhiteSpace(primaryKeyName))
                {
                    return await db.UpdateAsync(tableName, primaryKeyName, poco, primaryKeyValue, columns);
                }

                return await db.UpdateAsync(poco, primaryKeyValue, columns);
            }
        }

        public static T Scalar<T>(string catalog, string sql, params object[] args)
        {
            using (Database db = Provider.Get(ConnectionString.GetConnectionString(catalog)).GetDatabase())
            {
                return db.ExecuteScalar<T>(sql, args);
            }
        }

        public static async Task<T> ScalarAsync<T>(string catalog, string sql, params object[] args)
        {
            using (Database db = Provider.Get(ConnectionString.GetConnectionString(catalog)).GetDatabase())
            {
                return await db.ExecuteScalarAsync<T>(sql, args);
            }
        }

        public static T Scalar<T>(string catalog, Sql sql)
        {
            using (Database db = Provider.Get(ConnectionString.GetConnectionString(catalog)).GetDatabase())
            {
                return db.ExecuteScalar<T>(sql);
            }
        }

        public static async Task<T> ScalarAsync<T>(string catalog, Sql sql)
        {
            using (Database db = Provider.Get(ConnectionString.GetConnectionString(catalog)).GetDatabase())
            {
                return await db.ExecuteScalarAsync<T>(sql);
            }
        }

        public static void NonQuery(string catalog, string sql, params object[] args)
        {
            using (Database db = Provider.Get(ConnectionString.GetConnectionString(catalog)).GetDatabase())
            {
                db.Execute(sql, args);
            }
        }

        public static async Task NonQueryAsync(string catalog, string sql, params object[] args)
        {
            using (Database db = Provider.Get(ConnectionString.GetConnectionString(catalog)).GetDatabase())
            {
                await db.ExecuteAsync(sql, args);
            }
        }
    }
}
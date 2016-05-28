using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Frapid.NPoco.DatabaseTypes;
using Frapid.NPoco.Expressions;

namespace Frapid.NPoco
{
    /// <summary>
    ///     Base class for DatabaseType handlers - provides default/common handling for different database engines
    /// </summary>
    public abstract class DatabaseType
    {
        internal const string LinqBinary = "System.Data.Linq.Binary";

        private readonly Dictionary<Type, DbType> _typeMap;

        protected DatabaseType()
        {
            this._typeMap = new Dictionary<Type, DbType>
                            {
                                [typeof(byte)] = DbType.Byte,
                                [typeof(sbyte)] = DbType.SByte,
                                [typeof(short)] = DbType.Int16,
                                [typeof(ushort)] = DbType.UInt16,
                                [typeof(int)] = DbType.Int32,
                                [typeof(uint)] = DbType.UInt32,
                                [typeof(long)] = DbType.Int64,
                                [typeof(ulong)] = DbType.UInt64,
                                [typeof(float)] = DbType.Single,
                                [typeof(double)] = DbType.Double,
                                [typeof(decimal)] = DbType.Decimal,
                                [typeof(bool)] = DbType.Boolean,
                                [typeof(string)] = DbType.String,
                                [typeof(char)] = DbType.StringFixedLength,
                                [typeof(Guid)] = DbType.Guid,
                                [typeof(DateTime)] = DbType.DateTime,
                                [typeof(DateTimeOffset)] = DbType.DateTimeOffset,
                                [typeof(TimeSpan)] = DbType.Time,
                                [typeof(byte[])] = DbType.Binary,
                                [typeof(byte?)] = DbType.Byte,
                                [typeof(sbyte?)] = DbType.SByte,
                                [typeof(short?)] = DbType.Int16,
                                [typeof(ushort?)] = DbType.UInt16,
                                [typeof(int?)] = DbType.Int32,
                                [typeof(uint?)] = DbType.UInt32,
                                [typeof(long?)] = DbType.Int64,
                                [typeof(ulong?)] = DbType.UInt64,
                                [typeof(float?)] = DbType.Single,
                                [typeof(double?)] = DbType.Double,
                                [typeof(decimal?)] = DbType.Decimal,
                                [typeof(bool?)] = DbType.Boolean,
                                [typeof(char?)] = DbType.StringFixedLength,
                                [typeof(Guid?)] = DbType.Guid,
                                [typeof(DateTime?)] = DbType.DateTime,
                                [typeof(DateTimeOffset?)] = DbType.DateTimeOffset,
                                [typeof(TimeSpan?)] = DbType.Time,
                                [typeof(object)] = DbType.Object
                            };
        }

        // Helper Properties
        public static DatabaseType SqlServer2012 => Singleton<SqlServer2012DatabaseType>.Instance;

        public static DatabaseType SqlServer2008 => Singleton<SqlServer2008DatabaseType>.Instance;

        public static DatabaseType SqlServer2005 => Singleton<SqlServerDatabaseType>.Instance;

        public static DatabaseType PostgreSQL => Singleton<PostgreSQLDatabaseType>.Instance;

        public static DatabaseType Oracle => Singleton<OracleDatabaseType>.Instance;

        public static DatabaseType OracleManaged => Singleton<OracleManagedDatabaseType>.Instance;

        public static DatabaseType MySQL => Singleton<MySqlDatabaseType>.Instance;

        public static DatabaseType SQLite => Singleton<SQLiteDatabaseType>.Instance;

        public static DatabaseType SQLCe => Singleton<SqlServerCEDatabaseType>.Instance;

        public static DatabaseType Firebird => Singleton<FirebirdDatabaseType>.Instance;

        /// <summary>
        ///     Configire the specified type to be mapped to a given db-type
        /// </summary>
        protected void AddTypeMap(Type type, DbType dbType)
        {
            this._typeMap[type] = dbType;
        }

        public virtual DbType? LookupDbType(Type type, string name)
        {
            DbType dbType;
            var nullUnderlyingType = Nullable.GetUnderlyingType(type);
            if(nullUnderlyingType != null)
                type = nullUnderlyingType;
            if(type.GetTypeInfo().IsEnum &&
               !this._typeMap.ContainsKey(type))
            {
                type = Enum.GetUnderlyingType(type);
            }
            if(this._typeMap.TryGetValue(type, out dbType))
            {
                return dbType;
            }
            if(type.FullName == LinqBinary)
            {
                return DbType.Binary;
            }

            return null;
        }

        /// <summary>
        ///     Returns the prefix used to delimit parameters in SQL query strings.
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public virtual string GetParameterPrefix(string connectionString)
        {
            return "@";
        }

        /// <summary>
        ///     Converts a supplied C# object value into a value suitable for passing to the database
        /// </summary>
        /// <param name="value">The value to convert</param>
        /// <returns>The converted value</returns>
        public virtual object MapParameterValue(object value)
        {
            // Cast bools to integer
            if(value is bool)
            {
                return (bool)value ? 1 : 0;
            }

            // Leave it
            return value;
        }

        /// <summary>
        ///     Called immediately before a command is executed, allowing for modification of the DbCommand before it's passed to
        ///     the database provider
        /// </summary>
        /// <param name="cmd"></param>
        public virtual void PreExecute(DbCommand cmd)
        {
        }

        /// <summary>
        ///     Builds an SQL query suitable for performing page based queries to the database
        /// </summary>
        /// <param name="skip">The number of rows that should be skipped by the query</param>
        /// <param name="take">The number of rows that should be retruend by the query</param>
        /// <param name="parts">The original SQL query after being parsed into it's component parts</param>
        /// <param name="args">Arguments to any embedded parameters in the SQL query</param>
        /// <returns>The final SQL query that should be executed.</returns>
        public virtual string BuildPageQuery(long skip, long take, PagingHelper.SQLParts parts, ref object[] args)
        {
            string sql = string.Format("{0}\nLIMIT @{1} OFFSET @{2}", parts.sql, args.Length, args.Length + 1);
            args = args.Concat
                (
                 new object[]
                 {
                     take,
                     skip
                 }).ToArray();
            return sql;
        }

        public virtual bool UseColumnAliases()
        {
            return false;
        }

        /// <summary>
        ///     Returns an SQL Statement that can check for the existance of a row in the database.
        /// </summary>
        /// <returns></returns>
        public virtual string GetExistsSql()
        {
            return "SELECT COUNT(*) FROM {0} WHERE {1}";
        }

        /// <summary>
        ///     Escape a tablename into a suitable format for the associated database provider.
        /// </summary>
        /// <param name="tableName">
        ///     The name of the table (as specified by the client program, or as attributes on the associated
        ///     POCO class.
        /// </param>
        /// <returns>The escaped table name</returns>
        public virtual string EscapeTableName(string tableName)
        {
            // Assume table names with "dot" are already escaped
            return tableName.IndexOf('.') >= 0 ? tableName : this.EscapeSqlIdentifier(tableName);
        }

        /// <summary>
        ///     Escape and arbitary SQL identifier into a format suitable for the associated database provider
        /// </summary>
        /// <param name="str">The SQL identifier to be escaped</param>
        /// <returns>The escaped identifier</returns>
        public virtual string EscapeSqlIdentifier(string str)
        {
            return string.Format("[{0}]", str);
        }

        /// <summary>
        ///     Return an SQL expression that can be used to populate the primary key column of an auto-increment column.
        /// </summary>
        /// <param name="ti">Table info describing the table</param>
        /// <returns>An SQL expressions</returns>
        /// <remarks>See the Oracle database type for an example of how this method is used.</remarks>
        public virtual string GetAutoIncrementExpression(TableInfo ti)
        {
            return null;
        }

        /// <summary>
        ///     Returns an SQL expression that can be used to specify the return value of auto incremented columns.
        /// </summary>
        /// <param name="primaryKeyName">The primary key of the row being inserted.</param>
        /// <returns>An expression describing how to return the new primary key value</returns>
        /// <remarks>See the SQLServer database provider for an example of how this method is used.</remarks>
        public virtual string GetInsertOutputClause(string primaryKeyName, bool useOutputClause)
        {
            return string.Empty;
        }

        /// <summary>
        ///     Performs an Insert operation
        /// </summary>
        /// <param name="db">The calling Database object</param>
        /// <param name="cmd">The insert command to be executed</param>
        /// <param name="primaryKeyName">The primary key of the table being inserted into</param>
        /// <param name="useOutputClause"></param>
        /// <param name="poco"></param>
        /// <param name="args"></param>
        /// <returns>The ID of the newly inserted record</returns>
        public virtual object ExecuteInsert<T>(Database db, DbCommand cmd, string primaryKeyName, bool useOutputClause, T poco, object[] args)
        {
            cmd.CommandText += ";\nSELECT @@IDENTITY AS NewID;";
            return db.ExecuteScalarHelper(cmd);
        }

#if !NET35 && !NET40
        public virtual async Task<object> ExecuteInsertAsync<T>(Database db, DbCommand cmd, string primaryKeyName, bool useOutputClause, T poco, object[] args)
        {
            cmd.CommandText += ";\nSELECT @@IDENTITY AS NewID;";
            return await db.ExecuteScalarHelperAsync(cmd);
        }
#endif

        public virtual void InsertBulk<T>(IDatabase db, IEnumerable<T> pocos)
        {
            foreach(var poco in pocos)
            {
                db.Insert(poco);
            }
        }

        /// <summary>
        ///     Look at the type and provider name being used and instantiate a suitable DatabaseType instance.
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="providerName"></param>
        /// <returns></returns>
        public static DatabaseType Resolve(string typeName, string providerName)
        {
            // Try using type name first (more reliable)
            if(typeName.StartsWith("MySql"))
                return Singleton<MySqlDatabaseType>.Instance;
            if(typeName.StartsWith("SqlCe"))
                return Singleton<SqlServerCEDatabaseType>.Instance;
            if(typeName.StartsWith("Npgsql") ||
               typeName.StartsWith("PgSql"))
                return Singleton<PostgreSQLDatabaseType>.Instance;
            if(typeName.StartsWith("OracleManaged"))
                return Singleton<OracleDatabaseType>.Instance;
            if(typeName.StartsWith("Oracle"))
                return Singleton<OracleDatabaseType>.Instance;
            if(typeName.StartsWith("SQLite"))
                return Singleton<SQLiteDatabaseType>.Instance;
            if(typeName.StartsWith("SqlConnection"))
                return Singleton<SqlServerDatabaseType>.Instance;
            if(typeName.StartsWith("Fb") ||
               typeName.StartsWith("Firebird"))
                return Singleton<FirebirdDatabaseType>.Instance;

            if(!string.IsNullOrEmpty(providerName))
            {
                // Try again with provider name
                if(providerName.IndexOf("MySql", StringComparison.OrdinalIgnoreCase) >= 0)
                    return Singleton<MySqlDatabaseType>.Instance;
                if(providerName.IndexOf("SqlServerCe", StringComparison.OrdinalIgnoreCase) >= 0)
                    return Singleton<SqlServerCEDatabaseType>.Instance;
                if(providerName.IndexOf("pgsql", StringComparison.OrdinalIgnoreCase) >= 0)
                    return Singleton<PostgreSQLDatabaseType>.Instance;
                if(providerName.IndexOf("Oracle.DataAccess", StringComparison.OrdinalIgnoreCase) >= 0)
                    return Singleton<OracleDatabaseType>.Instance;
                if(providerName.IndexOf("Oracle.ManagedDataAccess", StringComparison.OrdinalIgnoreCase) >= 0)
                    return Singleton<OracleManagedDatabaseType>.Instance;
                if(providerName.IndexOf("SQLite", StringComparison.OrdinalIgnoreCase) >= 0)
                    return Singleton<SQLiteDatabaseType>.Instance;
                if(providerName.IndexOf("Firebird", StringComparison.OrdinalIgnoreCase) >= 0)
                    return Singleton<FirebirdDatabaseType>.Instance;
            }

            // Assume SQL Server
            return Singleton<SqlServerDatabaseType>.Instance;
        }

        public virtual string GetDefaultInsertSql(string tableName, string primaryKeyName, bool useOutputClause, string[] names, string[] parameters)
        {
            return string.Format("INSERT INTO {0} DEFAULT VALUES", this.EscapeTableName(tableName));
        }

        public virtual IsolationLevel GetDefaultTransactionIsolationLevel()
        {
            return IsolationLevel.ReadCommitted;
        }

        public virtual string GetSQLForTransactionLevel(IsolationLevel isolationLevel)
        {
            switch(isolationLevel)
            {
                case IsolationLevel.ReadCommitted:
                    return "SET TRANSACTION ISOLATION LEVEL READ COMMITTED;";

                case IsolationLevel.ReadUncommitted:
                    return "SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;";

                case IsolationLevel.RepeatableRead:
                    return "SET TRANSACTION ISOLATION LEVEL REPEATABLE READ;";

                case IsolationLevel.Serializable:
                    return "SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;";

                case IsolationLevel.Snapshot:
                    return "SET TRANSACTION ISOLATION LEVEL SNAPSHOT;";

                default:
                    return "SET TRANSACTION ISOLATION LEVEL READ COMMITTED;";
            }
        }

        public SqlExpression<T> ExpressionVisitor<T>(IDatabase db, PocoData pocoData)
        {
            return this.ExpressionVisitor<T>(db, pocoData, false);
        }

        public virtual SqlExpression<T> ExpressionVisitor<T>(IDatabase db, PocoData pocoData, bool prefixTableName)
        {
            return new DefaultSqlExpression<T>(db, pocoData, prefixTableName);
        }

        public virtual string GetProviderName()
        {
            return "System.Data.SqlClient";
        }

        public virtual object ProcessDefaultMappings(PocoColumn pocoColumn, object value)
        {
            return value;
        }

#if !NET35 && !NET40
        public virtual Task<int> ExecuteNonQueryAsync(Database database, DbCommand cmd)
        {
            return cmd.ExecuteNonQueryAsync();
        }

        public virtual Task<object> ExecuteScalarAsync(Database database, DbCommand cmd)
        {
            return cmd.ExecuteScalarAsync();
        }

        public virtual Task<DbDataReader> ExecuteReaderAsync(Database database, DbCommand cmd)
        {
            return cmd.ExecuteReaderAsync();
        }
#endif
    }
}
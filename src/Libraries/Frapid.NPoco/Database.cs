/* NPoco 3.0 - A Tiny ORMish thing for your POCO's.
 * Copyright 2011-2015. All Rights Reserved.
 *
 * Apache License 2.0 - http://www.apache.org/licenses/LICENSE-2.0
 *
 * Originally created by Brad Robinson (@toptensoftware)
 *
 * Special thanks to Rob Conery (@robconery) for original inspiration (ie:Massive) and for
 * use of Subsonic's T4 templates, Rob Sullivan (@DataChomp) for hard core DBA advice
 * and Adam Schroder (@schotime) for lots of suggestions, improvements and Oracle support
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Frapid.NPoco.Expressions;
using Frapid.NPoco.Linq;
#if !DNXCORE50

#endif

namespace Frapid.NPoco
{
    public partial class Database : IDatabase
    {
        public const bool DefaultEnableAutoSelect = true;

        public Database(DbConnection connection)
            : this(connection, null, null, null, DefaultEnableAutoSelect)
        { }

        public Database(DbConnection connection, DatabaseType dbType)
            : this(connection, dbType, null, null, DefaultEnableAutoSelect)
        { }

        public Database(DbConnection connection, DatabaseType dbType, IsolationLevel? isolationLevel)
            : this(connection, dbType, null, isolationLevel, DefaultEnableAutoSelect)
        { }

        public Database(DbConnection connection, DatabaseType dbType, DbProviderFactory dbProviderFactory)
            : this(connection, dbType, dbProviderFactory, null, DefaultEnableAutoSelect)
        { }

        public Database(DbConnection connection, DatabaseType dbType, DbProviderFactory dbProviderFactory, IsolationLevel? isolationLevel)
            : this(connection, dbType, dbProviderFactory, isolationLevel, DefaultEnableAutoSelect)
        { }

        public Database(DbConnection connection, DatabaseType dbType, DbProviderFactory dbProviderFactory, IsolationLevel? isolationLevel, bool enableAutoSelect)
        {
            this.EnableAutoSelect = enableAutoSelect;
            this.KeepConnectionAlive = true;

            this._sharedConnection = connection;
            this._connectionString = connection.ConnectionString;
            this._factory = dbProviderFactory;
            string dbTypeName = (this._factory == null ? this._sharedConnection.GetType() : this._factory.GetType()).Name;
            this._dbType = dbType ?? DatabaseType.Resolve(dbTypeName, null);
            this._providerName = this._dbType.GetProviderName();
            this._isolationLevel = isolationLevel.HasValue ? isolationLevel.Value : this._dbType.GetDefaultTransactionIsolationLevel();
            this._paramPrefix = this._dbType.GetParameterPrefix(this._connectionString);

            // Cause it is an external connection ensure that the isolation level matches ours
            //using (var cmd = _sharedConnection.CreateCommand())
            //{
            //    cmd.CommandTimeout = CommandTimeout;
            //    cmd.CommandText = _dbType.GetSQLForTransactionLevel(_isolationLevel);
            //    cmd.ExecuteNonQuery();
            //}
        }

#if !DNXCORE50
        public Database(string connectionString, string providerName)
            : this(connectionString, providerName, DefaultEnableAutoSelect)
        { }

        public Database(string connectionString, string providerName, IsolationLevel isolationLevel)
            : this(connectionString, providerName, isolationLevel, DefaultEnableAutoSelect)
        { }

        public Database(string connectionString, string providerName, bool enableAutoSelect)
            : this(connectionString, providerName, null, enableAutoSelect)
        { }

        public Database(string connectionString, string providerName, IsolationLevel? isolationLevel, bool enableAutoSelect)
        {
            this.EnableAutoSelect = enableAutoSelect;
            this.KeepConnectionAlive = false;

            this._connectionString = connectionString;
            this._factory = DbProviderFactories.GetFactory(providerName);
            string dbTypeName = (this._factory == null ? this._sharedConnection.GetType() : this._factory.GetType()).Name;
            this._dbType = DatabaseType.Resolve(dbTypeName, providerName);
            this._providerName = providerName;
            this._isolationLevel = isolationLevel.HasValue ? isolationLevel.Value : this._dbType.GetDefaultTransactionIsolationLevel();
            this._paramPrefix = this._dbType.GetParameterPrefix(this._connectionString);
        }

        public Database(string connectionString, DatabaseType dbType)
            : this(connectionString, dbType, null, DefaultEnableAutoSelect)
        { }

        public Database(string connectionString, DatabaseType dbType, IsolationLevel? isolationLevel)
            : this(connectionString, dbType, isolationLevel,  DefaultEnableAutoSelect)
        { }

        public Database(string connectionString, DatabaseType dbType, IsolationLevel? isolationLevel, bool enableAutoSelect)
        {
            this.EnableAutoSelect = enableAutoSelect;
            this.KeepConnectionAlive = false;

            this._connectionString = connectionString;
            this._dbType = dbType;
            this._providerName = this._dbType.GetProviderName();
            this._factory = DbProviderFactories.GetFactory(this._dbType.GetProviderName());
            this._isolationLevel = isolationLevel.HasValue ? isolationLevel.Value : this._dbType.GetDefaultTransactionIsolationLevel();
            this._paramPrefix = this._dbType.GetParameterPrefix(this._connectionString);
        }
#endif

        public Database(string connectionString, DatabaseType databaseType, DbProviderFactory provider)
            : this(connectionString, databaseType, provider, null, DefaultEnableAutoSelect)
        { }

        public Database(string connectionString, DatabaseType databaseType, DbProviderFactory provider, IsolationLevel? isolationLevel = null, bool enableAutoSelect = DefaultEnableAutoSelect)
        {
            this.EnableAutoSelect = enableAutoSelect;
            this.KeepConnectionAlive = false;

            this._connectionString = connectionString;
            this._factory = provider;
            this._dbType = databaseType ?? DatabaseType.Resolve(this._factory.GetType().Name, null);
            this._providerName = this._dbType.GetProviderName();
            this._isolationLevel = isolationLevel.HasValue ? isolationLevel.Value : this._dbType.GetDefaultTransactionIsolationLevel();
            this._paramPrefix = this._dbType.GetParameterPrefix(this._connectionString);
        }

#if !DNXCORE50
        public Database(string connectionStringName)
            : this(connectionStringName, DefaultEnableAutoSelect)
        { }

        public Database(string connectionStringName, IsolationLevel isolationLevel)
            : this(connectionStringName, isolationLevel, DefaultEnableAutoSelect)
        { }

        public Database(string connectionStringName, bool enableAutoSelect)
            : this(connectionStringName, (IsolationLevel?) null, enableAutoSelect)
        { }

        public Database(string connectionStringName, IsolationLevel? isolationLevel,  bool enableAutoSelect)
        {
            this.EnableAutoSelect = enableAutoSelect;
            this.KeepConnectionAlive = false;

            // Use first?
            if (connectionStringName == "") connectionStringName = ConfigurationManager.ConnectionStrings[0].Name;

            // Work out connection string and provider name
            string providerName = "System.Data.SqlClient";
            if (ConfigurationManager.ConnectionStrings[connectionStringName] != null)
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.ConnectionStrings[connectionStringName].ProviderName))
                {
                    providerName = ConfigurationManager.ConnectionStrings[connectionStringName].ProviderName;
                }
            }
            else
            {
                throw new InvalidOperationException("Can't find a connection string with the name '" + connectionStringName + "'");
            }

            // Store factory and connection string
            this._connectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
            this._providerName = providerName;

            this._factory = DbProviderFactories.GetFactory(this._providerName);
            this._dbType = DatabaseType.Resolve(this._factory.GetType().Name, this._providerName);
            this._isolationLevel = isolationLevel.HasValue ? isolationLevel.Value : this._dbType.GetDefaultTransactionIsolationLevel();
            this._paramPrefix = this._dbType.GetParameterPrefix(this._connectionString);
        }
#endif

        private readonly DatabaseType _dbType;
        public DatabaseType DatabaseType => this._dbType;
        public IsolationLevel IsolationLevel => this._isolationLevel;

        private IDictionary<string, object> _data;
        public IDictionary<string, object> Data => this._data ?? (this._data = new Dictionary<string, object>());

        // Automatically close connection
        public void Dispose()
        {
            if (this.KeepConnectionAlive) return;
            this.CloseSharedConnection();
        }

        // Set to true to keep the first opened connection alive until this object is disposed
        public bool KeepConnectionAlive { get; set; }

        private bool ShouldCloseConnectionAutomatically { get; set; }

        // Open a connection (can be nested)
        public IDatabase OpenSharedConnection()
        {
            this.OpenSharedConnectionImp(false);
            return this;
        }

        private void OpenSharedConnectionInternal()
        {
            this.OpenSharedConnectionImp(true);
        }

        private void OpenSharedConnectionImp(bool isInternal)
        {
            if (this._sharedConnection != null && this._sharedConnection.State != ConnectionState.Broken && this._sharedConnection.State != ConnectionState.Closed)
                return;

            this.ShouldCloseConnectionAutomatically = isInternal;

            this._sharedConnection = this._factory.CreateConnection();
            if (this._sharedConnection == null) throw new Exception("SQL Connection failed to configure.");

            this._sharedConnection.ConnectionString = this._connectionString;

            if (this._sharedConnection.State == ConnectionState.Broken)
            {
                this._sharedConnection.Close();
            }

            if (this._sharedConnection.State == ConnectionState.Closed)
            {
                this._sharedConnection.Open();
                this._sharedConnection = this.OnConnectionOpenedInternal(this._sharedConnection);

                //using (var cmd = _sharedConnection.CreateCommand())
                //{
                //    cmd.CommandTimeout = CommandTimeout;
                //    cmd.CommandText = _dbType.GetSQLForTransactionLevel(_isolationLevel);
                //    cmd.ExecuteNonQuery();
                //}
            }
        }

        private void CloseSharedConnectionInternal()
        {
            if (this.ShouldCloseConnectionAutomatically && this._transaction == null)
                this.CloseSharedConnection();
        }

        // Close a previously opened connection
        public void CloseSharedConnection()
        {
            if (this.KeepConnectionAlive) return;

            if (this._transaction != null)
            {
                this._transaction.Dispose();
                this._transaction = null;
            }

            if (this._sharedConnection == null) return;

            this.OnConnectionClosingInternal(this._sharedConnection);

            this._sharedConnection.Close();
            this._sharedConnection.Dispose();
            this._sharedConnection = null;
        }

        public VersionExceptionHandling VersionException
        {
            get { return this._versionException; }
            set { this._versionException = value; }
        }

        // Access to our shared connection
        public DbConnection Connection => this._sharedConnection;

        public DbTransaction Transaction => this._transaction;

        public DbParameter CreateParameter()
        {
            using (DbConnection conn = this._sharedConnection ?? this._factory.CreateConnection())
            {
                if (conn == null) throw new Exception("DB Connection no longer active and failed to reset.");
                using (DbCommand comm = conn.CreateCommand())
                {
                    return comm.CreateParameter();
                }
            }
        }

        // Helper to create a transaction scope
        public ITransaction GetTransaction()
        {
            return this.GetTransaction(this._isolationLevel);
        }

        public ITransaction GetTransaction(IsolationLevel isolationLevel)
        {
            return new Transaction(this, isolationLevel);
        }

        public void SetTransaction(DbTransaction tran)
        {
            this._transaction = tran;
        }

        private void OnBeginTransactionInternal()
        {
#if DEBUG
            System.Diagnostics.Debug.WriteLine("Created new transaction using isolation level of " + this._transaction.IsolationLevel + ".");
#endif
            this.OnBeginTransaction();
            foreach (ITransactionInterceptor interceptor in this.Interceptors.OfType<ITransactionInterceptor>())
            {
                interceptor.OnBeginTransaction(this);
            }
        }

        protected virtual void OnBeginTransaction()
        {
        }

        private void OnAbortTransactionInternal()
        {
#if DEBUG
            System.Diagnostics.Debug.WriteLine("Rolled back a transaction");
#endif
            this.OnAbortTransaction();
            foreach (ITransactionInterceptor interceptor in this.Interceptors.OfType<ITransactionInterceptor>())
            {
                interceptor.OnAbortTransaction(this);
            }
        }

        protected virtual void OnAbortTransaction()
        {
        }

        private void OnCompleteTransactionInternal()
        {
#if DEBUG
            System.Diagnostics.Debug.WriteLine("Committed the transaction");
#endif
            this.OnCompleteTransaction();
            foreach (ITransactionInterceptor interceptor in this.Interceptors.OfType<ITransactionInterceptor>())
            {
                interceptor.OnCompleteTransaction(this);
            }
        }

        protected virtual void OnCompleteTransaction()
        {
        }

        public void BeginTransaction()
        {
            this.BeginTransaction(this._isolationLevel);
        }

        // Start a new transaction, can be nested, every call must be
        //	matched by a call to AbortTransaction or CompleteTransaction
        // Use `using (var scope=db.Transaction) { scope.Complete(); }` to ensure correct semantics
        public void BeginTransaction(IsolationLevel isolationLevel)
        {
            if (this._transaction == null)
            {
                this.TransactionCount = 0;
                this.OpenSharedConnectionInternal();
                this._transaction = this._sharedConnection.BeginTransaction(isolationLevel);
                this.OnBeginTransactionInternal();
            }

            if (this._transaction != null)
            {
                this.TransactionCount++;
            }
        }

        // Abort the entire outer most transaction scope
        public void AbortTransaction()
        {
            this.TransactionIsAborted = true;
            this.AbortTransaction(false);
        }

        public void AbortTransaction(bool fromComplete)
        {
            if (this._transaction == null)
            {
                this.TransactionIsAborted = false;
                return;
            }

            if (fromComplete == false)
            {
                this.TransactionCount--;
                if (this.TransactionCount >= 1)
                {
                    this.TransactionIsAborted = true;
                    return;
                }
            }

            if (this.TransactionIsOk())
                this._transaction.Rollback();

            if (this._transaction != null)
                this._transaction.Dispose();

            this._transaction = null;
            this.TransactionIsAborted = false;

            // You cannot continue to use a connection after a transaction has been rolled back
            if (this._sharedConnection != null)
            {
                this._sharedConnection.Close();
                this._sharedConnection.Open();
            }

            this.OnAbortTransactionInternal();
            this.CloseSharedConnectionInternal();
        }

        // Complete the transaction
        public void CompleteTransaction()
        {
            if (this._transaction == null)
                return;

            this.TransactionCount--;
            if (this.TransactionCount >= 1)
                return;

            if (this.TransactionIsAborted)
            {
                this.AbortTransaction(true);
                return;
            }

            if (this.TransactionIsOk())
                this._transaction.Commit();

            if (this._transaction != null)
                this._transaction.Dispose();

            this._transaction = null;

            this.OnCompleteTransactionInternal();
            this.CloseSharedConnectionInternal();
        }

        internal bool TransactionIsAborted { get; set; }
        internal int TransactionCount { get; set; }

        private bool TransactionIsOk()
        {
            return this._sharedConnection != null
                && this._transaction != null
                && this._transaction.Connection != null
                && this._transaction.Connection.State == ConnectionState.Open;
        }

        // Add a parameter to a DB command
        public virtual void AddParameter(DbCommand cmd, object value)
        {
            // Convert value to from poco type to db type
            if (this.Mappers != null && value != null)
            {
                value = this.Mappers.FindAndExecute(x => x.GetParameterConverter(cmd, value.GetType()), value);
            }

            // Support passed in parameters
            DbParameter idbParam = value as DbParameter;
            if (idbParam != null)
            {
                idbParam.ParameterName = string.Format("{0}{1}", this._paramPrefix, cmd.Parameters.Count);
                cmd.Parameters.Add(idbParam);
                return;
            }

            DbParameter p = cmd.CreateParameter();
            p.ParameterName = string.Format("{0}{1}", this._paramPrefix, cmd.Parameters.Count);

            bool dbtypeSet = false;

            if (value == null)
            {
                p.Value = DBNull.Value;
            }
            else
            {
                // Give the database type first crack at converting to DB required type
                value = this._dbType.MapParameterValue(value);

                Type t = value.GetType();
                Type underlyingT = Nullable.GetUnderlyingType(t);
                if (t.GetTypeInfo().IsEnum || (underlyingT != null && underlyingT.GetTypeInfo().IsEnum))		// PostgreSQL .NET driver wont cast enum to int
                {
                    p.Value = (int)value;
                }
                else if (t == typeof(Guid))
                {
                    p.Value = value;
                    p.DbType = DbType.Guid;
                    p.Size = 40;
                    dbtypeSet = true;
                }
                else if (t == typeof(string))
                {
                    string strValue = value as string;
                    if (strValue == null)
                    {
                        p.Size = 0;
                        p.Value = String.Empty;
                    }
                    else
                    {
                        // out of memory exception occurs if trying to save more than 4000 characters to SQL Server CE NText column. Set before attempting to set Size, or Size will always max out at 4000
                        if (strValue.Length + 1 > 4000 && p.GetType().Name == "SqlCeParameter")
                        {
                            p.GetType().GetProperty("SqlDbType").SetValue(p, SqlDbType.NText, null);
                        }

                        p.Size = Math.Max(strValue.Length + 1, 4000); // Help query plan caching by using common size
                        p.Value = value;
                    }
                }
                else if (t == typeof(AnsiString))
                {
                    AnsiString ansistrValue = value as AnsiString;
                    if (ansistrValue == null)
                    {
                        p.Size = 0;
                        p.Value = String.Empty;
                        p.DbType = DbType.AnsiString;
                    }
                    else
                    {
                        // Thanks @DataChomp for pointing out the SQL Server indexing performance hit of using wrong string type on varchar
                        p.Size = Math.Max(ansistrValue.Value.Length + 1, 4000);
                        p.Value = ansistrValue.Value;
                        p.DbType = DbType.AnsiString;
                    }
                    dbtypeSet = true;
                }
                else if (value.GetType().Name == "SqlGeography") //SqlGeography is a CLR Type
                {
                    p.GetType().GetProperty("UdtTypeName").SetValue(p, "geography", null); //geography is the equivalent SQL Server Type
                    p.Value = value;
                }

                else if (value.GetType().Name == "SqlGeometry") //SqlGeometry is a CLR Type
                {
                    p.GetType().GetProperty("UdtTypeName").SetValue(p, "geometry", null); //geography is the equivalent SQL Server Type
                    p.Value = value;
                }
                else
                {
                    p.Value = value;
                }

                if (!dbtypeSet)
                {
                    DbType? dbType = this._dbType.LookupDbType(p.Value.GetTheType(), p.ParameterName);
                    if (dbType.HasValue)
                    {
                        p.DbType = dbType.Value;
                    }
                }
            }

            cmd.Parameters.Add(p);
        }

        // Create a command
        public virtual DbCommand CreateCommand(DbConnection connection, string sql, params object[] args)
        {
            // Perform parameter prefix replacements
            if (this._paramPrefix != "@")
                sql = ParameterHelper.rxParamsPrefix.Replace(sql, m => this._paramPrefix + m.Value.Substring(1));
            sql = sql.Replace("@@", "@");		   // <- double @@ escapes a single @

            // Create the command and add parameters
            DbCommand cmd = connection.CreateCommand();
            cmd.Connection = connection;
            cmd.CommandText = sql;
            cmd.Transaction = this._transaction;

            foreach (object item in args)
            {
                this.AddParameter(cmd, item);
            }

            // Notify the DB type
            this._dbType.PreExecute(cmd);

            return cmd;
        }

        protected virtual void OnException(Exception exception)
        {
        }

        // Override this to log/capture exceptions
        private void OnExceptionInternal(Exception exception)
        {
#if DEBUG
            System.Diagnostics.Debug.WriteLine("***** EXCEPTION *****" + Environment.NewLine + Environment.NewLine + exception.Message + Environment.NewLine + exception.StackTrace);
            System.Diagnostics.Debug.WriteLine("***** LAST COMMAND *****" + Environment.NewLine + Environment.NewLine + this.LastCommand);
            System.Diagnostics.Debug.WriteLine("***** CONN INFO *****" + Environment.NewLine + Environment.NewLine + "Provider: " + this._providerName + Environment.NewLine + "Connection String: " + this._connectionString + Environment.NewLine + "DB Type: " + this._dbType);
#endif
            this.OnException(exception);
            foreach (IExceptionInterceptor interceptor in this.Interceptors.OfType<IExceptionInterceptor>())
            {
                interceptor.OnException(this, exception);
            }
        }

        protected virtual DbConnection OnConnectionOpened(DbConnection conn)
        {
            return conn;
        }

        private DbConnection OnConnectionOpenedInternal(DbConnection conn)
        {
            DbConnection newConnection = this.OnConnectionOpened(conn);
            foreach (IConnectionInterceptor interceptor in this.Interceptors.OfType<IConnectionInterceptor>())
            {
                newConnection = interceptor.OnConnectionOpened(this, newConnection);
            }
            return newConnection;
        }

        protected virtual void OnConnectionClosing(DbConnection conn)
        {
        }

        private void OnConnectionClosingInternal(DbConnection conn)
        {
            this.OnConnectionClosing(conn);
            foreach (IConnectionInterceptor interceptor in this.Interceptors.OfType<IConnectionInterceptor>())
            {
                interceptor.OnConnectionClosing(this, conn);
            }
        }

        protected virtual void OnExecutingCommand(DbCommand cmd)
        {

        }

        private void OnExecutingCommandInternal(DbCommand cmd)
        {
            this.OnExecutingCommand(cmd);
            foreach (IExecutingInterceptor interceptor in this.Interceptors.OfType<IExecutingInterceptor>())
            {
                interceptor.OnExecutingCommand(this, cmd);
            }
        }

        protected virtual void OnExecutedCommand(DbCommand cmd)
        {

        }

        private void OnExecutedCommandInternal(DbCommand cmd)
        {
#if DEBUG
            System.Diagnostics.Debug.WriteLine(this.LastCommand);
#endif
            this.OnExecutedCommand(cmd);
            foreach (IExecutingInterceptor interceptor in this.Interceptors.OfType<IExecutingInterceptor>())
            {
                interceptor.OnExecutedCommand(this, cmd);
            }
        }

        private List<IInterceptor> _interceptors = new List<IInterceptor>();
        public List<IInterceptor> Interceptors => this._interceptors;

        protected virtual bool OnInserting(InsertContext insertContext)
        {
            return true;
        }

        private bool OnInsertingInternal(InsertContext insertContext)
        {
            bool result = this.OnInserting(insertContext);
            return result && this.Interceptors.OfType<IDataInterceptor>().All(x => x.OnInserting(this, insertContext));
        }

        protected virtual bool OnUpdating(UpdateContext updateContext)
        {
            return true;
        }

        private bool OnUpdatingInternal(UpdateContext updateContext)
        {
            bool result = this.OnUpdating(updateContext);
            return result && this.Interceptors.OfType<IDataInterceptor>().All(x => x.OnUpdating(this, updateContext));
        }

        protected virtual bool OnDeleting(DeleteContext deleteContext)
        {
            return true;
        }

        private bool OnDeletingInternal(DeleteContext deleteContext)
        {
            bool result = this.OnDeleting(deleteContext);
            return result && this.Interceptors.OfType<IDataInterceptor>().All(x => x.OnDeleting(this, deleteContext));
        }

        // Execute a non-query command
        public int Execute(string sql, params object[] args)
        {
            return this.Execute(new Sql(sql, args));
        }

        public int Execute(Sql Sql)
        {
            string sql = Sql.SQL;
            object[] args = Sql.Arguments;

            try
            {
                this.OpenSharedConnectionInternal();
                using (DbCommand cmd = this.CreateCommand(this._sharedConnection, sql, args))
                {
                    int result = this.ExecuteNonQueryHelper(cmd);
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

        // Execute and cast a scalar property
        public T ExecuteScalar<T>(string sql, params object[] args)
        {
            return this.ExecuteScalar<T>(new Sql(sql, args));
        }

        public T ExecuteScalar<T>(Sql Sql)
        {
            string sql = Sql.SQL;
            object[] args = Sql.Arguments;

            try
            {
                this.OpenSharedConnectionInternal();
                using (DbCommand cmd = this.CreateCommand(this._sharedConnection, sql, args))
                {
                    object val = this.ExecuteScalarHelper(cmd);

                    if (val == null || val == DBNull.Value)
                        return default(T);

                    Type t = typeof (T);
                    Type u = Nullable.GetUnderlyingType(t);

                    return (T) Convert.ChangeType(val, u ?? t);
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

        public bool EnableAutoSelect { get; set; }

        // Return a typed list of pocos
        public List<T> Fetch<T>(string sql, params object[] args)
        {
            return this.Fetch<T>(new Sql(sql, args));
        }

        public List<T> Fetch<T>(Sql sql)
        {
            return this.Query<T>(sql).ToList();
        }

        public List<T> Fetch<T>()
        {
            return this.Fetch<T>("");
        }

        public void BuildPageQueries<T>(long skip, long take, string sql, ref object[] args, out string sqlCount, out string sqlPage)
        {
            // Add auto select clause
            if (this.EnableAutoSelect)
                sql = AutoSelectHelper.AddSelectClause(this, typeof(T), sql);

            // Split the SQL
            PagingHelper.SQLParts parts;
            if (!PagingHelper.SplitSQL(sql, out parts)) throw new Exception("Unable to parse SQL statement for paged query");

            sqlPage = this._dbType.BuildPageQuery(skip, take, parts, ref args);
            sqlCount = parts.sqlCount;
        }

        // Fetch a page
        public Page<T> Page<T>(long page, long itemsPerPage, Sql sql)
        {
            return this.Page<T>(page, itemsPerPage, sql.SQL, sql.Arguments);
        }

        public List<T> Fetch<T>(long page, long itemsPerPage, string sql, params object[] args)
        {
            return this.SkipTake<T>((page - 1) * itemsPerPage, itemsPerPage, sql, args);
        }

        public List<T> Fetch<T>(long page, long itemsPerPage, Sql sql)
        {
            return this.SkipTake<T>((page - 1) * itemsPerPage, itemsPerPage, sql.SQL, sql.Arguments);
        }

        public List<T> SkipTake<T>(long skip, long take, string sql, params object[] args)
        {
            string sqlCount, sqlPage;
            this.BuildPageQueries<T>(skip, take, sql, ref args, out sqlCount, out sqlPage);
            return this.Fetch<T>(sqlPage, args);
        }

        public List<T> SkipTake<T>(long skip, long take, Sql sql)
        {
            return this.SkipTake<T>(skip, take, sql.SQL, sql.Arguments);
        }

        public Dictionary<TKey, TValue> Dictionary<TKey, TValue>(Sql Sql)
        {
            return this.Dictionary<TKey, TValue>(Sql.SQL, Sql.Arguments);
        }

        public Dictionary<TKey, TValue> Dictionary<TKey, TValue>(string sql, params object[] args)
        {
            Dictionary<TKey, TValue> newDict = new Dictionary<TKey, TValue>();
            bool isConverterSet = false;
            Func<object, object> converter1 = x => x, converter2 = x => x;

            foreach (Dictionary<string, object> line in this.Query<Dictionary<string, object>>(sql, args))
            {
                object key = line.ElementAt(0).Value;
                object value = line.ElementAt(1).Value;

                if (isConverterSet == false)
                {
                    converter1 = MappingHelper.GetConverter(this.Mappers, null, typeof(TKey), key.GetType()) ?? (x => x);
                    converter2 = (value != null ? MappingHelper.GetConverter(this.Mappers, null, typeof(TValue), value.GetType()) : null) ?? (x => x);
                    isConverterSet = true;
                }

                TKey keyConverted = (TKey)Convert.ChangeType(converter1(key), typeof(TKey));

                Type valueType = Nullable.GetUnderlyingType(typeof(TValue)) ?? typeof(TValue);
                object valConv = converter2(value);
                TValue valConverted = valConv != null ? (TValue)Convert.ChangeType(valConv, valueType) : default(TValue);

                if (keyConverted != null)
                {
                    newDict.Add(keyConverted, valConverted);
                }
            }
            return newDict;
        }

        // Return an enumerable collection of pocos
        public IEnumerable<T> Query<T>(string sql, params object[] args)
        {
            return this.Query<T>(new Sql(sql, args));
        }

        public IEnumerable<T> Query<T>(Sql Sql)
        {
            return this.Query(default(T), Sql);
        }

        private IEnumerable<T> Read<T>(Type type, object instance, DbDataReader r)
        {
            try
            {
                using (r)
                {
                    PocoData pd = this.PocoDataFactory.ForType(type);
                    MappingFactory factory = new MappingFactory(pd, r);
                    while (true)
                    {
                        T poco;
                        try
                        {
                            if (!r.Read()) yield break;
                            poco = (T)factory.Map(r, instance);
                        }
                        catch (Exception x)
                        {
                            this.OnExceptionInternal(x);
                            throw;
                        }

                        yield return poco;
                    }
                }
            }
            finally
            {
                this.CloseSharedConnectionInternal();
            }
        }

        private IEnumerable<T> ReadOneToMany<T>(T instance, DbDataReader r, Expression<Func<T, IList>> listExpression, Func<T, object[]> idFunc)
        {
            Func<T, IList> listFunc = null;
            PocoMember pocoMember = null;
            PocoMember foreignMember = null;

            try
            {
                using (r)
                {
                    PocoData pocoData = this.PocoDataFactory.ForType(typeof (T));
                    if (listExpression != null)
                    {
                        idFunc = idFunc ?? (x => pocoData.GetPrimaryKeyValues(x));
                        listFunc = listExpression.Compile();
                        string key = PocoColumn.GenerateKey(MemberChainHelper.GetMembers(listExpression));
                        pocoMember = pocoData.Members.FirstOrDefault(x => x.Name == key);
                        foreignMember = pocoMember != null ? pocoMember.PocoMemberChildren.FirstOrDefault(x => x.Name == pocoMember.ReferenceMemberName && x.ReferenceType == ReferenceType.Foreign) : null;
                    }

                    MappingFactory factory = new MappingFactory(pocoData, r);
                    object prevPoco = null;

                    while (true)
                    {
                        T poco;
                        try
                        {
                            if (!r.Read()) break;
                            poco = (T) factory.Map(r, instance);
                        }
                        catch (Exception x)
                        {
                            this.OnExceptionInternal(x);
                            throw;
                        }

                        if (prevPoco != null)
                        {
                            if (listFunc != null
                                && pocoMember != null
                                && idFunc(poco).SequenceEqual(idFunc((T) prevPoco)))
                            {
                                OneToManyHelper.SetListValue(listFunc, pocoMember, prevPoco, poco);
                                continue;
                            }

                            OneToManyHelper.SetForeignList(listFunc, foreignMember, prevPoco);
                            yield return (T)prevPoco;
                        }

                        prevPoco = poco;
                    }

                    if (prevPoco != null)
                    {
                        OneToManyHelper.SetForeignList(listFunc, foreignMember, prevPoco);
                        yield return (T)prevPoco;
                    }
                }
            }
            finally
            {
                this.CloseSharedConnectionInternal();
            }
        }

        public IQueryProviderWithIncludes<T> Query<T>()
        {
            return new QueryProvider<T>(this);
        }

        private IEnumerable<T> Query<T>(T instance, Sql Sql)
        {
            return this.QueryImp(instance, null, null, Sql);
        }

        public List<object> Fetch(Type type, string sql, params object[] args)
        {
            return this.Fetch(type, new Sql(sql, args));
        }

        public List<object> Fetch(Type type, Sql Sql)
        {
            return this.Query(type, Sql).ToList();
        }

        public IEnumerable<object> Query(Type type, string sql, params object[] args)
        {
            return this.Query(type, new Sql(sql, args));
        }

        public IEnumerable<object> Query(Type type, Sql Sql)
        {
            string sql = Sql.SQL;
            object[] args = Sql.Arguments;

            if (this.EnableAutoSelect) sql = AutoSelectHelper.AddSelectClause(this, type, sql);

            try
            {
                this.OpenSharedConnectionInternal();
                using (DbCommand cmd = this.CreateCommand(this._sharedConnection, sql, args))
                {
                    DbDataReader r = this.ExecuteDataReader(cmd);
                    IEnumerable<object> read = this.Read<object>(type, null, r);
                    foreach (object item in read)
                    {
                        yield return item;
                    }
                }
            }
            finally
            {
                this.CloseSharedConnectionInternal();
            }
        }

        internal IEnumerable<T> QueryImp<T>(T instance, Expression<Func<T, IList>> listExpression, Func<T, object[]> idFunc, Sql Sql)
        {
            string sql = Sql.SQL;
            object[] args = Sql.Arguments;

            if (this.EnableAutoSelect) sql = AutoSelectHelper.AddSelectClause(this, typeof (T), sql);

            try
            {
                this.OpenSharedConnectionInternal();
                using (DbCommand cmd = this.CreateCommand(this._sharedConnection, sql, args))
                {
                    DbDataReader r = this.ExecuteDataReader(cmd);
                    IEnumerable<T> read = listExpression != null ? this.ReadOneToMany(instance, r, listExpression, idFunc) : this.Read<T>(typeof(T), instance, r);
                    foreach (T item in read)
                    {
                        yield return item;
                    }
                }
            }
            finally
            {
                this.CloseSharedConnectionInternal();
            }
        }

        private DbDataReader ExecuteDataReader(DbCommand cmd)
        {
            DbDataReader r;
            try
            {
                r = this.ExecuteReaderHelper(cmd);
            }
            catch (Exception x)
            {
                this.OnExceptionInternal(x);
                throw;
            }
            return r;
        }

        public List<T> FetchOneToMany<T>(Expression<Func<T, IList>> many, Sql sql)
        {
            return this.QueryImp(default(T), many, null, sql).ToList();
        }

        public List<T> FetchOneToMany<T>(Expression<Func<T, IList>> many, string sql, params object[] args)
        {
            return this.FetchOneToMany(many, new Sql(sql, args));
        }

        public List<T> FetchOneToMany<T>(Expression<Func<T, IList>> many, Func<T, object> idFunc, Sql sql)
        {
            return this.QueryImp(default(T), many, x => new[] { idFunc(x) }, sql).ToList();
        }

        public List<T> FetchOneToMany<T>(Expression<Func<T, IList>> many, Func<T, object> idFunc, string sql, params object[] args)
        {
            return this.FetchOneToMany(many, idFunc, new Sql(sql, args));
        }

        public Page<T> Page<T>(long page, long itemsPerPage, string sql, params object[] args)
        {
            return this.PageImp<T, Page<T>>(page, itemsPerPage, sql, args, (paged, thesql) =>
            {
                paged.Items =  this.Query<T>(thesql).ToList();
                return paged;
            });
        }

        // Actual implementation of the multi-poco paging
        protected TRet PageImp<T, TRet>(long page, long itemsPerPage, string sql, object[] args, Func<Page<T>, Sql, TRet> executeQueryFunc)
        {
            string sqlCount, sqlPage;

            long offset = (page - 1) * itemsPerPage;

            this.BuildPageQueries<T>(offset, itemsPerPage, sql, ref args, out sqlCount, out sqlPage);

            // Save the one-time command time out and use it for both queries
            int saveTimeout = this.OneTimeCommandTimeout;

            // Setup the paged result
            Page<T> result = new Page<T>();
            result.CurrentPage = page;
            result.ItemsPerPage = itemsPerPage;
            result.TotalItems = this.ExecuteScalar<long>(sqlCount, args);
            result.TotalPages = result.TotalItems / itemsPerPage;
            if ((result.TotalItems % itemsPerPage) != 0)
                result.TotalPages++;

            this.OneTimeCommandTimeout = saveTimeout;

            // Get the records
            return executeQueryFunc(result, new Sql(sqlPage, args));
        }

        public TRet FetchMultiple<T1, T2, TRet>(Func<List<T1>, List<T2>, TRet> cb, string sql, params object[] args) { return this.FetchMultiple<T1, T2, DontMap, DontMap, TRet>(new[] { typeof(T1), typeof(T2) }, cb, new Sql(sql, args)); }
        public TRet FetchMultiple<T1, T2, T3, TRet>(Func<List<T1>, List<T2>, List<T3>, TRet> cb, string sql, params object[] args) { return this.FetchMultiple<T1, T2, T3, DontMap, TRet>(new[] { typeof(T1), typeof(T2), typeof(T3) }, cb, new Sql(sql, args)); }
        public TRet FetchMultiple<T1, T2, T3, T4, TRet>(Func<List<T1>, List<T2>, List<T3>, List<T4>, TRet> cb, string sql, params object[] args) { return this.FetchMultiple<T1, T2, T3, T4, TRet>(new[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4) }, cb, new Sql(sql, args)); }
        public TRet FetchMultiple<T1, T2, TRet>(Func<List<T1>, List<T2>, TRet> cb, Sql sql) { return this.FetchMultiple<T1, T2, DontMap, DontMap, TRet>(new[] { typeof(T1), typeof(T2) }, cb, sql); }
        public TRet FetchMultiple<T1, T2, T3, TRet>(Func<List<T1>, List<T2>, List<T3>, TRet> cb, Sql sql) { return this.FetchMultiple<T1, T2, T3, DontMap, TRet>(new[] { typeof(T1), typeof(T2), typeof(T3) }, cb, sql); }
        public TRet FetchMultiple<T1, T2, T3, T4, TRet>(Func<List<T1>, List<T2>, List<T3>, List<T4>, TRet> cb, Sql sql) { return this.FetchMultiple<T1, T2, T3, T4, TRet>(new[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4) }, cb, sql); }

        public Tuple<List<T1>, List<T2>> FetchMultiple<T1, T2>(string sql, params object[] args) { return this.FetchMultiple<T1, T2, DontMap, DontMap, Tuple<List<T1>, List<T2>>>(new[] { typeof(T1), typeof(T2) }, new Func<List<T1>, List<T2>, Tuple<List<T1>, List<T2>>>((y, z) => new Tuple<List<T1>, List<T2>>(y, z)), new Sql(sql, args)); }
        public Tuple<List<T1>, List<T2>, List<T3>> FetchMultiple<T1, T2, T3>(string sql, params object[] args) { return this.FetchMultiple<T1, T2, T3, DontMap, Tuple<List<T1>, List<T2>, List<T3>>>(new[] { typeof(T1), typeof(T2), typeof(T3) }, new Func<List<T1>, List<T2>, List<T3>, Tuple<List<T1>, List<T2>, List<T3>>>((x, y, z) => new Tuple<List<T1>, List<T2>, List<T3>>(x, y, z)), new Sql(sql, args)); }
        public Tuple<List<T1>, List<T2>, List<T3>, List<T4>> FetchMultiple<T1, T2, T3, T4>(string sql, params object[] args) { return this.FetchMultiple<T1, T2, T3, T4, Tuple<List<T1>, List<T2>, List<T3>, List<T4>>>(new[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4) }, new Func<List<T1>, List<T2>, List<T3>, List<T4>, Tuple<List<T1>, List<T2>, List<T3>, List<T4>>>((w, x, y, z) => new Tuple<List<T1>, List<T2>, List<T3>, List<T4>>(w, x, y, z)), new Sql(sql, args)); }
        public Tuple<List<T1>, List<T2>> FetchMultiple<T1, T2>(Sql sql) { return this.FetchMultiple<T1, T2, DontMap, DontMap, Tuple<List<T1>, List<T2>>>(new[] { typeof(T1), typeof(T2) }, new Func<List<T1>, List<T2>, Tuple<List<T1>, List<T2>>>((y, z) => new Tuple<List<T1>, List<T2>>(y, z)), sql); }
        public Tuple<List<T1>, List<T2>, List<T3>> FetchMultiple<T1, T2, T3>(Sql sql) { return this.FetchMultiple<T1, T2, T3, DontMap, Tuple<List<T1>, List<T2>, List<T3>>>(new[] { typeof(T1), typeof(T2), typeof(T3) }, new Func<List<T1>, List<T2>, List<T3>, Tuple<List<T1>, List<T2>, List<T3>>>((x, y, z) => new Tuple<List<T1>, List<T2>, List<T3>>(x, y, z)), sql); }
        public Tuple<List<T1>, List<T2>, List<T3>, List<T4>> FetchMultiple<T1, T2, T3, T4>(Sql sql) { return this.FetchMultiple<T1, T2, T3, T4, Tuple<List<T1>, List<T2>, List<T3>, List<T4>>>(new[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4) }, new Func<List<T1>, List<T2>, List<T3>, List<T4>, Tuple<List<T1>, List<T2>, List<T3>, List<T4>>>((w, x, y, z) => new Tuple<List<T1>, List<T2>, List<T3>, List<T4>>(w, x, y, z)), sql); }

        public class DontMap { }

        // Actual implementation of the multi query
        private TRet FetchMultiple<T1, T2, T3, T4, TRet>(Type[] types, object cb, Sql Sql)
        {
            string sql = Sql.SQL;
            object[] args = Sql.Arguments;

            try
            {
                this.OpenSharedConnectionInternal();
                using (DbCommand cmd = this.CreateCommand(this._sharedConnection, sql, args))
                {
                    DbDataReader r = this.ExecuteDataReader(cmd);
                    using (r)
                    {
                        int typeIndex = 1;
                        List<T1> list1 = new List<T1>();
                        List<T2> list2 = types.Length > 1 ? new List<T2>() : null;
                        List<T3> list3 = types.Length > 2 ? new List<T3>() : null;
                        List<T4> list4 = types.Length > 3 ? new List<T4>() : null;
                        do
                        {
                            if (typeIndex > types.Length)
                                break;

                            PocoData pd = this.PocoDataFactory.ForType(types[typeIndex - 1]);
                            MappingFactory factory = new MappingFactory(pd, r);

                            while (true)
                            {
                                try
                                {
                                    if (!r.Read())
                                        break;

                                    switch (typeIndex)
                                    {
                                        case 1:
                                            list1.Add((T1) factory.Map(r, default(T1)));
                                            break;
                                        case 2:
                                            list2.Add((T2) factory.Map(r, default(T2)));
                                            break;
                                        case 3:
                                            list3.Add((T3) factory.Map(r, default(T3)));
                                            break;
                                        case 4:
                                            list4.Add((T4) factory.Map(r, default(T4)));
                                            break;
                                    }
                                }
                                catch (Exception x)
                                {
                                    this.OnExceptionInternal(x);
                                    throw;
                                }
                            }

                            typeIndex++;
                        } while (r.NextResult());

                        switch (types.Length)
                        {
                            case 2:
                                return ((Func<List<T1>, List<T2>, TRet>) cb)(list1, list2);
                            case 3:
                                return ((Func<List<T1>, List<T2>, List<T3>, TRet>) cb)(list1, list2, list3);
                            case 4:
                                return ((Func<List<T1>, List<T2>, List<T3>, List<T4>, TRet>) cb)(list1, list2, list3, list4);
                        }

                        return default(TRet);
                    }
                }
            }
            finally
            {
                this.CloseSharedConnectionInternal();
            }
        }

        private bool PocoExists<T>(T poco)
        {
            int index = 0;
            PocoData pd = this.PocoDataFactory.ForType(typeof(T));
            Dictionary<string, object> primaryKeyValuePairs = this.GetPrimaryKeyValues(pd, pd.TableInfo.PrimaryKey, poco, true);
            return this.ExecuteScalar<int>(string.Format(this.DatabaseType.GetExistsSql(), this.DatabaseType.EscapeTableName(pd.TableInfo.TableName), this.BuildPrimaryKeySql(primaryKeyValuePairs, ref index)), primaryKeyValuePairs.Select(x => x.Value).ToArray()) > 0;
        }

        public bool Exists<T>(object primaryKey)
        {
            int index = 0;
            PocoData pd = this.PocoDataFactory.ForType(typeof (T));
            Dictionary<string, object> primaryKeyValuePairs = this.GetPrimaryKeyValues(pd, pd.TableInfo.PrimaryKey, primaryKey, false);
            return this.ExecuteScalar<int>(string.Format(this.DatabaseType.GetExistsSql(), this.DatabaseType.EscapeTableName(pd.TableInfo.TableName), this.BuildPrimaryKeySql(primaryKeyValuePairs, ref index)), primaryKeyValuePairs.Select(x => x.Value).ToArray()) > 0;
        }

        public T SingleById<T>(object primaryKey)
        {
            Sql sql = this.GenerateSingleByIdSql<T>(primaryKey);
            return this.Single<T>(sql);
        }

        public T SingleOrDefaultById<T>(object primaryKey)
        {
            Sql sql = this.GenerateSingleByIdSql<T>(primaryKey);
            return this.SingleOrDefault<T>(sql);
        }

        private Sql GenerateSingleByIdSql<T>(object primaryKey)
        {
            int index = 0;
            PocoData pd = this.PocoDataFactory.ForType(typeof (T));
            Dictionary<string, object> primaryKeyValuePairs = this.GetPrimaryKeyValues(pd, pd.TableInfo.PrimaryKey, primaryKey, false);
            string sql = AutoSelectHelper.AddSelectClause(this, typeof(T), string.Format("WHERE {0}", this.BuildPrimaryKeySql(primaryKeyValuePairs, ref index)));
            object[] args = primaryKeyValuePairs.Select(x => x.Value).ToArray();
            return new Sql(true, sql, args);
        }

        public T Single<T>(string sql, params object[] args)
        {
            return this.Query<T>(sql, args).Single();
        }
        public T SingleInto<T>(T instance, string sql, params object[] args)
        {
            return this.Query(instance, new Sql(sql, args)).Single();
        }
        public T SingleOrDefault<T>(string sql, params object[] args)
        {
            return this.Query<T>(sql, args).SingleOrDefault();
        }
        public T SingleOrDefaultInto<T>(T instance, string sql, params object[] args)
        {
            return this.Query(instance, new Sql(sql, args)).SingleOrDefault();
        }
        public T First<T>(string sql, params object[] args)
        {
            return this.Query<T>(sql, args).First();
        }
        public T FirstInto<T>(T instance, string sql, params object[] args)
        {
            return this.Query(instance, new Sql(sql, args)).First();
        }
        public T FirstOrDefault<T>(string sql, params object[] args)
        {
            return this.Query<T>(sql, args).FirstOrDefault();
        }
        public T FirstOrDefaultInto<T>(T instance, string sql, params object[] args)
        {
            return this.Query(instance, new Sql(sql, args)).FirstOrDefault();
        }
        public T Single<T>(Sql sql)
        {
            return this.Query<T>(sql).Single();
        }
        public T SingleInto<T>(T instance, Sql sql)
        {
            return this.Query(instance, sql).Single();
        }
        public T SingleOrDefault<T>(Sql sql)
        {
            return this.Query<T>(sql).SingleOrDefault();
        }
        public T SingleOrDefaultInto<T>(T instance, Sql sql)
        {
            return this.Query(instance, sql).SingleOrDefault();
        }
        public T First<T>(Sql sql)
        {
            return this.Query<T>(sql).First();
        }
        public T FirstInto<T>(T instance, Sql sql)
        {
            return this.Query(instance, sql).First();
        }
        public T FirstOrDefault<T>(Sql sql)
        {
            return this.Query<T>(sql).FirstOrDefault();
        }
        public T FirstOrDefaultInto<T>(T instance, Sql sql)
        {
            return this.Query(instance, sql).FirstOrDefault();
        }

        // Insert an annotated poco object
        public object Insert<T>(T poco)
        {
            TableInfo tableInfo = this.PocoDataFactory.TableInfoForType(poco.GetType());
            return this.Insert(tableInfo.TableName, tableInfo.PrimaryKey, tableInfo.AutoIncrement, poco);
        }

        public object Insert<T>(string tableName, string primaryKeyName, T poco)
        {
            return this.Insert(tableName, primaryKeyName, true, poco);
        }

        // Insert a poco into a table.  If the poco has a property with the same name
        // as the primary key the id of the new record is assigned to it.  Either way,
        // the new id is returned.
        public virtual object Insert<T>(string tableName, string primaryKeyName, bool autoIncrement, T poco)
        {
            PocoData pd = this.PocoDataFactory.ForObject(poco, primaryKeyName, autoIncrement);
            return this.InsertImp(pd, tableName, primaryKeyName, autoIncrement, poco);
        }

        private object InsertImp<T>(PocoData pocoData, string tableName, string primaryKeyName, bool autoIncrement, T poco)
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
                        this.ExecuteNonQueryHelper(cmd);
                        id = InsertStatements.AssignNonIncrementPrimaryKey(primaryKeyName, poco, preparedInsert);
                    }
                    else
                    {
                        id = this._dbType.ExecuteInsert(this, cmd, primaryKeyName, preparedInsert.PocoData.TableInfo.UseOutputClause, poco, preparedInsert.Rawvalues.ToArray());
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

        public void InsertBatch<T>(IEnumerable<T> pocos, BatchOptions options = null)
        {
            options = options ?? new BatchOptions();

            try
            {
                this.OpenSharedConnectionInternal();

                PocoData pd = this.PocoDataFactory.ForType(typeof(T));

                foreach (T[] batchedPocos in pocos.Chunkify(options.BatchSize))
                {
                    InsertStatements.PreparedInsertSql[] preparedInserts = batchedPocos.Select(x => InsertStatements.PrepareInsertSql(this, pd, pd.TableInfo.TableName, pd.TableInfo.PrimaryKey,pd.TableInfo.AutoIncrement, x)).ToArray();

                    Sql sql = new Sql();
                    foreach (InsertStatements.PreparedInsertSql preparedInsertSql in preparedInserts)
                    {
                        sql.Append(preparedInsertSql.Sql + options.StatementSeperator, preparedInsertSql.Rawvalues.ToArray());
                    }

                    using (DbCommand cmd = this.CreateCommand(this._sharedConnection, sql.SQL, sql.Arguments))
                    {
                        this.ExecuteNonQueryHelper(cmd);
                    }
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

        public void InsertBulk<T>(IEnumerable<T> pocos)
        {
            try
            {
                this.OpenSharedConnectionInternal();
                this._dbType.InsertBulk(this, pocos);
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

        public int Update(string tableName, string primaryKeyName, object poco, object primaryKeyValue)
        {
            return this.Update(tableName, primaryKeyName, poco, primaryKeyValue, null);
        }

        public virtual int Update(string tableName, string primaryKeyName, object poco, object primaryKeyValue, IEnumerable<string> columns)
        {
            return this.UpdateImp(tableName, primaryKeyName, poco, primaryKeyValue, columns, (sql, args, next) => next(this.Execute(sql, args)), 0);
        }

        // Update a record with values from a poco.  primary key value can be either supplied or read from the poco
        private TRet UpdateImp<TRet>(string tableName, string primaryKeyName, object poco, object primaryKeyValue, IEnumerable<string> columns, Func<string, object[], Func<int, int>, TRet> executeFunc, TRet defaultId)
        {
            if (!this.OnUpdatingInternal(new UpdateContext(poco, tableName, primaryKeyName, primaryKeyValue, columns)))
                return defaultId;

            if (columns != null && !columns.Any())
                return defaultId;

            StringBuilder sb = new StringBuilder();
            int index = 0;
            List<object> rawvalues = new List<object>();
            PocoData pd = this.PocoDataFactory.ForObject(poco, primaryKeyName, true);
            string versionName = null;
            object versionValue = null;
            VersionColumnType versionColumnType = VersionColumnType.Number;

            Dictionary<string, object> primaryKeyValuePairs = this.GetPrimaryKeyValues(pd, primaryKeyName, primaryKeyValue ?? poco, primaryKeyValue == null);

            foreach (PocoColumn pocoColumn in pd.Columns.Values)
            {
                // Don't update the primary key, but grab the value if we don't have it
                if (primaryKeyValue == null && primaryKeyValuePairs.ContainsKey(pocoColumn.ColumnName))
                {
                    primaryKeyValuePairs[pocoColumn.ColumnName] = this.ProcessMapper(pocoColumn, pocoColumn.GetValue(poco));
                    continue;
                }

                // Dont update result only columns
                if (pocoColumn.ResultColumn
                    || (pocoColumn.ComputedColumn && (pocoColumn.ComputedColumnType == ComputedColumnType.Always || pocoColumn.ComputedColumnType == ComputedColumnType.ComputedOnUpdate)))
                {
                    continue;
                }

                if (!pocoColumn.VersionColumn && columns != null && !columns.Contains(pocoColumn.ColumnName, StringComparer.OrdinalIgnoreCase))
                    continue;

                object value = pocoColumn.GetColumnValue(pd, poco, this.ProcessMapper);

                if (pocoColumn.VersionColumn)
                {
                    versionName = pocoColumn.ColumnName;
                    versionValue = value;
                    if (pocoColumn.VersionColumnType == VersionColumnType.Number)
                    {
                        versionColumnType = VersionColumnType.Number;
                        value = Convert.ToInt64(value) + 1;
                    }
                    else if (pocoColumn.VersionColumnType == VersionColumnType.RowVersion)
                    {
                        versionColumnType = VersionColumnType.RowVersion;
                        continue;
                    }
                }

                // Build the sql
                if (index > 0)
                    sb.Append(", ");
                sb.AppendFormat("{0} = @{1}", this._dbType.EscapeSqlIdentifier(pocoColumn.ColumnName), index++);

                rawvalues.Add(value);
            }

            if (columns != null && columns.Any() && sb.Length == 0)
                throw new ArgumentException("There were no columns in the columns list that matched your table", "columns");

            string sql = string.Format("UPDATE {0} SET {1} WHERE {2}", this._dbType.EscapeTableName(tableName), sb, this.BuildPrimaryKeySql(primaryKeyValuePairs, ref index));

            rawvalues.AddRange(primaryKeyValuePairs.Select(keyValue => keyValue.Value));

            if (!string.IsNullOrEmpty(versionName))
            {
                sql += string.Format(" AND {0} = @{1}", this._dbType.EscapeSqlIdentifier(versionName), index++);
                rawvalues.Add(versionValue);
            }

            TRet result = executeFunc(sql, rawvalues.ToArray(), (id) =>
            {
                if (id == 0 && !string.IsNullOrEmpty(versionName) && this.VersionException == VersionExceptionHandling.Exception)
                {
#if DNXCORE50
                    throw new Exception(string.Format("A Concurrency update occurred in table '{0}' for primary key value(s) = '{1}' and version = '{2}'", tableName, string.Join(",", primaryKeyValuePairs.Values.Select(x => x.ToString()).ToArray()), versionValue));
#else
                    throw new DBConcurrencyException(string.Format("A Concurrency update occurred in table '{0}' for primary key value(s) = '{1}' and version = '{2}'", tableName, string.Join(",", primaryKeyValuePairs.Values.Select(x => x.ToString()).ToArray()), versionValue));
#endif
                }

                // Set Version
                if (!string.IsNullOrEmpty(versionName) && versionColumnType == VersionColumnType.Number)
                {
                    PocoColumn pc;
                    if (pd.Columns.TryGetValue(versionName, out pc))
                    {
                        pc.SetValue(poco, Convert.ChangeType(Convert.ToInt64(versionValue) + 1, pc.MemberInfoData.MemberType));
                    }
                }

                return id;
            });

            return result;
        }

        private string BuildPrimaryKeySql(Dictionary<string, object> primaryKeyValuePair, ref int index)
        {
            int tempIndex = index;
            index += primaryKeyValuePair.Count;
            return string.Join(" AND ", primaryKeyValuePair.Select((x, i) => x.Value == null || x.Value == DBNull.Value ? string.Format("{0} IS NULL", this._dbType.EscapeSqlIdentifier(x.Key)) : string.Format("{0} = @{1}", this._dbType.EscapeSqlIdentifier(x.Key), tempIndex + i)).ToArray());
        }

        private Dictionary<string, object> GetPrimaryKeyValues(PocoData pocoData, string primaryKeyName, object primaryKeyValueOrPoco, bool isPoco)
        {
            Dictionary<string, object> primaryKeyValues;

            string[] multiplePrimaryKeysNames = primaryKeyName.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToArray();
            if (isPoco == false)
            {
                if (multiplePrimaryKeysNames.Length == 1)
                {
                    primaryKeyValues = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase) { { primaryKeyName, primaryKeyValueOrPoco } };
                }
                else
                {
                    Dictionary<string, object> dict = primaryKeyValueOrPoco as Dictionary<string, object>;
                    primaryKeyValues = dict ?? multiplePrimaryKeysNames.ToDictionary(x => x, x => primaryKeyValueOrPoco.GetType().GetProperties().Single(y => string.Equals(x, y.Name, StringComparison.OrdinalIgnoreCase)).GetValue(primaryKeyValueOrPoco, null), StringComparer.OrdinalIgnoreCase);
                }
            }
            else
            {
                primaryKeyValues = this.ProcessMapper(pocoData, multiplePrimaryKeysNames.ToDictionary(x => x, x => pocoData.Columns[x].GetValue(primaryKeyValueOrPoco), StringComparer.OrdinalIgnoreCase));
            }

            return primaryKeyValues;
        }

        private Dictionary<string, object> ProcessMapper(PocoData pd, Dictionary<string, object> primaryKeyValuePairs)
        {
            string[] keys = primaryKeyValuePairs.Keys.ToArray();
            foreach (string primaryKeyValuePair in keys)
            {
                PocoColumn col = pd.Columns[primaryKeyValuePair];
                primaryKeyValuePairs[primaryKeyValuePair] = this.ProcessMapper(col, primaryKeyValuePairs[primaryKeyValuePair]);
            }
            return primaryKeyValuePairs;
        }

        public IUpdateQueryProvider<T> UpdateMany<T>()
        {
            return new UpdateQueryProvider<T>(this);
        }

        public int Update(string tableName, string primaryKeyName, object poco)
        {
            return this.Update(tableName, primaryKeyName, poco, null);
        }

        public int Update(string tableName, string primaryKeyName, object poco, IEnumerable<string> columns)
        {
            return this.Update(tableName, primaryKeyName, poco, null, columns);
        }

        public int Update(object poco, IEnumerable<string> columns)
        {
            return this.Update(poco, null, columns);
        }

        public int Update<T>(T poco, Expression<Func<T, object>> fields)
        {
            SqlExpression<T> expression = this.DatabaseType.ExpressionVisitor<T>(this, this.PocoDataFactory.ForType(typeof(T)));
            expression = expression.Select(fields);
            IEnumerable<string> columnNames = ((ISqlExpression) expression).SelectMembers.Select(x => x.PocoColumn.ColumnName);
            IEnumerable<string> otherNames = ((ISqlExpression) expression).GeneralMembers.Select(x => x.PocoColumn.ColumnName);
            return this.Update(poco, columnNames.Union(otherNames));
        }

        public int Update(object poco)
        {
            return this.Update(poco, null, null);
        }

        public int Update(object poco, object primaryKeyValue)
        {
            return this.Update(poco, primaryKeyValue, null);
        }

        public int Update(object poco, object primaryKeyValue, IEnumerable<string> columns)
        {
            TableInfo tableInfo = this.PocoDataFactory.TableInfoForType(poco.GetType());
            return this.Update(tableInfo.TableName, tableInfo.PrimaryKey, poco, primaryKeyValue, columns);
        }

        public int Update<T>(string sql, params object[] args)
        {
            TableInfo tableInfo = this.PocoDataFactory.TableInfoForType(typeof(T));
            return this.Execute(string.Format("UPDATE {0} {1}", this._dbType.EscapeTableName(tableInfo.TableName), sql), args);
        }

        public int Update<T>(Sql sql)
        {
            TableInfo tableInfo = this.PocoDataFactory.TableInfoForType(typeof(T));
            return this.Execute(new Sql(string.Format("UPDATE {0}", this._dbType.EscapeTableName(tableInfo.TableName))).Append(sql));
        }

        public IDeleteQueryProvider<T> DeleteMany<T>()
        {
            return new DeleteQueryProvider<T>(this);
        }

        public int Delete(string tableName, string primaryKeyName, object poco)
        {
            return this.Delete(tableName, primaryKeyName, poco, null);
        }

        public virtual int Delete(string tableName, string primaryKeyName, object poco, object primaryKeyValue)
        {
            return this.DeleteImp(tableName, primaryKeyName, poco, primaryKeyValue, this.Execute, 0);
        }

        private TRet DeleteImp<TRet>(string tableName, string primaryKeyName, object poco, object primaryKeyValue, Func<string, object[], TRet> executeFunc, TRet defaultRet)
        {
            if (!this.OnDeletingInternal(new DeleteContext(poco, tableName, primaryKeyName, primaryKeyValue)))
                return defaultRet;

            PocoData pd = poco != null ? this.PocoDataFactory.ForObject(poco, primaryKeyName, true) : null;
            Dictionary<string, object> primaryKeyValuePairs = this.GetPrimaryKeyValues(pd, primaryKeyName, primaryKeyValue ?? poco, primaryKeyValue == null);

            // Do it
            int index = 0;
            string sql = string.Format("DELETE FROM {0} WHERE {1}", this._dbType.EscapeTableName(tableName), this.BuildPrimaryKeySql(primaryKeyValuePairs, ref index));
            return executeFunc(sql, primaryKeyValuePairs.Select(x => x.Value).ToArray());
        }

        public int Delete(object poco)
        {
            TableInfo tableInfo = this.PocoDataFactory.TableInfoForType(poco.GetType());
            return this.Delete(tableInfo.TableName, tableInfo.PrimaryKey, poco);
        }

        public int Delete<T>(object pocoOrPrimaryKey)
        {
            if (pocoOrPrimaryKey.GetType() == typeof(T))
                return this.Delete(pocoOrPrimaryKey);
            TableInfo tableInfo = this.PocoDataFactory.TableInfoForType(typeof(T));
            return this.Delete(tableInfo.TableName, tableInfo.PrimaryKey, null, pocoOrPrimaryKey);
        }

        public int Delete<T>(string sql, params object[] args)
        {
            TableInfo tableInfo = this.PocoDataFactory.TableInfoForType(typeof(T));
            return this.Execute(string.Format("DELETE FROM {0} {1}", this._dbType.EscapeTableName(tableInfo.TableName), sql), args);
        }

        public int Delete<T>(Sql sql)
        {
            TableInfo tableInfo = this.PocoDataFactory.TableInfoForType(typeof(T));
            return this.Execute(new Sql(string.Format("DELETE FROM {0}", this._dbType.EscapeTableName(tableInfo.TableName))).Append(sql));
        }

        /// <summary>Checks if a poco represents a new record.</summary>
        public bool IsNew<T>(T poco)
        {
#if !NET35
            if (poco is System.Dynamic.ExpandoObject || poco is PocoExpando)
            {
                return true;
            }
#endif
            PocoData pd = this.PocoDataFactory.ForType(poco.GetType());
            object pk;
            PocoColumn pc;

            if (pd.Columns.TryGetValue(pd.TableInfo.PrimaryKey, out pc))
            {
                pk = pc.GetValue(poco);
            }
            else if (pd.TableInfo.PrimaryKey.Contains(","))
            {
                return !this.PocoExists(poco);
            }
            else
            {
                PropertyInfo pi = poco.GetType().GetProperty(pd.TableInfo.PrimaryKey);
                if (pi == null) throw new ArgumentException(string.Format("The object doesn't have a property matching the primary key column name '{0}'", pd.TableInfo.PrimaryKey));
                pk = pi.GetValue(poco, null);
            }

            if (pk == null)
                return true;

            if (!pd.TableInfo.AutoIncrement)
                return !this.Exists<T>(pk);

            Type type = pk.GetType();

            if (type.GetTypeInfo().IsValueType)
            {
                // Common primary key types
                if (type == typeof(long)) return (long)pk == default(long);
                if (type == typeof(ulong)) return (ulong)pk == default(ulong);
                if (type == typeof(int)) return (int)pk == default(int);
                if (type == typeof(uint)) return (uint)pk == default(uint);
                if (type == typeof(Guid)) return (Guid)pk == default(Guid);

                // Create a default instance and compare
                return pk == Activator.CreateInstance(pk.GetType());
            }

            return false;
        }

        // Insert new record or Update existing record
        public void Save<T>(T poco)
        {
            TableInfo tableInfo = this.PocoDataFactory.TableInfoForType(poco.GetType());
            if (this.IsNew(poco))
            {
                this.Insert(tableInfo.TableName, tableInfo.PrimaryKey, tableInfo.AutoIncrement, poco);
            }
            else
            {
                this.Update(tableInfo.TableName, tableInfo.PrimaryKey, poco);
            }
        }

        public int CommandTimeout { get; set; }
        public int OneTimeCommandTimeout { get; set; }

        void DoPreExecute(DbCommand cmd)
        {
            // Setup command timeout
            if (this.OneTimeCommandTimeout != 0)
            {
                cmd.CommandTimeout = this.OneTimeCommandTimeout;
                this.OneTimeCommandTimeout = 0;
            }
            else if (this.CommandTimeout != 0)
            {
                cmd.CommandTimeout = this.CommandTimeout;
            }

            // Call hook
            this.OnExecutingCommandInternal(cmd);

            // Save it
            this._lastSql = cmd.CommandText;
            this._lastArgs = (from DbParameter parameter in cmd.Parameters select parameter.Value).ToArray();
        }

        public string LastSQL => this._lastSql;
        public object[] LastArgs => this._lastArgs;
        public string LastCommand => this.FormatCommand(this._lastSql, this._lastArgs);

        private class FormattedParameter
        {
            public Type Type { get; set; }
            public object Value { get; set; }
            public DbParameter Parameter { get; set; }
        }

        public string FormatCommand(DbCommand cmd)
        {
            IEnumerable<FormattedParameter> parameters = cmd.Parameters.Cast<DbParameter>().Select(parameter => new FormattedParameter()
            {
                Type = parameter.Value.GetTheType(),
                Value = parameter.Value,
                Parameter = parameter
            });
            return this.FormatCommand(cmd.CommandText, parameters.Cast<object>().ToArray());
        }

        public string FormatCommand(string sql, object[] args)
        {
            StringBuilder sb = new StringBuilder();
            if (sql == null)
                return "";
            sb.Append(sql);
            if (args != null && args.Length > 0)
            {
                sb.Append("\n");
                for (int i = 0; i < args.Length; i++)
                {
                    string type = args[i] != null ? args[i].GetType().Name : string.Empty;
                    object value = args[i];
                    FormattedParameter formatted = args[i] as FormattedParameter;
                    if (formatted != null)
                    {
                        type = formatted.Type != null ? formatted.Type.Name : string.Format("{0}, {1}", formatted.Parameter.GetType().Name, formatted.Parameter.DbType);
                        value = formatted.Value;
                    }
                    sb.AppendFormat("\t -> {0}{1} [{2}] = \"{3}\"\n", this._paramPrefix, i, type, value);
                }
                sb.Remove(sb.Length - 1, 1);
            }
            return sb.ToString();
        }

        private MapperCollection _mappers = new MapperCollection();
        public MapperCollection Mappers => this._mappers;

        private IPocoDataFactory _pocoDataFactory;
        public IPocoDataFactory PocoDataFactory
        {
            get { return this._pocoDataFactory ?? (this._pocoDataFactory = new PocoDataFactory(this.Mappers)); }
            set { this._pocoDataFactory = value; }
        }

        public string ConnectionString => this._connectionString;

        // Member variables
        private readonly string _connectionString;
        private readonly string _providerName;
        private DbProviderFactory _factory;
        private DbConnection _sharedConnection;
        private DbTransaction _transaction;
        private IsolationLevel _isolationLevel;
        private string _lastSql;
        private object[] _lastArgs;
        private string _paramPrefix = "@";
        private VersionExceptionHandling _versionException = VersionExceptionHandling.Exception;

        internal int ExecuteNonQueryHelper(DbCommand cmd)
        {
            this.DoPreExecute(cmd);
            int result = cmd.ExecuteNonQuery();
            this.OnExecutedCommandInternal(cmd);
            return result;
        }

        internal object ExecuteScalarHelper(DbCommand cmd)
        {
            this.DoPreExecute(cmd);
            object r = cmd.ExecuteScalar();
            this.OnExecutedCommandInternal(cmd);
            return r;
        }

        internal DbDataReader ExecuteReaderHelper(DbCommand cmd)
        {
            this.DoPreExecute(cmd);
            DbDataReader r = cmd.ExecuteReader();
            this.OnExecutedCommandInternal(cmd);
            return r;
        }

        internal object ProcessMapper(PocoColumn pc, object value)
        {
            Func<object, object> converter = this.Mappers.Find(x => x.GetToDbConverter(pc.ColumnType, pc.MemberInfoData.MemberInfo));
            return converter != null ? converter(value) : this.ProcessDefaultMappings(pc, value);
        }

        internal static bool IsEnum(MemberInfoData memberInfo)
        {
            Type underlyingType = Nullable.GetUnderlyingType(memberInfo.MemberType);
            return memberInfo.MemberType.GetTypeInfo().IsEnum || (underlyingType != null && underlyingType.GetTypeInfo().IsEnum);
        }

        private object ProcessDefaultMappings(PocoColumn pocoColumn, object value)
        {
            if (pocoColumn.SerializedColumn)
            {
                return DatabaseFactory.ColumnSerializer.Serialize(value);
            }
            if (pocoColumn.ColumnType == typeof (string) && IsEnum(pocoColumn.MemberInfoData) && value != null)
            {
                return value.ToString();
            }

            return this._dbType.ProcessDefaultMappings(pocoColumn, value);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data.Common;
using Frapid.NPoco.FluentMappings;

namespace Frapid.NPoco
{
    public class DatabaseFactory
    {
        public static IColumnSerializer ColumnSerializer = new JsonNetColumnSerializer();

        private DatabaseFactoryConfigOptions _options;

        public DatabaseFactory() { }

        public DatabaseFactory(DatabaseFactoryConfigOptions options)
        {
            this._options = options;
        }

        public DatabaseFactoryConfig Config()
        {
            this._options = new DatabaseFactoryConfigOptions();
            return new DatabaseFactoryConfig(this._options);
        }

        public static DatabaseFactory Config(Action<DatabaseFactoryConfig> optionsAction)
        {
            DatabaseFactoryConfigOptions options = new DatabaseFactoryConfigOptions();
            DatabaseFactoryConfig databaseFactoryConfig = new DatabaseFactoryConfig(options);
            optionsAction(databaseFactoryConfig);
            DatabaseFactory dbFactory = new DatabaseFactory(options);
            return dbFactory;
        }

        public Database Build(Database database)
        {
            this.ConfigureMappers(database);
            this.ConfigurePocoDataFactory(database);
            this.ConfigureInterceptors(database);
            return database;
        }

        private void ConfigureInterceptors(Database database)
        {
            database.Interceptors.AddRange(this._options.Interceptors);
        }

        private void ConfigurePocoDataFactory(Database database)
        {
            if (this._options.PocoDataFactory != null)
                database.PocoDataFactory = this._options.PocoDataFactory.Config(database.Mappers);
        }

        private void ConfigureMappers(Database database)
        {
            database.Mappers.InsertRange(0, this._options.Mapper);

            foreach (KeyValuePair<Type, MapperCollection.ObjectFactoryDelegate> factory in this._options.Mapper.Factories)
            {
                database.Mappers.Factories[factory.Key] = factory.Value;
            }
        }

        public IPocoDataFactory GetPocoDataFactory()
        {
            if (this._options.PocoDataFactory != null)
            {
                return this._options.PocoDataFactory.Config(this._options.Mapper);
            }
            throw new Exception("No PocoDataFactory configured");
        }

        public Database GetDatabase()
        {
            if (this._options.Database == null)
                throw new NullReferenceException("Database cannot be null. Use UsingDatabase()");

            Database db = this._options.Database();
            this.Build(db);
            return db;
        }
    }

    public class DatabaseFactoryConfigOptions
    {
        public DatabaseFactoryConfigOptions()
        {
            this.Mapper = new MapperCollection();
            this.Interceptors = new List<IInterceptor>();
        }

        public Func<Database> Database { get; set; }
        public MapperCollection Mapper { get; private set; }
        public FluentConfig PocoDataFactory { get; set; }
        public List<IInterceptor> Interceptors { get; private set; }
    }

    public class DatabaseFactoryConfig
    {
        private readonly DatabaseFactoryConfigOptions _options;

        public DatabaseFactoryConfig(DatabaseFactoryConfigOptions options)
        {
            this._options = options;
        }

        public DatabaseFactoryConfig UsingDatabase(Func<Database> database)
        {
            this._options.Database = database;
            return this;
        }

        public DatabaseFactoryConfig WithMapper(IMapper mapper)
        {
            this._options.Mapper.Add(mapper);
            return this;
        }

        public DatabaseFactoryConfig WithFluentConfig(FluentConfig fluentConfig)
        {
            this._options.PocoDataFactory = fluentConfig;
            return this;
        }

        public DatabaseFactoryConfig WithMapperFactory<T>(Func<DbDataReader, T> factory)
        {
            this._options.Mapper.RegisterFactory(factory);
            return this;
        }

        public DatabaseFactoryConfig WithInterceptor(IInterceptor interceptor)
        {
            this._options.Interceptors.Add(interceptor);
            return this;
        }
    }
}

using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Frapid.NPoco;
using Frapid.NPoco.FluentMappings;
using Serilog;

namespace Frapid.Configuration.Db
{
    public static class DbProvider
    {
        public static FluentConfig Config;

        public static void Setup(Type type)
        {
            try
            {
                var items = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(s => s.GetTypes())
                    .Where(type.IsAssignableFrom).Select(t => t.Assembly);


                var fluentConfig = FluentMappingConfiguration.Scan(s =>
                {
                    foreach (var item in items)
                    {
                        s.Assembly(item);
                        s.WithSmartConventions();
                        s.TablesNamed(
                            t =>
                                t.GetCustomAttributes(true).OfType<TableNameAttribute>().FirstOrDefault()?.Value ??
                                Inflector.AddUnderscores(Inflector.MakePlural(t.Name)).ToLower());

                        s.Columns.Named(m => Inflector.AddUnderscores(m.Name).ToLower());
                        s.Columns.IgnoreWhere(x => x.GetCustomAttributes<IgnoreAttribute>().Any());
                        s.PrimaryKeysNamed(t => Inflector.AddUnderscores(t.Name).ToLower() + "_id");
                        s.PrimaryKeysAutoIncremented(t => true);
                    }
                });


                Config = fluentConfig;
            }
            catch (ReflectionTypeLoadException ex)
            {
                Log.Error("Error in fluent configuration. {ex}", ex);
                //Swallow
            }
        }

        public static string GetProviderName(string tenant)
        {
            if (string.IsNullOrWhiteSpace(tenant))
            {
                return string.Empty;
            }

            var site = DbConvention.GetSite(tenant);
            return site.DbProvider;
        }

        public static string GetDbConfigurationFilePath(string tenant)
        {
            string provider = GetProviderName(tenant);
            string path = "/Resources/Configs/PostgreSQL.config";

            if (!provider.ToUpperInvariant().Equals("NPGSQL"))
            {
                path = "/Resources/Configs/SQLServer.config";
            }

            return path;
        }

        public static string GetMetaDatabase(string tenant)
        {
            if (string.IsNullOrWhiteSpace(tenant))
            {
                return string.Empty;
            }

            string provider = GetProviderName(tenant);
            string path = GetDbConfigurationFilePath(tenant);
            string meta = "postgres";

            if (!provider.ToUpperInvariant().Equals("NPGSQL"))
            {
                meta = "master";
            }

            path = PathMapper.MapPath(path);

            if (File.Exists(path))
            {
                meta = ConfigurationManager.ReadConfigurationValue(path, "MetaDatabase");
            }
            else
            {
                Log.Warning(
                    $"The meta database for provider '{provider}' could not be determined because the configuration file '{path}' does not exist. Returned value '{meta}' by convention.");
            }

            return meta;
        }


        public static DatabaseFactory Get(string connectionString, string tenant)
        {
            return DatabaseFactory.Config(x =>
            {
                x.WithMapper(new Mapper());
                x.UsingDatabase(() => new Database(connectionString, GetProviderName(tenant)));
                x.WithFluentConfig(Config);
            });
        }
    }
}
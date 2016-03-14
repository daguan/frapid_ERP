using System;
using System.Linq;
using System.Reflection;
using Frapid.NPoco;
using Frapid.NPoco.FluentMappings;

namespace Frapid.Configuration
{
    public static class DbProvider
    {
        public static FluentConfig Config;
        public static readonly string ProviderName = ConfigurationManager.GetConfigurationValue("DbServerConfigFileLocation", "ProviderName");
        public static readonly string MetaDatabase = ConfigurationManager.GetConfigurationValue("DbServerConfigFileLocation", "MetaDatabase");

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
                        s.Columns.IgnoreWhere(x=> x.GetCustomAttributes<IgnoreAttribute>().Any());
                        s.PrimaryKeysNamed(t => Inflector.AddUnderscores(t.Name).ToLower() + "_id");
                        s.PrimaryKeysAutoIncremented(t => true);
                    }
                });


                Config = fluentConfig;
            }
            catch (ReflectionTypeLoadException)
            {
                //Swallow
            }
        }


        public static DatabaseFactory Get(string connectionString)
        {
            return DatabaseFactory.Config(x =>
            {
                x.UsingDatabase(() => new Database(connectionString, ProviderName));
                x.WithFluentConfig(Config);
            });
        }
    }
}
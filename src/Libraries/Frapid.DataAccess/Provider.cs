using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NPoco;
using NPoco.FluentMappings;

namespace Frapid.DataAccess
{
    public static class Provider
    {
        public static FluentConfig Config;

        public static void Setup()
        {
            try
            {
                Type type = typeof(IPoco);

                IEnumerable<Assembly> items = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(s => s.GetTypes())
                    .Where(p => type.IsAssignableFrom(p)).Select(t => t.Assembly);


                FluentConfig fluentConfig = FluentMappingConfiguration.Scan(s =>
                {
                    foreach (Assembly item in items)
                    {
                        s.Assembly(item);
                        s.WithSmartConventions();
                        s.TablesNamed(t => Inflector.AddUnderscores(Inflector.MakePlural(t.Name)).ToLower());
                        s.Columns.Named(m => Inflector.AddUnderscores(m.Name).ToLower());
                        s.PrimaryKeysNamed(t => Inflector.AddUnderscores(t.Name).ToLower() + "_id");
                        s.PrimaryKeysAutoIncremented(t => true);
                    }
                });

                Config = fluentConfig;
            }
            catch (ReflectionTypeLoadException)
            {
            }
        }

        public static DatabaseFactory Get(string connectionString)
        {
            return DatabaseFactory.Config(x =>
            {
                x.UsingDatabase(() => new Database(connectionString, "Npgsql"));
                x.WithFluentConfig(Config);
            });
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Frapid.NPoco;
using Frapid.NPoco.FluentMappings;

namespace Frapid.Configuration
{
    public static class DbProvider
    {
        public static FluentConfig Config;

        public static void Setup(Type type)
        {
            try
            {
                IEnumerable<Assembly> items = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(s => s.GetTypes())
                    .Where(type.IsAssignableFrom).Select(t => t.Assembly);


                FluentConfig fluentConfig = FluentMappingConfiguration.Scan(s =>
                {
                    foreach (Assembly item in items)
                    {
                        s.Assembly(item);
                        s.WithSmartConventions();
                        s.TablesNamed(t => t.GetCustomAttributes(true).OfType<TableNameAttribute>().FirstOrDefault()?.Value??Inflector.AddUnderscores(Inflector.MakePlural(t.Name)).ToLower());
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
using System.Collections.Generic;
using System.Threading.Tasks;
using Frapid.Configuration;

namespace Frapid.DataAccess.Models
{
    public sealed class EntityView
    {
        public string PrimaryKey { get; set; }
        public IEnumerable<EntityColumn> Columns { get; set; }
        public object PrimaryKeyValue { get; set; }

        public static async Task<EntityView> GetAsync(string database, string primaryKey, string schemaName,
            string tableName)
        {
            var db = FrapidDbServer.GetServer(database);

            string sql = @"SELECT 
                    column_name, 
                    nullable,
                    udt_name as db_data_type,
                    column_default as value,
                    max_length,
                    primary_key,
                    data_type
                FROM public.poco_get_table_function_definition(@0::text, @1::text);";

            if (!db.ProviderName.ToUpperInvariant().Equals("NPGSQL"))
            {
                string procedure = FrapidDbServer.DefaultSchemaQualify(database, "poco_get_table_function_definition");
                sql = db.GetProcedureCommand(procedure, new[] {"@0", "@1"});
            }

            var columns =
                await Factory.GetAsync<EntityColumn>(database, sql, schemaName, tableName).ConfigureAwait(false);

            var meta = new EntityView
            {
                PrimaryKey = primaryKey,
                Columns = columns
            };

            return meta;
        }
    }
}
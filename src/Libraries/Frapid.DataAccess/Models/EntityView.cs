using System.Collections.Generic;
using System.Linq;

namespace Frapid.DataAccess.Models
{
    public class EntityView
    {
        public string PrimaryKey { get; set; }
        public List<EntityColumn> Columns { get; set; }
        public object PrimaryKeyValue { get; set; }

        public static EntityView Get(string database, string primaryKey, string schemaName, string tableName)
        {
            const string sql =
                @"SELECT 
                    column_name, 
                    is_nullable = 'YES' AS is_nullable, 
                    udt_name as db_data_type,
                    column_default as value,
                    max_length,
                    is_primary_key = 'YES' AS is_primary_key,
                    data_type
                FROM public.poco_get_table_function_definition(@0::text, @1::text);";

            var columns = Factory.Get<EntityColumn>(database, sql, schemaName, tableName).ToList();

            var meta = new EntityView
            {
                PrimaryKey = primaryKey,
                Columns = columns
            };

            return meta;
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Frapid.Configuration;

namespace Frapid.DataAccess.Models
{
    public sealed class EntityView
    {
        public string PrimaryKey { get; set; }
        public IEnumerable<EntityColumn> Columns { get; set; }
        public object PrimaryKeyValue { get; set; }

        public static async Task<EntityView> GetAsync(string database, string primaryKey, string schemaName, string tableName)
        {
            string procedure = FrapidDbServer.DefaultSchemaQualify(database, "poco_get_table_function_definition");
            string sql = $"SELECT * FROM {procedure}(@0, @1)";

            var columns = (await Factory.GetAsync<EntityColumn>(database, sql, schemaName, tableName).ConfigureAwait(false)).ToList();

            var candidate = columns.FirstOrDefault(x => x.PrimaryKey.ToUpperInvariant().StartsWith("Y"));

            if (candidate != null)
            {
                primaryKey = candidate.ColumnName;
            }

            var meta = new EntityView
            {
                PrimaryKey = primaryKey,
                Columns = columns
            };

            return meta;
        }
    }
}
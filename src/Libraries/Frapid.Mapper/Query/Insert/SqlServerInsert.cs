using System.Collections.Generic;
using System.Linq;
using Frapid.Mapper.Extensions;
using Frapid.Mapper.Helpers;

namespace Frapid.Mapper.Query.Insert
{
    public sealed class SqlServerInsert : InsertOperation
    {
        protected override Sql GetSql<T>(string tableName, string primaryKeyName, bool autoincrement, T poco)
        {
            var dictionary = poco.AsDictionary();
            var ignored = poco.GetIgnoredColumns();

            List<string> columns;

            if (autoincrement)
            {
                columns = dictionary.Keys.Where(x => x.ToUnderscoreLowerCase() != primaryKeyName)
                    .Where(x => !ignored.Contains(x))
                    .Select(key => $"\"{key.ToUnderscoreLowerCase()}\"").ToList();
            }
            else
            {
                columns = dictionary.Keys
                    .Where(x => !ignored.Contains(x))
                    .Select(key => $"\"{key.ToUnderscoreLowerCase()}\"").ToList();
            }

            var sql = new Sql();

            if (!string.IsNullOrWhiteSpace(primaryKeyName))
            {
                string output = "DECLARE @OUTPUT TABLE(id national character varying(1000));";
                sql.Append(output);
            }

            sql.Append($"INSERT INTO {tableName} ({string.Join(",", columns)})");
            sql.Append(!string.IsNullOrWhiteSpace(primaryKeyName) ? $"OUTPUT CAST(INSERTED.\"{primaryKeyName}\" AS national character varying(1000)) INTO @OUTPUT" : "");
            sql.Append($"SELECT {string.Join(",", Enumerable.Range(0, columns.Count).Select(x => "@" + x))};");

            if (!string.IsNullOrWhiteSpace(primaryKeyName))
            {
                sql.Append("SELECT * FROM @OUTPUT;");
            }
            
            List<object> values;

            if (autoincrement)
            {
                values = dictionary.Where(x => x.Key.ToUnderscoreLowerCase() != primaryKeyName)
                    .Where(x => !ignored.Contains(x.Key))
                    .Select(x => x.Value).ToList();
            }
            else
            {
                values = dictionary.Values
                    .Where(x => !ignored.Contains(x))
                    .Select(x => x).ToList();
            }

            sql.AppendParameters(values);

            return sql;
        }

    }
}
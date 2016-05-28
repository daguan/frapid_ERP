using System.Data.Common;
using System.Threading.Tasks;

namespace Frapid.NPoco.DatabaseTypes
{
    public class PostgreSQLDatabaseType : DatabaseType
    {
        public override object MapParameterValue(object value)
        {
            // Don't map bools to ints in PostgreSQL
            if (value is bool) return value;

            return base.MapParameterValue(value);
        }

        //Todo:The following changes were done to NPoco
        #region "Changes"
        public override async Task<object> ExecuteInsertAsync<T>(Database db, DbCommand cmd, string primaryKeyName, bool useOutputClause, T poco, object[] args)
        {
            if (primaryKeyName != null)
            {
                cmd.CommandText += $" returning {this.EscapeSqlIdentifier(primaryKeyName)} as NewID";
                return await db.ExecuteScalarHelperAsync(cmd);
            }

            await db.ExecuteNonQueryHelperAsync(cmd);
            return -1;
        }
        #endregion

        public override string EscapeSqlIdentifier(string str)
        {
            return $"\"{str}\"";
        }

        public override object ExecuteInsert<T>(Database db, DbCommand cmd, string primaryKeyName, bool useOutputClause, T poco, object[] args)
        {
            if (primaryKeyName != null)
            {
                cmd.CommandText += $" returning {this.EscapeSqlIdentifier(primaryKeyName)} as NewID";
                return db.ExecuteScalarHelper(cmd);
            }

            db.ExecuteNonQueryHelper(cmd);
            return -1;
        }

        public override string GetProviderName()
        {
            return "Npgsql2";
        }
    }
}
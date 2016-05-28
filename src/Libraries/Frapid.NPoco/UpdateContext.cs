using System.Collections.Generic;

namespace Frapid.NPoco
{
    public class UpdateContext
    {
        public UpdateContext(object poco, string tableName, string primaryKeyName, object primaryKeyValue, IEnumerable<string> columnsToUpdate)
        {
            this.Poco = poco;
            this.TableName = tableName;
            this.PrimaryKeyName = primaryKeyName;
            this.PrimaryKeyValue = primaryKeyValue;
            this.ColumnsToUpdate = columnsToUpdate;
        }

        public object Poco { get; private set; }
        public string TableName { get; private set; }
        public string PrimaryKeyName { get; private set; }
        public object PrimaryKeyValue { get; private set; }
        public IEnumerable<string> ColumnsToUpdate { get; private set; }
    }
}
namespace Frapid.NPoco
{
    public class DeleteContext
    {
        public DeleteContext(object poco, string tableName, string primaryKeyName, object primaryKeyValue)
        {
            this.Poco = poco;
            this.TableName = tableName;
            this.PrimaryKeyName = primaryKeyName;
            this.PrimaryKeyValue = primaryKeyValue;
        }

        public object Poco { get; private set; }
        public string TableName { get; private set; }
        public string PrimaryKeyName { get; private set; }
        public object PrimaryKeyValue { get; private set; }
    }
}
using Frapid.DataAccess;
using Frapid.NPoco;

namespace Frapid.Authorization.DTO
{
    [TableName("auth.entity_view")]
    public class EntityView : IPoco
    {
        public string TableSchema { get; set; }
        public string TableName { get; set; }
        public string ObjectName { get; set; }
        public string TableType { get; set; }
    }
}
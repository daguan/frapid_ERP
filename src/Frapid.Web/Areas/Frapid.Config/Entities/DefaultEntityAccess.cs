// ReSharper disable All
using System;
using Frapid.DataAccess;
using Frapid.NPoco;

namespace Frapid.Config.Entities
{
    [TableName("config.default_entity_access")]
    [PrimaryKey("default_entity_access_id", AutoIncrement = true)]
    public sealed class DefaultEntityAccess : IPoco
    {
        public int DefaultEntityAccessId { get; set; }
        public string EntityName { get; set; }
        public int RoleId { get; set; }
        public int? AccessTypeId { get; set; }
        public bool AllowAccess { get; set; }
        public int? AuditUserId { get; set; }
        public DateTime? AuditTs { get; set; }
    }
}
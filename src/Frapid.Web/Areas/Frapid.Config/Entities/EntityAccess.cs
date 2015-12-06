// ReSharper disable All
using System;
using Frapid.DataAccess;
using Frapid.NPoco;

namespace Frapid.Config.Entities
{
    [TableName("config.entity_access")]
    [PrimaryKey("entity_access_id", AutoIncrement = true)]
    public sealed class EntityAccess : IPoco
    {
        public int EntityAccessId { get; set; }
        public string EntityName { get; set; }
        public int UserId { get; set; }
        public int? AccessTypeId { get; set; }
        public bool AllowAccess { get; set; }
        public int? AuditUserId { get; set; }
        public DateTime? AuditTs { get; set; }
    }
}
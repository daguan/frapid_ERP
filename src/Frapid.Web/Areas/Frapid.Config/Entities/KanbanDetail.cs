// ReSharper disable All
using System;
using Frapid.DataAccess;
using Frapid.NPoco;

namespace Frapid.Config.Entities
{
    [TableName("config.kanban_details")]
    [PrimaryKey("kanban_detail_id", AutoIncrement = true)]
    public sealed class KanbanDetail : IPoco
    {
        public long KanbanDetailId { get; set; }
        public long KanbanId { get; set; }
        public short? Rating { get; set; }
        public string ResourceId { get; set; }
        public int? AuditUserId { get; set; }
        public DateTime? AuditTs { get; set; }
    }
}
// ReSharper disable All
using System;
using Frapid.DataAccess;
using Frapid.NPoco;

namespace Frapid.Config.Entities
{
    [TableName("config.kanbans")]
    [PrimaryKey("kanban_id", AutoIncrement = true)]
    public sealed class Kanban : IPoco
    {
        public long KanbanId { get; set; }
        public string ObjectName { get; set; }
        public int? UserId { get; set; }
        public string KanbanName { get; set; }
        public string Description { get; set; }
        public int? AuditUserId { get; set; }
        public DateTime? AuditTs { get; set; }
    }
}
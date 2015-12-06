// ReSharper disable All
using System;
using Frapid.DataAccess;
using Frapid.NPoco;

namespace Frapid.Config.Entities
{
    [TableName("config.flag_types")]
    [PrimaryKey("flag_type_id", AutoIncrement = true)]
    public sealed class FlagType : IPoco
    {
        public int FlagTypeId { get; set; }
        public string FlagTypeName { get; set; }
        public string BackgroundColor { get; set; }
        public string ForegroundColor { get; set; }
        public int? AuditUserId { get; set; }
        public DateTime? AuditTs { get; set; }
    }
}
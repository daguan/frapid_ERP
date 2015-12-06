// ReSharper disable All
using System;
using Frapid.DataAccess;
using Frapid.NPoco;

namespace Frapid.Config.Entities
{
    [TableName("config.flag_view")]
    [PrimaryKey("", AutoIncrement = false)]
    public sealed class FlagView : IPoco
    {
        public long? FlagId { get; set; }
        public int? UserId { get; set; }
        public int? FlagTypeId { get; set; }
        public string ResourceId { get; set; }
        public string Resource { get; set; }
        public string ResourceKey { get; set; }
        public DateTime? FlaggedOn { get; set; }
        public string FlagTypeName { get; set; }
        public string BackgroundColor { get; set; }
        public string ForegroundColor { get; set; }
    }
}
// ReSharper disable All
using System;
using Frapid.DataAccess;
using Frapid.NPoco;

namespace Frapid.Config.Entities
{
    [TableName("config.flags")]
    [PrimaryKey("flag_id", AutoIncrement = true)]
    public sealed class Flag : IPoco
    {
        public long FlagId { get; set; }
        public int UserId { get; set; }
        public int FlagTypeId { get; set; }
        public string Resource { get; set; }
        public string ResourceKey { get; set; }
        public string ResourceId { get; set; }
        public DateTime? FlaggedOn { get; set; }
    }
}
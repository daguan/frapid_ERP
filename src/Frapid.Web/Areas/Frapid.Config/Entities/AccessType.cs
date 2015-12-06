// ReSharper disable All
using System;
using Frapid.DataAccess;
using Frapid.NPoco;

namespace Frapid.Config.Entities
{
    [TableName("config.access_types")]
    [PrimaryKey("access_type_id", AutoIncrement = false)]
    public sealed class AccessType : IPoco
    {
        public int AccessTypeId { get; set; }
        public string AccessTypeName { get; set; }
    }
}
// ReSharper disable All
using System;
using Frapid.DataAccess;
using Frapid.NPoco;

namespace Frapid.Config.Entities
{
    [TableName("config.custom_fields")]
    [PrimaryKey("custom_field_id", AutoIncrement = true)]
    public sealed class CustomField : IPoco
    {
        public long CustomFieldId { get; set; }
        public int CustomFieldSetupId { get; set; }
        public string ResourceId { get; set; }
        public string Value { get; set; }
    }
}
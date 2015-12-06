// ReSharper disable All
using System;
using Frapid.DataAccess;
using Frapid.NPoco;

namespace Frapid.Config.Entities
{
    [TableName("config.custom_field_setup")]
    [PrimaryKey("custom_field_setup_id", AutoIncrement = true)]
    public sealed class CustomFieldSetup : IPoco
    {
        public int CustomFieldSetupId { get; set; }
        public string FormName { get; set; }
        public int FieldOrder { get; set; }
        public string FieldName { get; set; }
        public string FieldLabel { get; set; }
        public string DataType { get; set; }
        public string Description { get; set; }
    }
}
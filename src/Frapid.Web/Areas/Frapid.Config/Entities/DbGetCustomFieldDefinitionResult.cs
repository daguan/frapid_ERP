// ReSharper disable All
using System;
using Frapid.DataAccess;
using Frapid.NPoco;

namespace Frapid.Config.Entities
{
    [TableName("config.get_custom_field_definition")]
    [PrimaryKey("", AutoIncrement = false)]
    public sealed class DbGetCustomFieldDefinitionResult : IPoco
    {
        public string TableName { get; set; }
        public string KeyName { get; set; }
        public int CustomFieldSetupId { get; set; }
        public string FormName { get; set; }
        public int FieldOrder { get; set; }
        public string FieldName { get; set; }
        public string FieldLabel { get; set; }
        public string Description { get; set; }
        public string DataType { get; set; }
        public bool IsNumber { get; set; }
        public bool IsDate { get; set; }
        public bool IsBoolean { get; set; }
        public bool IsLongText { get; set; }
        public string ResourceId { get; set; }
        public string Value { get; set; }
    }
}
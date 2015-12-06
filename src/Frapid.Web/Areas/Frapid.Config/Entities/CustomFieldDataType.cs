// ReSharper disable All
using System;
using Frapid.DataAccess;
using Frapid.NPoco;

namespace Frapid.Config.Entities
{
    [TableName("config.custom_field_data_types")]
    [PrimaryKey("data_type", AutoIncrement = false)]
    public sealed class CustomFieldDataType : IPoco
    {
        public string DataType { get; set; }
        public bool? IsNumber { get; set; }
        public bool? IsDate { get; set; }
        public bool? IsBoolean { get; set; }
        public bool? IsLongText { get; set; }
    }
}
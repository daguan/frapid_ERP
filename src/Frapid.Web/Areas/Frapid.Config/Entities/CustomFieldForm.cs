// ReSharper disable All
using System;
using Frapid.DataAccess;
using Frapid.NPoco;

namespace Frapid.Config.Entities
{
    [TableName("config.custom_field_forms")]
    [PrimaryKey("form_name", AutoIncrement = false)]
    public sealed class CustomFieldForm : IPoco
    {
        public string FormName { get; set; }
        public string TableName { get; set; }
        public string KeyName { get; set; }
    }
}
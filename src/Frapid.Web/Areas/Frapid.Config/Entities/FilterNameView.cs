// ReSharper disable All
using System;
using Frapid.DataAccess;
using Frapid.NPoco;

namespace Frapid.Config.Entities
{
    [TableName("config.filter_name_view")]
    [PrimaryKey("", AutoIncrement = false)]
    public sealed class FilterNameView : IPoco
    {
        public string ObjectName { get; set; }
        public string FilterName { get; set; }
        public bool? IsDefault { get; set; }
    }
}
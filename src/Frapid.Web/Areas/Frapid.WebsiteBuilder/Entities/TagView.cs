// ReSharper disable All
using System;
using Frapid.DataAccess;
using Frapid.NPoco;

namespace Frapid.WebsiteBuilder.Entities
{
    [TableName("website.tag_view")]
    [PrimaryKey("", AutoIncrement = false)]
    public sealed class TagView : IPoco
    {
        public string Tag { get; set; }
    }
}
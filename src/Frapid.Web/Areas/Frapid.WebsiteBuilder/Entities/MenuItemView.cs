// ReSharper disable All
using System;
using Frapid.DataAccess;
using Frapid.NPoco;

namespace Frapid.WebsiteBuilder.Entities
{
    [TableName("website.menu_item_view")]
    [PrimaryKey("", AutoIncrement = false)]
    public sealed class MenuItemView : IPoco
    {
        public int? MenuId { get; set; }
        public string MenuName { get; set; }
        public string Description { get; set; }
        public int? MenuItemId { get; set; }
        public int? Sort { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public int? ContentId { get; set; }
        public string ContentAlias { get; set; }
    }
}
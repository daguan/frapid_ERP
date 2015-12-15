// ReSharper disable All
using System;
using Frapid.DataAccess;
using Frapid.NPoco;

namespace Frapid.WebsiteBuilder.Entities
{
    [TableName("website.menu_items")]
    [PrimaryKey("menu_item_id", AutoIncrement = true)]
    public sealed class MenuItem : IPoco
    {
        public int MenuItemId { get; set; }
        public int? MenuId { get; set; }
        public int Sort { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public int? ContentId { get; set; }
        public int? AuditUserId { get; set; }
        public DateTime? AuditTs { get; set; }
    }
}
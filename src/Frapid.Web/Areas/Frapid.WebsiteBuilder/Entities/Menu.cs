// ReSharper disable All
using System;
using Frapid.DataAccess;
using Frapid.NPoco;

namespace Frapid.WebsiteBuilder.Entities
{
    [TableName("website.menus")]
    [PrimaryKey("menu_id", AutoIncrement = true)]
    public sealed class Menu : IPoco
    {
        public int MenuId { get; set; }
        public string MenuName { get; set; }
        public string Description { get; set; }
        public int? AuditUserId { get; set; }
        public DateTime? AuditTs { get; set; }
    }
}
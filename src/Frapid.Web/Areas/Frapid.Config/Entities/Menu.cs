// ReSharper disable All
using System;
using Frapid.DataAccess;
using Frapid.NPoco;

namespace Frapid.Config.Entities
{
    [TableName("config.menus")]
    [PrimaryKey("menu_id", AutoIncrement = true)]
    public sealed class Menu : IPoco
    {
        public int MenuId { get; set; }
        public int? Sort { get; set; }
        public string AppName { get; set; }
        public string MenuName { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
        public int? ParentMenuId { get; set; }
    }
}
// ReSharper disable All
using System;
using Frapid.DataAccess;
using Frapid.NPoco;

namespace Frapid.Core.Entities
{
    [TableName("core.menu_locale")]
    [PrimaryKey("menu_locale_id", AutoIncrement = true)]
    public sealed class MenuLocale : IPoco
    {
        public int MenuLocaleId { get; set; }
        public int MenuId { get; set; }
        public string Culture { get; set; }
        public string MenuText { get; set; }
    }
}
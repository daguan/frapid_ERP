using System.Collections.Generic;

namespace WebsiteBuilder.Models
{
    public sealed class Menu
    {
        public int MenuId { get; set; }
        public int MenuGroupId { get; set; }
        public int Sort { get; set; }
        public string MenuName { get; set; }
        public string MenuAlias { get; set; }
        public string Url { get; set; }
        public int? ParentMenuId { get; set; }

        public IEnumerable<Menu> Get(string group)
        {
            return DAL.Menu.Get(group);
        }
    }
}
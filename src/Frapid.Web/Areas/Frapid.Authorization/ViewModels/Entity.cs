using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Frapid.ApplicationState.Cache;
using Frapid.Authorization.DAL;

namespace Frapid.Authorization.ViewModels
{
    public class Entity
    {
        public string Name { get; set; }
        public string ObjectId { get; set; }

        public static IEnumerable<Entity> GetEntities()
        {
            string tenant = AppUsers.GetTenant();
            var data = Entities.Get(tenant);
            var english = new CultureInfo("en-US", false).TextInfo;

            var entities = data.Select(item => new Entity
            {
                ObjectId = item.ObjectName, Name = english.ToTitleCase(item.ObjectName.Replace(".", ": ").Replace("_", " "))
            }).OrderBy(x=>x.ObjectId).ToList();

            entities.Insert(0, new Entity
            {
                ObjectId = "",
                Name = "All Objects"
            });

            return entities;
        }
    }
}
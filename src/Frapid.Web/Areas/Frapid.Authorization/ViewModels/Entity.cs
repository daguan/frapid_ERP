using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Frapid.Authorization.DAL;

namespace Frapid.Authorization.ViewModels
{
    public class Entity
    {
        public string Name { get; set; }
        public string ObjectId { get; set; }

        public static IEnumerable<Entity> GetEntities()
        {
            var data = Entities.Get();
            var english = new CultureInfo("en-US", false).TextInfo;

            return data.Select(item => new Entity
            {
                ObjectId = item.ObjectName, Name = english.ToTitleCase(item.ObjectName.Replace(".", ": ").Replace("_", " "))
            }).OrderBy(x=>x.ObjectId).ToList();
        }
    }
}
// ReSharper disable All
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Frapid.WebsiteBuilder.DataAccess;
using Frapid.DataAccess;
using Frapid.DataAccess.Models;
using CustomField = Frapid.DataAccess.Models.CustomField;

namespace Frapid.WebsiteBuilder.Api.Fakes
{
    public class MenuItemRepository : IMenuItemRepository
    {
        public long Count()
        {
            return 1;
        }

        public IEnumerable<Frapid.WebsiteBuilder.Entities.MenuItem> GetAll()
        {
            return Enumerable.Repeat(new Frapid.WebsiteBuilder.Entities.MenuItem(), 1);
        }

        public IEnumerable<dynamic> Export()
        {
            return Enumerable.Repeat(new Frapid.WebsiteBuilder.Entities.MenuItem(), 1);
        }

        public Frapid.WebsiteBuilder.Entities.MenuItem Get(int menuItemId)
        {
            return new Frapid.WebsiteBuilder.Entities.MenuItem();
        }

        public IEnumerable<Frapid.WebsiteBuilder.Entities.MenuItem> Get(int[] menuItemIds)
        {
            return Enumerable.Repeat(new Frapid.WebsiteBuilder.Entities.MenuItem(), 1);
        }

        public Frapid.WebsiteBuilder.Entities.MenuItem GetFirst()
        {
            return new Frapid.WebsiteBuilder.Entities.MenuItem();
        }

        public Frapid.WebsiteBuilder.Entities.MenuItem GetPrevious(int menuItemId)
        {
            return new Frapid.WebsiteBuilder.Entities.MenuItem();
        }

        public Frapid.WebsiteBuilder.Entities.MenuItem GetNext(int menuItemId)
        {
            return new Frapid.WebsiteBuilder.Entities.MenuItem();
        }

        public Frapid.WebsiteBuilder.Entities.MenuItem GetLast()
        {
            return new Frapid.WebsiteBuilder.Entities.MenuItem();
        }

        public IEnumerable<Frapid.WebsiteBuilder.Entities.MenuItem> GetPaginatedResult()
        {
            return Enumerable.Repeat(new Frapid.WebsiteBuilder.Entities.MenuItem(), 1);
        }

        public IEnumerable<Frapid.WebsiteBuilder.Entities.MenuItem> GetPaginatedResult(long pageNumber)
        {
            return Enumerable.Repeat(new Frapid.WebsiteBuilder.Entities.MenuItem(), 1);
        }

        public long CountWhere(List<Frapid.DataAccess.Models.Filter> filters)
        {
            return 1;
        }

        public IEnumerable<Frapid.WebsiteBuilder.Entities.MenuItem> GetWhere(long pageNumber, List<Frapid.DataAccess.Models.Filter> filters)
        {
            return Enumerable.Repeat(new Frapid.WebsiteBuilder.Entities.MenuItem(), 1);
        }

        public long CountFiltered(string filterName)
        {
            return 1;
        }

        public List<Frapid.DataAccess.Models.Filter> GetFilters(string catalog, string filterName)
        {
            return Enumerable.Repeat(new Frapid.DataAccess.Models.Filter(), 1).ToList();
        }

        public IEnumerable<Frapid.WebsiteBuilder.Entities.MenuItem> GetFiltered(long pageNumber, string filterName)
        {
            return Enumerable.Repeat(new Frapid.WebsiteBuilder.Entities.MenuItem(), 1);
        }

        public IEnumerable<Frapid.DataAccess.Models.DisplayField> GetDisplayFields()
        {
            return Enumerable.Repeat(new DisplayField(), 1);
        }

        public IEnumerable<Frapid.DataAccess.Models.CustomField> GetCustomFields()
        {
            return Enumerable.Repeat(new CustomField(), 1);
        }

        public IEnumerable<Frapid.DataAccess.Models.CustomField> GetCustomFields(string resourceId)
        {
            return Enumerable.Repeat(new CustomField(), 1);
        }

        public object AddOrEdit(dynamic menuItem, List<Frapid.DataAccess.Models.CustomField> customFields)
        {
            return true;
        }

        public void Update(dynamic menuItem, int menuItemId)
        {
            if (menuItemId > 0)
            {
                return;
            }

            throw new ArgumentException("menuItemId is null.");
        }

        public object Add(dynamic menuItem)
        {
            return true;
        }

        public List<object> BulkImport(List<ExpandoObject> menuItems)
        {
            return Enumerable.Repeat(new object(), 1).ToList();
        }

        public void Delete(int menuItemId)
        {
            if (menuItemId > 0)
            {
                return;
            }

            throw new ArgumentException("menuItemId is null.");
        }


    }
}
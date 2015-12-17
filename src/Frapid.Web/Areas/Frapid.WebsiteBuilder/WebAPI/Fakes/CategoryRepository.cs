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
    public class CategoryRepository : ICategoryRepository
    {
        public long Count()
        {
            return 1;
        }

        public IEnumerable<Frapid.WebsiteBuilder.Entities.Category> GetAll()
        {
            return Enumerable.Repeat(new Frapid.WebsiteBuilder.Entities.Category(), 1);
        }

        public IEnumerable<dynamic> Export()
        {
            return Enumerable.Repeat(new Frapid.WebsiteBuilder.Entities.Category(), 1);
        }

        public Frapid.WebsiteBuilder.Entities.Category Get(int categoryId)
        {
            return new Frapid.WebsiteBuilder.Entities.Category();
        }

        public IEnumerable<Frapid.WebsiteBuilder.Entities.Category> Get(int[] categoryIds)
        {
            return Enumerable.Repeat(new Frapid.WebsiteBuilder.Entities.Category(), 1);
        }

        public Frapid.WebsiteBuilder.Entities.Category GetFirst()
        {
            return new Frapid.WebsiteBuilder.Entities.Category();
        }

        public Frapid.WebsiteBuilder.Entities.Category GetPrevious(int categoryId)
        {
            return new Frapid.WebsiteBuilder.Entities.Category();
        }

        public Frapid.WebsiteBuilder.Entities.Category GetNext(int categoryId)
        {
            return new Frapid.WebsiteBuilder.Entities.Category();
        }

        public Frapid.WebsiteBuilder.Entities.Category GetLast()
        {
            return new Frapid.WebsiteBuilder.Entities.Category();
        }

        public IEnumerable<Frapid.WebsiteBuilder.Entities.Category> GetPaginatedResult()
        {
            return Enumerable.Repeat(new Frapid.WebsiteBuilder.Entities.Category(), 1);
        }

        public IEnumerable<Frapid.WebsiteBuilder.Entities.Category> GetPaginatedResult(long pageNumber)
        {
            return Enumerable.Repeat(new Frapid.WebsiteBuilder.Entities.Category(), 1);
        }

        public long CountWhere(List<Frapid.DataAccess.Models.Filter> filters)
        {
            return 1;
        }

        public IEnumerable<Frapid.WebsiteBuilder.Entities.Category> GetWhere(long pageNumber, List<Frapid.DataAccess.Models.Filter> filters)
        {
            return Enumerable.Repeat(new Frapid.WebsiteBuilder.Entities.Category(), 1);
        }

        public long CountFiltered(string filterName)
        {
            return 1;
        }

        public List<Frapid.DataAccess.Models.Filter> GetFilters(string catalog, string filterName)
        {
            return Enumerable.Repeat(new Frapid.DataAccess.Models.Filter(), 1).ToList();
        }

        public IEnumerable<Frapid.WebsiteBuilder.Entities.Category> GetFiltered(long pageNumber, string filterName)
        {
            return Enumerable.Repeat(new Frapid.WebsiteBuilder.Entities.Category(), 1);
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

        public object AddOrEdit(dynamic category, List<Frapid.DataAccess.Models.CustomField> customFields)
        {
            return true;
        }

        public void Update(dynamic category, int categoryId)
        {
            if (categoryId > 0)
            {
                return;
            }

            throw new ArgumentException("categoryId is null.");
        }

        public object Add(dynamic category)
        {
            return true;
        }

        public List<object> BulkImport(List<ExpandoObject> categories)
        {
            return Enumerable.Repeat(new object(), 1).ToList();
        }

        public void Delete(int categoryId)
        {
            if (categoryId > 0)
            {
                return;
            }

            throw new ArgumentException("categoryId is null.");
        }


    }
}
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
    public class ContentRepository : IContentRepository
    {
        public long Count()
        {
            return 1;
        }

        public IEnumerable<Frapid.WebsiteBuilder.Entities.Content> GetAll()
        {
            return Enumerable.Repeat(new Frapid.WebsiteBuilder.Entities.Content(), 1);
        }

        public IEnumerable<dynamic> Export()
        {
            return Enumerable.Repeat(new Frapid.WebsiteBuilder.Entities.Content(), 1);
        }

        public Frapid.WebsiteBuilder.Entities.Content Get(int contentId)
        {
            return new Frapid.WebsiteBuilder.Entities.Content();
        }

        public IEnumerable<Frapid.WebsiteBuilder.Entities.Content> Get(int[] contentIds)
        {
            return Enumerable.Repeat(new Frapid.WebsiteBuilder.Entities.Content(), 1);
        }

        public Frapid.WebsiteBuilder.Entities.Content GetFirst()
        {
            return new Frapid.WebsiteBuilder.Entities.Content();
        }

        public Frapid.WebsiteBuilder.Entities.Content GetPrevious(int contentId)
        {
            return new Frapid.WebsiteBuilder.Entities.Content();
        }

        public Frapid.WebsiteBuilder.Entities.Content GetNext(int contentId)
        {
            return new Frapid.WebsiteBuilder.Entities.Content();
        }

        public Frapid.WebsiteBuilder.Entities.Content GetLast()
        {
            return new Frapid.WebsiteBuilder.Entities.Content();
        }

        public IEnumerable<Frapid.WebsiteBuilder.Entities.Content> GetPaginatedResult()
        {
            return Enumerable.Repeat(new Frapid.WebsiteBuilder.Entities.Content(), 1);
        }

        public IEnumerable<Frapid.WebsiteBuilder.Entities.Content> GetPaginatedResult(long pageNumber)
        {
            return Enumerable.Repeat(new Frapid.WebsiteBuilder.Entities.Content(), 1);
        }

        public long CountWhere(List<Frapid.DataAccess.Models.Filter> filters)
        {
            return 1;
        }

        public IEnumerable<Frapid.WebsiteBuilder.Entities.Content> GetWhere(long pageNumber, List<Frapid.DataAccess.Models.Filter> filters)
        {
            return Enumerable.Repeat(new Frapid.WebsiteBuilder.Entities.Content(), 1);
        }

        public long CountFiltered(string filterName)
        {
            return 1;
        }

        public List<Frapid.DataAccess.Models.Filter> GetFilters(string catalog, string filterName)
        {
            return Enumerable.Repeat(new Frapid.DataAccess.Models.Filter(), 1).ToList();
        }

        public IEnumerable<Frapid.WebsiteBuilder.Entities.Content> GetFiltered(long pageNumber, string filterName)
        {
            return Enumerable.Repeat(new Frapid.WebsiteBuilder.Entities.Content(), 1);
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

        public object AddOrEdit(dynamic content, List<Frapid.DataAccess.Models.CustomField> customFields)
        {
            return true;
        }

        public void Update(dynamic content, int contentId)
        {
            if (contentId > 0)
            {
                return;
            }

            throw new ArgumentException("contentId is null.");
        }

        public object Add(dynamic content)
        {
            return true;
        }

        public List<object> BulkImport(List<ExpandoObject> contents)
        {
            return Enumerable.Repeat(new object(), 1).ToList();
        }

        public void Delete(int contentId)
        {
            if (contentId > 0)
            {
                return;
            }

            throw new ArgumentException("contentId is null.");
        }


    }
}
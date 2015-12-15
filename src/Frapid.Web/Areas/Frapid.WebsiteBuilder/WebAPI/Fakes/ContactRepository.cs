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
    public class ContactRepository : IContactRepository
    {
        public long Count()
        {
            return 1;
        }

        public IEnumerable<Frapid.WebsiteBuilder.Entities.Contact> GetAll()
        {
            return Enumerable.Repeat(new Frapid.WebsiteBuilder.Entities.Contact(), 1);
        }

        public IEnumerable<dynamic> Export()
        {
            return Enumerable.Repeat(new Frapid.WebsiteBuilder.Entities.Contact(), 1);
        }

        public Frapid.WebsiteBuilder.Entities.Contact Get(int contactId)
        {
            return new Frapid.WebsiteBuilder.Entities.Contact();
        }

        public IEnumerable<Frapid.WebsiteBuilder.Entities.Contact> Get(int[] contactIds)
        {
            return Enumerable.Repeat(new Frapid.WebsiteBuilder.Entities.Contact(), 1);
        }

        public Frapid.WebsiteBuilder.Entities.Contact GetFirst()
        {
            return new Frapid.WebsiteBuilder.Entities.Contact();
        }

        public Frapid.WebsiteBuilder.Entities.Contact GetPrevious(int contactId)
        {
            return new Frapid.WebsiteBuilder.Entities.Contact();
        }

        public Frapid.WebsiteBuilder.Entities.Contact GetNext(int contactId)
        {
            return new Frapid.WebsiteBuilder.Entities.Contact();
        }

        public Frapid.WebsiteBuilder.Entities.Contact GetLast()
        {
            return new Frapid.WebsiteBuilder.Entities.Contact();
        }

        public IEnumerable<Frapid.WebsiteBuilder.Entities.Contact> GetPaginatedResult()
        {
            return Enumerable.Repeat(new Frapid.WebsiteBuilder.Entities.Contact(), 1);
        }

        public IEnumerable<Frapid.WebsiteBuilder.Entities.Contact> GetPaginatedResult(long pageNumber)
        {
            return Enumerable.Repeat(new Frapid.WebsiteBuilder.Entities.Contact(), 1);
        }

        public long CountWhere(List<Frapid.DataAccess.Models.Filter> filters)
        {
            return 1;
        }

        public IEnumerable<Frapid.WebsiteBuilder.Entities.Contact> GetWhere(long pageNumber, List<Frapid.DataAccess.Models.Filter> filters)
        {
            return Enumerable.Repeat(new Frapid.WebsiteBuilder.Entities.Contact(), 1);
        }

        public long CountFiltered(string filterName)
        {
            return 1;
        }

        public List<Frapid.DataAccess.Models.Filter> GetFilters(string catalog, string filterName)
        {
            return Enumerable.Repeat(new Frapid.DataAccess.Models.Filter(), 1).ToList();
        }

        public IEnumerable<Frapid.WebsiteBuilder.Entities.Contact> GetFiltered(long pageNumber, string filterName)
        {
            return Enumerable.Repeat(new Frapid.WebsiteBuilder.Entities.Contact(), 1);
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

        public object AddOrEdit(dynamic contact, List<Frapid.DataAccess.Models.CustomField> customFields)
        {
            return true;
        }

        public void Update(dynamic contact, int contactId)
        {
            if (contactId > 0)
            {
                return;
            }

            throw new ArgumentException("contactId is null.");
        }

        public object Add(dynamic contact)
        {
            return true;
        }

        public List<object> BulkImport(List<ExpandoObject> contacts)
        {
            return Enumerable.Repeat(new object(), 1).ToList();
        }

        public void Delete(int contactId)
        {
            if (contactId > 0)
            {
                return;
            }

            throw new ArgumentException("contactId is null.");
        }


    }
}
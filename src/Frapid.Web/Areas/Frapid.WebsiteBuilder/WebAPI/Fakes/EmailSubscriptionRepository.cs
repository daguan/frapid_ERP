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
    public class EmailSubscriptionRepository : IEmailSubscriptionRepository
    {
        public long Count()
        {
            return 1;
        }

        public IEnumerable<Frapid.WebsiteBuilder.Entities.EmailSubscription> GetAll()
        {
            return Enumerable.Repeat(new Frapid.WebsiteBuilder.Entities.EmailSubscription(), 1);
        }

        public IEnumerable<dynamic> Export()
        {
            return Enumerable.Repeat(new Frapid.WebsiteBuilder.Entities.EmailSubscription(), 1);
        }

        public Frapid.WebsiteBuilder.Entities.EmailSubscription Get(System.Guid emailSubscriptionId)
        {
            return new Frapid.WebsiteBuilder.Entities.EmailSubscription();
        }

        public IEnumerable<Frapid.WebsiteBuilder.Entities.EmailSubscription> Get(System.Guid[] emailSubscriptionIds)
        {
            return Enumerable.Repeat(new Frapid.WebsiteBuilder.Entities.EmailSubscription(), 1);
        }

        public Frapid.WebsiteBuilder.Entities.EmailSubscription GetFirst()
        {
            return new Frapid.WebsiteBuilder.Entities.EmailSubscription();
        }

        public Frapid.WebsiteBuilder.Entities.EmailSubscription GetPrevious(System.Guid emailSubscriptionId)
        {
            return new Frapid.WebsiteBuilder.Entities.EmailSubscription();
        }

        public Frapid.WebsiteBuilder.Entities.EmailSubscription GetNext(System.Guid emailSubscriptionId)
        {
            return new Frapid.WebsiteBuilder.Entities.EmailSubscription();
        }

        public Frapid.WebsiteBuilder.Entities.EmailSubscription GetLast()
        {
            return new Frapid.WebsiteBuilder.Entities.EmailSubscription();
        }

        public IEnumerable<Frapid.WebsiteBuilder.Entities.EmailSubscription> GetPaginatedResult()
        {
            return Enumerable.Repeat(new Frapid.WebsiteBuilder.Entities.EmailSubscription(), 1);
        }

        public IEnumerable<Frapid.WebsiteBuilder.Entities.EmailSubscription> GetPaginatedResult(long pageNumber)
        {
            return Enumerable.Repeat(new Frapid.WebsiteBuilder.Entities.EmailSubscription(), 1);
        }

        public long CountWhere(List<Frapid.DataAccess.Models.Filter> filters)
        {
            return 1;
        }

        public IEnumerable<Frapid.WebsiteBuilder.Entities.EmailSubscription> GetWhere(long pageNumber, List<Frapid.DataAccess.Models.Filter> filters)
        {
            return Enumerable.Repeat(new Frapid.WebsiteBuilder.Entities.EmailSubscription(), 1);
        }

        public long CountFiltered(string filterName)
        {
            return 1;
        }

        public List<Frapid.DataAccess.Models.Filter> GetFilters(string catalog, string filterName)
        {
            return Enumerable.Repeat(new Frapid.DataAccess.Models.Filter(), 1).ToList();
        }

        public IEnumerable<Frapid.WebsiteBuilder.Entities.EmailSubscription> GetFiltered(long pageNumber, string filterName)
        {
            return Enumerable.Repeat(new Frapid.WebsiteBuilder.Entities.EmailSubscription(), 1);
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

        public object AddOrEdit(dynamic emailSubscription, List<Frapid.DataAccess.Models.CustomField> customFields)
        {
            return true;
        }

        public void Update(dynamic emailSubscription, System.Guid emailSubscriptionId)
        {
            if (emailSubscriptionId != null)
            {
                return;
            }

            throw new ArgumentException("emailSubscriptionId is null.");
        }

        public object Add(dynamic emailSubscription)
        {
            return true;
        }

        public List<object> BulkImport(List<ExpandoObject> emailSubscriptions)
        {
            return Enumerable.Repeat(new object(), 1).ToList();
        }

        public void Delete(System.Guid emailSubscriptionId)
        {
            if (emailSubscriptionId != null)
            {
                return;
            }

            throw new ArgumentException("emailSubscriptionId is null.");
        }


    }
}
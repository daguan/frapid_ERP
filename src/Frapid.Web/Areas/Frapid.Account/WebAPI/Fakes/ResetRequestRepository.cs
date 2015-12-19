// ReSharper disable All
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Frapid.Account.DataAccess;
using Frapid.DataAccess;
using Frapid.DataAccess.Models;
using CustomField = Frapid.DataAccess.Models.CustomField;

namespace Frapid.Account.Api.Fakes
{
    public class ResetRequestRepository : IResetRequestRepository
    {
        public long Count()
        {
            return 1;
        }

        public IEnumerable<Frapid.Account.Entities.ResetRequest> GetAll()
        {
            return Enumerable.Repeat(new Frapid.Account.Entities.ResetRequest(), 1);
        }

        public IEnumerable<dynamic> Export()
        {
            return Enumerable.Repeat(new Frapid.Account.Entities.ResetRequest(), 1);
        }

        public Frapid.Account.Entities.ResetRequest Get(System.Guid requestId)
        {
            return new Frapid.Account.Entities.ResetRequest();
        }

        public IEnumerable<Frapid.Account.Entities.ResetRequest> Get(System.Guid[] requestIds)
        {
            return Enumerable.Repeat(new Frapid.Account.Entities.ResetRequest(), 1);
        }

        public Frapid.Account.Entities.ResetRequest GetFirst()
        {
            return new Frapid.Account.Entities.ResetRequest();
        }

        public Frapid.Account.Entities.ResetRequest GetPrevious(System.Guid requestId)
        {
            return new Frapid.Account.Entities.ResetRequest();
        }

        public Frapid.Account.Entities.ResetRequest GetNext(System.Guid requestId)
        {
            return new Frapid.Account.Entities.ResetRequest();
        }

        public Frapid.Account.Entities.ResetRequest GetLast()
        {
            return new Frapid.Account.Entities.ResetRequest();
        }

        public IEnumerable<Frapid.Account.Entities.ResetRequest> GetPaginatedResult()
        {
            return Enumerable.Repeat(new Frapid.Account.Entities.ResetRequest(), 1);
        }

        public IEnumerable<Frapid.Account.Entities.ResetRequest> GetPaginatedResult(long pageNumber)
        {
            return Enumerable.Repeat(new Frapid.Account.Entities.ResetRequest(), 1);
        }

        public long CountWhere(List<Frapid.DataAccess.Models.Filter> filters)
        {
            return 1;
        }

        public IEnumerable<Frapid.Account.Entities.ResetRequest> GetWhere(long pageNumber, List<Frapid.DataAccess.Models.Filter> filters)
        {
            return Enumerable.Repeat(new Frapid.Account.Entities.ResetRequest(), 1);
        }

        public long CountFiltered(string filterName)
        {
            return 1;
        }

        public List<Frapid.DataAccess.Models.Filter> GetFilters(string catalog, string filterName)
        {
            return Enumerable.Repeat(new Frapid.DataAccess.Models.Filter(), 1).ToList();
        }

        public IEnumerable<Frapid.Account.Entities.ResetRequest> GetFiltered(long pageNumber, string filterName)
        {
            return Enumerable.Repeat(new Frapid.Account.Entities.ResetRequest(), 1);
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

        public object AddOrEdit(dynamic resetRequest, List<Frapid.DataAccess.Models.CustomField> customFields)
        {
            return true;
        }

        public void Update(dynamic resetRequest, System.Guid requestId)
        {
            if (requestId != null)
            {
                return;
            }

            throw new ArgumentException("requestId is null.");
        }

        public object Add(dynamic resetRequest)
        {
            return true;
        }

        public List<object> BulkImport(List<ExpandoObject> resetRequests)
        {
            return Enumerable.Repeat(new object(), 1).ToList();
        }

        public void Delete(System.Guid requestId)
        {
            if (requestId != null)
            {
                return;
            }

            throw new ArgumentException("requestId is null.");
        }


    }
}
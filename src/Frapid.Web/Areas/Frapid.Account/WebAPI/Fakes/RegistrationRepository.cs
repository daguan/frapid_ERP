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
    public class RegistrationRepository : IRegistrationRepository
    {
        public long Count()
        {
            return 1;
        }

        public IEnumerable<Frapid.Account.Entities.Registration> GetAll()
        {
            return Enumerable.Repeat(new Frapid.Account.Entities.Registration(), 1);
        }

        public IEnumerable<dynamic> Export()
        {
            return Enumerable.Repeat(new Frapid.Account.Entities.Registration(), 1);
        }

        public Frapid.Account.Entities.Registration Get(System.Guid registrationId)
        {
            return new Frapid.Account.Entities.Registration();
        }

        public IEnumerable<Frapid.Account.Entities.Registration> Get(System.Guid[] registrationIds)
        {
            return Enumerable.Repeat(new Frapid.Account.Entities.Registration(), 1);
        }

        public Frapid.Account.Entities.Registration GetFirst()
        {
            return new Frapid.Account.Entities.Registration();
        }

        public Frapid.Account.Entities.Registration GetPrevious(System.Guid registrationId)
        {
            return new Frapid.Account.Entities.Registration();
        }

        public Frapid.Account.Entities.Registration GetNext(System.Guid registrationId)
        {
            return new Frapid.Account.Entities.Registration();
        }

        public Frapid.Account.Entities.Registration GetLast()
        {
            return new Frapid.Account.Entities.Registration();
        }

        public IEnumerable<Frapid.Account.Entities.Registration> GetPaginatedResult()
        {
            return Enumerable.Repeat(new Frapid.Account.Entities.Registration(), 1);
        }

        public IEnumerable<Frapid.Account.Entities.Registration> GetPaginatedResult(long pageNumber)
        {
            return Enumerable.Repeat(new Frapid.Account.Entities.Registration(), 1);
        }

        public long CountWhere(List<Frapid.DataAccess.Models.Filter> filters)
        {
            return 1;
        }

        public IEnumerable<Frapid.Account.Entities.Registration> GetWhere(long pageNumber, List<Frapid.DataAccess.Models.Filter> filters)
        {
            return Enumerable.Repeat(new Frapid.Account.Entities.Registration(), 1);
        }

        public long CountFiltered(string filterName)
        {
            return 1;
        }

        public List<Frapid.DataAccess.Models.Filter> GetFilters(string catalog, string filterName)
        {
            return Enumerable.Repeat(new Frapid.DataAccess.Models.Filter(), 1).ToList();
        }

        public IEnumerable<Frapid.Account.Entities.Registration> GetFiltered(long pageNumber, string filterName)
        {
            return Enumerable.Repeat(new Frapid.Account.Entities.Registration(), 1);
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

        public object AddOrEdit(dynamic registration, List<Frapid.DataAccess.Models.CustomField> customFields)
        {
            return true;
        }

        public void Update(dynamic registration, System.Guid registrationId)
        {
            if (registrationId != null)
            {
                return;
            }

            throw new ArgumentException("registrationId is null.");
        }

        public object Add(dynamic registration)
        {
            return true;
        }

        public List<object> BulkImport(List<ExpandoObject> registrations)
        {
            return Enumerable.Repeat(new object(), 1).ToList();
        }

        public void Delete(System.Guid registrationId)
        {
            if (registrationId != null)
            {
                return;
            }

            throw new ArgumentException("registrationId is null.");
        }


    }
}
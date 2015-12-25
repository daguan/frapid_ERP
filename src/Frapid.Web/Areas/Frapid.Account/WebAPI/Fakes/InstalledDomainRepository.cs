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
    public class InstalledDomainRepository : IInstalledDomainRepository
    {
        public long Count()
        {
            return 1;
        }

        public IEnumerable<Frapid.Account.Entities.InstalledDomain> GetAll()
        {
            return Enumerable.Repeat(new Frapid.Account.Entities.InstalledDomain(), 1);
        }

        public IEnumerable<dynamic> Export()
        {
            return Enumerable.Repeat(new Frapid.Account.Entities.InstalledDomain(), 1);
        }

        public Frapid.Account.Entities.InstalledDomain Get(int domainId)
        {
            return new Frapid.Account.Entities.InstalledDomain();
        }

        public IEnumerable<Frapid.Account.Entities.InstalledDomain> Get(int[] domainIds)
        {
            return Enumerable.Repeat(new Frapid.Account.Entities.InstalledDomain(), 1);
        }

        public Frapid.Account.Entities.InstalledDomain GetFirst()
        {
            return new Frapid.Account.Entities.InstalledDomain();
        }

        public Frapid.Account.Entities.InstalledDomain GetPrevious(int domainId)
        {
            return new Frapid.Account.Entities.InstalledDomain();
        }

        public Frapid.Account.Entities.InstalledDomain GetNext(int domainId)
        {
            return new Frapid.Account.Entities.InstalledDomain();
        }

        public Frapid.Account.Entities.InstalledDomain GetLast()
        {
            return new Frapid.Account.Entities.InstalledDomain();
        }

        public IEnumerable<Frapid.Account.Entities.InstalledDomain> GetPaginatedResult()
        {
            return Enumerable.Repeat(new Frapid.Account.Entities.InstalledDomain(), 1);
        }

        public IEnumerable<Frapid.Account.Entities.InstalledDomain> GetPaginatedResult(long pageNumber)
        {
            return Enumerable.Repeat(new Frapid.Account.Entities.InstalledDomain(), 1);
        }

        public long CountWhere(List<Frapid.DataAccess.Models.Filter> filters)
        {
            return 1;
        }

        public IEnumerable<Frapid.Account.Entities.InstalledDomain> GetWhere(long pageNumber, List<Frapid.DataAccess.Models.Filter> filters)
        {
            return Enumerable.Repeat(new Frapid.Account.Entities.InstalledDomain(), 1);
        }

        public long CountFiltered(string filterName)
        {
            return 1;
        }

        public List<Frapid.DataAccess.Models.Filter> GetFilters(string catalog, string filterName)
        {
            return Enumerable.Repeat(new Frapid.DataAccess.Models.Filter(), 1).ToList();
        }

        public IEnumerable<Frapid.Account.Entities.InstalledDomain> GetFiltered(long pageNumber, string filterName)
        {
            return Enumerable.Repeat(new Frapid.Account.Entities.InstalledDomain(), 1);
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

        public object AddOrEdit(dynamic installedDomain, List<Frapid.DataAccess.Models.CustomField> customFields)
        {
            return true;
        }

        public void Update(dynamic installedDomain, int domainId)
        {
            if (domainId > 0)
            {
                return;
            }

            throw new ArgumentException("domainId is null.");
        }

        public object Add(dynamic installedDomain)
        {
            return true;
        }

        public List<object> BulkImport(List<ExpandoObject> installedDomains)
        {
            return Enumerable.Repeat(new object(), 1).ToList();
        }

        public void Delete(int domainId)
        {
            if (domainId > 0)
            {
                return;
            }

            throw new ArgumentException("domainId is null.");
        }


    }
}
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
    public class LoginRepository : ILoginRepository
    {
        public long Count()
        {
            return 1;
        }

        public IEnumerable<Frapid.Account.Entities.Login> GetAll()
        {
            return Enumerable.Repeat(new Frapid.Account.Entities.Login(), 1);
        }

        public IEnumerable<dynamic> Export()
        {
            return Enumerable.Repeat(new Frapid.Account.Entities.Login(), 1);
        }

        public Frapid.Account.Entities.Login Get(long loginId)
        {
            return new Frapid.Account.Entities.Login();
        }

        public IEnumerable<Frapid.Account.Entities.Login> Get(long[] loginIds)
        {
            return Enumerable.Repeat(new Frapid.Account.Entities.Login(), 1);
        }

        public Frapid.Account.Entities.Login GetFirst()
        {
            return new Frapid.Account.Entities.Login();
        }

        public Frapid.Account.Entities.Login GetPrevious(long loginId)
        {
            return new Frapid.Account.Entities.Login();
        }

        public Frapid.Account.Entities.Login GetNext(long loginId)
        {
            return new Frapid.Account.Entities.Login();
        }

        public Frapid.Account.Entities.Login GetLast()
        {
            return new Frapid.Account.Entities.Login();
        }

        public IEnumerable<Frapid.Account.Entities.Login> GetPaginatedResult()
        {
            return Enumerable.Repeat(new Frapid.Account.Entities.Login(), 1);
        }

        public IEnumerable<Frapid.Account.Entities.Login> GetPaginatedResult(long pageNumber)
        {
            return Enumerable.Repeat(new Frapid.Account.Entities.Login(), 1);
        }

        public long CountWhere(List<Frapid.DataAccess.Models.Filter> filters)
        {
            return 1;
        }

        public IEnumerable<Frapid.Account.Entities.Login> GetWhere(long pageNumber, List<Frapid.DataAccess.Models.Filter> filters)
        {
            return Enumerable.Repeat(new Frapid.Account.Entities.Login(), 1);
        }

        public long CountFiltered(string filterName)
        {
            return 1;
        }

        public List<Frapid.DataAccess.Models.Filter> GetFilters(string catalog, string filterName)
        {
            return Enumerable.Repeat(new Frapid.DataAccess.Models.Filter(), 1).ToList();
        }

        public IEnumerable<Frapid.Account.Entities.Login> GetFiltered(long pageNumber, string filterName)
        {
            return Enumerable.Repeat(new Frapid.Account.Entities.Login(), 1);
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

        public object AddOrEdit(dynamic login, List<Frapid.DataAccess.Models.CustomField> customFields)
        {
            return true;
        }

        public void Update(dynamic login, long loginId)
        {
            if (loginId > 0)
            {
                return;
            }

            throw new ArgumentException("loginId is null.");
        }

        public object Add(dynamic login)
        {
            return true;
        }

        public List<object> BulkImport(List<ExpandoObject> logins)
        {
            return Enumerable.Repeat(new object(), 1).ToList();
        }

        public void Delete(long loginId)
        {
            if (loginId > 0)
            {
                return;
            }

            throw new ArgumentException("loginId is null.");
        }


    }
}
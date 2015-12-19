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
    public class RoleRepository : IRoleRepository
    {
        public long Count()
        {
            return 1;
        }

        public IEnumerable<Frapid.Account.Entities.Role> GetAll()
        {
            return Enumerable.Repeat(new Frapid.Account.Entities.Role(), 1);
        }

        public IEnumerable<dynamic> Export()
        {
            return Enumerable.Repeat(new Frapid.Account.Entities.Role(), 1);
        }

        public Frapid.Account.Entities.Role Get(int roleId)
        {
            return new Frapid.Account.Entities.Role();
        }

        public IEnumerable<Frapid.Account.Entities.Role> Get(int[] roleIds)
        {
            return Enumerable.Repeat(new Frapid.Account.Entities.Role(), 1);
        }

        public Frapid.Account.Entities.Role GetFirst()
        {
            return new Frapid.Account.Entities.Role();
        }

        public Frapid.Account.Entities.Role GetPrevious(int roleId)
        {
            return new Frapid.Account.Entities.Role();
        }

        public Frapid.Account.Entities.Role GetNext(int roleId)
        {
            return new Frapid.Account.Entities.Role();
        }

        public Frapid.Account.Entities.Role GetLast()
        {
            return new Frapid.Account.Entities.Role();
        }

        public IEnumerable<Frapid.Account.Entities.Role> GetPaginatedResult()
        {
            return Enumerable.Repeat(new Frapid.Account.Entities.Role(), 1);
        }

        public IEnumerable<Frapid.Account.Entities.Role> GetPaginatedResult(long pageNumber)
        {
            return Enumerable.Repeat(new Frapid.Account.Entities.Role(), 1);
        }

        public long CountWhere(List<Frapid.DataAccess.Models.Filter> filters)
        {
            return 1;
        }

        public IEnumerable<Frapid.Account.Entities.Role> GetWhere(long pageNumber, List<Frapid.DataAccess.Models.Filter> filters)
        {
            return Enumerable.Repeat(new Frapid.Account.Entities.Role(), 1);
        }

        public long CountFiltered(string filterName)
        {
            return 1;
        }

        public List<Frapid.DataAccess.Models.Filter> GetFilters(string catalog, string filterName)
        {
            return Enumerable.Repeat(new Frapid.DataAccess.Models.Filter(), 1).ToList();
        }

        public IEnumerable<Frapid.Account.Entities.Role> GetFiltered(long pageNumber, string filterName)
        {
            return Enumerable.Repeat(new Frapid.Account.Entities.Role(), 1);
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

        public object AddOrEdit(dynamic role, List<Frapid.DataAccess.Models.CustomField> customFields)
        {
            return true;
        }

        public void Update(dynamic role, int roleId)
        {
            if (roleId > 0)
            {
                return;
            }

            throw new ArgumentException("roleId is null.");
        }

        public object Add(dynamic role)
        {
            return true;
        }

        public List<object> BulkImport(List<ExpandoObject> roles)
        {
            return Enumerable.Repeat(new object(), 1).ToList();
        }

        public void Delete(int roleId)
        {
            if (roleId > 0)
            {
                return;
            }

            throw new ArgumentException("roleId is null.");
        }


    }
}
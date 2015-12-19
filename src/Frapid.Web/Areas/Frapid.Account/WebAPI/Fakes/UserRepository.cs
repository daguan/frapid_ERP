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
    public class UserRepository : IUserRepository
    {
        public long Count()
        {
            return 1;
        }

        public IEnumerable<Frapid.Account.Entities.User> GetAll()
        {
            return Enumerable.Repeat(new Frapid.Account.Entities.User(), 1);
        }

        public IEnumerable<dynamic> Export()
        {
            return Enumerable.Repeat(new Frapid.Account.Entities.User(), 1);
        }

        public Frapid.Account.Entities.User Get(int userId)
        {
            return new Frapid.Account.Entities.User();
        }

        public IEnumerable<Frapid.Account.Entities.User> Get(int[] userIds)
        {
            return Enumerable.Repeat(new Frapid.Account.Entities.User(), 1);
        }

        public Frapid.Account.Entities.User GetFirst()
        {
            return new Frapid.Account.Entities.User();
        }

        public Frapid.Account.Entities.User GetPrevious(int userId)
        {
            return new Frapid.Account.Entities.User();
        }

        public Frapid.Account.Entities.User GetNext(int userId)
        {
            return new Frapid.Account.Entities.User();
        }

        public Frapid.Account.Entities.User GetLast()
        {
            return new Frapid.Account.Entities.User();
        }

        public IEnumerable<Frapid.Account.Entities.User> GetPaginatedResult()
        {
            return Enumerable.Repeat(new Frapid.Account.Entities.User(), 1);
        }

        public IEnumerable<Frapid.Account.Entities.User> GetPaginatedResult(long pageNumber)
        {
            return Enumerable.Repeat(new Frapid.Account.Entities.User(), 1);
        }

        public long CountWhere(List<Frapid.DataAccess.Models.Filter> filters)
        {
            return 1;
        }

        public IEnumerable<Frapid.Account.Entities.User> GetWhere(long pageNumber, List<Frapid.DataAccess.Models.Filter> filters)
        {
            return Enumerable.Repeat(new Frapid.Account.Entities.User(), 1);
        }

        public long CountFiltered(string filterName)
        {
            return 1;
        }

        public List<Frapid.DataAccess.Models.Filter> GetFilters(string catalog, string filterName)
        {
            return Enumerable.Repeat(new Frapid.DataAccess.Models.Filter(), 1).ToList();
        }

        public IEnumerable<Frapid.Account.Entities.User> GetFiltered(long pageNumber, string filterName)
        {
            return Enumerable.Repeat(new Frapid.Account.Entities.User(), 1);
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

        public object AddOrEdit(dynamic user, List<Frapid.DataAccess.Models.CustomField> customFields)
        {
            return true;
        }

        public void Update(dynamic user, int userId)
        {
            if (userId > 0)
            {
                return;
            }

            throw new ArgumentException("userId is null.");
        }

        public object Add(dynamic user)
        {
            return true;
        }

        public List<object> BulkImport(List<ExpandoObject> users)
        {
            return Enumerable.Repeat(new object(), 1).ToList();
        }

        public void Delete(int userId)
        {
            if (userId > 0)
            {
                return;
            }

            throw new ArgumentException("userId is null.");
        }


    }
}
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
    public class FbAccessTokenRepository : IFbAccessTokenRepository
    {
        public long Count()
        {
            return 1;
        }

        public IEnumerable<Frapid.Account.Entities.FbAccessToken> GetAll()
        {
            return Enumerable.Repeat(new Frapid.Account.Entities.FbAccessToken(), 1);
        }

        public IEnumerable<dynamic> Export()
        {
            return Enumerable.Repeat(new Frapid.Account.Entities.FbAccessToken(), 1);
        }

        public Frapid.Account.Entities.FbAccessToken Get(int userId)
        {
            return new Frapid.Account.Entities.FbAccessToken();
        }

        public IEnumerable<Frapid.Account.Entities.FbAccessToken> Get(int[] userIds)
        {
            return Enumerable.Repeat(new Frapid.Account.Entities.FbAccessToken(), 1);
        }

        public Frapid.Account.Entities.FbAccessToken GetFirst()
        {
            return new Frapid.Account.Entities.FbAccessToken();
        }

        public Frapid.Account.Entities.FbAccessToken GetPrevious(int userId)
        {
            return new Frapid.Account.Entities.FbAccessToken();
        }

        public Frapid.Account.Entities.FbAccessToken GetNext(int userId)
        {
            return new Frapid.Account.Entities.FbAccessToken();
        }

        public Frapid.Account.Entities.FbAccessToken GetLast()
        {
            return new Frapid.Account.Entities.FbAccessToken();
        }

        public IEnumerable<Frapid.Account.Entities.FbAccessToken> GetPaginatedResult()
        {
            return Enumerable.Repeat(new Frapid.Account.Entities.FbAccessToken(), 1);
        }

        public IEnumerable<Frapid.Account.Entities.FbAccessToken> GetPaginatedResult(long pageNumber)
        {
            return Enumerable.Repeat(new Frapid.Account.Entities.FbAccessToken(), 1);
        }

        public long CountWhere(List<Frapid.DataAccess.Models.Filter> filters)
        {
            return 1;
        }

        public IEnumerable<Frapid.Account.Entities.FbAccessToken> GetWhere(long pageNumber, List<Frapid.DataAccess.Models.Filter> filters)
        {
            return Enumerable.Repeat(new Frapid.Account.Entities.FbAccessToken(), 1);
        }

        public long CountFiltered(string filterName)
        {
            return 1;
        }

        public List<Frapid.DataAccess.Models.Filter> GetFilters(string catalog, string filterName)
        {
            return Enumerable.Repeat(new Frapid.DataAccess.Models.Filter(), 1).ToList();
        }

        public IEnumerable<Frapid.Account.Entities.FbAccessToken> GetFiltered(long pageNumber, string filterName)
        {
            return Enumerable.Repeat(new Frapid.Account.Entities.FbAccessToken(), 1);
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

        public object AddOrEdit(dynamic fbAccessToken, List<Frapid.DataAccess.Models.CustomField> customFields)
        {
            return true;
        }

        public void Update(dynamic fbAccessToken, int userId)
        {
            if (userId > 0)
            {
                return;
            }

            throw new ArgumentException("userId is null.");
        }

        public object Add(dynamic fbAccessToken)
        {
            return true;
        }

        public List<object> BulkImport(List<ExpandoObject> fbAccessTokens)
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
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
    public class SignInViewRepository : ISignInViewRepository
    {
        public long Count()
        {
            return 1;
        }

        public IEnumerable<Frapid.Account.Entities.SignInView> Get()
        {
            return Enumerable.Repeat(new Frapid.Account.Entities.SignInView(), 1);
        }

        public IEnumerable<Frapid.Account.Entities.SignInView> GetPaginatedResult()
        {
            return Enumerable.Repeat(new Frapid.Account.Entities.SignInView(), 1);
        }

        public IEnumerable<Frapid.Account.Entities.SignInView> GetPaginatedResult(long pageNumber)
        {
            return Enumerable.Repeat(new Frapid.Account.Entities.SignInView(), 1);
        }

        public IEnumerable<DisplayField> GetDisplayFields()
        {
            return Enumerable.Repeat(new DisplayField(), 1);
        }


        public long CountWhere(List<Frapid.DataAccess.Models.Filter> filters)
        {
            return 1;
        }

        public IEnumerable<Frapid.Account.Entities.SignInView> GetWhere(long pageNumber, List<Frapid.DataAccess.Models.Filter> filters)
        {
            return Enumerable.Repeat(new Frapid.Account.Entities.SignInView(), 1);
        }

        public List<Frapid.DataAccess.Models.Filter> GetFilters(string catalog, string filterName)
        {
            return Enumerable.Repeat(new Frapid.DataAccess.Models.Filter(), 1).ToList();
        }

        public long CountFiltered(string filterName)
        {
            return 1;
        }

        public IEnumerable<Frapid.Account.Entities.SignInView> GetFiltered(long pageNumber, string filterName)
        {
            return Enumerable.Repeat(new Frapid.Account.Entities.SignInView(), 1);
        }

    }
}
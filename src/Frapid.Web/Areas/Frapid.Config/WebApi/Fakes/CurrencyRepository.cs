// ReSharper disable All
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Frapid.Config.DataAccess;
using Frapid.DataAccess;
using Frapid.DataAccess.Models;
using CustomField = Frapid.DataAccess.Models.CustomField;

namespace Frapid.Config.Api.Fakes
{
    public class CurrencyRepository : ICurrencyRepository
    {
        public long Count()
        {
            return 1;
        }

        public IEnumerable<Frapid.Config.Entities.Currency> GetAll()
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.Currency(), 1);
        }

        public IEnumerable<dynamic> Export()
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.Currency(), 1);
        }

        public Frapid.Config.Entities.Currency Get(string currencyCode)
        {
            return new Frapid.Config.Entities.Currency();
        }

        public IEnumerable<Frapid.Config.Entities.Currency> Get(string[] currencyCodes)
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.Currency(), 1);
        }

        public Frapid.Config.Entities.Currency GetFirst()
        {
            return new Frapid.Config.Entities.Currency();
        }

        public Frapid.Config.Entities.Currency GetPrevious(string currencyCode)
        {
            return new Frapid.Config.Entities.Currency();
        }

        public Frapid.Config.Entities.Currency GetNext(string currencyCode)
        {
            return new Frapid.Config.Entities.Currency();
        }

        public Frapid.Config.Entities.Currency GetLast()
        {
            return new Frapid.Config.Entities.Currency();
        }

        public IEnumerable<Frapid.Config.Entities.Currency> GetPaginatedResult()
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.Currency(), 1);
        }

        public IEnumerable<Frapid.Config.Entities.Currency> GetPaginatedResult(long pageNumber)
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.Currency(), 1);
        }

        public long CountWhere(List<Frapid.DataAccess.Models.Filter> filters)
        {
            return 1;
        }

        public IEnumerable<Frapid.Config.Entities.Currency> GetWhere(long pageNumber, List<Frapid.DataAccess.Models.Filter> filters)
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.Currency(), 1);
        }

        public long CountFiltered(string filterName)
        {
            return 1;
        }

        public List<Frapid.DataAccess.Models.Filter> GetFilters(string catalog, string filterName)
        {
            return Enumerable.Repeat(new Frapid.DataAccess.Models.Filter(), 1).ToList();
        }

        public IEnumerable<Frapid.Config.Entities.Currency> GetFiltered(long pageNumber, string filterName)
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.Currency(), 1);
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

        public object AddOrEdit(dynamic currency, List<Frapid.DataAccess.Models.CustomField> customFields)
        {
            return true;
        }

        public void Update(dynamic currency, string currencyCode)
        {
            if (!string.IsNullOrWhiteSpace(currencyCode))
            {
                return;
            }

            throw new ArgumentException("currencyCode is null.");
        }

        public object Add(dynamic currency)
        {
            return true;
        }

        public List<object> BulkImport(List<ExpandoObject> currencies)
        {
            return Enumerable.Repeat(new object(), 1).ToList();
        }

        public void Delete(string currencyCode)
        {
            if (!string.IsNullOrWhiteSpace(currencyCode))
            {
                return;
            }

            throw new ArgumentException("currencyCode is null.");
        }


    }
}
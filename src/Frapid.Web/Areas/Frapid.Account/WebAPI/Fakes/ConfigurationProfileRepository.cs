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
    public class ConfigurationProfileRepository : IConfigurationProfileRepository
    {
        public long Count()
        {
            return 1;
        }

        public IEnumerable<Frapid.Account.Entities.ConfigurationProfile> GetAll()
        {
            return Enumerable.Repeat(new Frapid.Account.Entities.ConfigurationProfile(), 1);
        }

        public IEnumerable<dynamic> Export()
        {
            return Enumerable.Repeat(new Frapid.Account.Entities.ConfigurationProfile(), 1);
        }

        public Frapid.Account.Entities.ConfigurationProfile Get(int profileId)
        {
            return new Frapid.Account.Entities.ConfigurationProfile();
        }

        public IEnumerable<Frapid.Account.Entities.ConfigurationProfile> Get(int[] profileIds)
        {
            return Enumerable.Repeat(new Frapid.Account.Entities.ConfigurationProfile(), 1);
        }

        public Frapid.Account.Entities.ConfigurationProfile GetFirst()
        {
            return new Frapid.Account.Entities.ConfigurationProfile();
        }

        public Frapid.Account.Entities.ConfigurationProfile GetPrevious(int profileId)
        {
            return new Frapid.Account.Entities.ConfigurationProfile();
        }

        public Frapid.Account.Entities.ConfigurationProfile GetNext(int profileId)
        {
            return new Frapid.Account.Entities.ConfigurationProfile();
        }

        public Frapid.Account.Entities.ConfigurationProfile GetLast()
        {
            return new Frapid.Account.Entities.ConfigurationProfile();
        }

        public IEnumerable<Frapid.Account.Entities.ConfigurationProfile> GetPaginatedResult()
        {
            return Enumerable.Repeat(new Frapid.Account.Entities.ConfigurationProfile(), 1);
        }

        public IEnumerable<Frapid.Account.Entities.ConfigurationProfile> GetPaginatedResult(long pageNumber)
        {
            return Enumerable.Repeat(new Frapid.Account.Entities.ConfigurationProfile(), 1);
        }

        public long CountWhere(List<Frapid.DataAccess.Models.Filter> filters)
        {
            return 1;
        }

        public IEnumerable<Frapid.Account.Entities.ConfigurationProfile> GetWhere(long pageNumber, List<Frapid.DataAccess.Models.Filter> filters)
        {
            return Enumerable.Repeat(new Frapid.Account.Entities.ConfigurationProfile(), 1);
        }

        public long CountFiltered(string filterName)
        {
            return 1;
        }

        public List<Frapid.DataAccess.Models.Filter> GetFilters(string catalog, string filterName)
        {
            return Enumerable.Repeat(new Frapid.DataAccess.Models.Filter(), 1).ToList();
        }

        public IEnumerable<Frapid.Account.Entities.ConfigurationProfile> GetFiltered(long pageNumber, string filterName)
        {
            return Enumerable.Repeat(new Frapid.Account.Entities.ConfigurationProfile(), 1);
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

        public object AddOrEdit(dynamic configurationProfile, List<Frapid.DataAccess.Models.CustomField> customFields)
        {
            return true;
        }

        public void Update(dynamic configurationProfile, int profileId)
        {
            if (profileId > 0)
            {
                return;
            }

            throw new ArgumentException("profileId is null.");
        }

        public object Add(dynamic configurationProfile)
        {
            return true;
        }

        public List<object> BulkImport(List<ExpandoObject> configurationProfiles)
        {
            return Enumerable.Repeat(new object(), 1).ToList();
        }

        public void Delete(int profileId)
        {
            if (profileId > 0)
            {
                return;
            }

            throw new ArgumentException("profileId is null.");
        }


    }
}
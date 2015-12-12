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
    public class SmtpConfigRepository : ISmtpConfigRepository
    {
        public long Count()
        {
            return 1;
        }

        public IEnumerable<Frapid.Config.Entities.SmtpConfig> GetAll()
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.SmtpConfig(), 1);
        }

        public IEnumerable<dynamic> Export()
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.SmtpConfig(), 1);
        }

        public Frapid.Config.Entities.SmtpConfig Get(int smtpId)
        {
            return new Frapid.Config.Entities.SmtpConfig();
        }

        public IEnumerable<Frapid.Config.Entities.SmtpConfig> Get(int[] smtpIds)
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.SmtpConfig(), 1);
        }

        public Frapid.Config.Entities.SmtpConfig GetFirst()
        {
            return new Frapid.Config.Entities.SmtpConfig();
        }

        public Frapid.Config.Entities.SmtpConfig GetPrevious(int smtpId)
        {
            return new Frapid.Config.Entities.SmtpConfig();
        }

        public Frapid.Config.Entities.SmtpConfig GetNext(int smtpId)
        {
            return new Frapid.Config.Entities.SmtpConfig();
        }

        public Frapid.Config.Entities.SmtpConfig GetLast()
        {
            return new Frapid.Config.Entities.SmtpConfig();
        }

        public IEnumerable<Frapid.Config.Entities.SmtpConfig> GetPaginatedResult()
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.SmtpConfig(), 1);
        }

        public IEnumerable<Frapid.Config.Entities.SmtpConfig> GetPaginatedResult(long pageNumber)
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.SmtpConfig(), 1);
        }

        public long CountWhere(List<Frapid.DataAccess.Models.Filter> filters)
        {
            return 1;
        }

        public IEnumerable<Frapid.Config.Entities.SmtpConfig> GetWhere(long pageNumber, List<Frapid.DataAccess.Models.Filter> filters)
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.SmtpConfig(), 1);
        }

        public long CountFiltered(string filterName)
        {
            return 1;
        }

        public List<Frapid.DataAccess.Models.Filter> GetFilters(string catalog, string filterName)
        {
            return Enumerable.Repeat(new Frapid.DataAccess.Models.Filter(), 1).ToList();
        }

        public IEnumerable<Frapid.Config.Entities.SmtpConfig> GetFiltered(long pageNumber, string filterName)
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.SmtpConfig(), 1);
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

        public object AddOrEdit(dynamic smtpConfig, List<Frapid.DataAccess.Models.CustomField> customFields)
        {
            return true;
        }

        public void Update(dynamic smtpConfig, int smtpId)
        {
            if (smtpId > 0)
            {
                return;
            }

            throw new ArgumentException("smtpId is null.");
        }

        public object Add(dynamic smtpConfig)
        {
            return true;
        }

        public List<object> BulkImport(List<ExpandoObject> smtpConfigs)
        {
            return Enumerable.Repeat(new object(), 1).ToList();
        }

        public void Delete(int smtpId)
        {
            if (smtpId > 0)
            {
                return;
            }

            throw new ArgumentException("smtpId is null.");
        }


    }
}
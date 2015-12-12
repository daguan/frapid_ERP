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
    public class EmailQueueRepository : IEmailQueueRepository
    {
        public long Count()
        {
            return 1;
        }

        public IEnumerable<Frapid.Config.Entities.EmailQueue> GetAll()
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.EmailQueue(), 1);
        }

        public IEnumerable<dynamic> Export()
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.EmailQueue(), 1);
        }

        public Frapid.Config.Entities.EmailQueue Get(long queueId)
        {
            return new Frapid.Config.Entities.EmailQueue();
        }

        public IEnumerable<Frapid.Config.Entities.EmailQueue> Get(long[] queueIds)
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.EmailQueue(), 1);
        }

        public Frapid.Config.Entities.EmailQueue GetFirst()
        {
            return new Frapid.Config.Entities.EmailQueue();
        }

        public Frapid.Config.Entities.EmailQueue GetPrevious(long queueId)
        {
            return new Frapid.Config.Entities.EmailQueue();
        }

        public Frapid.Config.Entities.EmailQueue GetNext(long queueId)
        {
            return new Frapid.Config.Entities.EmailQueue();
        }

        public Frapid.Config.Entities.EmailQueue GetLast()
        {
            return new Frapid.Config.Entities.EmailQueue();
        }

        public IEnumerable<Frapid.Config.Entities.EmailQueue> GetPaginatedResult()
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.EmailQueue(), 1);
        }

        public IEnumerable<Frapid.Config.Entities.EmailQueue> GetPaginatedResult(long pageNumber)
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.EmailQueue(), 1);
        }

        public long CountWhere(List<Frapid.DataAccess.Models.Filter> filters)
        {
            return 1;
        }

        public IEnumerable<Frapid.Config.Entities.EmailQueue> GetWhere(long pageNumber, List<Frapid.DataAccess.Models.Filter> filters)
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.EmailQueue(), 1);
        }

        public long CountFiltered(string filterName)
        {
            return 1;
        }

        public List<Frapid.DataAccess.Models.Filter> GetFilters(string catalog, string filterName)
        {
            return Enumerable.Repeat(new Frapid.DataAccess.Models.Filter(), 1).ToList();
        }

        public IEnumerable<Frapid.Config.Entities.EmailQueue> GetFiltered(long pageNumber, string filterName)
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.EmailQueue(), 1);
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

        public object AddOrEdit(dynamic emailQueue, List<Frapid.DataAccess.Models.CustomField> customFields)
        {
            return true;
        }

        public void Update(dynamic emailQueue, long queueId)
        {
            if (queueId > 0)
            {
                return;
            }

            throw new ArgumentException("queueId is null.");
        }

        public object Add(dynamic emailQueue)
        {
            return true;
        }

        public List<object> BulkImport(List<ExpandoObject> emailQueues)
        {
            return Enumerable.Repeat(new object(), 1).ToList();
        }

        public void Delete(long queueId)
        {
            if (queueId > 0)
            {
                return;
            }

            throw new ArgumentException("queueId is null.");
        }


    }
}
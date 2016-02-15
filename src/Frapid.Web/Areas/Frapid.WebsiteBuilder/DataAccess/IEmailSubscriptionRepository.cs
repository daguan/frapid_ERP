// ReSharper disable All
using System.Collections.Generic;
using System.Dynamic;
using Frapid.DataAccess;
using Frapid.DataAccess.Models;

namespace Frapid.WebsiteBuilder.DataAccess
{
    public interface IEmailSubscriptionRepository
    {
        /// <summary>
        /// Counts the number of EmailSubscription in IEmailSubscriptionRepository.
        /// </summary>
        /// <returns>Returns the count of IEmailSubscriptionRepository.</returns>
        long Count();

        /// <summary>
        /// Returns all instances of EmailSubscription. 
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instances of EmailSubscription.</returns>
        IEnumerable<Frapid.WebsiteBuilder.Entities.EmailSubscription> GetAll();

        /// <summary>
        /// Returns all instances of EmailSubscription to export. 
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instances of EmailSubscription.</returns>
        IEnumerable<dynamic> Export();

        /// <summary>
        /// Returns a single instance of the EmailSubscription against emailSubscriptionId. 
        /// </summary>
        /// <param name="emailSubscriptionId">The column "email_subscription_id" parameter used on where filter.</param>
        /// <returns>Returns a non-live, non-mapped instance of EmailSubscription.</returns>
        Frapid.WebsiteBuilder.Entities.EmailSubscription Get(System.Guid emailSubscriptionId);

        /// <summary>
        /// Gets the first record of EmailSubscription.
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instance of EmailSubscription.</returns>
        Frapid.WebsiteBuilder.Entities.EmailSubscription GetFirst();

        /// <summary>
        /// Gets the previous record of EmailSubscription sorted by emailSubscriptionId. 
        /// </summary>
        /// <param name="emailSubscriptionId">The column "email_subscription_id" parameter used to find the previous record.</param>
        /// <returns>Returns a non-live, non-mapped instance of EmailSubscription.</returns>
        Frapid.WebsiteBuilder.Entities.EmailSubscription GetPrevious(System.Guid emailSubscriptionId);

        /// <summary>
        /// Gets the next record of EmailSubscription sorted by emailSubscriptionId. 
        /// </summary>
        /// <param name="emailSubscriptionId">The column "email_subscription_id" parameter used to find the next record.</param>
        /// <returns>Returns a non-live, non-mapped instance of EmailSubscription.</returns>
        Frapid.WebsiteBuilder.Entities.EmailSubscription GetNext(System.Guid emailSubscriptionId);

        /// <summary>
        /// Gets the last record of EmailSubscription.
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instance of EmailSubscription.</returns>
        Frapid.WebsiteBuilder.Entities.EmailSubscription GetLast();

        /// <summary>
        /// Returns multiple instances of the EmailSubscription against emailSubscriptionIds. 
        /// </summary>
        /// <param name="emailSubscriptionIds">Array of column "email_subscription_id" parameter used on where filter.</param>
        /// <returns>Returns a non-live, non-mapped collection of EmailSubscription.</returns>
        IEnumerable<Frapid.WebsiteBuilder.Entities.EmailSubscription> Get(System.Guid[] emailSubscriptionIds);

        /// <summary>
        /// Custom fields are user defined form elements for IEmailSubscriptionRepository.
        /// </summary>
        /// <returns>Returns an enumerable custom field collection for EmailSubscription.</returns>
        IEnumerable<Frapid.DataAccess.Models.CustomField> GetCustomFields(string resourceId);

        /// <summary>
        /// Displayfields provide a minimal name/value context for data binding EmailSubscription.
        /// </summary>
        /// <returns>Returns an enumerable name and value collection for EmailSubscription.</returns>
        IEnumerable<Frapid.DataAccess.Models.DisplayField> GetDisplayFields();

        /// <summary>
        /// Inserts the instance of EmailSubscription class to IEmailSubscriptionRepository.
        /// </summary>
        /// <param name="emailSubscription">The instance of EmailSubscription class to insert or update.</param>
        /// <param name="customFields">The custom field collection.</param>
        object AddOrEdit(dynamic emailSubscription, List<Frapid.DataAccess.Models.CustomField> customFields);

        /// <summary>
        /// Inserts the instance of EmailSubscription class to IEmailSubscriptionRepository.
        /// </summary>
        /// <param name="emailSubscription">The instance of EmailSubscription class to insert.</param>
        object Add(dynamic emailSubscription);

        /// <summary>
        /// Inserts or updates multiple instances of EmailSubscription class to IEmailSubscriptionRepository.;
        /// </summary>
        /// <param name="emailSubscriptions">List of EmailSubscription class to import.</param>
        /// <returns>Returns list of inserted object ids.</returns>
        List<object> BulkImport(List<ExpandoObject> emailSubscriptions);


        /// <summary>
        /// Updates IEmailSubscriptionRepository with an instance of EmailSubscription class against the primary key value.
        /// </summary>
        /// <param name="emailSubscription">The instance of EmailSubscription class to update.</param>
        /// <param name="emailSubscriptionId">The value of the column "email_subscription_id" which will be updated.</param>
        void Update(dynamic emailSubscription, System.Guid emailSubscriptionId);

        /// <summary>
        /// Deletes EmailSubscription from  IEmailSubscriptionRepository against the primary key value.
        /// </summary>
        /// <param name="emailSubscriptionId">The value of the column "email_subscription_id" which will be deleted.</param>
        void Delete(System.Guid emailSubscriptionId);

        /// <summary>
        /// Produces a paginated result of 50 EmailSubscription classes.
        /// </summary>
        /// <returns>Returns the first page of collection of EmailSubscription class.</returns>
        IEnumerable<Frapid.WebsiteBuilder.Entities.EmailSubscription> GetPaginatedResult();

        /// <summary>
        /// Produces a paginated result of 50 EmailSubscription classes.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result.</param>
        /// <returns>Returns collection of EmailSubscription class.</returns>
        IEnumerable<Frapid.WebsiteBuilder.Entities.EmailSubscription> GetPaginatedResult(long pageNumber);

        List<Frapid.DataAccess.Models.Filter> GetFilters(string catalog, string filterName);

        /// <summary>
        /// Performs a filtered count on IEmailSubscriptionRepository.
        /// </summary>
        /// <param name="filters">The list of filter conditions.</param>
        /// <returns>Returns number of rows of EmailSubscription class using the filter.</returns>
        long CountWhere(List<Frapid.DataAccess.Models.Filter> filters);

        /// <summary>
        /// Performs a filtered pagination against IEmailSubscriptionRepository producing result of 50 items.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result. If you provide a negative number, the result will not be paginated.</param>
        /// <param name="filters">The list of filter conditions.</param>
        /// <returns>Returns collection of EmailSubscription class.</returns>
        IEnumerable<Frapid.WebsiteBuilder.Entities.EmailSubscription> GetWhere(long pageNumber, List<Frapid.DataAccess.Models.Filter> filters);

        /// <summary>
        /// Performs a filtered count on IEmailSubscriptionRepository.
        /// </summary>
        /// <param name="filterName">The named filter.</param>
        /// <returns>Returns number of EmailSubscription class using the filter.</returns>
        long CountFiltered(string filterName);

        /// <summary>
        /// Gets a filtered result of IEmailSubscriptionRepository producing a paginated result of 50.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result. If you provide a negative number, the result will not be paginated.</param>
        /// <param name="filterName">The named filter.</param>
        /// <returns>Returns collection of EmailSubscription class.</returns>
        IEnumerable<Frapid.WebsiteBuilder.Entities.EmailSubscription> GetFiltered(long pageNumber, string filterName);



    }
}
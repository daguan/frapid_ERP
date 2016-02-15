// ReSharper disable All
using System.Collections.Generic;
using System.Dynamic;
using Frapid.DataAccess;
using Frapid.DataAccess.Models;
using Frapid.NPoco;

namespace Frapid.WebsiteBuilder.DataAccess
{
    public interface IEmailSubscriptionInsertViewRepository
    {
        /// <summary>
        /// Performs count on IEmailSubscriptionInsertViewRepository.
        /// </summary>
        /// <returns>Returns the number of IEmailSubscriptionInsertViewRepository.</returns>
        long Count();

        /// <summary>
        /// Return all instances of the "EmailSubscriptionInsertView" class from IEmailSubscriptionInsertViewRepository. 
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instances of "EmailSubscriptionInsertView" class.</returns>
        IEnumerable<Frapid.WebsiteBuilder.Entities.EmailSubscriptionInsertView> Get();

        /// <summary>
        /// Displayfields provide a minimal name/value context for data binding IEmailSubscriptionInsertViewRepository.
        /// </summary>
        /// <returns>Returns an enumerable name and value collection for IEmailSubscriptionInsertViewRepository.</returns>
        IEnumerable<DisplayField> GetDisplayFields();

        /// <summary>
        /// Produces a paginated result of 50 items from IEmailSubscriptionInsertViewRepository.
        /// </summary>
        /// <returns>Returns the first page of collection of "EmailSubscriptionInsertView" class.</returns>
        IEnumerable<Frapid.WebsiteBuilder.Entities.EmailSubscriptionInsertView> GetPaginatedResult();

        /// <summary>
        /// Produces a paginated result of 50 items from IEmailSubscriptionInsertViewRepository.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result.</param>
        /// <returns>Returns collection of "EmailSubscriptionInsertView" class.</returns>
        IEnumerable<Frapid.WebsiteBuilder.Entities.EmailSubscriptionInsertView> GetPaginatedResult(long pageNumber);

        List<Frapid.DataAccess.Models.Filter> GetFilters(string catalog, string filterName);

        /// <summary>
        /// Performs a filtered count on IEmailSubscriptionInsertViewRepository.
        /// </summary>
        /// <param name="filters">The list of filter conditions.</param>
        /// <returns>Returns number of rows of "EmailSubscriptionInsertView" class using the filter.</returns>
        long CountWhere(List<Frapid.DataAccess.Models.Filter> filters);

        /// <summary>
        /// Produces a paginated result of 50 items using the supplied filters from IEmailSubscriptionInsertViewRepository.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result. If you provide a negative number, the result will not be paginated.</param>
        /// <param name="filters">The list of filter conditions.</param>
        /// <returns>Returns collection of "EmailSubscriptionInsertView" class.</returns>
        IEnumerable<Frapid.WebsiteBuilder.Entities.EmailSubscriptionInsertView> GetWhere(long pageNumber, List<Frapid.DataAccess.Models.Filter> filters);

        /// <summary>
        /// Performs a filtered count on IEmailSubscriptionInsertViewRepository.
        /// </summary>
        /// <param name="filterName">The named filter.</param>
        /// <returns>Returns number of rows of "EmailSubscriptionInsertView" class using the filter.</returns>
        long CountFiltered(string filterName);

        /// <summary>
        /// Produces a paginated result of 50 items using the supplied filter name from IEmailSubscriptionInsertViewRepository.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result. If you provide a negative number, the result will not be paginated.</param>
        /// <param name="filterName">The named filter.</param>
        /// <returns>Returns collection of "EmailSubscriptionInsertView" class.</returns>
        IEnumerable<Frapid.WebsiteBuilder.Entities.EmailSubscriptionInsertView> GetFiltered(long pageNumber, string filterName);


    }
}
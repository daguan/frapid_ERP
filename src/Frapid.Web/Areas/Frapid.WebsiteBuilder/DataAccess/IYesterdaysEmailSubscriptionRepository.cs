// ReSharper disable All
using System.Collections.Generic;
using System.Dynamic;
using Frapid.DataAccess;
using Frapid.DataAccess.Models;
using Frapid.NPoco;

namespace Frapid.WebsiteBuilder.DataAccess
{
    public interface IYesterdaysEmailSubscriptionRepository
    {
        /// <summary>
        /// Performs count on IYesterdaysEmailSubscriptionRepository.
        /// </summary>
        /// <returns>Returns the number of IYesterdaysEmailSubscriptionRepository.</returns>
        long Count();

        /// <summary>
        /// Return all instances of the "YesterdaysEmailSubscription" class from IYesterdaysEmailSubscriptionRepository. 
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instances of "YesterdaysEmailSubscription" class.</returns>
        IEnumerable<Frapid.WebsiteBuilder.Entities.YesterdaysEmailSubscription> Get();



        /// <summary>
        /// Produces a paginated result of 50 items from IYesterdaysEmailSubscriptionRepository.
        /// </summary>
        /// <returns>Returns the first page of collection of "YesterdaysEmailSubscription" class.</returns>
        IEnumerable<Frapid.WebsiteBuilder.Entities.YesterdaysEmailSubscription> GetPaginatedResult();

        /// <summary>
        /// Produces a paginated result of 50 items from IYesterdaysEmailSubscriptionRepository.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result.</param>
        /// <returns>Returns collection of "YesterdaysEmailSubscription" class.</returns>
        IEnumerable<Frapid.WebsiteBuilder.Entities.YesterdaysEmailSubscription> GetPaginatedResult(long pageNumber);

        List<Frapid.DataAccess.Models.Filter> GetFilters(string catalog, string filterName);

        /// <summary>
        /// Performs a filtered count on IYesterdaysEmailSubscriptionRepository.
        /// </summary>
        /// <param name="filters">The list of filter conditions.</param>
        /// <returns>Returns number of rows of "YesterdaysEmailSubscription" class using the filter.</returns>
        long CountWhere(List<Frapid.DataAccess.Models.Filter> filters);

        /// <summary>
        /// Produces a paginated result of 50 items using the supplied filters from IYesterdaysEmailSubscriptionRepository.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result. If you provide a negative number, the result will not be paginated.</param>
        /// <param name="filters">The list of filter conditions.</param>
        /// <returns>Returns collection of "YesterdaysEmailSubscription" class.</returns>
        IEnumerable<Frapid.WebsiteBuilder.Entities.YesterdaysEmailSubscription> GetWhere(long pageNumber, List<Frapid.DataAccess.Models.Filter> filters);

        /// <summary>
        /// Performs a filtered count on IYesterdaysEmailSubscriptionRepository.
        /// </summary>
        /// <param name="filterName">The named filter.</param>
        /// <returns>Returns number of rows of "YesterdaysEmailSubscription" class using the filter.</returns>
        long CountFiltered(string filterName);

        /// <summary>
        /// Produces a paginated result of 50 items using the supplied filter name from IYesterdaysEmailSubscriptionRepository.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result. If you provide a negative number, the result will not be paginated.</param>
        /// <param name="filterName">The named filter.</param>
        /// <returns>Returns collection of "YesterdaysEmailSubscription" class.</returns>
        IEnumerable<Frapid.WebsiteBuilder.Entities.YesterdaysEmailSubscription> GetFiltered(long pageNumber, string filterName);


    }
}
// ReSharper disable All
using System.Collections.Generic;
using System.Dynamic;
using Frapid.DataAccess;
using Frapid.DataAccess.Models;
using Frapid.NPoco;

namespace Frapid.WebsiteBuilder.DataAccess
{
    public interface IPublishedContentViewRepository
    {
        /// <summary>
        /// Performs count on IPublishedContentViewRepository.
        /// </summary>
        /// <returns>Returns the number of IPublishedContentViewRepository.</returns>
        long Count();

        /// <summary>
        /// Return all instances of the "PublishedContentView" class from IPublishedContentViewRepository. 
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instances of "PublishedContentView" class.</returns>
        IEnumerable<Frapid.WebsiteBuilder.Entities.PublishedContentView> Get();

        /// <summary>
        /// Displayfields provide a minimal name/value context for data binding IPublishedContentViewRepository.
        /// </summary>
        /// <returns>Returns an enumerable name and value collection for IPublishedContentViewRepository.</returns>
        IEnumerable<DisplayField> GetDisplayFields();

        /// <summary>
        /// Produces a paginated result of 10 items from IPublishedContentViewRepository.
        /// </summary>
        /// <returns>Returns the first page of collection of "PublishedContentView" class.</returns>
        IEnumerable<Frapid.WebsiteBuilder.Entities.PublishedContentView> GetPaginatedResult();

        /// <summary>
        /// Produces a paginated result of 10 items from IPublishedContentViewRepository.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result.</param>
        /// <returns>Returns collection of "PublishedContentView" class.</returns>
        IEnumerable<Frapid.WebsiteBuilder.Entities.PublishedContentView> GetPaginatedResult(long pageNumber);

        List<Frapid.DataAccess.Models.Filter> GetFilters(string catalog, string filterName);

        /// <summary>
        /// Performs a filtered count on IPublishedContentViewRepository.
        /// </summary>
        /// <param name="filters">The list of filter conditions.</param>
        /// <returns>Returns number of rows of "PublishedContentView" class using the filter.</returns>
        long CountWhere(List<Frapid.DataAccess.Models.Filter> filters);

        /// <summary>
        /// Produces a paginated result of 10 items using the supplied filters from IPublishedContentViewRepository.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result. If you provide a negative number, the result will not be paginated.</param>
        /// <param name="filters">The list of filter conditions.</param>
        /// <returns>Returns collection of "PublishedContentView" class.</returns>
        IEnumerable<Frapid.WebsiteBuilder.Entities.PublishedContentView> GetWhere(long pageNumber, List<Frapid.DataAccess.Models.Filter> filters);

        /// <summary>
        /// Performs a filtered count on IPublishedContentViewRepository.
        /// </summary>
        /// <param name="filterName">The named filter.</param>
        /// <returns>Returns number of rows of "PublishedContentView" class using the filter.</returns>
        long CountFiltered(string filterName);

        /// <summary>
        /// Produces a paginated result of 10 items using the supplied filter name from IPublishedContentViewRepository.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result. If you provide a negative number, the result will not be paginated.</param>
        /// <param name="filterName">The named filter.</param>
        /// <returns>Returns collection of "PublishedContentView" class.</returns>
        IEnumerable<Frapid.WebsiteBuilder.Entities.PublishedContentView> GetFiltered(long pageNumber, string filterName);


    }
}
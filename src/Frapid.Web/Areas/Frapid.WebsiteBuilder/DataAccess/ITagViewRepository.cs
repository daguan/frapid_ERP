// ReSharper disable All
using System.Collections.Generic;
using System.Dynamic;
using Frapid.DataAccess;
using Frapid.DataAccess.Models;
using Frapid.NPoco;

namespace Frapid.WebsiteBuilder.DataAccess
{
    public interface ITagViewRepository
    {
        /// <summary>
        /// Performs count on ITagViewRepository.
        /// </summary>
        /// <returns>Returns the number of ITagViewRepository.</returns>
        long Count();

        /// <summary>
        /// Return all instances of the "TagView" class from ITagViewRepository. 
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instances of "TagView" class.</returns>
        IEnumerable<Frapid.WebsiteBuilder.Entities.TagView> Get();



        /// <summary>
        /// Produces a paginated result of 10 items from ITagViewRepository.
        /// </summary>
        /// <returns>Returns the first page of collection of "TagView" class.</returns>
        IEnumerable<Frapid.WebsiteBuilder.Entities.TagView> GetPaginatedResult();

        /// <summary>
        /// Produces a paginated result of 10 items from ITagViewRepository.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result.</param>
        /// <returns>Returns collection of "TagView" class.</returns>
        IEnumerable<Frapid.WebsiteBuilder.Entities.TagView> GetPaginatedResult(long pageNumber);

        List<Frapid.DataAccess.Models.Filter> GetFilters(string catalog, string filterName);

        /// <summary>
        /// Performs a filtered count on ITagViewRepository.
        /// </summary>
        /// <param name="filters">The list of filter conditions.</param>
        /// <returns>Returns number of rows of "TagView" class using the filter.</returns>
        long CountWhere(List<Frapid.DataAccess.Models.Filter> filters);

        /// <summary>
        /// Produces a paginated result of 10 items using the supplied filters from ITagViewRepository.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result. If you provide a negative number, the result will not be paginated.</param>
        /// <param name="filters">The list of filter conditions.</param>
        /// <returns>Returns collection of "TagView" class.</returns>
        IEnumerable<Frapid.WebsiteBuilder.Entities.TagView> GetWhere(long pageNumber, List<Frapid.DataAccess.Models.Filter> filters);

        /// <summary>
        /// Performs a filtered count on ITagViewRepository.
        /// </summary>
        /// <param name="filterName">The named filter.</param>
        /// <returns>Returns number of rows of "TagView" class using the filter.</returns>
        long CountFiltered(string filterName);

        /// <summary>
        /// Produces a paginated result of 10 items using the supplied filter name from ITagViewRepository.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result. If you provide a negative number, the result will not be paginated.</param>
        /// <param name="filterName">The named filter.</param>
        /// <returns>Returns collection of "TagView" class.</returns>
        IEnumerable<Frapid.WebsiteBuilder.Entities.TagView> GetFiltered(long pageNumber, string filterName);


    }
}
using System.Collections.Generic;
using Frapid.DataAccess.Models;

namespace Frapid.WebApi.DataAccess
{
    public interface IViewRepository
    {
        /// <summary>
        /// Performs count on IViewRepository.
        /// </summary>
        /// <returns>Returns the number of IViewRepository.</returns>
        long Count();

        /// <summary>
        /// Return all instances of the rows from IViewRepository. 
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instances of rows.</returns>
        IEnumerable<dynamic> Get();

        /// <summary>
        ///     Displayfields provide a minimal name/value context for data binding IViewRepository.
        /// </summary>
        /// <returns>Returns an enumerable name and value collection for IViewRepository.</returns>
        IEnumerable<DisplayField> GetDisplayFields();

        /// <summary>
        /// Produces a paginated result of 50 items from IViewRepository.
        /// </summary>
        /// <returns>Returns the first page of collection of rows.</returns>
        IEnumerable<dynamic> GetPaginatedResult();

        /// <summary>
        /// Produces a paginated result of 50 items from IViewRepository.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result.</param>
        /// <returns>Returns collection of rows.</returns>
        IEnumerable<dynamic> GetPaginatedResult(long pageNumber);

        List<Filter> GetFilters(string database, string filterName);

        /// <summary>
        /// Performs a filtered count on IViewRepository.
        /// </summary>
        /// <param name="filters">The list of filter conditions.</param>
        /// <returns>Returns number of rows of rows using the filter.</returns>
        long CountWhere(List<Filter> filters);

        /// <summary>
        /// Produces a paginated result of 50 items using the supplied filters from IViewRepository.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result. If you provide a negative number, the result will not be paginated.</param>
        /// <param name="filters">The list of filter conditions.</param>
        /// <returns>Returns collection of rows.</returns>
        IEnumerable<dynamic> GetWhere(long pageNumber, List<Filter> filters);

        /// <summary>
        /// Performs a filtered count on IViewRepository.
        /// </summary>
        /// <param name="filterName">The named filter.</param>
        /// <returns>Returns number of rows of rows using the filter.</returns>
        long CountFiltered(string filterName);

        /// <summary>
        /// Produces a paginated result of 50 items using the supplied filter name from IViewRepository.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result. If you provide a negative number, the result will not be paginated.</param>
        /// <param name="filterName">The named filter.</param>
        /// <returns>Returns collection of rows.</returns>
        IEnumerable<dynamic> GetFiltered(long pageNumber, string filterName);
    }
}
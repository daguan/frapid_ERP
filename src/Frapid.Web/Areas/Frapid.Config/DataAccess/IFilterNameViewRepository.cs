// ReSharper disable All
using System.Collections.Generic;
using System.Dynamic;
using Frapid.DataAccess;
using Frapid.DataAccess.Models;
using Frapid.NPoco;

namespace Frapid.Config.DataAccess
{
    public interface IFilterNameViewRepository
    {
        /// <summary>
        /// Performs count on IFilterNameViewRepository.
        /// </summary>
        /// <returns>Returns the number of IFilterNameViewRepository.</returns>
        long Count();

        /// <summary>
        /// Return all instances of the "FilterNameView" class from IFilterNameViewRepository. 
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instances of "FilterNameView" class.</returns>
        IEnumerable<Frapid.Config.Entities.FilterNameView> Get();



        /// <summary>
        /// Produces a paginated result of 10 items from IFilterNameViewRepository.
        /// </summary>
        /// <returns>Returns the first page of collection of "FilterNameView" class.</returns>
        IEnumerable<Frapid.Config.Entities.FilterNameView> GetPaginatedResult();

        /// <summary>
        /// Produces a paginated result of 10 items from IFilterNameViewRepository.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result.</param>
        /// <returns>Returns collection of "FilterNameView" class.</returns>
        IEnumerable<Frapid.Config.Entities.FilterNameView> GetPaginatedResult(long pageNumber);

        List<Frapid.DataAccess.Models.Filter> GetFilters(string catalog, string filterName);

        /// <summary>
        /// Performs a filtered count on IFilterNameViewRepository.
        /// </summary>
        /// <param name="filters">The list of filter conditions.</param>
        /// <returns>Returns number of rows of "FilterNameView" class using the filter.</returns>
        long CountWhere(List<Frapid.DataAccess.Models.Filter> filters);

        /// <summary>
        /// Produces a paginated result of 10 items using the supplied filters from IFilterNameViewRepository.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result. If you provide a negative number, the result will not be paginated.</param>
        /// <param name="filters">The list of filter conditions.</param>
        /// <returns>Returns collection of "FilterNameView" class.</returns>
        IEnumerable<Frapid.Config.Entities.FilterNameView> GetWhere(long pageNumber, List<Frapid.DataAccess.Models.Filter> filters);

        /// <summary>
        /// Performs a filtered count on IFilterNameViewRepository.
        /// </summary>
        /// <param name="filterName">The named filter.</param>
        /// <returns>Returns number of rows of "FilterNameView" class using the filter.</returns>
        long CountFiltered(string filterName);

        /// <summary>
        /// Produces a paginated result of 10 items using the supplied filter name from IFilterNameViewRepository.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result. If you provide a negative number, the result will not be paginated.</param>
        /// <param name="filterName">The named filter.</param>
        /// <returns>Returns collection of "FilterNameView" class.</returns>
        IEnumerable<Frapid.Config.Entities.FilterNameView> GetFiltered(long pageNumber, string filterName);


    }
}
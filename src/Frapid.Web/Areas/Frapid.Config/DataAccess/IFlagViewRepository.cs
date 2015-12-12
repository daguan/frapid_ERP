// ReSharper disable All
using System.Collections.Generic;
using System.Dynamic;
using Frapid.DataAccess;
using Frapid.DataAccess.Models;
using Frapid.NPoco;

namespace Frapid.Config.DataAccess
{
    public interface IFlagViewRepository
    {
        /// <summary>
        /// Performs count on IFlagViewRepository.
        /// </summary>
        /// <returns>Returns the number of IFlagViewRepository.</returns>
        long Count();

        /// <summary>
        /// Return all instances of the "FlagView" class from IFlagViewRepository. 
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instances of "FlagView" class.</returns>
        IEnumerable<Frapid.Config.Entities.FlagView> Get();

        /// <summary>
        /// Displayfields provide a minimal name/value context for data binding IFlagViewRepository.
        /// </summary>
        /// <returns>Returns an enumerable name and value collection for IFlagViewRepository.</returns>
        IEnumerable<DisplayField> GetDisplayFields();

        /// <summary>
        /// Produces a paginated result of 10 items from IFlagViewRepository.
        /// </summary>
        /// <returns>Returns the first page of collection of "FlagView" class.</returns>
        IEnumerable<Frapid.Config.Entities.FlagView> GetPaginatedResult();

        /// <summary>
        /// Produces a paginated result of 10 items from IFlagViewRepository.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result.</param>
        /// <returns>Returns collection of "FlagView" class.</returns>
        IEnumerable<Frapid.Config.Entities.FlagView> GetPaginatedResult(long pageNumber);

        List<Frapid.DataAccess.Models.Filter> GetFilters(string catalog, string filterName);

        /// <summary>
        /// Performs a filtered count on IFlagViewRepository.
        /// </summary>
        /// <param name="filters">The list of filter conditions.</param>
        /// <returns>Returns number of rows of "FlagView" class using the filter.</returns>
        long CountWhere(List<Frapid.DataAccess.Models.Filter> filters);

        /// <summary>
        /// Produces a paginated result of 10 items using the supplied filters from IFlagViewRepository.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result. If you provide a negative number, the result will not be paginated.</param>
        /// <param name="filters">The list of filter conditions.</param>
        /// <returns>Returns collection of "FlagView" class.</returns>
        IEnumerable<Frapid.Config.Entities.FlagView> GetWhere(long pageNumber, List<Frapid.DataAccess.Models.Filter> filters);

        /// <summary>
        /// Performs a filtered count on IFlagViewRepository.
        /// </summary>
        /// <param name="filterName">The named filter.</param>
        /// <returns>Returns number of rows of "FlagView" class using the filter.</returns>
        long CountFiltered(string filterName);

        /// <summary>
        /// Produces a paginated result of 10 items using the supplied filter name from IFlagViewRepository.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result. If you provide a negative number, the result will not be paginated.</param>
        /// <param name="filterName">The named filter.</param>
        /// <returns>Returns collection of "FlagView" class.</returns>
        IEnumerable<Frapid.Config.Entities.FlagView> GetFiltered(long pageNumber, string filterName);


        IEnumerable<Frapid.Config.Entities.FlagView> Get(string resource, int userId, object[] resourceIds);

    }
}
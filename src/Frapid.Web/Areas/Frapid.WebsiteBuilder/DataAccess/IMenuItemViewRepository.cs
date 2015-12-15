// ReSharper disable All
using System.Collections.Generic;
using System.Dynamic;
using Frapid.DataAccess;
using Frapid.DataAccess.Models;
using Frapid.NPoco;

namespace Frapid.WebsiteBuilder.DataAccess
{
    public interface IMenuItemViewRepository
    {
        /// <summary>
        /// Performs count on IMenuItemViewRepository.
        /// </summary>
        /// <returns>Returns the number of IMenuItemViewRepository.</returns>
        long Count();

        /// <summary>
        /// Return all instances of the "MenuItemView" class from IMenuItemViewRepository. 
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instances of "MenuItemView" class.</returns>
        IEnumerable<Frapid.WebsiteBuilder.Entities.MenuItemView> Get();

        /// <summary>
        /// Displayfields provide a minimal name/value context for data binding IMenuItemViewRepository.
        /// </summary>
        /// <returns>Returns an enumerable name and value collection for IMenuItemViewRepository.</returns>
        IEnumerable<DisplayField> GetDisplayFields();

        /// <summary>
        /// Produces a paginated result of 10 items from IMenuItemViewRepository.
        /// </summary>
        /// <returns>Returns the first page of collection of "MenuItemView" class.</returns>
        IEnumerable<Frapid.WebsiteBuilder.Entities.MenuItemView> GetPaginatedResult();

        /// <summary>
        /// Produces a paginated result of 10 items from IMenuItemViewRepository.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result.</param>
        /// <returns>Returns collection of "MenuItemView" class.</returns>
        IEnumerable<Frapid.WebsiteBuilder.Entities.MenuItemView> GetPaginatedResult(long pageNumber);

        List<Frapid.DataAccess.Models.Filter> GetFilters(string catalog, string filterName);

        /// <summary>
        /// Performs a filtered count on IMenuItemViewRepository.
        /// </summary>
        /// <param name="filters">The list of filter conditions.</param>
        /// <returns>Returns number of rows of "MenuItemView" class using the filter.</returns>
        long CountWhere(List<Frapid.DataAccess.Models.Filter> filters);

        /// <summary>
        /// Produces a paginated result of 10 items using the supplied filters from IMenuItemViewRepository.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result. If you provide a negative number, the result will not be paginated.</param>
        /// <param name="filters">The list of filter conditions.</param>
        /// <returns>Returns collection of "MenuItemView" class.</returns>
        IEnumerable<Frapid.WebsiteBuilder.Entities.MenuItemView> GetWhere(long pageNumber, List<Frapid.DataAccess.Models.Filter> filters);

        /// <summary>
        /// Performs a filtered count on IMenuItemViewRepository.
        /// </summary>
        /// <param name="filterName">The named filter.</param>
        /// <returns>Returns number of rows of "MenuItemView" class using the filter.</returns>
        long CountFiltered(string filterName);

        /// <summary>
        /// Produces a paginated result of 10 items using the supplied filter name from IMenuItemViewRepository.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result. If you provide a negative number, the result will not be paginated.</param>
        /// <param name="filterName">The named filter.</param>
        /// <returns>Returns collection of "MenuItemView" class.</returns>
        IEnumerable<Frapid.WebsiteBuilder.Entities.MenuItemView> GetFiltered(long pageNumber, string filterName);


    }
}
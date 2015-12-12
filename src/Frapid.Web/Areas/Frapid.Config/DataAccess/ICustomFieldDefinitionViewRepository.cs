// ReSharper disable All
using System.Collections.Generic;
using System.Dynamic;
using Frapid.DataAccess;
using Frapid.DataAccess.Models;
using Frapid.NPoco;

namespace Frapid.Config.DataAccess
{
    public interface ICustomFieldDefinitionViewRepository
    {
        /// <summary>
        /// Performs count on ICustomFieldDefinitionViewRepository.
        /// </summary>
        /// <returns>Returns the number of ICustomFieldDefinitionViewRepository.</returns>
        long Count();

        /// <summary>
        /// Return all instances of the "CustomFieldDefinitionView" class from ICustomFieldDefinitionViewRepository. 
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instances of "CustomFieldDefinitionView" class.</returns>
        IEnumerable<Frapid.Config.Entities.CustomFieldDefinitionView> Get();



        /// <summary>
        /// Produces a paginated result of 10 items from ICustomFieldDefinitionViewRepository.
        /// </summary>
        /// <returns>Returns the first page of collection of "CustomFieldDefinitionView" class.</returns>
        IEnumerable<Frapid.Config.Entities.CustomFieldDefinitionView> GetPaginatedResult();

        /// <summary>
        /// Produces a paginated result of 10 items from ICustomFieldDefinitionViewRepository.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result.</param>
        /// <returns>Returns collection of "CustomFieldDefinitionView" class.</returns>
        IEnumerable<Frapid.Config.Entities.CustomFieldDefinitionView> GetPaginatedResult(long pageNumber);

        List<Frapid.DataAccess.Models.Filter> GetFilters(string catalog, string filterName);

        /// <summary>
        /// Performs a filtered count on ICustomFieldDefinitionViewRepository.
        /// </summary>
        /// <param name="filters">The list of filter conditions.</param>
        /// <returns>Returns number of rows of "CustomFieldDefinitionView" class using the filter.</returns>
        long CountWhere(List<Frapid.DataAccess.Models.Filter> filters);

        /// <summary>
        /// Produces a paginated result of 10 items using the supplied filters from ICustomFieldDefinitionViewRepository.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result. If you provide a negative number, the result will not be paginated.</param>
        /// <param name="filters">The list of filter conditions.</param>
        /// <returns>Returns collection of "CustomFieldDefinitionView" class.</returns>
        IEnumerable<Frapid.Config.Entities.CustomFieldDefinitionView> GetWhere(long pageNumber, List<Frapid.DataAccess.Models.Filter> filters);

        /// <summary>
        /// Performs a filtered count on ICustomFieldDefinitionViewRepository.
        /// </summary>
        /// <param name="filterName">The named filter.</param>
        /// <returns>Returns number of rows of "CustomFieldDefinitionView" class using the filter.</returns>
        long CountFiltered(string filterName);

        /// <summary>
        /// Produces a paginated result of 10 items using the supplied filter name from ICustomFieldDefinitionViewRepository.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result. If you provide a negative number, the result will not be paginated.</param>
        /// <param name="filterName">The named filter.</param>
        /// <returns>Returns collection of "CustomFieldDefinitionView" class.</returns>
        IEnumerable<Frapid.Config.Entities.CustomFieldDefinitionView> GetFiltered(long pageNumber, string filterName);


    }
}
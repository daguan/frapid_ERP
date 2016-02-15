// ReSharper disable All
using System.Collections.Generic;
using System.Dynamic;
using Frapid.DataAccess;
using Frapid.DataAccess.Models;
using Frapid.NPoco;

namespace Frapid.Account.DataAccess
{
    public interface ISignInViewRepository
    {
        /// <summary>
        /// Performs count on ISignInViewRepository.
        /// </summary>
        /// <returns>Returns the number of ISignInViewRepository.</returns>
        long Count();

        /// <summary>
        /// Return all instances of the "SignInView" class from ISignInViewRepository. 
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instances of "SignInView" class.</returns>
        IEnumerable<Frapid.Account.Entities.SignInView> Get();

        /// <summary>
        /// Displayfields provide a minimal name/value context for data binding ISignInViewRepository.
        /// </summary>
        /// <returns>Returns an enumerable name and value collection for ISignInViewRepository.</returns>
        IEnumerable<DisplayField> GetDisplayFields();

        /// <summary>
        /// Produces a paginated result of 50 items from ISignInViewRepository.
        /// </summary>
        /// <returns>Returns the first page of collection of "SignInView" class.</returns>
        IEnumerable<Frapid.Account.Entities.SignInView> GetPaginatedResult();

        /// <summary>
        /// Produces a paginated result of 50 items from ISignInViewRepository.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result.</param>
        /// <returns>Returns collection of "SignInView" class.</returns>
        IEnumerable<Frapid.Account.Entities.SignInView> GetPaginatedResult(long pageNumber);

        List<Frapid.DataAccess.Models.Filter> GetFilters(string catalog, string filterName);

        /// <summary>
        /// Performs a filtered count on ISignInViewRepository.
        /// </summary>
        /// <param name="filters">The list of filter conditions.</param>
        /// <returns>Returns number of rows of "SignInView" class using the filter.</returns>
        long CountWhere(List<Frapid.DataAccess.Models.Filter> filters);

        /// <summary>
        /// Produces a paginated result of 50 items using the supplied filters from ISignInViewRepository.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result. If you provide a negative number, the result will not be paginated.</param>
        /// <param name="filters">The list of filter conditions.</param>
        /// <returns>Returns collection of "SignInView" class.</returns>
        IEnumerable<Frapid.Account.Entities.SignInView> GetWhere(long pageNumber, List<Frapid.DataAccess.Models.Filter> filters);

        /// <summary>
        /// Performs a filtered count on ISignInViewRepository.
        /// </summary>
        /// <param name="filterName">The named filter.</param>
        /// <returns>Returns number of rows of "SignInView" class using the filter.</returns>
        long CountFiltered(string filterName);

        /// <summary>
        /// Produces a paginated result of 50 items using the supplied filter name from ISignInViewRepository.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result. If you provide a negative number, the result will not be paginated.</param>
        /// <param name="filterName">The named filter.</param>
        /// <returns>Returns collection of "SignInView" class.</returns>
        IEnumerable<Frapid.Account.Entities.SignInView> GetFiltered(long pageNumber, string filterName);


    }
}
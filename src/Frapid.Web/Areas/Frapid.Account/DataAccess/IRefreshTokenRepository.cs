// ReSharper disable All
using System.Collections.Generic;
using System.Dynamic;
using Frapid.DataAccess;
using Frapid.DataAccess.Models;

namespace Frapid.Account.DataAccess
{
    public interface IRefreshTokenRepository
    {
        /// <summary>
        /// Counts the number of RefreshToken in IRefreshTokenRepository.
        /// </summary>
        /// <returns>Returns the count of IRefreshTokenRepository.</returns>
        long Count();

        /// <summary>
        /// Returns all instances of RefreshToken. 
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instances of RefreshToken.</returns>
        IEnumerable<Frapid.Account.Entities.RefreshToken> GetAll();

        /// <summary>
        /// Returns all instances of RefreshToken to export. 
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instances of RefreshToken.</returns>
        IEnumerable<dynamic> Export();

        /// <summary>
        /// Returns a single instance of the RefreshToken against refreshTokenId. 
        /// </summary>
        /// <param name="refreshTokenId">The column "refresh_token_id" parameter used on where filter.</param>
        /// <returns>Returns a non-live, non-mapped instance of RefreshToken.</returns>
        Frapid.Account.Entities.RefreshToken Get(System.Guid refreshTokenId);

        /// <summary>
        /// Gets the first record of RefreshToken.
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instance of RefreshToken.</returns>
        Frapid.Account.Entities.RefreshToken GetFirst();

        /// <summary>
        /// Gets the previous record of RefreshToken sorted by refreshTokenId. 
        /// </summary>
        /// <param name="refreshTokenId">The column "refresh_token_id" parameter used to find the previous record.</param>
        /// <returns>Returns a non-live, non-mapped instance of RefreshToken.</returns>
        Frapid.Account.Entities.RefreshToken GetPrevious(System.Guid refreshTokenId);

        /// <summary>
        /// Gets the next record of RefreshToken sorted by refreshTokenId. 
        /// </summary>
        /// <param name="refreshTokenId">The column "refresh_token_id" parameter used to find the next record.</param>
        /// <returns>Returns a non-live, non-mapped instance of RefreshToken.</returns>
        Frapid.Account.Entities.RefreshToken GetNext(System.Guid refreshTokenId);

        /// <summary>
        /// Gets the last record of RefreshToken.
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instance of RefreshToken.</returns>
        Frapid.Account.Entities.RefreshToken GetLast();

        /// <summary>
        /// Returns multiple instances of the RefreshToken against refreshTokenIds. 
        /// </summary>
        /// <param name="refreshTokenIds">Array of column "refresh_token_id" parameter used on where filter.</param>
        /// <returns>Returns a non-live, non-mapped collection of RefreshToken.</returns>
        IEnumerable<Frapid.Account.Entities.RefreshToken> Get(System.Guid[] refreshTokenIds);

        /// <summary>
        /// Custom fields are user defined form elements for IRefreshTokenRepository.
        /// </summary>
        /// <returns>Returns an enumerable custom field collection for RefreshToken.</returns>
        IEnumerable<Frapid.DataAccess.Models.CustomField> GetCustomFields(string resourceId);

        /// <summary>
        /// Displayfields provide a minimal name/value context for data binding RefreshToken.
        /// </summary>
        /// <returns>Returns an enumerable name and value collection for RefreshToken.</returns>
        IEnumerable<Frapid.DataAccess.Models.DisplayField> GetDisplayFields();

        /// <summary>
        /// Inserts the instance of RefreshToken class to IRefreshTokenRepository.
        /// </summary>
        /// <param name="refreshToken">The instance of RefreshToken class to insert or update.</param>
        /// <param name="customFields">The custom field collection.</param>
        object AddOrEdit(dynamic refreshToken, List<Frapid.DataAccess.Models.CustomField> customFields);

        /// <summary>
        /// Inserts the instance of RefreshToken class to IRefreshTokenRepository.
        /// </summary>
        /// <param name="refreshToken">The instance of RefreshToken class to insert.</param>
        object Add(dynamic refreshToken);

        /// <summary>
        /// Inserts or updates multiple instances of RefreshToken class to IRefreshTokenRepository.;
        /// </summary>
        /// <param name="refreshTokens">List of RefreshToken class to import.</param>
        /// <returns>Returns list of inserted object ids.</returns>
        List<object> BulkImport(List<ExpandoObject> refreshTokens);


        /// <summary>
        /// Updates IRefreshTokenRepository with an instance of RefreshToken class against the primary key value.
        /// </summary>
        /// <param name="refreshToken">The instance of RefreshToken class to update.</param>
        /// <param name="refreshTokenId">The value of the column "refresh_token_id" which will be updated.</param>
        void Update(dynamic refreshToken, System.Guid refreshTokenId);

        /// <summary>
        /// Deletes RefreshToken from  IRefreshTokenRepository against the primary key value.
        /// </summary>
        /// <param name="refreshTokenId">The value of the column "refresh_token_id" which will be deleted.</param>
        void Delete(System.Guid refreshTokenId);

        /// <summary>
        /// Produces a paginated result of 10 RefreshToken classes.
        /// </summary>
        /// <returns>Returns the first page of collection of RefreshToken class.</returns>
        IEnumerable<Frapid.Account.Entities.RefreshToken> GetPaginatedResult();

        /// <summary>
        /// Produces a paginated result of 10 RefreshToken classes.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result.</param>
        /// <returns>Returns collection of RefreshToken class.</returns>
        IEnumerable<Frapid.Account.Entities.RefreshToken> GetPaginatedResult(long pageNumber);

        List<Frapid.DataAccess.Models.Filter> GetFilters(string catalog, string filterName);

        /// <summary>
        /// Performs a filtered count on IRefreshTokenRepository.
        /// </summary>
        /// <param name="filters">The list of filter conditions.</param>
        /// <returns>Returns number of rows of RefreshToken class using the filter.</returns>
        long CountWhere(List<Frapid.DataAccess.Models.Filter> filters);

        /// <summary>
        /// Performs a filtered pagination against IRefreshTokenRepository producing result of 10 items.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result. If you provide a negative number, the result will not be paginated.</param>
        /// <param name="filters">The list of filter conditions.</param>
        /// <returns>Returns collection of RefreshToken class.</returns>
        IEnumerable<Frapid.Account.Entities.RefreshToken> GetWhere(long pageNumber, List<Frapid.DataAccess.Models.Filter> filters);

        /// <summary>
        /// Performs a filtered count on IRefreshTokenRepository.
        /// </summary>
        /// <param name="filterName">The named filter.</param>
        /// <returns>Returns number of RefreshToken class using the filter.</returns>
        long CountFiltered(string filterName);

        /// <summary>
        /// Gets a filtered result of IRefreshTokenRepository producing a paginated result of 10.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result. If you provide a negative number, the result will not be paginated.</param>
        /// <param name="filterName">The named filter.</param>
        /// <returns>Returns collection of RefreshToken class.</returns>
        IEnumerable<Frapid.Account.Entities.RefreshToken> GetFiltered(long pageNumber, string filterName);



    }
}
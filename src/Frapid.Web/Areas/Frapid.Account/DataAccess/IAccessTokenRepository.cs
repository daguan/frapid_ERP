// ReSharper disable All
using System.Collections.Generic;
using System.Dynamic;
using Frapid.DataAccess;
using Frapid.DataAccess.Models;

namespace Frapid.Account.DataAccess
{
    public interface IAccessTokenRepository
    {
        /// <summary>
        /// Counts the number of AccessToken in IAccessTokenRepository.
        /// </summary>
        /// <returns>Returns the count of IAccessTokenRepository.</returns>
        long Count();

        /// <summary>
        /// Returns all instances of AccessToken. 
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instances of AccessToken.</returns>
        IEnumerable<Frapid.Account.Entities.AccessToken> GetAll();

        /// <summary>
        /// Returns all instances of AccessToken to export. 
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instances of AccessToken.</returns>
        IEnumerable<dynamic> Export();

        /// <summary>
        /// Returns a single instance of the AccessToken against accessTokenId. 
        /// </summary>
        /// <param name="accessTokenId">The column "access_token_id" parameter used on where filter.</param>
        /// <returns>Returns a non-live, non-mapped instance of AccessToken.</returns>
        Frapid.Account.Entities.AccessToken Get(System.Guid accessTokenId);

        /// <summary>
        /// Gets the first record of AccessToken.
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instance of AccessToken.</returns>
        Frapid.Account.Entities.AccessToken GetFirst();

        /// <summary>
        /// Gets the previous record of AccessToken sorted by accessTokenId. 
        /// </summary>
        /// <param name="accessTokenId">The column "access_token_id" parameter used to find the previous record.</param>
        /// <returns>Returns a non-live, non-mapped instance of AccessToken.</returns>
        Frapid.Account.Entities.AccessToken GetPrevious(System.Guid accessTokenId);

        /// <summary>
        /// Gets the next record of AccessToken sorted by accessTokenId. 
        /// </summary>
        /// <param name="accessTokenId">The column "access_token_id" parameter used to find the next record.</param>
        /// <returns>Returns a non-live, non-mapped instance of AccessToken.</returns>
        Frapid.Account.Entities.AccessToken GetNext(System.Guid accessTokenId);

        /// <summary>
        /// Gets the last record of AccessToken.
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instance of AccessToken.</returns>
        Frapid.Account.Entities.AccessToken GetLast();

        /// <summary>
        /// Returns multiple instances of the AccessToken against accessTokenIds. 
        /// </summary>
        /// <param name="accessTokenIds">Array of column "access_token_id" parameter used on where filter.</param>
        /// <returns>Returns a non-live, non-mapped collection of AccessToken.</returns>
        IEnumerable<Frapid.Account.Entities.AccessToken> Get(System.Guid[] accessTokenIds);

        /// <summary>
        /// Custom fields are user defined form elements for IAccessTokenRepository.
        /// </summary>
        /// <returns>Returns an enumerable custom field collection for AccessToken.</returns>
        IEnumerable<Frapid.DataAccess.Models.CustomField> GetCustomFields(string resourceId);

        /// <summary>
        /// Displayfields provide a minimal name/value context for data binding AccessToken.
        /// </summary>
        /// <returns>Returns an enumerable name and value collection for AccessToken.</returns>
        IEnumerable<Frapid.DataAccess.Models.DisplayField> GetDisplayFields();

        /// <summary>
        /// Inserts the instance of AccessToken class to IAccessTokenRepository.
        /// </summary>
        /// <param name="accessToken">The instance of AccessToken class to insert or update.</param>
        /// <param name="customFields">The custom field collection.</param>
        object AddOrEdit(dynamic accessToken, List<Frapid.DataAccess.Models.CustomField> customFields);

        /// <summary>
        /// Inserts the instance of AccessToken class to IAccessTokenRepository.
        /// </summary>
        /// <param name="accessToken">The instance of AccessToken class to insert.</param>
        object Add(dynamic accessToken);

        /// <summary>
        /// Inserts or updates multiple instances of AccessToken class to IAccessTokenRepository.;
        /// </summary>
        /// <param name="accessTokens">List of AccessToken class to import.</param>
        /// <returns>Returns list of inserted object ids.</returns>
        List<object> BulkImport(List<ExpandoObject> accessTokens);


        /// <summary>
        /// Updates IAccessTokenRepository with an instance of AccessToken class against the primary key value.
        /// </summary>
        /// <param name="accessToken">The instance of AccessToken class to update.</param>
        /// <param name="accessTokenId">The value of the column "access_token_id" which will be updated.</param>
        void Update(dynamic accessToken, System.Guid accessTokenId);

        /// <summary>
        /// Deletes AccessToken from  IAccessTokenRepository against the primary key value.
        /// </summary>
        /// <param name="accessTokenId">The value of the column "access_token_id" which will be deleted.</param>
        void Delete(System.Guid accessTokenId);

        /// <summary>
        /// Produces a paginated result of 10 AccessToken classes.
        /// </summary>
        /// <returns>Returns the first page of collection of AccessToken class.</returns>
        IEnumerable<Frapid.Account.Entities.AccessToken> GetPaginatedResult();

        /// <summary>
        /// Produces a paginated result of 10 AccessToken classes.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result.</param>
        /// <returns>Returns collection of AccessToken class.</returns>
        IEnumerable<Frapid.Account.Entities.AccessToken> GetPaginatedResult(long pageNumber);

        List<Frapid.DataAccess.Models.Filter> GetFilters(string catalog, string filterName);

        /// <summary>
        /// Performs a filtered count on IAccessTokenRepository.
        /// </summary>
        /// <param name="filters">The list of filter conditions.</param>
        /// <returns>Returns number of rows of AccessToken class using the filter.</returns>
        long CountWhere(List<Frapid.DataAccess.Models.Filter> filters);

        /// <summary>
        /// Performs a filtered pagination against IAccessTokenRepository producing result of 10 items.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result. If you provide a negative number, the result will not be paginated.</param>
        /// <param name="filters">The list of filter conditions.</param>
        /// <returns>Returns collection of AccessToken class.</returns>
        IEnumerable<Frapid.Account.Entities.AccessToken> GetWhere(long pageNumber, List<Frapid.DataAccess.Models.Filter> filters);

        /// <summary>
        /// Performs a filtered count on IAccessTokenRepository.
        /// </summary>
        /// <param name="filterName">The named filter.</param>
        /// <returns>Returns number of AccessToken class using the filter.</returns>
        long CountFiltered(string filterName);

        /// <summary>
        /// Gets a filtered result of IAccessTokenRepository producing a paginated result of 10.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result. If you provide a negative number, the result will not be paginated.</param>
        /// <param name="filterName">The named filter.</param>
        /// <returns>Returns collection of AccessToken class.</returns>
        IEnumerable<Frapid.Account.Entities.AccessToken> GetFiltered(long pageNumber, string filterName);



    }
}
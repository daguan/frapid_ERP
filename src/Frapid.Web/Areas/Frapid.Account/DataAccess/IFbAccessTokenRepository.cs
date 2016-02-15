// ReSharper disable All
using System.Collections.Generic;
using System.Dynamic;
using Frapid.DataAccess;
using Frapid.DataAccess.Models;

namespace Frapid.Account.DataAccess
{
    public interface IFbAccessTokenRepository
    {
        /// <summary>
        /// Counts the number of FbAccessToken in IFbAccessTokenRepository.
        /// </summary>
        /// <returns>Returns the count of IFbAccessTokenRepository.</returns>
        long Count();

        /// <summary>
        /// Returns all instances of FbAccessToken. 
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instances of FbAccessToken.</returns>
        IEnumerable<Frapid.Account.Entities.FbAccessToken> GetAll();

        /// <summary>
        /// Returns all instances of FbAccessToken to export. 
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instances of FbAccessToken.</returns>
        IEnumerable<dynamic> Export();

        /// <summary>
        /// Returns a single instance of the FbAccessToken against userId. 
        /// </summary>
        /// <param name="userId">The column "user_id" parameter used on where filter.</param>
        /// <returns>Returns a non-live, non-mapped instance of FbAccessToken.</returns>
        Frapid.Account.Entities.FbAccessToken Get(int userId);

        /// <summary>
        /// Gets the first record of FbAccessToken.
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instance of FbAccessToken.</returns>
        Frapid.Account.Entities.FbAccessToken GetFirst();

        /// <summary>
        /// Gets the previous record of FbAccessToken sorted by userId. 
        /// </summary>
        /// <param name="userId">The column "user_id" parameter used to find the previous record.</param>
        /// <returns>Returns a non-live, non-mapped instance of FbAccessToken.</returns>
        Frapid.Account.Entities.FbAccessToken GetPrevious(int userId);

        /// <summary>
        /// Gets the next record of FbAccessToken sorted by userId. 
        /// </summary>
        /// <param name="userId">The column "user_id" parameter used to find the next record.</param>
        /// <returns>Returns a non-live, non-mapped instance of FbAccessToken.</returns>
        Frapid.Account.Entities.FbAccessToken GetNext(int userId);

        /// <summary>
        /// Gets the last record of FbAccessToken.
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instance of FbAccessToken.</returns>
        Frapid.Account.Entities.FbAccessToken GetLast();

        /// <summary>
        /// Returns multiple instances of the FbAccessToken against userIds. 
        /// </summary>
        /// <param name="userIds">Array of column "user_id" parameter used on where filter.</param>
        /// <returns>Returns a non-live, non-mapped collection of FbAccessToken.</returns>
        IEnumerable<Frapid.Account.Entities.FbAccessToken> Get(int[] userIds);

        /// <summary>
        /// Custom fields are user defined form elements for IFbAccessTokenRepository.
        /// </summary>
        /// <returns>Returns an enumerable custom field collection for FbAccessToken.</returns>
        IEnumerable<Frapid.DataAccess.Models.CustomField> GetCustomFields(string resourceId);

        /// <summary>
        /// Displayfields provide a minimal name/value context for data binding FbAccessToken.
        /// </summary>
        /// <returns>Returns an enumerable name and value collection for FbAccessToken.</returns>
        IEnumerable<Frapid.DataAccess.Models.DisplayField> GetDisplayFields();

        /// <summary>
        /// Inserts the instance of FbAccessToken class to IFbAccessTokenRepository.
        /// </summary>
        /// <param name="fbAccessToken">The instance of FbAccessToken class to insert or update.</param>
        /// <param name="customFields">The custom field collection.</param>
        object AddOrEdit(dynamic fbAccessToken, List<Frapid.DataAccess.Models.CustomField> customFields);

        /// <summary>
        /// Inserts the instance of FbAccessToken class to IFbAccessTokenRepository.
        /// </summary>
        /// <param name="fbAccessToken">The instance of FbAccessToken class to insert.</param>
        object Add(dynamic fbAccessToken);

        /// <summary>
        /// Inserts or updates multiple instances of FbAccessToken class to IFbAccessTokenRepository.;
        /// </summary>
        /// <param name="fbAccessTokens">List of FbAccessToken class to import.</param>
        /// <returns>Returns list of inserted object ids.</returns>
        List<object> BulkImport(List<ExpandoObject> fbAccessTokens);


        /// <summary>
        /// Updates IFbAccessTokenRepository with an instance of FbAccessToken class against the primary key value.
        /// </summary>
        /// <param name="fbAccessToken">The instance of FbAccessToken class to update.</param>
        /// <param name="userId">The value of the column "user_id" which will be updated.</param>
        void Update(dynamic fbAccessToken, int userId);

        /// <summary>
        /// Deletes FbAccessToken from  IFbAccessTokenRepository against the primary key value.
        /// </summary>
        /// <param name="userId">The value of the column "user_id" which will be deleted.</param>
        void Delete(int userId);

        /// <summary>
        /// Produces a paginated result of 50 FbAccessToken classes.
        /// </summary>
        /// <returns>Returns the first page of collection of FbAccessToken class.</returns>
        IEnumerable<Frapid.Account.Entities.FbAccessToken> GetPaginatedResult();

        /// <summary>
        /// Produces a paginated result of 50 FbAccessToken classes.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result.</param>
        /// <returns>Returns collection of FbAccessToken class.</returns>
        IEnumerable<Frapid.Account.Entities.FbAccessToken> GetPaginatedResult(long pageNumber);

        List<Frapid.DataAccess.Models.Filter> GetFilters(string catalog, string filterName);

        /// <summary>
        /// Performs a filtered count on IFbAccessTokenRepository.
        /// </summary>
        /// <param name="filters">The list of filter conditions.</param>
        /// <returns>Returns number of rows of FbAccessToken class using the filter.</returns>
        long CountWhere(List<Frapid.DataAccess.Models.Filter> filters);

        /// <summary>
        /// Performs a filtered pagination against IFbAccessTokenRepository producing result of 50 items.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result. If you provide a negative number, the result will not be paginated.</param>
        /// <param name="filters">The list of filter conditions.</param>
        /// <returns>Returns collection of FbAccessToken class.</returns>
        IEnumerable<Frapid.Account.Entities.FbAccessToken> GetWhere(long pageNumber, List<Frapid.DataAccess.Models.Filter> filters);

        /// <summary>
        /// Performs a filtered count on IFbAccessTokenRepository.
        /// </summary>
        /// <param name="filterName">The named filter.</param>
        /// <returns>Returns number of FbAccessToken class using the filter.</returns>
        long CountFiltered(string filterName);

        /// <summary>
        /// Gets a filtered result of IFbAccessTokenRepository producing a paginated result of 50.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result. If you provide a negative number, the result will not be paginated.</param>
        /// <param name="filterName">The named filter.</param>
        /// <returns>Returns collection of FbAccessToken class.</returns>
        IEnumerable<Frapid.Account.Entities.FbAccessToken> GetFiltered(long pageNumber, string filterName);



    }
}
// ReSharper disable All
using System.Collections.Generic;
using System.Dynamic;
using Frapid.DataAccess;
using Frapid.DataAccess.Models;

namespace Frapid.Account.DataAccess
{
    public interface ILoginRepository
    {
        /// <summary>
        /// Counts the number of Login in ILoginRepository.
        /// </summary>
        /// <returns>Returns the count of ILoginRepository.</returns>
        long Count();

        /// <summary>
        /// Returns all instances of Login. 
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instances of Login.</returns>
        IEnumerable<Frapid.Account.Entities.Login> GetAll();

        /// <summary>
        /// Returns all instances of Login to export. 
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instances of Login.</returns>
        IEnumerable<dynamic> Export();

        /// <summary>
        /// Returns a single instance of the Login against loginId. 
        /// </summary>
        /// <param name="loginId">The column "login_id" parameter used on where filter.</param>
        /// <returns>Returns a non-live, non-mapped instance of Login.</returns>
        Frapid.Account.Entities.Login Get(long loginId);

        /// <summary>
        /// Gets the first record of Login.
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instance of Login.</returns>
        Frapid.Account.Entities.Login GetFirst();

        /// <summary>
        /// Gets the previous record of Login sorted by loginId. 
        /// </summary>
        /// <param name="loginId">The column "login_id" parameter used to find the previous record.</param>
        /// <returns>Returns a non-live, non-mapped instance of Login.</returns>
        Frapid.Account.Entities.Login GetPrevious(long loginId);

        /// <summary>
        /// Gets the next record of Login sorted by loginId. 
        /// </summary>
        /// <param name="loginId">The column "login_id" parameter used to find the next record.</param>
        /// <returns>Returns a non-live, non-mapped instance of Login.</returns>
        Frapid.Account.Entities.Login GetNext(long loginId);

        /// <summary>
        /// Gets the last record of Login.
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instance of Login.</returns>
        Frapid.Account.Entities.Login GetLast();

        /// <summary>
        /// Returns multiple instances of the Login against loginIds. 
        /// </summary>
        /// <param name="loginIds">Array of column "login_id" parameter used on where filter.</param>
        /// <returns>Returns a non-live, non-mapped collection of Login.</returns>
        IEnumerable<Frapid.Account.Entities.Login> Get(long[] loginIds);

        /// <summary>
        /// Custom fields are user defined form elements for ILoginRepository.
        /// </summary>
        /// <returns>Returns an enumerable custom field collection for Login.</returns>
        IEnumerable<Frapid.DataAccess.Models.CustomField> GetCustomFields(string resourceId);

        /// <summary>
        /// Displayfields provide a minimal name/value context for data binding Login.
        /// </summary>
        /// <returns>Returns an enumerable name and value collection for Login.</returns>
        IEnumerable<Frapid.DataAccess.Models.DisplayField> GetDisplayFields();

        /// <summary>
        /// Inserts the instance of Login class to ILoginRepository.
        /// </summary>
        /// <param name="login">The instance of Login class to insert or update.</param>
        /// <param name="customFields">The custom field collection.</param>
        object AddOrEdit(dynamic login, List<Frapid.DataAccess.Models.CustomField> customFields);

        /// <summary>
        /// Inserts the instance of Login class to ILoginRepository.
        /// </summary>
        /// <param name="login">The instance of Login class to insert.</param>
        object Add(dynamic login);

        /// <summary>
        /// Inserts or updates multiple instances of Login class to ILoginRepository.;
        /// </summary>
        /// <param name="logins">List of Login class to import.</param>
        /// <returns>Returns list of inserted object ids.</returns>
        List<object> BulkImport(List<ExpandoObject> logins);


        /// <summary>
        /// Updates ILoginRepository with an instance of Login class against the primary key value.
        /// </summary>
        /// <param name="login">The instance of Login class to update.</param>
        /// <param name="loginId">The value of the column "login_id" which will be updated.</param>
        void Update(dynamic login, long loginId);

        /// <summary>
        /// Deletes Login from  ILoginRepository against the primary key value.
        /// </summary>
        /// <param name="loginId">The value of the column "login_id" which will be deleted.</param>
        void Delete(long loginId);

        /// <summary>
        /// Produces a paginated result of 10 Login classes.
        /// </summary>
        /// <returns>Returns the first page of collection of Login class.</returns>
        IEnumerable<Frapid.Account.Entities.Login> GetPaginatedResult();

        /// <summary>
        /// Produces a paginated result of 10 Login classes.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result.</param>
        /// <returns>Returns collection of Login class.</returns>
        IEnumerable<Frapid.Account.Entities.Login> GetPaginatedResult(long pageNumber);

        List<Frapid.DataAccess.Models.Filter> GetFilters(string catalog, string filterName);

        /// <summary>
        /// Performs a filtered count on ILoginRepository.
        /// </summary>
        /// <param name="filters">The list of filter conditions.</param>
        /// <returns>Returns number of rows of Login class using the filter.</returns>
        long CountWhere(List<Frapid.DataAccess.Models.Filter> filters);

        /// <summary>
        /// Performs a filtered pagination against ILoginRepository producing result of 10 items.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result. If you provide a negative number, the result will not be paginated.</param>
        /// <param name="filters">The list of filter conditions.</param>
        /// <returns>Returns collection of Login class.</returns>
        IEnumerable<Frapid.Account.Entities.Login> GetWhere(long pageNumber, List<Frapid.DataAccess.Models.Filter> filters);

        /// <summary>
        /// Performs a filtered count on ILoginRepository.
        /// </summary>
        /// <param name="filterName">The named filter.</param>
        /// <returns>Returns number of Login class using the filter.</returns>
        long CountFiltered(string filterName);

        /// <summary>
        /// Gets a filtered result of ILoginRepository producing a paginated result of 10.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result. If you provide a negative number, the result will not be paginated.</param>
        /// <param name="filterName">The named filter.</param>
        /// <returns>Returns collection of Login class.</returns>
        IEnumerable<Frapid.Account.Entities.Login> GetFiltered(long pageNumber, string filterName);



    }
}
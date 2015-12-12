// ReSharper disable All
using System.Collections.Generic;
using System.Dynamic;
using Frapid.DataAccess;
using Frapid.DataAccess.Models;

namespace Frapid.Config.DataAccess
{
    public interface ICustomFieldFormRepository
    {
        /// <summary>
        /// Counts the number of CustomFieldForm in ICustomFieldFormRepository.
        /// </summary>
        /// <returns>Returns the count of ICustomFieldFormRepository.</returns>
        long Count();

        /// <summary>
        /// Returns all instances of CustomFieldForm. 
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instances of CustomFieldForm.</returns>
        IEnumerable<Frapid.Config.Entities.CustomFieldForm> GetAll();

        /// <summary>
        /// Returns all instances of CustomFieldForm to export. 
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instances of CustomFieldForm.</returns>
        IEnumerable<dynamic> Export();

        /// <summary>
        /// Returns a single instance of the CustomFieldForm against formName. 
        /// </summary>
        /// <param name="formName">The column "form_name" parameter used on where filter.</param>
        /// <returns>Returns a non-live, non-mapped instance of CustomFieldForm.</returns>
        Frapid.Config.Entities.CustomFieldForm Get(string formName);

        /// <summary>
        /// Gets the first record of CustomFieldForm.
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instance of CustomFieldForm.</returns>
        Frapid.Config.Entities.CustomFieldForm GetFirst();

        /// <summary>
        /// Gets the previous record of CustomFieldForm sorted by formName. 
        /// </summary>
        /// <param name="formName">The column "form_name" parameter used to find the previous record.</param>
        /// <returns>Returns a non-live, non-mapped instance of CustomFieldForm.</returns>
        Frapid.Config.Entities.CustomFieldForm GetPrevious(string formName);

        /// <summary>
        /// Gets the next record of CustomFieldForm sorted by formName. 
        /// </summary>
        /// <param name="formName">The column "form_name" parameter used to find the next record.</param>
        /// <returns>Returns a non-live, non-mapped instance of CustomFieldForm.</returns>
        Frapid.Config.Entities.CustomFieldForm GetNext(string formName);

        /// <summary>
        /// Gets the last record of CustomFieldForm.
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instance of CustomFieldForm.</returns>
        Frapid.Config.Entities.CustomFieldForm GetLast();

        /// <summary>
        /// Returns multiple instances of the CustomFieldForm against formNames. 
        /// </summary>
        /// <param name="formNames">Array of column "form_name" parameter used on where filter.</param>
        /// <returns>Returns a non-live, non-mapped collection of CustomFieldForm.</returns>
        IEnumerable<Frapid.Config.Entities.CustomFieldForm> Get(string[] formNames);

        /// <summary>
        /// Custom fields are user defined form elements for ICustomFieldFormRepository.
        /// </summary>
        /// <returns>Returns an enumerable custom field collection for CustomFieldForm.</returns>
        IEnumerable<Frapid.DataAccess.Models.CustomField> GetCustomFields(string resourceId);

        /// <summary>
        /// Displayfields provide a minimal name/value context for data binding CustomFieldForm.
        /// </summary>
        /// <returns>Returns an enumerable name and value collection for CustomFieldForm.</returns>
        IEnumerable<Frapid.DataAccess.Models.DisplayField> GetDisplayFields();

        /// <summary>
        /// Inserts the instance of CustomFieldForm class to ICustomFieldFormRepository.
        /// </summary>
        /// <param name="customFieldForm">The instance of CustomFieldForm class to insert or update.</param>
        /// <param name="customFields">The custom field collection.</param>
        object AddOrEdit(dynamic customFieldForm, List<Frapid.DataAccess.Models.CustomField> customFields);

        /// <summary>
        /// Inserts the instance of CustomFieldForm class to ICustomFieldFormRepository.
        /// </summary>
        /// <param name="customFieldForm">The instance of CustomFieldForm class to insert.</param>
        object Add(dynamic customFieldForm);

        /// <summary>
        /// Inserts or updates multiple instances of CustomFieldForm class to ICustomFieldFormRepository.;
        /// </summary>
        /// <param name="customFieldForms">List of CustomFieldForm class to import.</param>
        /// <returns>Returns list of inserted object ids.</returns>
        List<object> BulkImport(List<ExpandoObject> customFieldForms);


        /// <summary>
        /// Updates ICustomFieldFormRepository with an instance of CustomFieldForm class against the primary key value.
        /// </summary>
        /// <param name="customFieldForm">The instance of CustomFieldForm class to update.</param>
        /// <param name="formName">The value of the column "form_name" which will be updated.</param>
        void Update(dynamic customFieldForm, string formName);

        /// <summary>
        /// Deletes CustomFieldForm from  ICustomFieldFormRepository against the primary key value.
        /// </summary>
        /// <param name="formName">The value of the column "form_name" which will be deleted.</param>
        void Delete(string formName);

        /// <summary>
        /// Produces a paginated result of 10 CustomFieldForm classes.
        /// </summary>
        /// <returns>Returns the first page of collection of CustomFieldForm class.</returns>
        IEnumerable<Frapid.Config.Entities.CustomFieldForm> GetPaginatedResult();

        /// <summary>
        /// Produces a paginated result of 10 CustomFieldForm classes.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result.</param>
        /// <returns>Returns collection of CustomFieldForm class.</returns>
        IEnumerable<Frapid.Config.Entities.CustomFieldForm> GetPaginatedResult(long pageNumber);

        List<Frapid.DataAccess.Models.Filter> GetFilters(string catalog, string filterName);

        /// <summary>
        /// Performs a filtered count on ICustomFieldFormRepository.
        /// </summary>
        /// <param name="filters">The list of filter conditions.</param>
        /// <returns>Returns number of rows of CustomFieldForm class using the filter.</returns>
        long CountWhere(List<Frapid.DataAccess.Models.Filter> filters);

        /// <summary>
        /// Performs a filtered pagination against ICustomFieldFormRepository producing result of 10 items.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result. If you provide a negative number, the result will not be paginated.</param>
        /// <param name="filters">The list of filter conditions.</param>
        /// <returns>Returns collection of CustomFieldForm class.</returns>
        IEnumerable<Frapid.Config.Entities.CustomFieldForm> GetWhere(long pageNumber, List<Frapid.DataAccess.Models.Filter> filters);

        /// <summary>
        /// Performs a filtered count on ICustomFieldFormRepository.
        /// </summary>
        /// <param name="filterName">The named filter.</param>
        /// <returns>Returns number of CustomFieldForm class using the filter.</returns>
        long CountFiltered(string filterName);

        /// <summary>
        /// Gets a filtered result of ICustomFieldFormRepository producing a paginated result of 10.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result. If you provide a negative number, the result will not be paginated.</param>
        /// <param name="filterName">The named filter.</param>
        /// <returns>Returns collection of CustomFieldForm class.</returns>
        IEnumerable<Frapid.Config.Entities.CustomFieldForm> GetFiltered(long pageNumber, string filterName);



    }
}
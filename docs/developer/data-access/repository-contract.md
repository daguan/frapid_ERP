# Creating Data Access Repositories

Frapid has a convention-based approach for data access repository classes.
These conventions enable you to use the powerful JavaScript ScrudFactory API to rapidly build
user interfaces without writing much code. But before digging into ScrudFactory, you would be more
interested to know how to write data access repository contracts.

## Convention for Tables (Example: core.apps)

| Name | Type | Returns | Description |
| --- | --- | --- | --- |
| IAppRepository | Interface Name | N/A | Singular Pascal case table name prefixed with indicator `I` and suffixed with `Repository`. Example: the interface repository for imaginary table `core.installed_applications` would be `IInstalledApplicationRepository`. |
| Count() | Function | long | Should return the count of the number of rows in the `core.apps` or an imaginary table. |
| GetAll() | Function | IEnumerable<[App](#docs/developer/data-access/dto.md)> | Should return an IEnumerable of mapped [POCO classes](#docs/developer/data-access/dto.md) of `core.apps` or an imaginary table. |
| GetPaginatedResult() | Function | IEnumerable<[App](#docs/developer/data-access/dto.md)> | Should return the first page of the paginated result of 10 IEnumerable [POCO classes](#docs/developer/data-access/dto.md) of `core.apps` or an imaginary table. |
| GetPaginatedResult(long pageNumber) | Function | IEnumerable<[App](#docs/developer/data-access/dto.md)> | Should return the requested page of the paginated result of 10 IEnumerable [POCO classes](#docs/developer/data-access/dto.md) of `core.apps` or an imaginary table. |
| Export() | Function | IEnumerable<dynamic> | Should return an IEnumerable of `dynamic`, which exactly represents columns of `core.apps` or an imaginary table.<br/><br/>Note that the function `GetAll()` is different because it maps each row to a `POCO class`. |
| BulkImport(List<ExpandoObject> apps) | Function | List<object> | Should bulk insert or update the collection of `expando objects` that represent the collection of columns of `core.apps` or an imaginary table into the database.<br/><br/>Should return the primary key value of the inserted or updated rows. |
| Get(string appName) | Function | [App](#docs/developer/data-access/dto.md) | Should return a single instance of mapped [POCO class](#docs/developer/data-access/dto.md) of `core.apps` or an imaginary table filtered by the primary key value. |
| Get(string[] appName) | Function | IEnumerable<[App](#docs/developer/data-access/dto.md)> | Should return instances of mapped [POCO class](#docs/developer/data-access/dto.md) of `core.apps` or an imaginary table filtered by the array of primary key values. |
| GetFirst() | Function | [App](#docs/developer/data-access/dto.md) | Should return the first record of mapped [POCO class](#docs/developer/data-access/dto.md) of `core.apps` or an imaginary table sorted by the primary key. |
| GetPrevious(string appName) | Function | [App](#docs/developer/data-access/dto.md) | Should return the previous record of mapped [POCO class](#docs/developer/data-access/dto.md) of `core.apps` or an imaginary table sorted by the primary key. |
| GetNext(string appName) | Function | [App](#docs/developer/data-access/dto.md) | Should return the next record of mapped [POCO class](#docs/developer/data-access/dto.md) of `core.apps` or an imaginary table sorted by the primary key. |
| GetLast() | Function | [App](#docs/developer/data-access/dto.md) | Should return the last record of mapped [POCO class](#docs/developer/data-access/dto.md) of `core.apps` or an imaginary table. |
| GetCustomFields(string resourceId) | Function | IEnumerable<[CustomField](#docs/developer/custom-fields.md)> | Should return an IEnumerable of custom fields. |
| GetCustomFields() | Function | IEnumerable<[DisplayField](#docs/developer/display-fields.md)> | Should return an IEnumerable of display fields. Display fields provide a minimal name/value context for data binding `core.apps` or an imaginary table. |
| AddOrEdit(dynamic app, List<CustomField> customFields) | Function | object | Should insert or update the dynamic object that represents the collection of columns of `core.apps` or an imaginary table along with the custom fields into the database.<br/><br/>Should return the primary key value of the inserted row. |
| Add(dynamic app) | Function | object | Should insert the dynamic object that represents the collection of columns of `core.apps` or an imaginary table into the database.<br/><br/>Should return the primary key value of the inserted row. |
| Update(dynamic app, string appName) | Method | void | Should update the dynamic object that represents the collection of columns of `core.apps` or an imaginary table into the database.<br/><br/>Should use the supplied primary key value to perform update operation. |
| Delete(string appName) | Method | void | Should delete the matching row which contains the supplied primary value of `core.apps` or an imaginary table from the database |
| GetFilters(string filterName) | Function | List<[Filter](#docs/developer/filters.md)> | Should return list of saved filters related to `core.apps` or an imaginary table into the database. |
| CountWhere(List<[Filter](#docs/developer/filters.md)> filters) | Function | IEnumerable<[App](#docs/developer/data-access/dto.md)> | Should return, using the supplied filters, the count of the number of rows in the `core.apps` or an imaginary table. |
| GetWhere(long pageNumber, List<[Filter](#docs/developer/filters.md)> filters) | Function | long | Should return, using the supplied filters, the requested page of the paginated result of 10 IEnumerable [POCO classes](#docs/developer/data-access/dto.md) of `core.apps` or an imaginary table.<br/><br/>If the supplied page number is a negative value, pagination should not happen and all rows should be returned. |
| CountFiltered(string filterName) | Function | IEnumerable<[App](#docs/developer/data-access/dto.md)> | Should return, using the supplied filter name, the count of the number of rows in the `core.apps` or an imaginary table. |
| GetWhere(long pageNumber, List<[Filter](#docs/developer/filters.md)> filters) | Function | long | Should return, using the supplied filter name, the requested page of the paginated result of 10 IEnumerable [POCO classes](#docs/developer/data-access/dto.md) of `core.apps` or an imaginary table.<br/><br/>If the supplied page number is a negative value, pagination should not happen and all rows should be returned. |

**Example:**

[https://github.com/frapid/frapid/blob/master/src/Frapid.Web/Areas/Frapid.Core/DataAccess/IAppRepository.cs](https://github.com/frapid/frapid/blob/master/src/Frapid.Web/Areas/Frapid.Core/DataAccess/IAppRepository.cs)

## Conventions for View

To be documented.

## Conventions for Functions

To be documented.



[Back to Developer Documentation](../README.md)

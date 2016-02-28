using System.Collections.Generic;
using System.Dynamic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Frapid.DataAccess;
using Frapid.DataAccess.Models;
using Frapid.Framework;
using Frapid.WebApi.DataAccess;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Frapid.WebApi.Service
{
    public class FormApiController : FrapidApiController
    {
        [AcceptVerbs("GET", "HEAD")]
        [Route("~/api/forms/{schemaName}/{tableName}/meta")]
        [RestAuthorize]
        public EntityView GetEntityView(string schemaName, string tableName)
        {
            try
            {
                var repository = new FormRepository(schemaName, tableName, this.MetaUser.Catalog, this.MetaUser.LoginId, this.MetaUser.UserId);
                return repository.GetMeta();
            }
            catch (UnauthorizedException)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Forbidden));
            }
            catch (DataAccessException ex)
            {
                throw new HttpResponseException(new HttpResponseMessage
                {
                    Content = new StringContent(ex.Message),
                    StatusCode = HttpStatusCode.InternalServerError
                });
            }
#if !DEBUG
            catch
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError));
            }
#endif
        }

        [AcceptVerbs("GET", "HEAD")]
        [Route("~/api/forms/{schemaName}/{tableName}/count")]
        [RestAuthorize]
        public long Count(string schemaName, string tableName)
        {
            try
            {
                var repository = new FormRepository(schemaName, tableName, this.MetaUser.Catalog, this.MetaUser.LoginId, this.MetaUser.UserId);
                return repository.Count();
            }
            catch (UnauthorizedException)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Forbidden));
            }
            catch (DataAccessException ex)
            {
                throw new HttpResponseException(new HttpResponseMessage
                {
                    Content = new StringContent(ex.Message),
                    StatusCode = HttpStatusCode.InternalServerError
                });
            }
#if !DEBUG
            catch
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError));
            }
#endif
        }

        [AcceptVerbs("GET", "HEAD")]
        [Route("~/api/forms/{schemaName}/{tableName}/all")]
        [Route("~/api/forms/{schemaName}/{tableName}/export")]
        [RestAuthorize]
        public IEnumerable<dynamic> GetAll(string schemaName, string tableName)
        {
            try
            {
                var repository = new FormRepository(schemaName, tableName, this.MetaUser.Catalog, this.MetaUser.LoginId, this.MetaUser.UserId);
                return repository.GetAll();
            }
            catch (UnauthorizedException)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Forbidden));
            }
            catch (DataAccessException ex)
            {
                throw new HttpResponseException(new HttpResponseMessage
                {
                    Content = new StringContent(ex.Message),
                    StatusCode = HttpStatusCode.InternalServerError
                });
            }
#if !DEBUG
            catch
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError));
            }
#endif
        }

        [AcceptVerbs("GET", "HEAD")]
        [Route("~/api/forms/{schemaName}/{tableName}/{primaryKey}")]
        [RestAuthorize]
        public dynamic Get(string schemaName, string tableName, string primaryKey)
        {
            try
            {
                var repository = new FormRepository(schemaName, tableName, this.MetaUser.Catalog, this.MetaUser.LoginId, this.MetaUser.UserId);
                return repository.Get(primaryKey);
            }
            catch (UnauthorizedException)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Forbidden));
            }
            catch (DataAccessException ex)
            {
                throw new HttpResponseException(new HttpResponseMessage
                {
                    Content = new StringContent(ex.Message),
                    StatusCode = HttpStatusCode.InternalServerError
                });
            }
#if !DEBUG
            catch
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError));
            }
#endif
        }

        [AcceptVerbs("GET", "HEAD")]
        [Route("~/api/forms/{schemaName}/{tableName}/get")]
        [RestAuthorize]
        public IEnumerable<dynamic> Get(string schemaName, string tableName, [FromUri] object[] primaryKeys)
        {
            try
            {
                var repository = new FormRepository(schemaName, tableName, this.MetaUser.Catalog, this.MetaUser.LoginId, this.MetaUser.UserId);
                return repository.Get(primaryKeys);
            }
            catch (UnauthorizedException)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Forbidden));
            }
            catch (DataAccessException ex)
            {
                throw new HttpResponseException(new HttpResponseMessage
                {
                    Content = new StringContent(ex.Message),
                    StatusCode = HttpStatusCode.InternalServerError
                });
            }
#if !DEBUG
            catch
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError));
            }
#endif
        }

        [AcceptVerbs("GET", "HEAD")]
        [Route("~/api/forms/{schemaName}/{tableName}/first")]
        [RestAuthorize]
        public dynamic GetFirst(string schemaName, string tableName)
        {
            try
            {
                var repository = new FormRepository(schemaName, tableName, this.MetaUser.Catalog, this.MetaUser.LoginId, this.MetaUser.UserId);
                return repository.GetFirst();
            }
            catch (UnauthorizedException)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Forbidden));
            }
            catch (DataAccessException ex)
            {
                throw new HttpResponseException(new HttpResponseMessage
                {
                    Content = new StringContent(ex.Message),
                    StatusCode = HttpStatusCode.InternalServerError
                });
            }
#if !DEBUG
            catch
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError));
            }
#endif
        }

        [AcceptVerbs("GET", "HEAD")]
        [Route("~/api/forms/{schemaName}/{tableName}/previous/{primaryKey}")]
        [RestAuthorize]
        public dynamic GetPrevious(string schemaName, string tableName, string primaryKey)
        {
            try
            {
                var repository = new FormRepository(schemaName, tableName, this.MetaUser.Catalog, this.MetaUser.LoginId, this.MetaUser.UserId);
                return repository.GetPrevious(primaryKey);
            }
            catch (UnauthorizedException)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Forbidden));
            }
            catch (DataAccessException ex)
            {
                throw new HttpResponseException(new HttpResponseMessage
                {
                    Content = new StringContent(ex.Message),
                    StatusCode = HttpStatusCode.InternalServerError
                });
            }
#if !DEBUG
            catch
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError));
            }
#endif
        }

        [AcceptVerbs("GET", "HEAD")]
        [Route("~/api/forms/{schemaName}/{tableName}/next/{primaryKey}")]
        [RestAuthorize]
        public dynamic GetNext(string schemaName, string tableName, string primaryKey)
        {
            try
            {
                var repository = new FormRepository(schemaName, tableName, this.MetaUser.Catalog, this.MetaUser.LoginId, this.MetaUser.UserId);
                return repository.GetNext(primaryKey);
            }
            catch (UnauthorizedException)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Forbidden));
            }
            catch (DataAccessException ex)
            {
                throw new HttpResponseException(new HttpResponseMessage
                {
                    Content = new StringContent(ex.Message),
                    StatusCode = HttpStatusCode.InternalServerError
                });
            }
#if !DEBUG
            catch
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError));
            }
#endif
        }

        [AcceptVerbs("GET", "HEAD")]
        [Route("~/api/forms/{schemaName}/{tableName}/last")]
        [RestAuthorize]
        public dynamic GetLast(string schemaName, string tableName)
        {
            try
            {
                var repository = new FormRepository(schemaName, tableName, this.MetaUser.Catalog, this.MetaUser.LoginId, this.MetaUser.UserId);
                return repository.GetLast();
            }
            catch (UnauthorizedException)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Forbidden));
            }
            catch (DataAccessException ex)
            {
                throw new HttpResponseException(new HttpResponseMessage
                {
                    Content = new StringContent(ex.Message),
                    StatusCode = HttpStatusCode.InternalServerError
                });
            }
#if !DEBUG
            catch
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError));
            }
#endif
        }

        [AcceptVerbs("GET", "HEAD")]
        [Route("~/api/forms/{schemaName}/{tableName}")]
        [RestAuthorize]
        public IEnumerable<dynamic> GetPaginatedResult(string schemaName, string tableName)
        {
            try
            {
                var repository = new FormRepository(schemaName, tableName, this.MetaUser.Catalog, this.MetaUser.LoginId, this.MetaUser.UserId);
                return repository.GetPaginatedResult();
            }
            catch (UnauthorizedException)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Forbidden));
            }
            catch (DataAccessException ex)
            {
                throw new HttpResponseException(new HttpResponseMessage
                {
                    Content = new StringContent(ex.Message),
                    StatusCode = HttpStatusCode.InternalServerError
                });
            }
#if !DEBUG
            catch
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError));
            }
#endif
        }

        [AcceptVerbs("GET", "HEAD")]
        [Route("~/api/forms/{schemaName}/{tableName}/page/{pageNumber}")]
        [RestAuthorize]
        public IEnumerable<dynamic> GetPaginatedResult(string schemaName, string tableName, long pageNumber)
        {
            try
            {
                var repository = new FormRepository(schemaName, tableName, this.MetaUser.Catalog, this.MetaUser.LoginId, this.MetaUser.UserId);
                return repository.GetPaginatedResult(pageNumber);
            }
            catch (UnauthorizedException)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Forbidden));
            }
            catch (DataAccessException ex)
            {
                throw new HttpResponseException(new HttpResponseMessage
                {
                    Content = new StringContent(ex.Message),
                    StatusCode = HttpStatusCode.InternalServerError
                });
            }
#if !DEBUG
            catch
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError));
            }
#endif
        }

        [AcceptVerbs("POST")]
        [Route("~/api/forms/{schemaName}/{tableName}/count-where")]
        [RestAuthorize]
        public long CountWhere(string schemaName, string tableName, [FromBody] JArray filters)
        {
            try
            {
                var f = Filter.FromJArray(filters);
                var repository = new FormRepository(schemaName, tableName, this.MetaUser.Catalog, this.MetaUser.LoginId, this.MetaUser.UserId);
                return repository.CountWhere(f);
            }
            catch (UnauthorizedException)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Forbidden));
            }
            catch (DataAccessException ex)
            {
                throw new HttpResponseException(new HttpResponseMessage
                {
                    Content = new StringContent(ex.Message),
                    StatusCode = HttpStatusCode.InternalServerError
                });
            }
#if !DEBUG
            catch
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError));
            }
#endif
        }


        [AcceptVerbs("POST")]
        [Route("~/api/forms/{schemaName}/{tableName}/get-where/{pageNumber}")]
        [RestAuthorize]
        public IEnumerable<dynamic> GetWhere(string schemaName, string tableName, long pageNumber, [FromBody] JArray filters)
        {
            try
            {
                var f = Filter.FromJArray(filters);
                var repository = new FormRepository(schemaName, tableName, this.MetaUser.Catalog, this.MetaUser.LoginId, this.MetaUser.UserId);
                return repository.GetWhere(pageNumber, f);
            }
            catch (UnauthorizedException)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Forbidden));
            }
            catch (DataAccessException ex)
            {
                throw new HttpResponseException(new HttpResponseMessage
                {
                    Content = new StringContent(ex.Message),
                    StatusCode = HttpStatusCode.InternalServerError
                });
            }
#if !DEBUG
            catch
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError));
            }
#endif
        }

        [AcceptVerbs("GET", "HEAD")]
        [Route("~/api/forms/{schemaName}/{tableName}/count-filtered/{filterName}")]
        [RestAuthorize]
        public long CountFiltered(string schemaName, string tableName, string filterName)
        {
            try
            {
                var repository = new FormRepository(schemaName, tableName, this.MetaUser.Catalog, this.MetaUser.LoginId, this.MetaUser.UserId);
                return repository.CountFiltered(filterName);
            }
            catch (UnauthorizedException)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Forbidden));
            }
            catch (DataAccessException ex)
            {
                throw new HttpResponseException(new HttpResponseMessage
                {
                    Content = new StringContent(ex.Message),
                    StatusCode = HttpStatusCode.InternalServerError
                });
            }
#if !DEBUG
            catch
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError));
            }
#endif
        }

        [AcceptVerbs("GET", "HEAD")]
        [Route("~/api/forms/{schemaName}/{tableName}/get-filtered/{pageNumber}/{filterName}")]
        [RestAuthorize]
        public IEnumerable<dynamic> GetFiltered(string schemaName, string tableName, long pageNumber, string filterName)
        {
            try
            {
                var repository = new FormRepository(schemaName, tableName, this.MetaUser.Catalog, this.MetaUser.LoginId, this.MetaUser.UserId);
                return repository.GetFiltered(pageNumber, filterName);
            }
            catch (UnauthorizedException)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Forbidden));
            }
            catch (DataAccessException ex)
            {
                throw new HttpResponseException(new HttpResponseMessage
                {
                    Content = new StringContent(ex.Message),
                    StatusCode = HttpStatusCode.InternalServerError
                });
            }
#if !DEBUG
            catch
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError));
            }
#endif
        }

        [AcceptVerbs("GET", "HEAD")]
        [Route("~/api/forms/{schemaName}/{tableName}/display-fields")]
        [RestAuthorize]
        public IEnumerable<DisplayField> GetDisplayFields(string schemaName, string tableName)
        {
            try
            {
                var repository = new FormRepository(schemaName, tableName, this.MetaUser.Catalog, this.MetaUser.LoginId, this.MetaUser.UserId);
                return repository.GetDisplayFields();
            }
            catch (UnauthorizedException)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Forbidden));
            }
            catch (DataAccessException ex)
            {
                throw new HttpResponseException(new HttpResponseMessage
                {
                    Content = new StringContent(ex.Message),
                    StatusCode = HttpStatusCode.InternalServerError
                });
            }
#if !DEBUG
            catch
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError));
            }
#endif
        }

        [AcceptVerbs("GET", "HEAD")]
        [Route("~/api/forms/{schemaName}/{tableName}/custom-fields")]
        [RestAuthorize]
        public IEnumerable<CustomField> GetCustomFields(string schemaName, string tableName)
        {
            try
            {
                var repository = new FormRepository(schemaName, tableName, this.MetaUser.Catalog, this.MetaUser.LoginId, this.MetaUser.UserId);
                return repository.GetCustomFields(null);
            }
            catch (UnauthorizedException)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Forbidden));
            }
            catch (DataAccessException ex)
            {
                throw new HttpResponseException(new HttpResponseMessage
                {
                    Content = new StringContent(ex.Message),
                    StatusCode = HttpStatusCode.InternalServerError
                });
            }
#if !DEBUG
            catch
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError));
            }
#endif
        }

        [AcceptVerbs("GET", "HEAD")]
        [Route("~/api/forms/{schemaName}/{tableName}/custom-fields/{resourceId}")]
        [RestAuthorize]
        public IEnumerable<CustomField> GetCustomFields(string schemaName, string tableName, string resourceId)
        {
            try
            {
                var repository = new FormRepository(schemaName, tableName, this.MetaUser.Catalog, this.MetaUser.LoginId, this.MetaUser.UserId);
                return repository.GetCustomFields(resourceId);
            }
            catch (UnauthorizedException)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Forbidden));
            }
            catch (DataAccessException ex)
            {
                throw new HttpResponseException(new HttpResponseMessage
                {
                    Content = new StringContent(ex.Message),
                    StatusCode = HttpStatusCode.InternalServerError
                });
            }
#if !DEBUG
            catch
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError));
            }
#endif
        }

        [AcceptVerbs("POST")]
        [Route("~/api/forms/{schemaName}/{tableName}/add-or-edit")]
        [RestAuthorize]
        public object AddOrEdit(string schemaName, string tableName, [FromBody] JArray form)
        {
            dynamic user = form[0].ToObject<ExpandoObject>(JsonHelper.GetJsonSerializer());
            var customFields = form[1].ToObject<List<CustomField>>(JsonHelper.GetJsonSerializer());

            if (user == null)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.MethodNotAllowed));
            }

            try
            {
                var repository = new FormRepository(schemaName, tableName, this.MetaUser.Catalog, this.MetaUser.LoginId, this.MetaUser.UserId);
                return repository.AddOrEdit(user, customFields);
            }
            catch (UnauthorizedException)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Forbidden));
            }
            catch (DataAccessException ex)
            {
                throw new HttpResponseException(new HttpResponseMessage
                {
                    Content = new StringContent(ex.Message),
                    StatusCode = HttpStatusCode.InternalServerError
                });
            }
#if !DEBUG
            catch
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError));
            }
#endif
        }

        [AcceptVerbs("POST")]
        [Route("~/api/forms/{schemaName}/{tableName}/add/{user}")]
        [RestAuthorize]
        public void Add(string schemaName, string tableName, dynamic user)
        {
            if (user == null)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.MethodNotAllowed));
            }

            try
            {
                var repository = new FormRepository(schemaName, tableName, this.MetaUser.Catalog, this.MetaUser.LoginId, this.MetaUser.UserId);
                repository.Add(user);
            }
            catch (UnauthorizedException)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Forbidden));
            }
            catch (DataAccessException ex)
            {
                throw new HttpResponseException(new HttpResponseMessage
                {
                    Content = new StringContent(ex.Message),
                    StatusCode = HttpStatusCode.InternalServerError
                });
            }
#if !DEBUG
            catch
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError));
            }
#endif
        }

        [AcceptVerbs("PUT")]
        [Route("~/api/forms/{schemaName}/{tableName}/edit/{primaryKey}")]
        [RestAuthorize]
        public void Edit(string schemaName, string tableName, string primaryKey, [FromBody] dynamic user)
        {
            if (user == null)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.MethodNotAllowed));
            }

            try
            {
                var repository = new FormRepository(schemaName, tableName, this.MetaUser.Catalog, this.MetaUser.LoginId, this.MetaUser.UserId);
                repository.Update(user, primaryKey);
            }
            catch (UnauthorizedException)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Forbidden));
            }
            catch (DataAccessException ex)
            {
                throw new HttpResponseException(new HttpResponseMessage
                {
                    Content = new StringContent(ex.Message),
                    StatusCode = HttpStatusCode.InternalServerError
                });
            }
#if !DEBUG
            catch
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError));
            }
#endif
        }

        private List<ExpandoObject> ParseCollection(JArray collection)
        {
            return JsonConvert.DeserializeObject<List<ExpandoObject>>(collection.ToString(),
                JsonHelper.GetJsonSerializerSettings());
        }

        [AcceptVerbs("POST")]
        [Route("~/api/forms/{schemaName}/{tableName}/bulk-import")]
        [RestAuthorize]
        public List<object> BulkImport(string schemaName, string tableName, [FromBody] JArray collection)
        {
            var userCollection = this.ParseCollection(collection);

            if (userCollection == null || userCollection.Count.Equals(0))
            {
                return null;
            }

            try
            {
                var repository = new FormRepository(schemaName, tableName, this.MetaUser.Catalog, this.MetaUser.LoginId, this.MetaUser.UserId);
                return repository.BulkImport(userCollection);
            }
            catch (UnauthorizedException)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Forbidden));
            }
            catch (DataAccessException ex)
            {
                throw new HttpResponseException(new HttpResponseMessage
                {
                    Content = new StringContent(ex.Message),
                    StatusCode = HttpStatusCode.InternalServerError
                });
            }
#if !DEBUG
            catch
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError));
            }
#endif
        }

        [AcceptVerbs("DELETE")]
        [Route("~/api/forms/{schemaName}/{tableName}/delete/{primaryKey}")]
        [RestAuthorize]
        public void Delete(string schemaName, string tableName, string primaryKey)
        {
            try
            {
                var repository = new FormRepository(schemaName, tableName, this.MetaUser.Catalog, this.MetaUser.LoginId, this.MetaUser.UserId);
                repository.Delete(primaryKey);
            }
            catch (UnauthorizedException)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Forbidden));
            }
            catch (DataAccessException ex)
            {
                throw new HttpResponseException(new HttpResponseMessage
                {
                    Content = new StringContent(ex.Message),
                    StatusCode = HttpStatusCode.InternalServerError
                });
            }
#if !DEBUG
            catch
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError));
            }
#endif
        }
    }
}
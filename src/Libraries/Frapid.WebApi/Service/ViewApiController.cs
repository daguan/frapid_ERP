using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Frapid.DataAccess;
using Frapid.DataAccess.Models;
using Frapid.Framework;
using Frapid.WebApi.DataAccess;
using Newtonsoft.Json.Linq;

namespace Frapid.WebApi.Service
{
    public class ViewApiController : FrapidApiController
    {
        [AcceptVerbs("GET", "HEAD")]
        [Route("~/api/views/{schemaName}/{tableName}/count")]
        [RestAuthorize]
        public long Count(string schemaName, string tableName)
        {
            try
            {
                var repository = new ViewRepository(schemaName, tableName, this.MetaUser.Tenant, this.MetaUser.LoginId,
                    this.MetaUser.UserId);
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
        [Route("~/api/views/{schemaName}/{tableName}/all")]
        [Route("~/api/views/{schemaName}/{tableName}/export")]
        [RestAuthorize]
        public IEnumerable<dynamic> GetAll(string schemaName, string tableName)
        {
            try
            {
                var repository = new ViewRepository(schemaName, tableName, this.MetaUser.Tenant, this.MetaUser.LoginId,
                    this.MetaUser.UserId);
                return repository.Get();
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
        [Route("~/api/views/{schemaName}/{tableName}")]
        [RestAuthorize]
        public IEnumerable<dynamic> GetPaginatedResult(string schemaName, string tableName)
        {
            try
            {
                var repository = new ViewRepository(schemaName, tableName, this.MetaUser.Tenant, this.MetaUser.LoginId,
                    this.MetaUser.UserId);
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
        [Route("~/api/views/{schemaName}/{tableName}/page/{pageNumber}")]
        [RestAuthorize]
        public IEnumerable<dynamic> GetPaginatedResult(string schemaName, string tableName, long pageNumber)
        {
            try
            {
                var repository = new ViewRepository(schemaName, tableName, this.MetaUser.Tenant, this.MetaUser.LoginId,
                    this.MetaUser.UserId);
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
        [Route("~/api/views/{schemaName}/{tableName}/count-where")]
        [RestAuthorize]
        public long CountWhere(string schemaName, string tableName, [FromBody] JArray filters)
        {
            try
            {
                var f = Filter.FromJArray(filters);
                var repository = new ViewRepository(schemaName, tableName, this.MetaUser.Tenant, this.MetaUser.LoginId,
                    this.MetaUser.UserId);
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
        [Route("~/api/views/{schemaName}/{tableName}/get-where/{pageNumber}")]
        [RestAuthorize]
        public IEnumerable<dynamic> GetWhere(string schemaName, string tableName, long pageNumber,
            [FromBody] JArray filters)
        {
            try
            {
                var f = Filter.FromJArray(filters);
                var repository = new ViewRepository(schemaName, tableName, this.MetaUser.Tenant, this.MetaUser.LoginId,
                    this.MetaUser.UserId);
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
        [Route("~/api/views/{schemaName}/{tableName}/count-filtered/{filterName}")]
        [RestAuthorize]
        public long CountFiltered(string schemaName, string tableName, string filterName)
        {
            try
            {
                var repository = new ViewRepository(schemaName, tableName, this.MetaUser.Tenant, this.MetaUser.LoginId,
                    this.MetaUser.UserId);
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
        [Route("~/api/views/{schemaName}/{tableName}/get-filtered/{pageNumber}/{filterName}")]
        [RestAuthorize]
        public IEnumerable<dynamic> GetFiltered(string schemaName, string tableName, long pageNumber, string filterName)
        {
            try
            {
                var repository = new ViewRepository(schemaName, tableName, this.MetaUser.Tenant, this.MetaUser.LoginId,
                    this.MetaUser.UserId);
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
        [Route("~/api/views/{schemaName}/{tableName}/display-fields")]
        [RestAuthorize]
        public IEnumerable<DisplayField> GetDisplayFields(string schemaName, string tableName)
        {
            try
            {
                var repository = new ViewRepository(schemaName, tableName, this.MetaUser.Tenant, this.MetaUser.LoginId,
                    this.MetaUser.UserId);
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
    }
}
using System.Collections.Generic;
using System.Dynamic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Frapid.DataAccess;
using Frapid.WebApi.DataAccess;

namespace Frapid.WebApi.Service
{
    public class FilterApiController : FrapidApiController
    {
        [AcceptVerbs("PUT")]
        [Route("~/api/filters/make-default/{objectName}/{filterName}")]
        public void MakeDefault(string objectName, string filterName)
        {
            try
            {
                var repository = new FilterRepository(this.MetaUser.Tenant, this.MetaUser.LoginId, this.MetaUser.UserId);
                repository.MakeDefault(objectName, filterName);
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
        [Route("~/api/filters/remove-default/{objectName}")]
        public void RemoveDefault(string objectName)
        {
            try
            {
                var repository = new FilterRepository(this.MetaUser.Tenant, this.MetaUser.LoginId, this.MetaUser.UserId);
                repository.RemoveDefault(objectName);
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
        [Route("~/api/filters/delete/by-name/{filterName}")]
        public void Delete(string filterName)
        {
            try
            {
                var repository = new FilterRepository(this.MetaUser.Tenant, this.MetaUser.LoginId, this.MetaUser.UserId);
                repository.Delete(filterName);
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
        [Route("~/api/filters/recreate/{objectName}/{filterName}")]
        public void RecreateFilters(string objectName, string filterName, [FromBody] List<ExpandoObject> collection)
        {
            try
            {
                var repository = new FilterRepository(this.MetaUser.Tenant, this.MetaUser.LoginId, this.MetaUser.UserId);
                repository.RecreateFilters(objectName, filterName, collection);
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
using System.Collections.Generic;
using System.Dynamic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Frapid.DataAccess;
using Frapid.WebApi.DataAccess;

namespace Frapid.WebApi.Service
{
    public class FilterApiController: FrapidApiController
    {
        [AcceptVerbs("PUT")]
        [Route("~/api/filters/make-default/{objectName}/{filterName}")]
        public async Task MakeDefaultAsync(string objectName, string filterName)
        {
            try
            {
                var repository = new FilterRepository(this.MetaUser.Tenant, this.MetaUser.LoginId, this.MetaUser.UserId);
                await repository.MakeDefaultAsync(objectName, filterName);
            }
            catch(UnauthorizedException)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Forbidden));
            }
            catch(DataAccessException ex)
            {
                throw new HttpResponseException
                    (
                    new HttpResponseMessage
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
        public async Task RemoveDefaultAsync(string objectName)
        {
            try
            {
                var repository = new FilterRepository(this.MetaUser.Tenant, this.MetaUser.LoginId, this.MetaUser.UserId);
                await repository.RemoveDefaultAsync(objectName);
            }
            catch(UnauthorizedException)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Forbidden));
            }
            catch(DataAccessException ex)
            {
                throw new HttpResponseException
                    (
                    new HttpResponseMessage
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
        public async Task DeleteAsync(string filterName)
        {
            try
            {
                var repository = new FilterRepository(this.MetaUser.Tenant, this.MetaUser.LoginId, this.MetaUser.UserId);
                await repository.DeleteAsync(filterName);
            }
            catch(UnauthorizedException)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Forbidden));
            }
            catch(DataAccessException ex)
            {
                throw new HttpResponseException
                    (
                    new HttpResponseMessage
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
        public async Task RecreateFiltersAsync(string objectName, string filterName, [FromBody] List<ExpandoObject> collection)
        {
            try
            {
                var repository = new FilterRepository(this.MetaUser.Tenant, this.MetaUser.LoginId, this.MetaUser.UserId);
                await repository.RecreateFiltersAsync(objectName, filterName, collection);
            }
            catch(UnauthorizedException)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Forbidden));
            }
            catch(DataAccessException ex)
            {
                throw new HttpResponseException
                    (
                    new HttpResponseMessage
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
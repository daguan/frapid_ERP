using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Frapid.DataAccess;
using Frapid.WebApi.DataAccess;

namespace Frapid.WebApi.Service
{
    public class FlagApiController : FrapidApiController
    {
        [AcceptVerbs("GET", "HEAD")]
        [Route("~/api/flags/get/{resource}/{userId}")]
        public IEnumerable<dynamic> Get(string resource, int userId, [FromUri] object[] resourceIds)
        {
            try
            {
                var repository = new FlagRepository(this.MetaUser.Tenant, this.MetaUser.LoginId, this.MetaUser.UserId);
                return repository.Get(resource, userId, resourceIds);
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
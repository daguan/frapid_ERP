using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Frapid.DataAccess;
using Frapid.WebApi.DataAccess;

namespace Frapid.WebApi.Service
{
    public class FlagApiController: FrapidApiController
    {
        [AcceptVerbs("GET", "HEAD")]
        [Route("~/api/flags/get/{resource}/{userId}")]
        public async Task<IEnumerable<dynamic>> GetAsync(string resource, int userId, [FromUri] object[] resourceIds)
        {
            try
            {
                var repository = new FlagRepository(this.AppUser.Tenant, this.AppUser.LoginId, this.AppUser.UserId);
                return await repository.GetAsync(resource, userId, resourceIds).ConfigureAwait(false);
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
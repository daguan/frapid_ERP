using System.Web.Http;
using Frapid.ApplicationState.Cache;

namespace Frapid.Areas
{
    public abstract class WebApiController: ApiController
    {
        protected WebApiController()
        {
            AppUsers.SetCurrentLogin();
        }
    }
}
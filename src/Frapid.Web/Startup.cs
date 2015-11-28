using Frapid.Web;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof (Startup))]

namespace Frapid.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
        }
    }
}
using System.IO;
using System.Web.Hosting;
using Frapid.Areas;

namespace Frapid.WebsiteBuilder.Controllers
{
    public class WebsiteBuilderController : FrapidController
    {
        public WebsiteBuilderController()
        {
            string theme = GetTheme();

            ViewBag.LayoutPath = GetLayoutPath(theme);
            ViewBag.Layout = this.GetLayout(theme);
        }

        protected string GetLayoutPath(string theme = "")
        {
            string layout = Configuration.GetCurrentThemePath();

            string layoutDirectory = HostingEnvironment.MapPath(layout);

            if (layoutDirectory != null && Directory.Exists(layoutDirectory))
            {
                return layout;
            }

            return null;
        }

        protected string GetTheme()
        {
            return Configuration.GetDefaultTheme();
        }

        protected string GetLayout(string theme = "")
        {
            if (string.IsNullOrWhiteSpace(theme))
            {
                theme = GetTheme();
            }

            return ThemeConfiguration.GetLayout(theme);
        }

        protected string GetHomepageLayout(string theme = "")
        {
            if (string.IsNullOrWhiteSpace(theme))
            {
                theme = GetTheme();
            }

            return ThemeConfiguration.GetHomepageLayout(theme);
        }
    }
}
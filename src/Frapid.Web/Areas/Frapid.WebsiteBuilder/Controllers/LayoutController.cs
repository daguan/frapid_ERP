using System.Net;
using System.Text;
using System.Web.Hosting;
using System.Web.Mvc;
using Frapid.Dashboard.Controllers;
using Frapid.WebsiteBuilder.ViewModels;

namespace Frapid.WebsiteBuilder.Controllers
{
    public class LayoutController : DashboardController
    {
        [Route("dashboard/website/layouts/master")]
        [Authorize]
        public ActionResult Master()
        {
            var model = this.GetModel(LayoutType.DefaultLayout);
            return this.FrapidView(this.GetRazorView<AreaRegistration>("Layout/Master.cshtml"), model);
        }
        [Route("dashboard/website/layouts/master/home")]
        [Authorize]
        public ActionResult Homepage()
        {
            var model = this.GetModel(LayoutType.HomepageLayout);
            return this.FrapidView(this.GetRazorView<AreaRegistration>("Layout/Homepage.cshtml"), model);
        }

        [Route("dashboard/website/layouts/header")]
        [Authorize]
        public ActionResult Header()
        {
            var model = this.GetModel(LayoutType.Header);
            return this.FrapidView(this.GetRazorView<AreaRegistration>("Layout/Header.cshtml"), model);
        }

        [Route("dashboard/website/layouts/footer")]
        [Authorize]
        public ActionResult Footer()
        {
            var model = this.GetModel(LayoutType.Footer);
            return this.FrapidView(this.GetRazorView<AreaRegistration>("Layout/Footer.cshtml"), model);
        }


        [Route("dashboard/website/layouts/404-not-found-document")]
        [Authorize]
        public ActionResult Http404Document()
        {
            var model = this.GetModel(LayoutType.Http404Document);
            return this.FrapidView(this.GetRazorView<AreaRegistration>("Layout/Http404Document.cshtml"), model);
        }

        [Route("dashboard/website/layouts/save")]
        [HttpPost]
        [Authorize]
        public ActionResult SaveLayoutFile(Layout layout)
        {
            string path = HostingEnvironment.MapPath(Configuration.GetCurrentThemePath());
            var type = (LayoutType)layout.Type;

            switch (type)
            {
                case LayoutType.DefaultLayout:
                    path += "Layout.cshtml";
                    break;
                case LayoutType.HomepageLayout:
                    path += "Layout-Home.cshtml";
                    break;
                case LayoutType.Header:
                    path += "Header.cshtml";
                    break;
                case LayoutType.Footer:
                    path += "Footer.cshtml";
                    break;
                case LayoutType.Http404Document:
                    path += "404.cshtml";
                    break;
            }


            if (path == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }

            System.IO.File.WriteAllText(path, layout.Contents, Encoding.UTF8);
            return this.Json("OK");
        }

        private Layout GetModel(LayoutType type)
        {
            string directory = HostingEnvironment.MapPath(Configuration.GetCurrentThemePath());
            string path = directory;
            string name = "";

            switch (type)
            {
                case LayoutType.DefaultLayout:
                    name = "Master Layout";
                    path += "Layout.cshtml";
                    break;
                case LayoutType.HomepageLayout:
                    name = "Homepage Layout";
                    path += "Layout-Home.cshtml";
                    break;
                case LayoutType.Header:
                    name = "Header";
                    path += "Header.cshtml";
                    break;
                case LayoutType.Footer:
                    name = "Footer";
                    path += "Footer.cshtml";
                    break;
                case LayoutType.Http404Document:
                    name = "Http 404 Not Found Document";
                    path += "404.cshtml";
                    break;
            }

            if (path == null)
            {
                return new Layout
                {
                    Contents = ""
                };
            }

            string contents = System.IO.File.ReadAllText(path, Encoding.UTF8);

            return new Layout
            {
                Name =  name,
                Type= (int)type,
                Contents = contents
            };
        }
    }
}
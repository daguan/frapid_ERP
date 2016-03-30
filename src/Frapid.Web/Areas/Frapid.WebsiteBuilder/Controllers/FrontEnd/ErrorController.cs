using System.Web.Mvc;
using Frapid.WebsiteBuilder.Models;

namespace Frapid.WebsiteBuilder.Controllers.FrontEnd
{
    public class ErrorController : WebsiteBuilderController
    {
        [Route("content-not-found")]
        public ActionResult Http404()
        {
            string query = this.Request.Url?.PathAndQuery;
            var model = ErrorModel.GetResult(query);

            string path = GetLayoutPath();
            string layout = this.GetLayout();

            model.LayoutPath = path;
            model.Layout = layout;

            this.Response.StatusCode = 404;
            return this.View(this.GetRazorView<AreaRegistration>("ErrorHandlers/404.cshtml"), model);
        }
    }
}
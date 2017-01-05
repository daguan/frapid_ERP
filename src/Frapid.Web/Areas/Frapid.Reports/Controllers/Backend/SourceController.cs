using System.Threading.Tasks;
using System.Web.Mvc;
using Frapid.Reports.Engine;
using Frapid.Reports.Helpers;

namespace Frapid.Reports.Controllers.Backend
{
    public class SourceController : BackendReportController
    {
        [Route("dashboard/reports/source/{*path}")]
        public async Task<ActionResult> IndexAsync(string path)
        {
            await Task.Delay(1).ConfigureAwait(true);
            return this.View("~/Areas/Frapid.Reports/Views/Source.cshtml", path);
        }

        [ActionName("ReportMarkup")]
        [ChildActionOnly]
        public ActionResult Markup(string path)
        {
            var parameters = ParameterHelper.GetParameters(this.Request.QueryString);

            using (var generator = new Generator(this.Tenant, path, parameters))
            {
                string contents = generator.Generate(this.Tenant);
                return this.Content(contents, "text/html");
            }
        }
    }
}
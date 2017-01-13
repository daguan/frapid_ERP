using System.Web.Mvc;
using Frapid.Areas;
using Frapid.Areas.CSRF;
using Frapid.Reports.ViewModels;

namespace Frapid.Reports.Controllers.Backend
{
    [AntiForgery]
    public sealed class ExportController : FrapidController
    {
        [Route("dashboard/reports/export/pdf")]
        [HttpPost]
        public ActionResult ExportToPdf(HtmlConverterViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.InvalidModelState(this.ModelState);
            }

            string result = ExportHelper.Export(this.Tenant, "pdf", model.Html);
            return this.Ok(result);
        }

        [Route("dashboard/reports/export/doc")]
        [HttpPost]
        public ActionResult ExportToDoc(HtmlConverterViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.InvalidModelState(this.ModelState);
            }

            string result = ExportHelper.Export(this.Tenant, "doc", model.Html);
            return this.Ok(result);
        }

        [Route("dashboard/reports/export/xls")]
        [HttpPost]
        public ActionResult ExportToXls(HtmlConverterViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.InvalidModelState(this.ModelState);
            }

            string result = ExportHelper.Export(this.Tenant, "xls", model.Html);
            return this.Ok(result);
        }
    }
}
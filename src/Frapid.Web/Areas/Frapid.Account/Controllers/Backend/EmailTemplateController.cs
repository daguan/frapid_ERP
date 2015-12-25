using System.IO;
using System.Text;
using System.Web.Mvc;
using Frapid.Account.ViewModels;
using Frapid.Areas;
using Frapid.Dashboard.Controllers;

namespace Frapid.Account.Controllers.Backend
{
    [AntiForgery]
    public class EmailTemplateController : DashboardController
    {
        [Route("dashboard/account/email-templates/{file}")]
        [RestrictAnonymous]
        public ActionResult Index(string file)
        {
            string contents = this.GetContents(file);

            if (string.IsNullOrWhiteSpace(contents))
            {
                throw new FileNotFoundException();
            }

            var model = new Template { Contents = contents, Title = file + ".html" };
            return this.FrapidView(this.GetRazorView<AreaRegistration>("EmailTemplate/Index.cshtml"), model);
        }

        [Route("dashboard/account/email-templates")]
        [RestrictAnonymous]
        [HttpPost]
        public ActionResult Save(Template model)
        {
            this.SetContents(model.Title, model.Contents);
            return Json("OK");
        }

        private string GetContents(string file)
        {
            string path = Configuration.GetOverridePath() + "/EmailTemplates/" + file + ".html";
            return System.IO.File.Exists(path) ? System.IO.File.ReadAllText(path, Encoding.UTF8) : string.Empty;
        }

        private void SetContents(string file, string contents)
        {
            string path = Configuration.GetOverridePath() + "/EmailTemplates/" + file;
            if (System.IO.File.Exists(path))
            {
                System.IO.File.WriteAllText(path, contents, Encoding.UTF8);
            }
        }
    }
}
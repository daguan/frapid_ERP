using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Frapid.Dashboard;
using Frapid.Framework.Extensions;
using Frapid.Reports.Engine;
using Frapid.Reports.Engine.Model;
using Frapid.Reports.Helpers;
using Frapid.Reports.Models;
using Frapid.Reports.ViewModels;

namespace Frapid.Reports.Controllers.Backend
{
    public class ReportController : BackendReportController
    {
        [Route("dashboard/reports/view/{*path}")]
        [MenuPolicy]
        public ActionResult Index(string path)
        {
            string query = this.Request?.Url?.Query.Or("");
            string sourcePath = "/dashboard/reports/source/" + path + query;

            var locator = new ReportLocator();
            path = locator.GetPathToDisk(this.Tenant, path);

            var parameters = ParameterHelper.GetParameters(this.Request?.QueryString);
            var parser = new ReportParser(path, this.Tenant, parameters);
            var report = parser.Get();

            var dbParams = new List<DataSourceParameter>();

            foreach (var dataSource in report.DataSources)
            {
                foreach (var parameter in dataSource.Parameters)
                {
                    if (dbParams.Any(x => x.Name.ToLower() == parameter.Name.Replace("@", "").ToLower()))
                    {
                        continue;
                    }

                    if (parameter.HasMetaValue)
                    {
                        continue;
                    }

                    parameter.Name = parameter.Name.Replace("@", "");
                    var fromQueryString = report.Parameters.FirstOrDefault(x => x.Name.ToLower().Equals(parameter.Name.ToLower()));

                    if (fromQueryString != null)
                    {
                        parameter.DefaultValue = DataSourceParameterHelper.CastValue(fromQueryString.Value, parameter.Type);
                    }

                    if (string.IsNullOrWhiteSpace(parameter.FieldLabel))
                    {
                        parameter.FieldLabel = parameter.Name;
                    }

                    dbParams.Add(parameter);
                }
            }


            var model = new ParameterMeta
            {
                ReportSourcePath = sourcePath,
                Parameters = dbParams,
                ReportTitle = report.Title
            };

            return this.View("~/Areas/Frapid.Reports/Views/Index.cshtml", model);
        }
    }
}
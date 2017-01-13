using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Frapid.Framework.Extensions;
using Frapid.Reports.DAL;
using Frapid.Reports.Engine.Model;
using Frapid.Reports.Helpers;
using Frapid.Reports.Models;

namespace Frapid.Reports.Engine
{
    public sealed class Generator : IDisposable
    {
        public Generator(string tenant, string path, List<Parameter> parameters)
        {
            this.Parameters = parameters;

            var locator = new ReportLocator();
            this.Path = locator.GetPathToDisk(tenant, path);

            var parser = new ReportParser(this.Path, tenant, parameters);
            this.Report = parser.Get();

            if (this.Report.DataSources == null)
            {
                return;
            }

            foreach (var dataSource in this.Report.DataSources)
            {
                dataSource.Data = this.GetDataSource(this.Report, dataSource);
            }
        }

        public List<Parameter> Parameters { get; set; }
        public string Path { get; set; }
        public Report Report { get; set; }

        public void Dispose()
        {
            if (this.Report.DataSources == null)
            {
                return;
            }

            foreach (var dataSource in this.Report.DataSources)
            {
                dataSource.Data?.Dispose();
            }
        }

        private string ParseExpressions(string html)
        {
            if (this.Report.DataSources == null)
            {
                return string.Empty;
            }

            html = ExpressionHelper.ParseExpression(this.Report.Tenant, html, this.Report.DataSources, ParameterHelper.GetPraParameterInfo(this.Report));
            html = ExpressionHelper.ParseDataSource(html, this.Report.DataSources);

            return html;
        }

        public string Generate(string tenant)
        {
            var type = typeof(IGenerator);
            var members = type.GetTypeMembers<IGenerator>();

            var source = new StringBuilder();

            foreach (var member in members.OrderBy(x => x.Order).ThenBy(x => x.Name))
            {
                string markup = member.Generate(tenant, this.Report);
                source.Append(markup);
            }

            return this.ParseExpressions(source.ToString());
        }

        private DataTable GetDataSource(Report report, DataSource dataSource)
        {
            var parameters = new ParameterInfo
            {
                Parameters = report.Parameters,
                DataSourceParameters = dataSource.Parameters
            };

            return DataSourceHelper.GetDataTable(report.Tenant, dataSource.Query, parameters);
        }
    }
}
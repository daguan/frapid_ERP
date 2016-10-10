using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Frapid.Configuration;
using Frapid.Framework.Extensions;
using Frapid.Reports.DAL;
using Frapid.Reports.Engine.Model;
using Frapid.Reports.Helpers;

namespace Frapid.Reports.Engine
{
    public sealed class Generator : IDisposable
    {
        public Generator(string tenant, string path, List<Parameter> parameters)
        {
            this.Parameters = parameters;

            string root = PathMapper.MapPath("/Reports");
            this.Path = System.IO.Path.Combine(root, path);

            var parser = new ReportParser(this.Path, tenant, parameters);
            this.Report = parser.Get();

            foreach (var dataSource in this.Report.DataSources)
            {
                dataSource.Data = this.GetDataSource(this.Report, dataSource);
            }
        }

        public List<Parameter> Parameters { get; set; }
        public string Path { get; set; }
        private Report Report { get; }

        public void Dispose()
        {
            foreach (var dataSource in this.Report.DataSources)
            {
                dataSource.Data?.Dispose();
            }
        }

        private string ParseExpressions(string html)
        {            
            html = ExpressionHelper.ParseExpression(this.Report.Tenant, html, this.Report.DataSources, ParameterHelper.GetPraParameterInfo(this.Report));
            html = ExpressionHelper.ParseDataSource(html, this.Report.DataSources);

            return html;
        }

        public string Generate()
        {
            var type = typeof(IGenerator);
            var members = type.GetTypeMembers<IGenerator>();

            var source = new StringBuilder();

            foreach (var member in members.OrderBy(x => x.Order).ThenBy(x => x.Name))
            {
                string markup = member.Generate(this.Report);
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
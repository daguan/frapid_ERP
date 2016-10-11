using System.Collections.Generic;
using Frapid.Reports.Engine.Model;

namespace Frapid.Reports.ViewModels
{
    public class ParameterMeta
    {
        public string ReportSourcePath { get; set; }
        public string ReportTitle { get; set; }
        public List<DataSourceParameter> Parameters { get; set; }
    }
}
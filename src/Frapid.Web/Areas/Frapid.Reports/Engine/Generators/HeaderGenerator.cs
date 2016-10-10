using System.IO;
using System.Text;
using Frapid.Configuration;
using Frapid.Reports.Engine.Model;

namespace Frapid.Reports.Engine.Generators
{
    public sealed class HeaderGenerator : IGenerator
    {
        public int Order { get; } = 1000;
        public string Name => "Header";
        public string Generate(Report report)
        {
            if (!report.HasHeader)
            {
                return string.Empty;
            }

            string pathToHeader = PathMapper.MapPath("~/Reports/Assets/Header.html");
            return File.ReadAllText(pathToHeader, Encoding.UTF8);
        }
    }
}
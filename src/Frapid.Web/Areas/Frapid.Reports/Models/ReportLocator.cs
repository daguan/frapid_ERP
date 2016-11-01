using System.IO;
using Frapid.Configuration;

namespace Frapid.Reports.Models
{
    public sealed class ReportLocator
    {
        public string GetPathToDisk(string tenant, string path)
        {
            string overridePath = $"Tenant/{tenant}/{path}";
            string root = PathMapper.MapPath("/");
            overridePath = Path.Combine(root, overridePath);

            if (File.Exists(overridePath))
            {
                return overridePath;
            }

            string requestedPath = Path.Combine(root, path);
            return requestedPath;
        }
    }
}
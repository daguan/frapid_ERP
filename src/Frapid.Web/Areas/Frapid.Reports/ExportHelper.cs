using System.Linq;
using Frapid.Framework.Extensions;

namespace Frapid.Reports
{
    public static class ExportHelper
    {
        public static string Export(string tenant, string extension, string html, string destination = "")
        {
            var type = typeof(IExportTo);

            var member = type.GetTypeMembers<IExportTo>().FirstOrDefault(x => x.Extension == extension && x.Enabled);

            return member?.Export(tenant, html);
        }
    }
}
using System.IO;
using System.Text;
using Frapid.Configuration;

namespace Frapid.Reports.HtmlConverters
{
    public static class HtmlWriter
    {
        public static void WriteHtml(string path, string html)
        {
            string file = PathMapper.MapPath(path);

            if (file != null)
            {
                File.WriteAllText(file, html, Encoding.UTF8);
            }
        }
    }
}
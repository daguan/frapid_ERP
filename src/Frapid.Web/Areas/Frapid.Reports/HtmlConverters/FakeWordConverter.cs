using System;

namespace Frapid.Reports.HtmlConverters
{
    public class FakeWordConverter : IExportTo
    {
        public bool Enabled { get; set; } = true;
        public string Extension => "doc";

        public string Export(string tenant, string html, string destination = "")
        {
            string id = Guid.NewGuid().ToString();

            if (string.IsNullOrWhiteSpace(destination))
            {
                destination = $"/Tenants/{tenant}/Documents/{id}.doc";
            }

            HtmlWriter.WriteHtml(destination, html);
            return destination;
        }
    }
}
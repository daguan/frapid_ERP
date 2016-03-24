using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace Frapid.Framework
{
    public static class SiteMapGenerator
    {
        public static string Get(string domain)
        {
            var xml = new MemoryStream();

            var writer = XmlWriter.Create(xml, new XmlWriterSettings
            {
                Encoding = Encoding.UTF8,
                Indent = false
            });

            writer.WriteStartElement("urlset", "http://www.sitemaps.org/schemas/sitemap/0.9");

            var urls = GetUrls();
            foreach (var url in urls)
            {
                writer.WriteStartElement("url");

                WriteTag(ref writer, "loc", UrlHelper.CombineUrl(domain, url.Location));
                WriteTag(ref writer, "lastmod", url.LastModified.ToString("yyyy-MM-ddTHH:mm:ssK"));
                //W3C Datetime format


                if (url.ChangeFrequency != SiteMapChangeFrequency.Undefined)
                {
                    WriteTag(ref writer, "changefreq", url.ChangeFrequency.ToString().ToLowerInvariant());
                }

                if (url.Priority >= 0 && url.Priority <= 1)
                {
                    WriteTag(ref writer, "priority", url.Priority.ToString("F1"));
                }

                writer.WriteEndElement(); //url
            }

            writer.WriteEndElement(); //urlset
            writer.Flush();

            return Encoding.UTF8.GetString(xml.ToArray());
        }

        private static void WriteTag(ref XmlWriter writer, string name, string innerText)
        {
            writer.WriteStartElement(name);
            writer.WriteString(innerText);
            writer.WriteEndElement();
        }

        private static List<SiteMapUrl> GetUrls()
        {
            var urls = new List<SiteMapUrl>();

            var iType = typeof (ISiteMapGenerator);

            var members = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => iType.IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                .Select(Activator.CreateInstance);

            foreach (ISiteMapGenerator member in members)
            {
                urls.AddRange(member.Generate());
            }

            return urls;
        }
    }
}
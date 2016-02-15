using Frapid.WebsiteBuilder.Contracts;
using Frapid.WebsiteBuilder.Models;
using HtmlAgilityPack;

namespace Frapid.WebsiteBuilder.Plugins
{
    public class ArticleExtension : IContentExtension
    {
        public string ParseHtml(string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var nodes = doc.DocumentNode.SelectNodes("//include[@article-alias and @category-alias]");
            if (nodes == null)
            {
                return html;
            }

            foreach (var node in nodes)
            {
                string alias = node.Attributes["article-alias"].Value;
                string categoryAlias = node.Attributes["category-alias"].Value;

                var model = ContentModel.GetContent(categoryAlias, alias);
                if (model != null)
                {
                    string contents = model.Contents;

                    var newNode = HtmlNode.CreateNode(contents);
                    node.ParentNode.ReplaceChild(newNode, node);
                }
            }

            return doc.DocumentNode.OuterHtml;
        }
    }
}
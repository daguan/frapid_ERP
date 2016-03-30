using System.Collections.Generic;
using System.Linq;
using System.Web;
using Frapid.Configuration;
using Frapid.Framework;
using Frapid.WebsiteBuilder.Contracts;
using Frapid.WebsiteBuilder.Extensions;
using Frapid.WebsiteBuilder.ViewModels;

namespace Frapid.WebsiteBuilder
{
    public class ContentSearch: IContentSearch
    {
        public List<SearchResultContent> Search(string query)
        {
            var context = new HttpContextWrapper(HttpContext.Current);
            var result = DAL.Contents.Search(query);
            string domain = DbConvention.GetBaseDomain(context, true);

            return result.Select(item => new SearchResultContent
            {
                Title = item.Title,
                Contents = item.Contents.ToText().Truncate(200),
                LastUpdatedOn = item.LastEditedOn,
                LinkUrl = item.IsBlog ? 
                UrlHelper.CombineUrl(domain, "/blog/" + item.CategoryAlias + "/" + item.Alias) 
                : UrlHelper.CombineUrl(domain, "/site/" + item.CategoryAlias + "/" + item.Alias)
            }).ToList();
        }
    }
}
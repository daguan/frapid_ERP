using System.Collections.Generic;
using Frapid.WebsiteBuilder.ViewModels;

namespace Frapid.WebsiteBuilder.Contracts
{
    public interface IContentSearch
    {
        List<SearchResultContent> Search(string query);
    }
}
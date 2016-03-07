namespace Frapid.WebsiteBuilder.Contracts
{
    public interface IContentExtension
    {
        string ParseHtml(string tenant, string html);
    }
}
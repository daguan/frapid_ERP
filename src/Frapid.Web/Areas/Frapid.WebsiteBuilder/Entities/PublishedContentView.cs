// ReSharper disable All
using System;
using Frapid.DataAccess;
using Frapid.NPoco;

namespace Frapid.WebsiteBuilder.Entities
{
    [TableName("website.published_content_view")]
    [PrimaryKey("", AutoIncrement = false)]
    public sealed class PublishedContentView : IPoco
    {
        public int? ContentId { get; set; }
        public int? CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string CategoryAlias { get; set; }
        public string Title { get; set; }
        public string Alias { get; set; }
        public int? AuthorId { get; set; }
        public string Markdown { get; set; }
        public string Contents { get; set; }
        public string Tags { get; set; }
        public string SeoKeywords { get; set; }
        public string SeoDescription { get; set; }
        public bool? IsHomepage { get; set; }
    }
}
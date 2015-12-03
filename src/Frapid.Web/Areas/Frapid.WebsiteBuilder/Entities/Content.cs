// ReSharper disable All
using NPoco;
using System;
using Frapid.DataAccess;

namespace Frapid.WebsiteBuilder.Entities
{
    public sealed class Content: IPoco
    {
        public int ContentId { get; set; }
        public string Title { get; set; }
        public string Alias { get; set; }
        public int? AuthorId { get; set; }
        public DateTime PublishedOn { get; set; }
        public string Contents { get; set; }
        public bool Draft { get; set; }
        public string SeoKeywords { get; set; }
        public string SeoDescription { get; set; }
        public bool IsDefault { get; set; }
        public int? AuditUserId { get; set; }
        public DateTime? AuditTs { get; set; }
    }
}
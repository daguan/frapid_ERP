using System;

namespace Frapid.WebsiteBuilder.Models
{
    public class Content
    {
        public int ContentId { get; set; }
        public string Title { get; set; }
        public string Alias { get; set; }
        public int? AuthorId { get; set; }
        public DateTime PublishOn { get; set; }
        public string Contents { get; set; }
        public bool IsDraft { get; set; }
        public string SeoKeywords { get; set; }
        public string SeoDescription { get; set; }
        public string LayoutPath { get; set; }
        public string Layout { get; set; }
    }
}
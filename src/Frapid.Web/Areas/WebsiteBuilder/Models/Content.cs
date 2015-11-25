using System;

namespace WebsiteBuilder.Models
{
    public sealed class Content
    {
        public int ContentId { get; set; }
        public string Title { get; set; }
        public string Alias { get; set; }
        public int AuthorId { get; set; }
        public DateTime PublishedOn { get; set; }
        public string Contents { get; set; }
        public bool Draft { get; set; }
        public string SeoKeywords { get; set; }
        public string SeoDescription { get; set; }
        public string Layout { get; set; }
        public string LayoutPath { get; set; }
    }
}
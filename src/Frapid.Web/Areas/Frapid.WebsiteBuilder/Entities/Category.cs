// ReSharper disable All
using System;
using Frapid.DataAccess;
using Frapid.NPoco;

namespace Frapid.WebsiteBuilder.Entities
{
    [TableName("website.categories")]
    [PrimaryKey("category_id", AutoIncrement = true)]
    public sealed class Category : IPoco
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Alias { get; set; }
        public string SeoKeywords { get; set; }
        public string SeoDescription { get; set; }
        public int? AuditUserId { get; set; }
        public DateTime? AuditTs { get; set; }
    }
}
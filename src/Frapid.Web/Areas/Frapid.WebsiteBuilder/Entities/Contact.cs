// ReSharper disable All
using System;
using Frapid.DataAccess;
using Frapid.NPoco;

namespace Frapid.WebsiteBuilder.Entities
{
    [TableName("website.contacts")]
    [PrimaryKey("contact_id", AutoIncrement = true)]
    public sealed class Contact : IPoco
    {
        public int ContactId { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public string Telephone { get; set; }
        public string Details { get; set; }
        public string Email { get; set; }
        public bool DisplayEmail { get; set; }
        public bool DisplayContactForm { get; set; }
        public int Sort { get; set; }
        public bool Status { get; set; }
        public int? AuditUserId { get; set; }
        public DateTime? AuditTs { get; set; }
    }
}
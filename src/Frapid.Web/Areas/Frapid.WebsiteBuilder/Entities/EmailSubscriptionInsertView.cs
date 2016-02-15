// ReSharper disable All
using System;
using Frapid.DataAccess;
using Frapid.NPoco;

namespace Frapid.WebsiteBuilder.Entities
{
    [TableName("website.email_subscription_insert_view")]
    [PrimaryKey("", AutoIncrement = false)]
    public sealed class EmailSubscriptionInsertView : IPoco
    {
        public System.Guid? EmailSubscriptionId { get; set; }
        public string Email { get; set; }
        public string Browser { get; set; }
        public string IpAddress { get; set; }
        public bool? Unsubscribed { get; set; }
        public DateTime? SubscribedOn { get; set; }
        public DateTime? UnsubscribedOn { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool? Confirmed { get; set; }
    }
}
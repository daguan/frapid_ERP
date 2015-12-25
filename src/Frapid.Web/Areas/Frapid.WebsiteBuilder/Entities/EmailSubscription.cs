// ReSharper disable All
using System;
using Frapid.DataAccess;
using Frapid.NPoco;

namespace Frapid.WebsiteBuilder.Entities
{
    [TableName("website.email_subscriptions")]
    [PrimaryKey("email_subscription_id", AutoIncrement = false)]
    public sealed class EmailSubscription : IPoco
    {
        public System.Guid EmailSubscriptionId { get; set; }
        public string Email { get; set; }
        public string Browser { get; set; }
        public string IpAddress { get; set; }
        public bool? Unsubscribed { get; set; }
        public DateTime? SubscribedOn { get; set; }
        public DateTime? UnsubscribedOn { get; set; }
    }
}
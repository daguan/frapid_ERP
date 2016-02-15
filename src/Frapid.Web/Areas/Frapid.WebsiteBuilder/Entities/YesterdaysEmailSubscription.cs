// ReSharper disable All
using System;
using Frapid.DataAccess;
using Frapid.NPoco;

namespace Frapid.WebsiteBuilder.Entities
{
    [TableName("website.yesterdays_email_subscriptions")]
    [PrimaryKey("", AutoIncrement = false)]
    public sealed class YesterdaysEmailSubscription : IPoco
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SubscriptionType { get; set; }
    }
}
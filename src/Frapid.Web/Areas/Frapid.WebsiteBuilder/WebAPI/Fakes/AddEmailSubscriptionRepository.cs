// ReSharper disable All
using System;
using System.Collections.Generic;
using Frapid.WebsiteBuilder.DataAccess;
using Frapid.WebsiteBuilder.Entities;

namespace Frapid.WebsiteBuilder.Api.Fakes
{
    public class AddEmailSubscriptionRepository : IAddEmailSubscriptionRepository
    {
        public string Email { get; set; }

        public bool Execute()
        {
            return new bool();
        }
    }
}
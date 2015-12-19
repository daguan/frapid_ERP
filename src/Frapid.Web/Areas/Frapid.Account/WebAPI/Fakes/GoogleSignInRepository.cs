// ReSharper disable All
using System;
using System.Collections.Generic;
using Frapid.Account.DataAccess;
using Frapid.Account.Entities;

namespace Frapid.Account.Api.Fakes
{
    public class GoogleSignInRepository : IGoogleSignInRepository
    {
        public string Email { get; set; }
        public int OfficeId { get; set; }
        public string Name { get; set; }
        public string Token { get; set; }
        public string Browser { get; set; }
        public string IpAddress { get; set; }
        public string Culture { get; set; }

        public IEnumerable<DbGoogleSignInResult> Execute()
        {
            return new List<DbGoogleSignInResult>();
        }
    }
}
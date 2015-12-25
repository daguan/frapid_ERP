// ReSharper disable All
using System;
using System.Collections.Generic;
using Frapid.Core.DataAccess;
using Frapid.Core.Entities;

namespace Frapid.Core.Api.Fakes
{
    public class CreateAppRepository : ICreateAppRepository
    {
        public string AppName { get; set; }
        public string Name { get; set; }
        public string VersionNumber { get; set; }
        public string Publisher { get; set; }
        public DateTime PublishedOn { get; set; }
        public string Icon { get; set; }
        public string LandingUrl { get; set; }
        public string[] Dependencies { get; set; }

        public void Execute()
        {
            return;
        }
    }
}
using System;
using Frapid.DataAccess;

namespace Frapid.Dashboard.DTO
{
    public class App : IPoco
    {
        public string AppName { get; set; }
        public string Name { get; set; }
        public string VersionNumber { get; set; }
        public string Publisher { get; set; }
        public DateTime? PublishedOn { get; set; }
        public string Icon { get; set; }
        public string LandingUrl { get; set; }
    }
}
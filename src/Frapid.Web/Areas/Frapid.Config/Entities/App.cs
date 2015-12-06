// ReSharper disable All
using System;
using Frapid.DataAccess;
using Frapid.NPoco;

namespace Frapid.Config.Entities
{
    [TableName("config.apps")]
    [PrimaryKey("app_name", AutoIncrement = false)]
    public sealed class App : IPoco
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
// ReSharper disable All
using System;
using Frapid.DataAccess;
using Frapid.NPoco;

namespace Frapid.Config.Entities
{
    [TableName("config.app_dependencies")]
    [PrimaryKey("app_dependency_id", AutoIncrement = true)]
    public sealed class AppDependency : IPoco
    {
        public int AppDependencyId { get; set; }
        public string AppName { get; set; }
        public string DependsOn { get; set; }
    }
}
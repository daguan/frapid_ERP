// ReSharper disable All

using System;
using Frapid.DataAccess;

namespace Frapid.Config.Entities
{
    public sealed class FilterNameView : IPoco
    {
        public string ObjectName { get; set; }
        public string FilterName { get; set; }
        public bool? IsDefault { get; set; }
    }
}
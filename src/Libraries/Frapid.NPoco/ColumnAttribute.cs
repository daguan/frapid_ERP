using System;

namespace Frapid.NPoco
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class ColumnAttribute : Attribute
    {
        public ColumnAttribute() { }
        public ColumnAttribute(string name) { this.Name = name; }
        public string Name { get; set; }
        public bool ForceToUtc { get; set; }
    }
}
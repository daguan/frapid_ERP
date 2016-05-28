using System;

namespace Frapid.NPoco
{
    [AttributeUsage(AttributeTargets.Class)]
    public class PrimaryKeyAttribute : Attribute
    {
        public PrimaryKeyAttribute(string primaryKey)
        {
            this.Value = primaryKey;
            this.AutoIncrement = true;
        }

        public string Value { get; private set; }
        public string SequenceName { get; set; }
        public bool AutoIncrement { get; set; }
        public bool UseOutputClause { get; set; }
    }
}
using System;

namespace Frapid.NPoco
{
    public class AliasAttribute : Attribute
    {
        public string Alias { get; set; }

        public AliasAttribute(string alias)
        {
            this.Alias = alias;
        }
    }
}
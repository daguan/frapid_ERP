using System;

namespace Frapid.NPoco
{
    public class ComplexMappingAttribute : Attribute
    {
        public string CustomPrefix { get; set; }

        public ComplexMappingAttribute()
        {
            
        }

        public ComplexMappingAttribute(string customPrefix)
        {
            this.CustomPrefix = customPrefix;
        }
    }
}
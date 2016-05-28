using System;
using System.Collections.Generic;
using System.Reflection;

namespace Frapid.NPoco.FluentMappings
{
    public class ConventionScannerSettings
    {
        public ConventionScannerSettings()
        {
            this.Assemblies = new HashSet<Assembly>();
            this.IgnorePropertiesWhere = new List<Func<MemberInfo, bool>>();
            this.IncludeTypes = new List<Func<Type, bool>>();
            this.MappingOverrides = new List<Mappings>();
        }

        public List<Mappings> MappingOverrides { get; set; }

        public HashSet<Assembly> Assemblies { get; set; }
        public bool TheCallingAssembly { get; set; }
        public List<Func<Type, bool>> IncludeTypes { get; set; }

        public Func<Type, string> TablesNamed { get; set; }
        public Func<Type, string> PrimaryKeysNamed { get; set; }
        public Func<Type, bool> PrimaryKeysAutoIncremented { get; set; }
        public Func<Type, string> SequencesNamed { get; set; }
        public Func<Type, bool> UseOutputClauseWhere { get; set; }

        public Func<MemberInfo, string> DbColumnsNamed { get; set; }
        public Func<MemberInfo, string> AliasNamed { get; set; }
        public Func<MemberInfo, Type> DbColumnTypesAs { get; set; }
        public List<Func<MemberInfo, bool>> IgnorePropertiesWhere { get; set; }
        public Func<MemberInfo, bool> VersionPropertiesWhere { get; set; }
        public Func<MemberInfo, VersionColumnType> VersionPropertyTypeAs { get; set; }
        public Func<MemberInfo, bool> ResultPropertiesWhere { get; set; }
        public Func<MemberInfo, bool> ComputedPropertiesWhere { get; set; }
        public Func<MemberInfo, ComputedColumnType> ComputedPropertyTypeAs { get; set; }
        public Func<MemberInfo, bool> ForceDateTimesToUtcWhere { get; set; }
        public Func<MemberInfo, bool> ReferencePropertiesWhere { get; set; }
        public Func<MemberInfo, bool> ComplexPropertiesWhere { get; set; }
        public Func<MemberInfo, string> ReferenceDbColumnsNamed { get; set; }
        public Func<MemberInfo, bool> DbColumnWhere { get; set; }
        public Func<MemberInfo, bool> SerializedWhere { get; set; }
        

        public bool Lazy { get; set; }

        //public bool OverrideWithAttributes { get; set; }
    }
}
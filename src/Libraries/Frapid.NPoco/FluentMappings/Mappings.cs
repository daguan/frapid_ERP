using System;
using System.Collections.Generic;

namespace Frapid.NPoco.FluentMappings
{
    public class Mappings
    {
        public Dictionary<Type, TypeDefinition> Config = new Dictionary<Type, TypeDefinition>();

        public Map<T> For<T>()
        {
            TypeDefinition definition = this.Config.ContainsKey(typeof(T)) ? this.Config[typeof(T)] : new TypeDefinition(typeof(T));
            Map<T> petaPocoMap = new Map<T>(definition);
            this.Config[typeof (T)] = definition;
            return petaPocoMap;
        }

        public static Mappings BuildMappingsFromMaps(params IMap[] petaPocoMaps)
        {
            Mappings petaPocoConfig = new Mappings();
            foreach (IMap petaPocoMap in petaPocoMaps)
            {
                Type type = petaPocoMap.TypeDefinition.Type;
                petaPocoConfig.Config[type] = petaPocoMap.TypeDefinition;
            }
            return petaPocoConfig;
        }
    }
}
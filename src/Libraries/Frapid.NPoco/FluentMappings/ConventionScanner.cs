using System;
using System.Reflection;

namespace Frapid.NPoco.FluentMappings
{
    public class ConventionScanner : IConventionScanner
    {
        private readonly ConventionScannerSettings _scannerSettings;

        public ConventionScanner(ConventionScannerSettings scannerSettings)
        {
            this._scannerSettings = scannerSettings;
        }

        public void OverrideMappingsWith(Mappings mappings)
        {
            this._scannerSettings.MappingOverrides.Add(mappings);
        }

        public void OverrideMappingsWith(params IMap[] maps)
        {
            Mappings mappings = Mappings.BuildMappingsFromMaps(maps);
            this._scannerSettings.MappingOverrides.Add(mappings);
        }

        public void Assembly(Assembly assembly)
        {
            this._scannerSettings.Assemblies.Add(assembly);
        }

#if !DNXCORE50
        public void TheCallingAssembly()
        {
            this._scannerSettings.TheCallingAssembly = true;
        }
#endif

        public void IncludeTypes(Func<Type, bool> typeIncludes)
        {
            this._scannerSettings.IncludeTypes.Add(typeIncludes);
        }

        public void TablesNamed(Func<Type, string> tableFunc)
        {
            this._scannerSettings.TablesNamed = tableFunc;
        }

        public void PrimaryKeysNamed(Func<Type, string> primaryKeyFunc)
        {
            this._scannerSettings.PrimaryKeysNamed = primaryKeyFunc;
        }

        public void SequencesNamed(Func<Type, string> sequencesFunc)
        {
            this._scannerSettings.SequencesNamed = sequencesFunc;
        }

        public void LazyLoadMappings()
        {
            this._scannerSettings.Lazy = true;
        }

        public void PrimaryKeysAutoIncremented(Func<Type, bool> primaryKeyAutoIncrementFunc)
        {
            this._scannerSettings.PrimaryKeysAutoIncremented = primaryKeyAutoIncrementFunc;
        }

        //public void OverrideWithAttributes()
        //{
        //    _scannerSettings.OverrideWithAttributes = true;
        //}

        public IColumnsBuilderConventions Columns => new PropertyBuilderConventions(this._scannerSettings);
    }
}
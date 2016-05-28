using System;
using System.Reflection;

namespace Frapid.NPoco.FluentMappings
{
    public class PropertyBuilderConventions : IColumnsBuilderConventions
    {
        private readonly ConventionScannerSettings _scannerSettings;

        public PropertyBuilderConventions(ConventionScannerSettings scannerSettings)
        {
            this._scannerSettings = scannerSettings;
        }

        public IColumnsBuilderConventions Named(Func<MemberInfo, string> propertiesNamedFunc)
        {
            this._scannerSettings.DbColumnsNamed = propertiesNamedFunc;
            return this;
        }

        public IColumnsBuilderConventions Aliased(Func<MemberInfo, string> aliasNamedFunc)
        {
            this._scannerSettings.AliasNamed = aliasNamedFunc;
            return this;
        }

        public IColumnsBuilderConventions IgnoreWhere(Func<MemberInfo, bool> ignorePropertiesWhereFunc)
        {
            this._scannerSettings.IgnorePropertiesWhere.Add(ignorePropertiesWhereFunc);
            return this;
        }

        public IColumnsBuilderConventions ResultWhere(Func<MemberInfo, bool> resultPropertiesWhereFunc)
        {
            this._scannerSettings.ResultPropertiesWhere = resultPropertiesWhereFunc;
            return this;
        }

        public IColumnsBuilderConventions ComputedWhere(Func<MemberInfo, bool> computedPropertiesWhereFunc)
        {
            this._scannerSettings.ComputedPropertiesWhere = computedPropertiesWhereFunc;
            return this;
        }

        public IColumnsBuilderConventions ComputedTypeAs(Func<MemberInfo, ComputedColumnType> computedPropertyTypeAsFunc)
        {
            this._scannerSettings.ComputedPropertyTypeAs = computedPropertyTypeAsFunc;
            return this;
        }

        public IColumnsBuilderConventions VersionWhere(Func<MemberInfo, bool> versionPropertiesWhereFunc)
        {
            this._scannerSettings.VersionPropertiesWhere = versionPropertiesWhereFunc;
            return this;
        }

        public IColumnsBuilderConventions VersionTypeAs(Func<MemberInfo, VersionColumnType> versionPropertyTypeAsFunc)
        {
            this._scannerSettings.VersionPropertyTypeAs = versionPropertyTypeAsFunc;
            return this;
        }

        public IColumnsBuilderConventions ForceDateTimesToUtcWhere(Func<MemberInfo, bool> forceDateTimesToUtcWhereFunc)
        {
            this._scannerSettings.ForceDateTimesToUtcWhere = forceDateTimesToUtcWhereFunc;
            return this;
        }

        public IColumnsBuilderConventions DbColumnTypeAs(Func<MemberInfo, Type> dbColumnTypeAsFunc)
        {
            this._scannerSettings.DbColumnTypesAs = dbColumnTypeAsFunc;
            return this;
        }

        public IColumnsBuilderConventions ReferenceNamed(Func<MemberInfo, string> refPropertiesNamedFunc)
        {
            this._scannerSettings.ReferenceDbColumnsNamed = refPropertiesNamedFunc;
            return this;
        }

        public IColumnsBuilderConventions ReferencePropertiesWhere(Func<MemberInfo, bool> referencePropertiesWhereFunc)
        {
            this._scannerSettings.ReferencePropertiesWhere = referencePropertiesWhereFunc;
            return this;
        }

        public IColumnsBuilderConventions ComplexPropertiesWhere(Func<MemberInfo, bool> complexPropertiesWhereFunc)
        {
            this._scannerSettings.ComplexPropertiesWhere = complexPropertiesWhereFunc;
            return this;
        }

        public IColumnsBuilderConventions SerializedWhere(Func<MemberInfo, bool> serializedWhereFunc)
        {
            this._scannerSettings.SerializedWhere = serializedWhereFunc;
            return this;
        }
    }
}
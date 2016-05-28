using System;

namespace Frapid.NPoco
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class VersionColumnAttribute : ColumnAttribute
    {
        public VersionColumnType VersionColumnType { get; private set; }

        public VersionColumnAttribute() : this(VersionColumnType.Number) {}
        public VersionColumnAttribute(VersionColumnType versionColumnType)
        {
            this.VersionColumnType = versionColumnType;
        }
        public VersionColumnAttribute(string name, VersionColumnType versionColumnType) : base(name)
        {
            this.VersionColumnType = versionColumnType;
        }
    }

    public enum VersionColumnType
    {
        Number,
        RowVersion
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Frapid.NPoco.FluentMappings
{
    public class FluentMappingsPocoDataBuilder : PocoDataBuilder
    {
        private readonly Mappings _mappings;

        public FluentMappingsPocoDataBuilder(Type type, Mappings mappings, MapperCollection mapper) : 
            base(type, mapper)
        {
            this._mappings = mappings;
        }

        protected override TableInfoPlan GetTableInfo(Type type, ColumnInfo[] columnInfos, List<MemberInfo> memberInfos)
        {
            if (!this._mappings.Config.ContainsKey(type))
                return base.GetTableInfo(type, columnInfos, memberInfos);

            TypeDefinition typeConfig = this._mappings.Config[type];
            // Get the table name
            string a = typeConfig.TableName ?? "";
            string tableName = a.Length == 0 ? type.Name : a;

            // Get the primary key
            a = typeConfig.PrimaryKey ?? "";
            string primaryKey = a.Length == 0 ? "ID" : a;

            if (memberInfos.Count == 0) // if top level
            {
                foreach (ColumnInfo ci in columnInfos)
                {
                    string[] originalPk = primaryKey.Split(',');
                    for (int i = 0; i < originalPk.Length; i++)
                    {
                        if (originalPk[i].Equals(ci.MemberInfo.Name, StringComparison.OrdinalIgnoreCase))
                            originalPk[i] = (ci.ColumnName ?? ci.MemberInfo.Name);
                    }
                    primaryKey = string.Join(",", originalPk);
                }
            }
            
            a = typeConfig.SequenceName ?? "";
            string sequenceName = a.Length == 0 ? null : a;

            bool autoIncrement = typeConfig.AutoIncrement ?? true;

            // Set autoincrement false if primary key has multiple columns
            autoIncrement = autoIncrement ? !primaryKey.Contains(',') : autoIncrement;
            
            // Set auto alias
            string autoAlias = this.CreateAlias(type.Name, type);
            
            return () => new TableInfo
            {
                TableName = tableName,
                PrimaryKey = primaryKey,
                SequenceName = sequenceName,
                AutoIncrement = autoIncrement,
                AutoAlias = autoAlias
            };
        }

        protected override ColumnInfo GetColumnInfo(MemberInfo mi, Type type)
        {
            if (!this._mappings.Config.ContainsKey(type))
                return base.GetColumnInfo(mi, type);

            TypeDefinition typeConfig = this._mappings.Config[type];
            ColumnInfo columnInfo = new ColumnInfo() {MemberInfo = mi};
            string key = mi.Name;

            bool explicitColumns = typeConfig.ExplicitColumns ?? false;
            bool isColumnDefined = typeConfig.ColumnConfiguration.ContainsKey(key);

            if (isColumnDefined && typeConfig.ColumnConfiguration[key].IsComplexMapping.HasValue && typeConfig.ColumnConfiguration[key].IsComplexMapping.Value)
            {
                columnInfo.ComplexMapping = true;

                if (typeConfig.ColumnConfiguration[key].ComplexPrefix != null)
                    columnInfo.ComplexPrefix = typeConfig.ColumnConfiguration[key].ComplexPrefix;
            }
            else if (isColumnDefined && typeConfig.ColumnConfiguration[key].IsReferenceMember.HasValue && typeConfig.ColumnConfiguration[key].IsReferenceMember.Value)
            {
                if (typeConfig.ColumnConfiguration[key].ReferenceType != null)
                    columnInfo.ReferenceType = typeConfig.ColumnConfiguration[key].ReferenceType.Value;

                if (typeConfig.ColumnConfiguration[key].ReferenceMember != null)
                    columnInfo.ReferenceMemberName = typeConfig.ColumnConfiguration[key].ReferenceMember.Name;
            }
            else if (isColumnDefined && typeConfig.ColumnConfiguration[key].Serialized.HasValue && typeConfig.ColumnConfiguration[key].Serialized.Value)
            {
                columnInfo.SerializedColumn = true;
            }

            if (explicitColumns && !isColumnDefined)
                columnInfo.IgnoreColumn = true;

            if (isColumnDefined && (typeConfig.ColumnConfiguration[key].IgnoreColumn.HasValue && typeConfig.ColumnConfiguration[key].IgnoreColumn.Value))
                columnInfo.IgnoreColumn = true;

            // Work out the DB column name
            if (isColumnDefined)
            {
                ColumnDefinition colattr = typeConfig.ColumnConfiguration[key];
                columnInfo.ColumnName = colattr.DbColumnName;
                columnInfo.ColumnAlias = colattr.DbColumnAlias;
                if (colattr.ResultColumn.HasValue && colattr.ResultColumn.Value)
                {
                    columnInfo.ResultColumn = true;
                }
                else if (colattr.VersionColumn.HasValue && colattr.VersionColumn.Value)
                {
                    columnInfo.VersionColumn = true;
                    columnInfo.VersionColumnType = colattr.VersionColumnType ?? VersionColumnType.Number;
                }
                else if (colattr.ComputedColumn.HasValue && colattr.ComputedColumn.Value)
                {
                    columnInfo.ComputedColumn = true;
                    columnInfo.ComputedColumnType = colattr.ComputedColumnType ?? ComputedColumnType.Always;
                }

                if (colattr.ForceUtc.HasValue)
                {
                    columnInfo.ForceToUtc = colattr.ForceUtc.Value;
                }

                columnInfo.ColumnType = colattr.DbColumnType;
            }
            
            return columnInfo;
        }
    }
}

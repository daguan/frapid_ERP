using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Frapid.NPoco.FluentMappings
{
    public class Map<T> : IMap
    {
        private readonly TypeDefinition _petaPocoTypeDefinition;

        public Map() : this(new TypeDefinition(typeof(T)))
        {
        }

        public Map(TypeDefinition petaPocoTypeDefinition)
        {
            this._petaPocoTypeDefinition = petaPocoTypeDefinition;
        }

        public void UseMap<TMap>() where TMap : IMap
        {
            Activator.CreateInstance(typeof (TMap), this._petaPocoTypeDefinition);

            List<string> keys = this._petaPocoTypeDefinition.ColumnConfiguration.Select(x => x.Key).ToList();
            List<MemberInfo> fieldsAndPropertiesForClasses = ReflectionUtils.GetFieldsAndPropertiesForClasses(typeof(T));
            
            foreach (string key in keys.Where(key => fieldsAndPropertiesForClasses.All(x => x.Name != key)))
            {
                this._petaPocoTypeDefinition.ColumnConfiguration.Remove(key);
            }
        }

        public Map<T> TableName(string tableName)
        {
            this._petaPocoTypeDefinition.TableName = tableName;
            return this;
        }

        public Map<T> Columns(Action<ColumnConfigurationBuilder<T>> columnConfiguration)
        {
            return this.Columns(columnConfiguration, null);
        }

        public Map<T> Columns(Action<ColumnConfigurationBuilder<T>> columnConfiguration, bool? explicitColumns)
        {
            this._petaPocoTypeDefinition.ExplicitColumns = explicitColumns;
            columnConfiguration(new ColumnConfigurationBuilder<T>(this._petaPocoTypeDefinition.ColumnConfiguration));
            return this;
        }

        public Map<T> PrimaryKey(Expression<Func<T, object>> column, string sequenceName)
        {
            MemberInfo[] members = MemberHelper<T>.GetMembers(column);
            return this.PrimaryKey(members.Last().Name, sequenceName);
        }

        public Map<T> PrimaryKey(Expression<Func<T, object>> column)
        {
            this._petaPocoTypeDefinition.AutoIncrement = true;
            return this.PrimaryKey(column, null);
        }

        public Map<T> PrimaryKey(Expression<Func<T, object>> column, bool autoIncrement)
        {
            MemberInfo[] members = MemberHelper<T>.GetMembers(column);
            return this.PrimaryKey(members.Last().Name, autoIncrement);
        }

        public Map<T> CompositePrimaryKey(params Expression<Func<T, object>>[] columns)
        {
            string[] columnNames = new string[columns.Length];
            for (int i = 0; i < columns.Length; i++)
            {
                columnNames[i] = MemberHelper<T>.GetMembers(columns[i]).Last().Name;
            }

            this._petaPocoTypeDefinition.PrimaryKey = string.Join(",", columnNames);
            return this;
        }

        public Map<T> PrimaryKey(string primaryKeyColumn, bool autoIncrement)
        {
            this._petaPocoTypeDefinition.PrimaryKey = primaryKeyColumn;
            this._petaPocoTypeDefinition.AutoIncrement = autoIncrement;
            return this;
        }

        public Map<T> PrimaryKey(string primaryKeyColumn, bool autoIncrement, bool useOutputClause)
        {
            this._petaPocoTypeDefinition.PrimaryKey = primaryKeyColumn;
            this._petaPocoTypeDefinition.AutoIncrement = autoIncrement;
            this._petaPocoTypeDefinition.UseOutputClause = useOutputClause;
            return this;
        }

        public Map<T> PrimaryKey(string primaryKeyColumn, string sequenceName)
        {
            this._petaPocoTypeDefinition.PrimaryKey = primaryKeyColumn;
            this._petaPocoTypeDefinition.SequenceName = sequenceName;
            return this;
        }

        public Map<T> PrimaryKey(string primaryKeyColumn, string sequenceName, bool useOutputClause)
        {
            this._petaPocoTypeDefinition.PrimaryKey = primaryKeyColumn;
            this._petaPocoTypeDefinition.SequenceName = sequenceName;
            this._petaPocoTypeDefinition.UseOutputClause = useOutputClause;
            return this;
        }

        public Map<T> PrimaryKey(string primaryKeyColumn)
        {
            return this.PrimaryKey(primaryKeyColumn, null);
        }

        TypeDefinition IMap.TypeDefinition => this._petaPocoTypeDefinition;
    }
}
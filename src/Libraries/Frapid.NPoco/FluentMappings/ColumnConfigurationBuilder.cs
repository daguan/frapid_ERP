using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Frapid.NPoco.FluentMappings
{
    public class ColumnConfigurationBuilder<T>
    {
        private readonly Dictionary<string, ColumnDefinition> _columnDefinitions;

        public ColumnConfigurationBuilder(Dictionary<string, ColumnDefinition> columnDefinitions)
        {
            this._columnDefinitions = columnDefinitions;
        }

        public IColumnBuilder<T2> Column<T2>(Expression<Func<T, T2>> property)
        {
            MemberInfo[] members = MemberHelper<T>.GetMembers(property);
            MemberInfo memberInfo = members.Last();
            ColumnDefinition columnDefinition = new ColumnDefinition() { MemberInfo = memberInfo };
            ColumnBuilder<T2> builder = new ColumnBuilder<T2>(columnDefinition);
            string key = PocoColumn.GenerateKey(members);
            this._columnDefinitions[key] = columnDefinition;
            return builder;
        }

        public IManyColumnBuilder<T2> Many<T2>(Expression<Func<T, IList<T2>>> property)
        {
            MemberInfo[] members = MemberHelper<T>.GetMembers(property);
            ColumnDefinition columnDefinition = new ColumnDefinition() { MemberInfo = members.Last() };
            ManyColumnBuilder<T2> builder = new ManyColumnBuilder<T2>(columnDefinition);
            string key = PocoColumn.GenerateKey(members);
            this._columnDefinitions[key] = columnDefinition;
            return builder;
        }
    }

    public interface IManyColumnBuilder<TModel>
    {
        IManyColumnBuilder<TModel> WithName(string name);
        IManyColumnBuilder<TModel> WithDbType(Type type);
        IManyColumnBuilder<TModel> WithDbType<T>();
        IManyColumnBuilder<TModel> Reference(Expression<Func<TModel, object>> member);
    }

    public class ManyColumnBuilder<TModel> : IManyColumnBuilder<TModel>
    {
        private readonly ColumnDefinition _columnDefinition;

        public ManyColumnBuilder(ColumnDefinition columnDefinition)
        {
            this._columnDefinition = columnDefinition;
        }

        public IManyColumnBuilder<TModel> WithName(string name)
        {
            this._columnDefinition.DbColumnName = name;
            return this;
        }

        public IManyColumnBuilder<TModel> WithDbType(Type type)
        {
            this._columnDefinition.DbColumnType = type;
            return this;
        }

        public IManyColumnBuilder<TModel> WithDbType<T>()
        {
            return this.WithDbType(typeof(T));
        }

        public IManyColumnBuilder<TModel> Reference(Expression<Func<TModel, object>> member)
        {
            this._columnDefinition.IsReferenceMember = true;
            this._columnDefinition.ReferenceType = ReferenceType.Many;
            this._columnDefinition.ReferenceMember = MemberHelper<TModel>.GetMembers(member).Last();
            return this;
        }
    }

    public interface IColumnBuilder<TModel>
    {
        IColumnBuilder<TModel> WithName(string name);
        IColumnBuilder<TModel> WithAlias(string alias);
        IColumnBuilder<TModel> WithDbType(Type type);
        IColumnBuilder<TModel> WithDbType<T>();
        IColumnBuilder<TModel> Version();
        IColumnBuilder<TModel> Version(VersionColumnType versionColumnType);
        IColumnBuilder<TModel> Ignore();
        IColumnBuilder<TModel> Result();
        IColumnBuilder<TModel> Computed();
        IColumnBuilder<TModel> Computed(ComputedColumnType computedColumnType);
        IColumnBuilder<TModel> Reference(ReferenceType referenceType = ReferenceType.Foreign);
        IColumnBuilder<TModel> Reference(Expression<Func<TModel, object>> member, ReferenceType referenceType = ReferenceType.Foreign);
        IColumnBuilder<TModel> Serialized();
        IColumnBuilder<TModel> ComplexMapping(string prefix = null);
        IColumnBuilder<TModel> ForceToUtc(bool enabled);
    }

    public class ColumnBuilder<TModel> : IColumnBuilder<TModel>
    {
        private readonly ColumnDefinition _columnDefinition;

        public ColumnBuilder(ColumnDefinition columnDefinition)
        {
            this._columnDefinition = columnDefinition;
        }

        public IColumnBuilder<TModel> WithName(string name)
        {
            this._columnDefinition.DbColumnName = name;
            return this;
        }

        public IColumnBuilder<TModel> WithAlias(string alias)
        {
            this._columnDefinition.DbColumnAlias = alias;
            return this;
        }

        public IColumnBuilder<TModel> WithDbType(Type type)
        {
            this._columnDefinition.DbColumnType = type;
            return this;
        }

        public IColumnBuilder<TModel> WithDbType<T>()
        {
            return this.WithDbType(typeof (T));
        }

        public IColumnBuilder<TModel> Version()
        {
            this._columnDefinition.VersionColumn = true;
            return this;
        }

        public IColumnBuilder<TModel> Version(VersionColumnType versionColumnType)
        {
            this._columnDefinition.VersionColumn = true;
            this._columnDefinition.VersionColumnType = versionColumnType;
            return this;
        }

        public IColumnBuilder<TModel> Ignore()
        {
            this._columnDefinition.IgnoreColumn = true;
            return this;
        }

        public IColumnBuilder<TModel> Result()
        {
            this._columnDefinition.ResultColumn = true;
            return this;
        }

        public IColumnBuilder<TModel> Computed()
        {
            this._columnDefinition.ComputedColumn = true;
            return this;
        }

        public IColumnBuilder<TModel> Computed(ComputedColumnType computedColumnType)
        {
            this._columnDefinition.ComputedColumnType = computedColumnType;
            return this;
        }

        public IColumnBuilder<TModel> Reference(ReferenceType referenceType = ReferenceType.Foreign)
        {
            if (referenceType == ReferenceType.Many)
            {
                throw new Exception("Use Many(x => x.Items) instead of Column(x => x.Items) for one to many relationships");
            }

            this._columnDefinition.IsReferenceMember = true;
            this._columnDefinition.ReferenceType = referenceType;
            return this;
        }

        public IColumnBuilder<TModel> Reference(Expression<Func<TModel, object>> member, ReferenceType referenceType = ReferenceType.Foreign)
        {
            this.Reference(referenceType);
            this._columnDefinition.ReferenceMember = MemberHelper<TModel>.GetMembers(member).Last();
            return this;
        }

        public IColumnBuilder<TModel> Serialized()
        {
            this._columnDefinition.Serialized = true;
            return this;
        }

        public IColumnBuilder<TModel> ComplexMapping(string prefix = null)
        {
            this._columnDefinition.IsComplexMapping = true;
            this._columnDefinition.ComplexPrefix = prefix;
            return this;
        }

        public IColumnBuilder<TModel> ForceToUtc(bool enabled)
        {
            this._columnDefinition.ForceUtc = enabled;
            return this;
        }
    }
}
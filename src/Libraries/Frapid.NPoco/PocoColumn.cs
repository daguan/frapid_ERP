using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Frapid.NPoco
{
    public class PocoColumn
    {
        public PocoColumn()
        {
            this.ForceToUtc = true;
            this.MemberInfoChain = new List<MemberInfo>();
        }
        
        public static string GenerateKey(IEnumerable<MemberInfo> memberInfoChain)
        {
            return string.Join(PocoData.Separator, memberInfoChain.Select(x => x.Name).ToArray());
        }

        public TableInfo TableInfo;
        public string ColumnName;

        public List<MemberInfo> MemberInfoChain { get; set; }

        private string _memberInfoKey;
        public string MemberInfoKey => this._memberInfoKey ?? (this._memberInfoKey = GenerateKey(this.MemberInfoChain));

        public MemberInfoData MemberInfoData { get; set; }

        public bool ResultColumn;
        public bool VersionColumn;
        public VersionColumnType VersionColumnType;
        public bool ComputedColumn;
        public ComputedColumnType ComputedColumnType;
        private Type _columnType;
        private MemberAccessor _memberAccessor;
        private List<MemberAccessor> _memberAccessorChain = new List<MemberAccessor>();

        public Type ColumnType
        {
            get { return this._columnType ?? this.MemberInfoData.MemberType; }
            set { this._columnType = value; }
        }

        public bool ForceToUtc { get; set; }
        public string ColumnAlias { get; set; }

        public ReferenceType ReferenceType { get; set; }
        public bool SerializedColumn { get; set; }

        internal void SetMemberAccessors(List<MemberAccessor> memberAccessors)
        {
            this._memberAccessor = memberAccessors[memberAccessors.Count-1];
            this._memberAccessorChain = memberAccessors;
        }

        public virtual void SetValue(object target, object val)
        {
            this._memberAccessor.Set(target, val);
        }

        public virtual object GetValue(object target)
        {
            foreach (MemberAccessor memberAccessor in this._memberAccessorChain)
            {
                target = target == null ? null : memberAccessor.Get(target);
            }
            //foreach (var memberInfo in MemberInfoChain)
            //{
            //    target = target == null ? null : memberInfo.GetMemberInfoValue(target);
            //}
            return target;
        }

        public virtual object ChangeType(object val) { return Convert.ChangeType(val, this.MemberInfoData.MemberType); }

        public object GetColumnValue(PocoData pd, object target, Func<PocoColumn, object, object> callback = null)
        {
            callback = callback ?? ((_, o) => o);
            if (this.ReferenceType == ReferenceType.Foreign)
            {
                PocoMember member = pd.Members.Single(x => x.MemberInfoData == this.MemberInfoData);
                PocoMember column = member.PocoMemberChildren.SingleOrDefault(x => x.Name == member.ReferenceMemberName);
                if (column == null)
                {
                    throw new Exception(string.Format("Could not find member on '{0}' with name '{1}'", member.MemberInfoData.MemberType, member.ReferenceMemberName));
                }
                return callback(column.PocoColumn, column.PocoColumn.GetValue(target));
            }
            else
            {
                return callback(this, this.GetValue(target));
            }
        }
    }
}
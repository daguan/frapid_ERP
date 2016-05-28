using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Reflection;

namespace Frapid.NPoco
{
    public class PocoMember
    {
        public PocoMember()
        {
            this.PocoMemberChildren = new List<PocoMember>();
            this.ReferenceType = ReferenceType.None;
        }

        public string Name => this.MemberInfoData.Name;

        public MemberInfoData MemberInfoData
        {
            get;
            set;
        }

        public PocoColumn PocoColumn
        {
            get;
            set;
        }

        public List<PocoMember> PocoMemberChildren
        {
            get;
            set;
        }

        public ReferenceType ReferenceType
        {
            get;
            set;
        }

        public string ReferenceMemberName
        {
            get;
            set;
        }

        public bool IsList
        {
            get;
            set;
        }

        public bool IsDynamic
        {
            get;
            set;
        }

        public List<MemberInfo> MemberInfoChain
        {
            get;
            set;
        }

        private FastCreate _creator;
        private MemberAccessor _memberAccessor;
        private Type _listType;

        public virtual object Create(DbDataReader dataReader)
        {
            return this._creator.Create(dataReader);
        }

        public IList CreateList()
        {
            object list = Activator.CreateInstance(this._listType);
            return (IList)list;
        }

        public void SetMemberAccessor(MemberAccessor memberAccessor, FastCreate fastCreate, Type listType)
        {
            this._listType = listType;
            this._memberAccessor = memberAccessor;
            this._creator = fastCreate;
        }

        public virtual void SetValue(object target, object value)
        {
            this._memberAccessor.Set(target, value);
        }

        public virtual object GetValue(object target)
        {
            return this._memberAccessor.Get(target);
        }
    }
}
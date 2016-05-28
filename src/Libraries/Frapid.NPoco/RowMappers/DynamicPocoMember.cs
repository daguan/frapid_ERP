using System.Collections.Generic;
using System.Data.Common;

namespace Frapid.NPoco.RowMappers
{
    public class DynamicPocoMember : PocoMember
    {
        private readonly MapperCollection _mapperCollection;

        public DynamicPocoMember(MapperCollection mapperCollection)
        {
            this._mapperCollection = mapperCollection;
        }

        public override object Create(DbDataReader dataReader)
        {
            return this._mapperCollection.GetFactory(this.MemberInfoData.MemberType)(dataReader);
        }

        public override void SetValue(object target, object value)
        {
            ((IDictionary<string, object>) target)[this.Name] = value;
        }

        public override object GetValue(object target)
        {
            object val;
            ((IDictionary<string, object>)target).TryGetValue(this.Name, out val);
            return val;
        }
    }
}
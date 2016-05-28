using System;
using System.Data.Common;
using System.Reflection;

namespace Frapid.NPoco
{
    public interface IMapper
    {
        Func<object, object> GetFromDbConverter(MemberInfo memberInfo, Type sourceType);
        Func<object, object> GetFromDbConverter(Type destType, Type sourceType);
        Func<object, object> GetParameterConverter(DbCommand dbCommand, Type sourceType);
        Func<object, object> GetToDbConverter(Type destType, MemberInfo sourceMemberInfo);
    }
}
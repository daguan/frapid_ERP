using System;
using System.Linq;
using System.Reflection;

namespace Frapid.NPoco
{
    public class MappingHelper
    {
        private static readonly EnumMapper EnumMapper = new EnumMapper();
        private static readonly Cache<Type, Type> UnderlyingTypes = Cache<Type, Type>.CreateStaticCache();

        public static Func<object, object> GetConverter(MapperCollection mapper, PocoColumn pc, Type srcType, Type dstType)
        {
            Func<object, object> converter = null;

            // Get converter from the mapper
            if(mapper != null)
            {
                converter = pc?.MemberInfoData != null ? mapper.Find(x => x.GetFromDbConverter(pc.MemberInfoData.MemberInfo, srcType)) : mapper.Find(x => x.GetFromDbConverter(dstType, srcType));
                if(converter != null)
                    return converter;
            }

            if(pc != null &&
               pc.SerializedColumn)
            {
                converter = src => DatabaseFactory.ColumnSerializer.Deserialize((string)src, dstType);
                return converter;
            }

            // Standard DateTime->Utc mapper
            if(pc != null &&
               pc.ForceToUtc &&
               srcType == typeof(DateTime) &&
               (dstType == typeof(DateTime) || dstType == typeof(DateTime?)))
            {
                converter = src => new DateTime(((DateTime)src).Ticks, DateTimeKind.Utc);
                return converter;
            }

            //Todo:The following changes were done to NPoco

            #region "Changes"

            // DateTime->DateTimeOffset
            if(pc != null &&
               srcType == typeof(DateTime) &&
               (dstType == typeof(DateTimeOffset) || dstType == typeof(DateTimeOffset?)))
            {
                converter = src => new DateTimeOffset((DateTime)src);
                return converter;
            }

            // DateTimeOffset --> DateTime
            if(pc != null &&
               srcType == typeof(DateTimeOffset) &&
               (dstType == typeof(DateTime) || dstType == typeof(DateTime?)))
            {
                converter = src => (DateTime)src;
                return converter;
            }

            #endregion

            // Forced type conversion including integral types -> enum
            var underlyingType = UnderlyingTypes.Get(dstType, () => Nullable.GetUnderlyingType(dstType));
            if(dstType.GetTypeInfo().IsEnum ||
               (underlyingType != null && underlyingType.GetTypeInfo().IsEnum))
            {
                if(srcType == typeof(string))
                {
                    converter = src => EnumMapper.EnumFromString(underlyingType ?? dstType, (string)src);
                    return converter;
                }

                if(IsIntegralType(srcType))
                {
                    converter = src => Enum.ToObject(underlyingType ?? dstType, src);
                    return converter;
                }
            }
            else if(!dstType.IsAssignableFrom(srcType))
            {
                converter = src => Convert.ChangeType(src, underlyingType ?? dstType, null);
            }
            return converter;
        }

        private static bool IsIntegralType(Type t)
        {
            //var tc = Type.GetTypeCode(t);
            //return tc >= TypeCode.SByte && tc <= TypeCode.UInt64;
            //Not available for now

            return new[]
                   {
                       typeof(sbyte),
                       typeof(byte),
                       typeof(short),
                       typeof(ushort),
                       typeof(int),
                       typeof(uint),
                       typeof(long),
                       typeof(ulong)
                   }.Contains(t);
        }

        public static object GetDefault(Type type)
        {
            if(type.GetTypeInfo().IsValueType)
            {
                return Activator.CreateInstance(type);
            }
            return null;
        }
    }
}
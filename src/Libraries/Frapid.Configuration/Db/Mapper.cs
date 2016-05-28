using System;
using System.Linq;
using System.Reflection;
using Frapid.NPoco;
using NpgsqlTypes;

namespace Frapid.Configuration.Db
{
    public class Mapper: DefaultMapper
    {
        private object GetFromDbConverter(object value, Type destType, Type sourceType)
        {
            if(value is NpgsqlTimeStampTZ)
            {
                if(destType == typeof(DateTime))
                {
                    return (DateTime)(NpgsqlTimeStampTZ)value;
                }
                if(destType == typeof(DateTime?))
                {
                    return (DateTime?)(NpgsqlTimeStampTZ)value;
                }
                if(destType == typeof(DateTimeOffset))
                {
                    var tstz = (NpgsqlTimeStampTZ)value;
                    return new DateTimeOffset((DateTime)tstz);
                }
                if(destType == typeof(DateTimeOffset?))
                {
                    var tstz = (NpgsqlTimeStampTZ)value;
                    return new DateTimeOffset((DateTime)tstz);
                }
            }

            if(value is NpgsqlTimeStamp)
            {
                if(destType == typeof(DateTime))
                {
                    return (DateTime)(NpgsqlTimeStamp)value;
                }
                if(destType == typeof(DateTime?))
                {
                    return (DateTime?)(NpgsqlTimeStamp)value;
                }
            }

            if(value is NpgsqlDate)
            {
                if(destType == typeof(DateTime))
                {
                    return (DateTime)(NpgsqlDate)value;
                }
                if(destType == typeof(DateTime?))
                {
                    return (DateTime?)(NpgsqlDate)value;
                }
            }

            if(value is DateTime)
            {
                if(destType == typeof(DateTimeOffset) ||
                   destType == typeof(DateTimeOffset?))
                {
                    //value is not null
                    return new DateTimeOffset((DateTime)value);
                }

                return value;
            }

            if(value is DateTimeOffset)
            {
                if(destType == typeof(DateTime) ||
                   destType == typeof(DateTime?))
                {
                    //value is not null
                    return ((DateTimeOffset)value).DateTime;
                }

                return (DateTimeOffset)value;
            }

            return value;
        }

        public override Func<object, object> GetFromDbConverter(Type destType, Type sourceType)
        {
            if(destType == typeof(DateTimeOffset) ||
               destType == typeof(DateTimeOffset?) ||
               destType == typeof(DateTime) ||
               destType == typeof(DateTime?))
            {
                return x => this.GetFromDbConverter(x, destType, sourceType);
            }

            if(destType == typeof(HStore) &&
               sourceType == typeof(string))
            {
                return x => HStore.Create((string)x);
            }

            return base.GetFromDbConverter(destType, sourceType);
        }


        public override Func<object, object> GetToDbConverter(Type destType, MemberInfo sourceInfo)
        {
            if(sourceInfo == null)
            {
                return x => x;
            }


            if(sourceInfo == typeof(HStore))
            {
                return x => ((HStore)x).ToSqlString();
            }

            if(sourceInfo == typeof(string) &&
               destType == typeof(bool))
            {
                return x => new[]
                            {
                                "TRUE",
                                "YES",
                                "T"
                            }.Contains(x.ToString().ToUpperInvariant());
            }

            return base.GetToDbConverter(destType, sourceInfo);
        }
    }
}
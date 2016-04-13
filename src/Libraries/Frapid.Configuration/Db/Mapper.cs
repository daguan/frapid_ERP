using System;
using Frapid.NPoco;
using NpgsqlTypes;

namespace Frapid.Configuration.Db
{
    public class Mapper : DefaultMapper
    {
        public override Func<object, object> GetFromDbConverter(Type destType, Type sourceType)
        {
            if (destType == typeof (DateTimeOffset) || destType == typeof (DateTimeOffset?)
                || destType == typeof (DateTime) || destType == typeof (DateTime?))
            {
                return x =>
                {
                    if (x is NpgsqlTimeStampTZ)
                    {
                        if (destType == typeof (DateTime))
                        {
                            return (DateTime) (NpgsqlTimeStampTZ) x;
                        }
                        if (destType == typeof (DateTime?))
                        {
                            return (DateTime?) (NpgsqlTimeStampTZ) x;
                        }
                        if (destType == typeof (DateTimeOffset))
                        {
                            var tstz = (NpgsqlTimeStampTZ) x;
                            return new DateTimeOffset((DateTime) tstz);
                        }
                        if (destType == typeof (DateTimeOffset?))
                        {
                            var tstz = (NpgsqlTimeStampTZ) x;
                            return new DateTimeOffset((DateTime) tstz);
                        }
                    }
                    if (x is NpgsqlTimeStamp)
                    {
                        if (destType == typeof (DateTime))
                        {
                            return (DateTime) (NpgsqlTimeStamp) x;
                        }
                        if (destType == typeof (DateTime?))
                        {
                            return (DateTime?) (NpgsqlTimeStamp) x;
                        }
                    }
                    if (x is NpgsqlDate)
                    {
                        if (destType == typeof (DateTime))
                        {
                            return (DateTime) (NpgsqlDate) x;
                        }
                        if (destType == typeof (DateTime?))
                        {
                            return (DateTime?) (NpgsqlDate) x;
                        }
                    }
                    if (x is DateTime)
                    {
                        if (destType == typeof (DateTimeOffset))
                        {
                            return new DateTimeOffset((DateTime) x);
                        }
                    }
                    if (x is DateTimeOffset)
                    {
                        if (destType == typeof (DateTime))
                        {
                            return ((DateTimeOffset) x).DateTime;
                        }
                    }

                    return x;
                };
            }

            if (destType == typeof (HStore) && sourceType == typeof (string))
            {
                return x => HStore.Create((string) x);
            }

            return base.GetFromDbConverter(destType, sourceType);
        }

        public override Func<object, object> GetToDbConverter(Type destType, Type sourceType)
        {
            if (sourceType == typeof (HStore))
            {
                return x => ((HStore) x).ToSqlString();
            }

            return base.GetToDbConverter(destType, sourceType);
        }
    }
}
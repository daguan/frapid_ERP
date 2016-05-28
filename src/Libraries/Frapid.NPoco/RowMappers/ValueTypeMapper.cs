using System;
using System.Data.Common;
using System.Reflection;

namespace Frapid.NPoco.RowMappers
{
    public class ValueTypeMapper : RowMapper
    {
        private Func<object, object> _converter;

        public override bool ShouldMap(PocoData pocoData)
        {
            return pocoData.Type.GetTypeInfo().IsValueType || pocoData.Type == typeof (string) || pocoData.Type == typeof (byte[]);
        }

        public override void Init(DbDataReader dataReader, PocoData pocoData)
        {
            this._converter = GetConverter(pocoData, null, dataReader.GetFieldType(0), pocoData.Type) ?? (x => x);
            base.Init(dataReader, pocoData);
        }

        public override object Map(DbDataReader dataReader, RowMapperContext context)
        {
            if (dataReader.IsDBNull(0))
                return null;

            return this._converter(dataReader.GetValue(0));
        }
    }
}
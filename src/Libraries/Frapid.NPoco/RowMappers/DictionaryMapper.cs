using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Frapid.NPoco.RowMappers
{
    public class DictionaryMapper : RowMapper
    {
        private PosName[] _posNames;

        public override bool ShouldMap(PocoData pocoData)
        {
            return pocoData.Type == typeof (object)
                   || pocoData.Type == typeof (Dictionary<string, object>)
                   || pocoData.Type == typeof (IDictionary<string, object>);
        }

        public override void Init(DbDataReader dataReader, PocoData pocoData)
        {
            this._posNames = this.GetColumnNames(dataReader, pocoData);
        }

        public override object Map(DbDataReader dataReader, RowMapperContext context)
        {
            IDictionary<string, object> target = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

#if !NET35
            if (context.Type == typeof(object))
                target = new PocoExpando();
#endif

            for (int i = 0; i < this._posNames.Length; i++)
            {
                Func<object, object> converter = context.PocoData.Mapper.Find(x => x.GetFromDbConverter(typeof(object), dataReader.GetFieldType(this._posNames[i].Pos))) ?? (x => x);
                target.Add(this._posNames[i].Name, dataReader.IsDBNull(this._posNames[i].Pos) ? null : converter(dataReader.GetValue(this._posNames[i].Pos)));
            }

            return target;
        }
    }
}
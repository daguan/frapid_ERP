using System;
using System.Data.Common;

namespace Frapid.NPoco.RowMappers
{
    public class ArrayMapper : RowMapper
    {
        private PosName[] _posNames;

        public override bool ShouldMap(PocoData pocoData)
        {
            return pocoData.Type.IsArray;
        }

        public override void Init(DbDataReader dataReader, PocoData pocoData)
        {
            this._posNames = this.GetColumnNames(dataReader, pocoData);
        }

        public override object Map(DbDataReader dataReader, RowMapperContext context)
        {
            Type arrayType = context.Type.GetElementType();
            Array array = Array.CreateInstance(arrayType, this._posNames.Length);

            for (int i = 0; i < this._posNames.Length; i++)
            {
                if (!dataReader.IsDBNull(this._posNames[i].Pos))
                {
                    array.SetValue(dataReader.GetValue(this._posNames[i].Pos), i);
                }
            }

            return array;
        }
    }
}
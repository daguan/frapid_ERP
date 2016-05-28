using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Frapid.NPoco.RowMappers;

namespace Frapid.NPoco
{
    public class MappingFactory
    {
        public static List<Func<IRowMapper>> RowMappers { get; private set; } 
        private readonly PocoData _pocoData;
        private readonly IRowMapper _rowMapper;

        static MappingFactory()
        {
            RowMappers = new List<Func<IRowMapper>>()
            {
                () => new DictionaryMapper(),
                () => new ValueTypeMapper(),
                () => new ArrayMapper(),
                () => new PropertyMapper()
            };
        }

        public MappingFactory(PocoData pocoData, DbDataReader dataReader)
        {
            this._pocoData = pocoData;
            this._rowMapper = RowMappers.Select(mapper => mapper()).First(x => x.ShouldMap(pocoData));
            this._rowMapper.Init(dataReader, pocoData);
        }

        public object Map(DbDataReader dataReader, object instance)
        {
            return this._rowMapper.Map(dataReader, new RowMapperContext()
            {
                Instance = instance,
                PocoData = this._pocoData
            });
        }
    }
}

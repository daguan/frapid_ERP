using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace Frapid.NPoco.RowMappers
{
    public interface IRowMapper
    {
        bool ShouldMap(PocoData pocoData);
        object Map(DbDataReader dataReader, RowMapperContext context);
        void Init(DbDataReader dataReader, PocoData pocoData);
    }

    public abstract class RowMapper : IRowMapper
    {
        public abstract bool ShouldMap(PocoData pocoData);

        public virtual void Init(DbDataReader dataReader, PocoData pocoData)
        {
        }

        private PosName[] _columnNames;

        protected PosName[] GetColumnNames(DbDataReader dataReader, PocoData pocoData)
        {
            if (this._columnNames != null)
                return this._columnNames;

            List<PosName> cols = Enumerable.Range(0, dataReader.FieldCount)
                .Select(x => new PosName { Pos = x, Name = dataReader.GetName(x) })
                .Where(x => !string.Equals("poco_rn", x.Name))
                .ToList();

            if (cols.Any(x => x.Name.StartsWith(PropertyMapperNameConvention.SplitPrefix)))
            {
                return (this._columnNames = cols.ConvertFromNewConvention(pocoData).ToArray());
            }

            return (this._columnNames = cols.ConvertFromOldConvention(pocoData.Members).ToArray());
        }

        public abstract object Map(DbDataReader dataReader, RowMapperContext context);

        public static Func<object, object> GetConverter(PocoData pocoData, PocoColumn pocoColumn, Type sourceType, Type desType)
        {
            Func<object, object> converter = MappingHelper.GetConverter(pocoData.Mapper, pocoColumn, sourceType, desType);
            return converter;
        }
    }
}
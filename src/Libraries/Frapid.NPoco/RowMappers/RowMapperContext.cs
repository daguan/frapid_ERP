using System;

namespace Frapid.NPoco.RowMappers
{
    public struct RowMapperContext
    {
        public object Instance { get; set; }
        public PocoData PocoData { get; set; }
        public Type Type => this.PocoData.Type;
    }
}